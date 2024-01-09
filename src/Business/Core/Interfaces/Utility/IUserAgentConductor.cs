using Microsoft.AspNetCore.Http;

namespace XploringMe.Core.Interfaces.Utility;

public interface IUserAgentConductor
{
    (string IpAddress, string OperatingSystem, string Browser, string Device) GetUserAgent(HttpContext context);
}
