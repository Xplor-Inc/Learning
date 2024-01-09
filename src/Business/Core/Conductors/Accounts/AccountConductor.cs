﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using XploringMe.Core.Extensions;
using XploringMe.Core.Interfaces.Conductor;
using XploringMe.Core.Interfaces.Conductors.Accounts;
using XploringMe.Core.Interfaces.Emails.Templates;
using XploringMe.Core.Interfaces.Utility;
using XploringMe.Core.Interfaces.Utility.Security;
using XploringMe.Core.Models.Entities.Audits;
using XploringMe.Core.Models.Entities.Users;
using XploringMe.Core.Models.Errors;
using XploringMe.Core.Utilities;

namespace XploringMe.Core.Conductors.Users;
public class AccountConductor : IAccountConductor
{
    #region Properties
    private IRepositoryConductor<AccountRecovery>   AccountRecoveryRepository   { get; }
    private IEncryption                             Encryption                  { get; }
    public  IWebHostEnvironment                     Environment                 { get; }
    public  IHtmlTemplate                            HtmlTemplate                { get; }
    private IHttpContextAccessor                    HttpContext                 { get; }
    public IUserAgentConductor                      UserAgentConductor          { get; }
    private ILogger<AccountConductor>               Logger                      { get; }
    private IRepositoryConductor<UserLogin>         UserLoginRepository         { get; }
    private IRepositoryConductor<User>              UserRepository              { get; }
    #endregion


    #region Constructor
    public AccountConductor(
        IRepositoryConductor<AccountRecovery>   accountRecoveryRepository,
        IEncryption                             encryption,
        IWebHostEnvironment                     environment,
        IHtmlTemplate                           htmlTemplate,
        ILogger<AccountConductor>               logger,
        IHttpContextAccessor                    httpContext,
        IUserAgentConductor                     userAgentConductor,
        IRepositoryConductor<UserLogin>         userLoginRepository,
        IRepositoryConductor<User>              userRepository)
    {
        AccountRecoveryRepository   = accountRecoveryRepository;
        Encryption                  = encryption;
        Environment                 = environment;
        HtmlTemplate                = htmlTemplate;
        Logger                      = logger;
        HttpContext                 = httpContext;
        UserAgentConductor          = userAgentConductor;
        UserLoginRepository         = userLoginRepository;
        UserRepository              = userRepository;
    }
    #endregion 

    public Result<bool> ActivateAccount(string emailAddress, string link, string password) => Do<bool>.Try(r =>
    {        
        var userResult = IsActivationLinkValid(emailAddress, link);
        if (userResult.HasErrors)
        {
            r.AddErrors(userResult.Errors);
            return false;
        }
        var user = userResult.ResultObject;
        if (user == null)
        {
            r.AddError("Activation link is not valid");
            return false;
        }

        var passwordSalt = Encryption.GenerateSalt();

        user.SecurityStamp          = $"{Guid.NewGuid():N}";
        user.PasswordSalt           = passwordSalt;
        user.PasswordHash           = Encryption.GenerateHash(password, passwordSalt);
        user.IsActive               = true;
        user.IsAccountActivated     = true;
        user.AccountActivateDate    = DateTime.Now;

        var userUpdateResult = UserRepository.Update(user, user.Id);
        if (userUpdateResult.HasErrors)
        {
            r.AddErrors(userUpdateResult.Errors);
            return false;
        }
        Logger.LogInformation("Account is activated for user '{emailAddress}'", emailAddress);
        return false;

    }).Result;

    public Result<User?> Authenticate(string emailAddress, string password) => Do<User?>.Try(r =>
    {
        var userResult = UserRepository.FindAll(w => w.EmailAddress == emailAddress && w.DeletedOn == null && w.IsAccountActivated && w.IsActive);
        if (userResult.HasErrors)
        {
            r.AddErrors(userResult.Errors);
            return null;
        }
        var user = userResult.ResultObject.FirstOrDefault();
        if (user != null)
        {
            string passwordHash = Encryption.GenerateHash(password, user.PasswordSalt);
            if (passwordHash == user.PasswordHash)
            {
                user.LastLoginDate = DateTimeOffset.Now;
                var userUpdateCreateResult = UserRepository.Update(user, user.Id);
                if (userUpdateCreateResult.HasErrors)
                {
                    r.AddErrors(userUpdateCreateResult.Errors);
                    return null;
                }

                CreateUserLogin(user, emailAddress, true);
                return user;
            }
            else
            {
                CreateUserLogin(user, emailAddress, false);
                r.AddError("Invalid Email Address or Password");
                return null;
            }
        }
        else
        {
            CreateUserLogin(user, emailAddress, false);
            r.AddError("Invalid Email Address or Password");
            return null;
        }
    }).Result;

