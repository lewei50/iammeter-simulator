namespace HomeSimulator.Web.Models;
using System.ComponentModel;

public enum SourceTypeEnums
{
    Simulation = 0,
    [Description("IAMMETER Cloud")]
    IAMMETERCloud = 1,
    [Description("IAMMETER Local")]
    IAMMETERLocal = 2
}

public enum LocationTypeEnums
{
    [Description("Northern Hemisphere")]
    Northern = 0,
    [Description("Southern Hemisphere")]
    Southern = 1
}

public enum RunModeEnums
{
    Manual = 0,
    Timing = 1
}

public enum SetModeEnums
{
    Cannot = 0,
    Configurable = 1
}
