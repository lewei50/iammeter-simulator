namespace HomeSimulator.Web.Helpers;
using System.Reflection;
using System.ComponentModel;
using System.Linq;
using HomeSimulator.Web.Models;
using HomeSimulator.Web.Models.DTO;
using Newtonsoft.Json;
public class CommonHelper
{
    public static string GetNewGuidString()
    {
        var guid = Guid.NewGuid();
        return guid.ToString().Replace("-", "");
    }


    public static decimal GetRandomNumber(decimal? a, decimal? b)
    {
        if (a == null) a = 0;
        if (b == null) b = 0;
        Random r = new Random();
        var n = (decimal)r.NextDouble();
        return (decimal)(a + (b - a) * n);
    }

    public static string GetEnumDescription(Enum value)
    {
        // Get the Description attribute value for the enum value
        FieldInfo fi = value.GetType().GetField(value.ToString());
        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes.Length > 0)
            return attributes[0].Description;
        else
            return value.ToString();
    }

    public static List<T> GetEnumList<T>()
    {
        var pType = typeof(T);
        var Values = Enum.GetValues(pType).Cast<T>().ToList();
        return Values;
    }

    // public static Dictionary<string, T> GetEnumList<T>()
    // {
    //     var pType = typeof(T);
    //     var dict = new Dictionary<string,T>();
    //     string[] Names = Enum.GetNames(pType);
    //     int[] Values = Enum.GetValues(pType) as int[];
    //     var list = from p in Names
    //                from o in Values
    //                where Values[Array.IndexOf(Names, p)] == o
    //                select new
    //                {
    //                    value = p,
    //                    key = o.ToString()
    //                };
    //     foreach (var item in list)
    //     {
    //         var vv= GetEnumDescription(int.Parse(item.key) as T);
    //         dict.Add(item.key,item.value);
    //     }
    //     return dict;
    // }
}

