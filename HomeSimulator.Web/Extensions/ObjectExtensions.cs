using HomeSimulator.Web.Models;
namespace HomeSimulator.Web.Extensions;
using Microsoft.EntityFrameworkCore;

public static class OjbectExtensions
{

    public static Decimal? ToNullDecimal(this object s)
    {
        if (s == null) return null;
        if (string.IsNullOrEmpty(s.ToString()))
            return null;
        Decimal? r = null;
        try
        {
            r = Decimal.Parse($"{s}");
        }
        catch { }
        return r;
    }

    public static string ToSHA256String(this string s)
    {
        if (String.IsNullOrEmpty(s)) return string.Empty;
        using (var sha = System.Security.Cryptography.SHA256.Create())
        {
            var enc = System.Text.Encoding.UTF8;
            var bytes = enc.GetBytes(s);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    public static int TodaySeconds(this DateTime t)
    {
        var date = t.Date;
        return (int)(t - date).TotalSeconds;
    }

    public static decimal? ToDecimalNum(this object o, int count = 2)
    {
        return o.ToDecimalString(count).ToNullDecimal();
    }
    public static string ToDecimalString(this object o, int dCount = 2, bool hasDot = true)
    {
        var v = o.ToNullDecimal();
        var leftStr = "{0:";
        if (hasDot == true) leftStr = "{0:#,##";
        string rr = string.Format("{0}", v);
        switch (dCount)
        {
            case 2:
                rr = string.Format(leftStr + "0.##}", v);
                break;
            case 1:
                rr = string.Format(leftStr + "0.#}", v);
                break;
            case 3:
                rr = string.Format(leftStr + "0.###}", v);
                break;
            case 4:
                rr = string.Format(leftStr + "0.####}", v);
                break;
            case 0:
                rr = string.Format(leftStr + "0}", v);
                break;
            default:

                break;

        }
        return rr;
    }

    public static string GetUserIp(this HttpContext context)
    {
        var ip = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        if (string.IsNullOrEmpty(ip))
        {
            ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = $"{context.Connection.RemoteIpAddress}";
            }
        }
        return ip;
    }
}

