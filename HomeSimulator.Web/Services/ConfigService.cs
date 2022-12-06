namespace HomeSimulator.Services;
using HomeSimulator.Web.Models;
using HomeSimulator.Web.Models.DTO;
using HomeSimulator.Web.Models.Entities;
using HomeSimulator.Web.Extensions;
public class ConfigService
{
    private readonly MyContext _myContext;

    public ConfigService(MyContext myContext)
    {
        _myContext = myContext;
    }

    public Config GetConfig()
    {
        return _myContext.Configs.OrderBy(o => o.Id).First();
    }

    public bool ValidateUser(string username, string password)
    {
        var config = GetConfig();
        var pwd = password.ToSHA256String();
        return string.Equals(username, config.Username, StringComparison.CurrentCultureIgnoreCase) && config.Password == pwd;
    }


}