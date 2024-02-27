using HandyControl.Data;

using System;

namespace wjtool.Data
{
    public class VueAppConfig
    {
        public static readonly string SavePath = $"{AppDomain.CurrentDomain.BaseDirectory}VueAppConfig.json";

        public string Lang { get; set; } = "zh-cn";

        public SkinType Skin { get; set; }

        /// <summary>
        /// 数据库类型
        ///SqlServer = 0,
		///MySql = 1,
		///Oracle = 22
        /// </summary>
        public int DbType { get; set; } = 2;

        /// <summary>
        /// 数据库链接地址
        /// </summary>
        public string DataConnection { get; set; } = "Data Source=192.168.1.136:1521/ORCL;User ID=CZGJLGCZNEW2;Password=CZGJLGCZNEW2;Pooling = true;Min Pool Size = 1;Max Pool Size = 75;Connection Lifetime = 260;";

        /// <summary>
        /// 模块化地址
        /// </summary>
        public string Module { get; set; } = "00_Sys";

        /// <summary>
        /// 仓储层地址
        /// </summary>
        public string SaveCodePath { get; set; } = @"";

        /// <summary>
        /// 命名空间前缀
        /// </summary>
        public string PrefixSpace { get; set; } = "SL.Mkh";
    }
}