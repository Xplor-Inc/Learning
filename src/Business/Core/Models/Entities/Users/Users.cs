﻿using XploringMe.Core.Enumerations;

namespace XploringMe.Core.Models.Entities.Users;
public class User : Auditable
{
    #region Properties
    public DateTimeOffset?  AccountActivateDate     { get; set; }
    public bool             ActivationEmailSent     { get; set; }
    public string           EmailAddress            { get; set; } = string.Empty;
    public string           FirstName               { get; set; } = string.Empty;
    public Gender           Gender                  { get; set; }
    public bool             HasFamilyAccess         { get; set; }
    public bool             HasFinanceAccess        { get; set; }
    public string           ImagePath               { get; set; } = string.Empty;
    public bool             IsAccountActivated      { get; set; }
    public bool             IsActive                { get; set; }
    public DateTimeOffset?  LastLoginDate           { get; set; }
    public string           LastName                { get; set; } = string.Empty;
    public long?            FamilyMemberId          { get; set; }
    public DateTimeOffset?  PasswordChangeDate      { get; set; }
    public string           PasswordHash            { get; set; } = string.Empty;
    public string           PasswordSalt            { get; set; } = string.Empty;
    public UserRole         Role                    { get; set; }
    public string           SecurityStamp           { get; set; } = string.Empty;

    #endregion Properties

    #region Virtual Users

    public virtual List<User>?       UsersCreatedBy          { get; set; }
    public virtual List<User>?       UsersDeletedBy          { get; set; }
    public virtual List<User>?       UsersUpdatedBy          { get; set; }
    #endregion

}