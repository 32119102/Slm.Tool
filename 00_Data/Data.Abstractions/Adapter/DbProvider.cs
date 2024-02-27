using System.ComponentModel;

namespace Data.Abstractions.Adapter
{
    public enum DbProvider
    {
        [Description("SqlServer")]
        SqlServer = 0,

        [Description("MySql")]
        MySql = 1,

        [Description("Oracle")]
        Oracle = 2
    }
}