using HandyControl.Data;

using System;

namespace wjtool.Data
{
    public class NewModuleConfig
    {
        public static readonly string SavePath = $"{AppDomain.CurrentDomain.BaseDirectory}NewModuleConfig.json";

        public string Lang { get; set; } = "zh-cn";

        public SkinType Skin { get; set; }

        /// <summary>
        /// 模块化地址
        /// </summary>
        public string Module { get; set; } = "00_Sys";

        /// <summary>
        /// 仓储层地址
        /// </summary>
        public string SaveCodePath { get; set; } = @"";
    }
}