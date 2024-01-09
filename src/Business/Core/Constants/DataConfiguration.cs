namespace XploringMe.Core.Constants;
public class StaticConfiguration
{
    public const int MOBILE_LENGTH          = 15;
    public const int NAME_LENGTH            = 28;
    public const int EMAIL_LENGTH           = 128;
    public const int PASSWORD_MAX_LENGTH    = 16;
    public const int PASSWORD_MIN_LENGTH    = 8;
    public const int COMMAN_LENGTH          = 256;
    public const int PINCODE_LENGTH         = 6;
    public const int COLOR_HEX_LENGTH       = 7;
    public const int MAX_LENGTH             = 5000;
    public const int DECIMAL_PRECISION      = 38;
    public const int DECIMAL_SCALE          = 0;
    public const int DECIMAL_SCALE_2        = 2;
    public const int DECIMAL_SCALE_3        = 3;
    public const int DECIMAL_SCALE_4        = 4;
}

public static class ClaimTypeConstant
{
    public const string MemberId            = "memberId";
    public const string HasFamilyAccess     = "hasfamilyaccess";
    public const string HasFinanceAccess    = "hasfinanceaccess";
}