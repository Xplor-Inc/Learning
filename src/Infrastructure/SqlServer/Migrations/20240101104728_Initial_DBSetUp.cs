using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XploringMe.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Initial_DBSetUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountActivateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ActivationEmailSent = table.Column<bool>(type: "bit", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    HasFamilyAccess = table.Column<bool>(type: "bit", nullable: false),
                    HasFinanceAccess = table.Column<bool>(type: "bit", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsAccountActivated = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    FamilyMemberId = table.Column<long>(type: "bigint", nullable: true),
                    PasswordChangeDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedById = table.Column<long>(type: "bigint", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountRecoveries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailSent = table.Column<bool>(type: "bit", nullable: false),
                    PasswordResetAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PasswordResetSuccessfully = table.Column<bool>(type: "bit", nullable: false),
                    ResetLink = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ResetLinkExpired = table.Column<bool>(type: "bit", nullable: false),
                    ResetLinkSentAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedById = table.Column<long>(type: "bigint", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRecoveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountRecoveries_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountRecoveries_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountRecoveries_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountRecoveries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChangeLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    OldValue = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    PrimaryKey = table.Column<long>(type: "bigint", nullable: false),
                    PropertyName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeLogs_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Counters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Browser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    Device = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LastVisit = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    OperatingSystem = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Page = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Search = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    VisitorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResumeDownloads = table.Column<int>(type: "int", nullable: false),
                    EnquirySent = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Counters_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Enquiries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    Resolution = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    VisitorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedById = table.Column<long>(type: "bigint", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enquiries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enquiries_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enquiries_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enquiries_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Finance_Budgets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Actual = table.Column<decimal>(type: "decimal(38,2)", precision: 38, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Expected = table.Column<decimal>(type: "decimal(38,2)", precision: 38, scale: 2, nullable: false),
                    Month = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedById = table.Column<long>(type: "bigint", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finance_Budgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Finance_Budgets_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_Budgets_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_Budgets_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Finance_Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedById = table.Column<long>(type: "bigint", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finance_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Finance_Categories_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_Categories_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_Categories_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Finance_TransactionAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(38,2)", precision: 38, scale: 2, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningBalance = table.Column<decimal>(type: "decimal(38,2)", precision: 38, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PaymentDueDay = table.Column<int>(type: "int", nullable: false),
                    StatementDay = table.Column<int>(type: "int", nullable: false),
                    IFSCCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UPIId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebitCardNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebitCardPIN = table.Column<int>(type: "int", nullable: true),
                    DebitCardCVV = table.Column<int>(type: "int", nullable: true),
                    DebitCardExpireDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    MobileBankingPIN = table.Column<int>(type: "int", nullable: true),
                    UPIPIN = table.Column<int>(type: "int", nullable: true),
                    NetBankingURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetBankingUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetBankingPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetBankingTransPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedById = table.Column<long>(type: "bigint", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finance_TransactionAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Finance_TransactionAccounts_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_TransactionAccounts_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_TransactionAccounts_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResumeDownloads",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    VisitorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeDownloads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeDownloads_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Browser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    Device = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IP = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsLoginSuccess = table.Column<bool>(type: "bit", nullable: false),
                    IsValidUser = table.Column<bool>(type: "bit", nullable: false),
                    OperatingSystem = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ServerName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Finance_RecurringBills",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(38,2)", precision: 38, scale: 2, nullable: false),
                    BillName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AccountNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Paid = table.Column<bool>(type: "bit", nullable: false),
                    AutoDebit = table.Column<bool>(type: "bit", nullable: false),
                    DebitAccountId = table.Column<long>(type: "bigint", nullable: true),
                    NextPaymentDays = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedById = table.Column<long>(type: "bigint", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finance_RecurringBills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Finance_RecurringBills_Finance_TransactionAccounts_DebitAccountId",
                        column: x => x.DebitAccountId,
                        principalTable: "Finance_TransactionAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_RecurringBills_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_RecurringBills_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_RecurringBills_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Finance_Transactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Credit = table.Column<decimal>(type: "decimal(38,2)", precision: 38, scale: 2, nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(38,2)", precision: 38, scale: 2, nullable: false),
                    TransactionAccountId = table.Column<long>(type: "bigint", nullable: false),
                    CreditAccountId = table.Column<long>(type: "bigint", nullable: true),
                    DebitAccountId = table.Column<long>(type: "bigint", nullable: true),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "decimal(38,2)", precision: 38, scale: 2, nullable: false),
                    InvoicePath = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    IsRefunded = table.Column<bool>(type: "bit", nullable: false),
                    Particular = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PaymentDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PreviousBalance = table.Column<decimal>(type: "decimal(38,2)", precision: 38, scale: 2, nullable: false),
                    TransactionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DebtStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedById = table.Column<long>(type: "bigint", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finance_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Finance_Transactions_Finance_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Finance_Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_Transactions_Finance_TransactionAccounts_CreditAccountId",
                        column: x => x.CreditAccountId,
                        principalTable: "Finance_TransactionAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_Transactions_Finance_TransactionAccounts_DebitAccountId",
                        column: x => x.DebitAccountId,
                        principalTable: "Finance_TransactionAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_Transactions_Finance_TransactionAccounts_TransactionAccountId",
                        column: x => x.TransactionAccountId,
                        principalTable: "Finance_TransactionAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_Transactions_Finance_Transactions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Finance_Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_Transactions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_Transactions_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_Transactions_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Finance_RefundHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Total = table.Column<decimal>(type: "decimal(38,2)", precision: 38, scale: 2, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(38,2)", precision: 38, scale: 2, nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Refund = table.Column<decimal>(type: "decimal(38,2)", precision: 38, scale: 2, nullable: false),
                    RefundAccountId = table.Column<long>(type: "bigint", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TransactionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finance_RefundHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Finance_RefundHistories_Finance_TransactionAccounts_RefundAccountId",
                        column: x => x.RefundAccountId,
                        principalTable: "Finance_TransactionAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_RefundHistories_Finance_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Finance_Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_RefundHistories_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Finance_Taggings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagId = table.Column<long>(type: "bigint", nullable: false),
                    TransactionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedById = table.Column<long>(type: "bigint", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finance_Taggings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Finance_Taggings_Finance_Categories_TagId",
                        column: x => x.TagId,
                        principalTable: "Finance_Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_Taggings_Finance_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Finance_Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_Taggings_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_Taggings_Users_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Finance_Taggings_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountRecoveries_CreatedById",
                table: "AccountRecoveries",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRecoveries_DeletedById",
                table: "AccountRecoveries",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRecoveries_UpdatedById",
                table: "AccountRecoveries",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRecoveries_UserId",
                table: "AccountRecoveries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_CreatedById",
                table: "ChangeLogs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Counters_CreatedById",
                table: "Counters",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Enquiries_CreatedById",
                table: "Enquiries",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Enquiries_DeletedById",
                table: "Enquiries",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Enquiries_UpdatedById",
                table: "Enquiries",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Budgets_CreatedById",
                table: "Finance_Budgets",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Budgets_DeletedById",
                table: "Finance_Budgets",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Budgets_UpdatedById",
                table: "Finance_Budgets",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Categories_CreatedById",
                table: "Finance_Categories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Categories_DeletedById",
                table: "Finance_Categories",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Categories_UpdatedById",
                table: "Finance_Categories",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_RecurringBills_CreatedById",
                table: "Finance_RecurringBills",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_RecurringBills_DebitAccountId",
                table: "Finance_RecurringBills",
                column: "DebitAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_RecurringBills_DeletedById",
                table: "Finance_RecurringBills",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_RecurringBills_UpdatedById",
                table: "Finance_RecurringBills",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_RefundHistories_CreatedById",
                table: "Finance_RefundHistories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_RefundHistories_RefundAccountId",
                table: "Finance_RefundHistories",
                column: "RefundAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_RefundHistories_TransactionId",
                table: "Finance_RefundHistories",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Taggings_CreatedById",
                table: "Finance_Taggings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Taggings_DeletedById",
                table: "Finance_Taggings",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Taggings_TagId",
                table: "Finance_Taggings",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Taggings_TransactionId",
                table: "Finance_Taggings",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Taggings_UpdatedById",
                table: "Finance_Taggings",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_TransactionAccounts_CreatedById",
                table: "Finance_TransactionAccounts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_TransactionAccounts_DeletedById",
                table: "Finance_TransactionAccounts",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_TransactionAccounts_UpdatedById",
                table: "Finance_TransactionAccounts",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Transactions_CategoryId",
                table: "Finance_Transactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Transactions_CreatedById",
                table: "Finance_Transactions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Transactions_CreditAccountId",
                table: "Finance_Transactions",
                column: "CreditAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Transactions_DebitAccountId",
                table: "Finance_Transactions",
                column: "DebitAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Transactions_DeletedById",
                table: "Finance_Transactions",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Transactions_ParentId",
                table: "Finance_Transactions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Transactions_TransactionAccountId",
                table: "Finance_Transactions",
                column: "TransactionAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Transactions_UpdatedById",
                table: "Finance_Transactions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeDownloads_CreatedById",
                table: "ResumeDownloads",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_CreatedById",
                table: "UserLogins",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedById",
                table: "Users",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DeletedById",
                table: "Users",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailAddress_DeletedOn",
                table: "Users",
                columns: new[] { "EmailAddress", "DeletedOn" },
                unique: true,
                filter: "[DeletedOn] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedById",
                table: "Users",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountRecoveries");

            migrationBuilder.DropTable(
                name: "ChangeLogs");

            migrationBuilder.DropTable(
                name: "Counters");

            migrationBuilder.DropTable(
                name: "Enquiries");

            migrationBuilder.DropTable(
                name: "Finance_Budgets");

            migrationBuilder.DropTable(
                name: "Finance_RecurringBills");

            migrationBuilder.DropTable(
                name: "Finance_RefundHistories");

            migrationBuilder.DropTable(
                name: "Finance_Taggings");

            migrationBuilder.DropTable(
                name: "ResumeDownloads");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "Finance_Transactions");

            migrationBuilder.DropTable(
                name: "Finance_Categories");

            migrationBuilder.DropTable(
                name: "Finance_TransactionAccounts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
