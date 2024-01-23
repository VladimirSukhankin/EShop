using EShopFanerum.AuthService.Logic;
using Microsoft.Extensions.Caching.Memory;

namespace EShopFanerum.AuthService.Helpers.Extensions;

/// <summary>
/// Класс содержащий методы расширения для проверка ip адреса из запроса.
/// </summary>
public static class RequestExtension
{
    /// <summary>
    /// Проверка ip.
    /// </summary>
    public static bool CheckIp(this HttpRequest httpRequest, IMemoryCache cache)
    {
        var ip = httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString();
        if (ip is not { Length: > 0 }) return false;
        var dt = DateTime.Now;

        if (!cache.TryGetValue(Consts.IpKeys, out Dictionary<string, List<DateTime>>? ips) || ips == null)
        {
            cache.Set(Consts.IpKeys, new Dictionary<string, List<DateTime>>
            {
                {
                    ip,
                    new List<DateTime> { dt }
                }
            });
        }
        else
        {
            try
            {
                var tddt = dt.AddMinutes(-30);
                var toDel = (from td in ips where td.Value.Max(x => x) < tddt select td.Key).ToList();
                if (!toDel.Any())
                {
                    foreach (var td in toDel) ips.Remove(td);
                }
            }
            catch
            {
                // ignored
            }

            if (ips.TryGetValue(ip, out var dates))
            {
                dates.Add(dt);
                dates = dates.Where(x => (dt - x).TotalMinutes < 20).ToList();
                ips[ip] = dates;
                if (dates.Count > 100) return false;
            }
            else ips.Add(ip, new List<DateTime> { dt });
        }
        return true;
    }
}