using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.IO;
using XploringMe.Core.Enumerations.Finance;
using XploringMe.Core.Interfaces.Utility.Security;

namespace XploringMe.SqlServer;
public static class XploringMeExtensions
{
    public static void AddInitialData(this XploringMeContext context, IEncryption encryption, IWebHostEnvironment environment)
    {
        context.SeedUsers(encryption);
        context.SeedCategories();
        //  context.SeedInvestmentData();
        if(environment.IsDevelopment())
        {
            context.SeedAccounts();
            context.SeedTagCategories();
        }
    }

    private static void SeedUsers(this XploringMeContext context, IEncryption encryption)
    {
        if (!context.Users.Any())
        {
            var salt = encryption.GenerateSalt();
            var user = new User
            {
                AccountActivateDate = DateTimeOffset.Now,
                CreatedById = 1,
                CreatedOn = DateTimeOffset.Now,
                EmailAddress = "test@app.com",
                FirstName = "Admin",
                ImagePath = "no-image.jpg",
                LastName = "User",
                IsAccountActivated = true,
                IsActive = true,
                PasswordHash = encryption.GenerateHash("1qazxsw2", salt),
                PasswordSalt = salt,
                Role = UserRole.Admin,
                SecurityStamp = $"{Guid.NewGuid():N}",
                UniqueId = Guid.NewGuid()
            };
            context.Users.Add(user);
            context.SaveChanges();
        }
    }

    private static void SeedAccounts(this XploringMeContext context)
    {
        var accounts = context.TransactionAccounts.ToList();
        if (accounts.Count > 0)
        {
            accounts.ForEach(e => e.Id = 0);
            var x = JsonConvert.SerializeObject(accounts);
            File.WriteAllText("Data\\TransactionAccounts.json", x);
        }
        else if(File.Exists("Data\\TransactionAccounts.json"))
        {            
            var x = File.ReadAllText("Data\\TransactionAccounts.json");
            var accountX = JsonConvert.DeserializeObject<List<TransactionAccount>>(x);
            context.AddRange(accountX);
            context.SaveChanges();
        }
    }

    private static void SeedTagCategories(this XploringMeContext context)
    {
        var categories = context.Categories.Where(e => !e.DeletedOn.HasValue).ToList();
        if (categories.Count > 0)
        {
            categories.ForEach(e => e.Id = 0);
            var x = JsonConvert.SerializeObject(categories);
            File.WriteAllText("Data\\Categories.json", x);
        }
        else if (File.Exists("Data\\Categories.json"))
        {
            var x = File.ReadAllText("Data\\Categories.json");
            var catsX = JsonConvert.DeserializeObject<List<Category>>(x);
            context.AddRange(catsX);
            context.SaveChanges();
        }
    }
    private static void SeedCategories(this XploringMeContext context)
    {
        if (context.Categories.Any()) return;

        List<Category> categories =
        [
            new()
            {
                Name        = "Transactions",
                Color       = "#008000",
                Description = "Transactions",
                IsActive    = true,
                Type        = CategoryType.Category,
                UniqueId    = Guid.NewGuid(),
                CreatedById = 1
            },
            new()
            {
                Name        = "Interest",
                Color       = "#008000",
                Description = "Interest",
                IsActive    = true,
                Type        = CategoryType.Category,
                UniqueId    = Guid.NewGuid(),
                CreatedById = 1
            },
            new()
            {
                Name        = "Spiritual",
                Color       = "#008000",
                Description = "Spiritual",
                IsActive    = true,
                Type        = CategoryType.Category,
                UniqueId    = Guid.NewGuid(),
                CreatedById = 1
            },
            new()
            {
                Name        = "Investment",
                Color       = "#008000",
                Description = "Investment",
                IsActive    = true,
                Type        = CategoryType.Category,
                UniqueId    = Guid.NewGuid(),
                CreatedById = 1
            },
        ];
        context.AddRange(categories);
        context.SaveChanges();
    }
    //private static void SeedRecurringPayments(this XploringMeContext context)
    //{
    //    if (!context.RecurringBills.Any(e => e.BillName == "BSNL Internet"))
    //    {
    //        DateTimeOffset date = DateTimeOffset.Now;

    //        var bsnl = new RecurringBill
    //        {
    //            CreatedById = 1,
    //            CreatedOn   = DateTimeOffset.Now,
    //            UniqueId    = Guid.NewGuid(),
    //            Amount      = 825,
    //            AutoDebit   = false,
    //            BillName    = "BSNL Internet",
    //            Paid        = false,
    //            PaymentDate = date
    //        };
    //        var dth = new RecurringBill
    //        {
    //            CreatedById = 1,
    //            CreatedOn   = DateTimeOffset.Now,
    //            UniqueId    = Guid.NewGuid(),
    //            Amount      = 300,
    //            AutoDebit   = false,
    //            BillName    = "Airtel DTH",
    //            Paid        = false,
    //            PaymentDate = date
    //        };