    public Result<User?> ChangePassword(long userId, string oldPassword, string newPassword) => Do<User?>.Try(r =>
    {
        var userResult = UserRepository.FindById(userId);
        if (userResult.HasErrors)
        {
            r.AddErrors(userResult.Errors);
            return null;
        }
        var user = userResult.ResultObject;
        if (user != null && !user.DeletedOn.HasValue && user.IsActive)
        {
            string passwordHash = Encryption.GenerateHash(oldPassword, user.PasswordSalt);
            if (passwordHash == user.PasswordHash)
            {
                user.PasswordSalt       = Encryption.GenerateSalt();
                user.PasswordHash       = Encryption.GenerateHash(newPassword, user.PasswordSalt);
                user.PasswordChangeDate = DateTime.Now;
                user.SecurityStamp      = $"{Guid.NewGuid():N}";
                var userUpdateResult = UserRepository.Update(user, user.Id);
                if (userUpdateResult.HasErrors)
                {
                    r.AddErrors(userUpdateResult.Errors);
                    return null;
                }
                return user;
            }
            else
            {
                r.AddError("Old Password doesn't match");
                return null;
            }
        }
        else
        {
            r.AddError("Invalid user account");
            return null;
        }
    }).Result;

    public Result<User?> CreateAccount(User user, long createdById) => Do<User?>.Try(r =>
    {
        var userResult = UserRepository.FindAll(e => e.EmailAddress == user.EmailAddress);
        if (userResult.HasErrors)
        {
            r.AddErrors(userResult.Errors);
            return null;
        }
        var userCount = userResult.ResultObject.Count();

        if (userCount > 0)
        {
            r.AddError($"User already exists with email address {user.EmailAddress}");
            return null;
        }
        user.PasswordSalt   = Encryption.GenerateSalt();
        user.PasswordHash   = Encryption.GenerateHash("1qazxsw2", user.PasswordSalt);
        user.SecurityStamp  = $"{Guid.NewGuid():N}";

        var createResult = UserRepository.Create(user, createdById);
        if (createResult.HasErrors)
        {
            r.AddErrors(createResult.Errors);
            return null;
        }

        return user;
    }).Result;

    public Result<bool> ResetPasswordByEmailLink(string emailAddress, string link, string password) => Do<bool>.Try(r =>
    {
        Expression<Func<AccountRecovery, bool>> filter = w => w.ResetLink           == link
                                                           && w.DeletedOn           == null
                                                           && w.User                != null
                                                           && w.User.EmailAddress   == emailAddress
                                                           && !w.PasswordResetSuccessfully;
        var accountRecoveryResult = AccountRecoveryRepository.FindAll(filter: filter, includeProperties: "User", orderBy: e => e.OrderBy("CreatedOn", "DESC"));
        if (accountRecoveryResult.HasErrors)
        {
            r.AddErrors(accountRecoveryResult.Errors);
            return false;
        }
        var accountRecovery = accountRecoveryResult.ResultObject.FirstOrDefault();
        if (accountRecovery != null)
        {
            if (accountRecovery.ResetLinkSentAt < DateTime.Now.AddHours(-1))
            {
                accountRecovery.PasswordResetSuccessfully = false;
                accountRecovery.ResetLinkExpired = true;
                var accountRecoveryExpireUpdateResult = AccountRecoveryRepository.Update(accountRecovery, accountRecovery.UserId);
                if (accountRecoveryExpireUpdateResult.HasErrors)
                {
                    r.AddErrors(accountRecoveryExpireUpdateResult.Errors);
                    return false;
                }
                r.AddError("Password reset link expired");
                return false;
            }
            
            var passwordSalt = Encryption.GenerateSalt();
            accountRecovery.PasswordResetSuccessfully   = true;
            accountRecovery.RetryCount                  = +1;
            accountRecovery.PasswordResetAt             = DateTime.Now;
            if (accountRecovery.User != null)
            {
                accountRecovery.User.SecurityStamp      = $"{Guid.NewGuid():N}";
                accountRecovery.User.PasswordSalt       = passwordSalt;
                accountRecovery.User.PasswordHash       = Encryption.GenerateHash(password, passwordSalt);
                accountRecovery.User.UpdatedById        = accountRecovery.User.Id;
                accountRecovery.User.UpdatedOn          = DateTime.Now;
                accountRecovery.User.IsAccountActivated = true;
            }

            var accountRecoveryUpdateResult = AccountRecoveryRepository.Update(accountRecovery, accountRecovery.UserId);
            if (accountRecoveryUpdateResult.HasErrors)
            {
                r.AddErrors(accountRecoveryUpdateResult.Errors);
                return false;
            }
            return true;
        }
        else
        {
            r.AddError("Password reset link expired");
            return false;
        }

    }).Result;

