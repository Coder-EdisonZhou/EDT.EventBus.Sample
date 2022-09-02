using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Sockets;

namespace EDT.MSA.Stocking.API.Controllers
{
    [Route("api/Health")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok($"ok");

        [HttpGet("node")]
        public IActionResult GetNodeInfo()
        {
            var result = string.Empty;
            var HostName = Dns.GetHostName();
            var IpEntry = Dns.GetHostEntry(HostName);
            for (int i = 0; i < IpEntry.AddressList.Length; i++)
            {
                // 从IP地址列表中筛选出IPv4类型的IP地址
                if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    result = IpEntry.AddressList[i].ToString();
                }
            }

            return Ok($"Current Node: {result}");
        }
    }
}