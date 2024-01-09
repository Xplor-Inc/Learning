namespace XploringMe.BlazorApp.Models.Dtos.Users;
public class UserDto : AuditableDto
{
    #region Properties
    public DateTimeOffset?  AccountActivateDate     { get; set; }
    public string           EmailAddress            { get; set; } = default!;
    public string           FirstName               { get; set; } = default!;
    public bool             HasFamilyAccess         { get; set; }
    public bool             HasFinanceAccess        { get; set; }
    public Gender           Gender                  { get; set; }
    public bool             IsActive                { get; set; }
    public DateTimeOffset?  LastLoginDate           { get; set; }
    public string           LastName                { get; set; } = default!;
    public UserRole         Role                    { get; set; }
    public string?          ImagePath               { get; set; } = default!;
    public long?            FamilyMemberId          { get; set; } 
    #endregion Properties
}