    //        var mobile_H_Jio = new RecurringBill
    //        {
    //            CreatedById = 1,
    //            CreatedOn   = DateTimeOffset.Now,
    //            UniqueId    = Guid.NewGuid(),
    //            Amount      = 666,
    //            AutoDebit   = false,
    //            BillName    = "JIO 9024236927",
    //            Paid        = false,
    //            PaymentDate = date
    //        };
    //        var mobile_H_VI = new RecurringBill
    //        {
    //            CreatedById = 1,
    //            CreatedOn   = DateTimeOffset.Now,
    //            UniqueId    = Guid.NewGuid(),
    //            Amount      = 339,
    //            AutoDebit   = false,
    //            BillName    = "VI 9024430863",
    //            Paid        = false,
    //            PaymentDate = date
    //        };
    //        var mobile_P = new RecurringBill
    //        {
    //            CreatedById = 1,
    //            CreatedOn   = DateTimeOffset.Now,
    //            UniqueId    = Guid.NewGuid(),
    //            Amount      = 666,
    //            AutoDebit   = false,
    //            BillName    = "JIO 8104289451",
    //            Paid        = false,
    //            PaymentDate = date
    //        };

    //        var LIC_H_Monthly = new RecurringBill
    //        {
    //            CreatedById = 1,
    //            CreatedOn   = DateTimeOffset.Now,
    //            UniqueId    = Guid.NewGuid(),
    //            Amount      = 5860,
    //            AutoDebit   = false,
    //            BillName    = "LIC_H Monthly",
    //            Paid        = false,
    //            PaymentDate = date
    //        };
    //        var LIC_H_Quartly = new RecurringBill
    //        {
    //            CreatedById = 1,
    //            CreatedOn   = DateTimeOffset.Now,
    //            UniqueId    = Guid.NewGuid(),
    //            Amount      = 4990,
    //            AutoDebit   = false,
    //            BillName    = "LIC_H Quartly",
    //            Paid        = false,
    //            PaymentDate = date
    //        };
    //        var LIC_D_HalfYear = new RecurringBill
    //        {
    //            CreatedById = 1,
    //            CreatedOn   = DateTimeOffset.Now,
    //            UniqueId    = Guid.NewGuid(),
    //            Amount      = 25010,
    //            AutoDebit   = false,
    //            BillName    = "LIC_D Half Yearly",
    //            Paid        = false,
    //            PaymentDate = date
    //        };

    //        var bills = new List<RecurringBill> { bsnl, dth, mobile_H_Jio, mobile_H_VI, mobile_P, LIC_D_HalfYear, LIC_H_Monthly, LIC_H_Quartly };
    //        context.AddRange(bills);
    //        context.SaveChanges();
    //    }
    //}

    //private static void SeedInvestmentData(this XploringMeContext context)
    //{
    //    if (!context.Scrips.Any())
    //    {
    //        var st = System.IO.File.ReadAllText("scrips.json");
    //        var scrips = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Scrip>>(st);
    //        scrips.ForEach(e => { e.CreatedById = 1; e.CreatedOn = DateTimeOffset.Now;e.UniqueId = Guid.NewGuid(); });
    //        context.AddRange(scrips);
    //        context.SaveChanges();
    //    }
    //    if (!context.DematAccounts.Any())
    //    {
    //        context.Add(new DematAccount
    //        {
    //            AccountHolder   = "Hoshiyar Singh",
    //            ClientCode      = "ShareKhan",
    //            ContactNo       = "9024236927",
    //            CreatedById     = 1,
    //            AccountNo       = "ShareKhan",
    //            BrockerName     = "ShareKhan",
    //            DPId            = "ShareKhan",
    //            EmailId         = "hoshiyar.singh@live.com",
    //            TPIN            = 257384,
    //            CreatedOn       = DateTimeOffset.Now,
    //            UniqueId        = Guid.NewGuid()
    //        });
    //        context.Add(new DematAccount
    //        {
    //            AccountHolder   = "Hoshiyar Singh",
    //            ClientCode      = "8282887680",
    //            ContactNo       = "9024236927",
    //            CreatedById     = 1,
    //            AccountNo       = "1208870134852913",
    //            BrockerName     = "Groww",
    //            DPId            = "12088701",
    //            EmailId         = "sd23my@gmail.com",
    //            TPIN            = 884447,
    //            CreatedOn       = DateTimeOffset.Now,
    //            UniqueId        = Guid.NewGuid()
    //        });
    //        context.Add(new DematAccount
    //        {
    //            AccountHolder   = "Pinky Saini",
    //            ClientCode      = "Upstock",
    //            ContactNo       = "9024430863",
    //            CreatedById     = 1,
    //            AccountNo       = "Upstock",
    //            BrockerName     = "Upstock",
    //            DPId            = "Upstock",
    //            EmailId         = "info@xploring.me",
    //            TPIN            = 123,
    //            CreatedOn       = DateTimeOffset.Now,
    //            UniqueId        = Guid.NewGuid()
    //        });
    //        context.Add(new DematAccount
    //        {
    //            AccountHolder   = "Pinky Saini",
    //            ClientCode      = "4113764200",
    //            ContactNo       = "9024430863",
    //            CreatedById     = 1,
    //            AccountNo       = "1208870166236221",
    //            BrockerName     = "Groww",
    //            DPId            = "Groww",
    //            EmailId         = "info@xploring.me",
    //            TPIN            = 880180,
    //            CreatedOn       = DateTimeOffset.Now,
    //            UniqueId        = Guid.NewGuid()
    //        });
    //        context.SaveChanges();
    //    }

    //    if(!context.Stocks.Any())
    //    {
    //        var st = System.IO.File.ReadAllText("stocks.json");
    //        var stocks = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Stock>>(st);
    //        stocks.ForEach(e => { e.CreatedById = 1; e.CreatedOn = DateTimeOffset.Now; e.UniqueId = Guid.NewGuid(); });
    //        context.AddRange(stocks);
    //        context.SaveChanges();
    //    }
    //}
}