    public Result<bool> ValidateEmailLink(string emailAddress, string link) => Do<bool>.Try(r =>
    {
        Expression<Func<AccountRecovery, bool>> filter = w => w.ResetLink       == link
                                                      && w.DeletedOn            == null
                                                      && w.User                 != null
                                                      && w.User.EmailAddress    == emailAddress
                                                      && !w.PasswordResetSuccessfully;

        var accountRecoveryResult = AccountRecoveryRepository.FindAll(filter: filter, e => e.OrderBy("CreatedOn", "DESC"));
        if (accountRecoveryResult.HasErrors)
        {
            r.AddErrors(accountRecoveryResult.Errors);
            return false;
        }
        var accountRecovery = accountRecoveryResult.ResultObject.FirstOrDefault();
        if (accountRecovery != null)
        {
            if(accountRecovery.ResetLinkSentAt <= DateTime.Now.AddHours(-24))
            {
                accountRecovery.PasswordResetSuccessfully   = false;
                accountRecovery.ResetLinkExpired            = true;
                var accountRecoveryExpireUpdateResult = AccountRecoveryRepository.Update(accountRecovery, accountRecovery.UserId);
                if (accountRecoveryExpireUpdateResult.HasErrors)
                {
                    r.AddErrors(accountRecoveryExpireUpdateResult.Errors);
                    return false;
                }
                r.AddError("Password reset link expired");
                return false;
            }
            accountRecovery.RetryCount += 1;
            var accountRecoveryUpdateResult = AccountRecoveryRepository.Update(accountRecovery, accountRecovery.UserId);
            if (accountRecoveryUpdateResult.HasErrors)
            {
                r.AddErrors(accountRecoveryUpdateResult.Errors);
                return false;
            }
            return true;
        }
        else
        {
            r.AddError("Password reset link expired");
            return false;
        }

    }).Result;

    public Result<User?> IsActivationLinkValid(string emailAddress, string link) => Do<User?>.Try(r =>
    {
        var userResult = UserRepository.FindAll(w => w.EmailAddress     == emailAddress
                                                  && w.DeletedOn        == null
                                                  && !w.IsAccountActivated
                                                  && !w.IsActive);
        if (userResult.HasErrors)
        {
            r.AddErrors(userResult.Errors);
            return null;
        }
        
        var user = userResult.ResultObject.FirstOrDefault();
        if (user == null)
        {
            r.AddError("Activation link is not valid");
            return null;
        }
        if (user.CreatedOn <= DateTimeOffset.Now.AddDays(-1))
        {
            r.AddError("Activation link is expired");
            return null;
        }
        var userLink = $"{user.UniqueId}/{user.SecurityStamp}/{user.EmailAddress}";
        if(userLink != link)
        {
            r.AddError("Activation link is not valid");
            return null;
        }
        return user;

    }).Result;

    private void CreateUserLogin(User? user, string emailAddress, bool loginSuccess)
    {
        if(HttpContext.HttpContext == null) return;
        var (ipAddress, operatingSystem, browser, device) = UserAgentConductor.GetUserAgent(HttpContext.HttpContext);

        var userLogin = new UserLogin
        {
            Browser         = browser,
            CreatedById     = user?.Id,
            Device          = device,
            Email           = emailAddress,
            IP              = ipAddress,
            IsLoginSuccess  = loginSuccess,
            IsValidUser     = user != null,
            OperatingSystem = operatingSystem,
            ServerName      = System.Environment.MachineName,
            UserId          = user?.Id
        };
        
        var userLoginCreateResult = UserLoginRepository.Create(userLogin, user?.Id ?? 0);
        if (userLoginCreateResult.HasErrors)
        {
            var message = userLoginCreateResult.GetErrors();
            Logger.LogError("UserLogin history creation failed with Error :{message}", message);
        }
    }

}

