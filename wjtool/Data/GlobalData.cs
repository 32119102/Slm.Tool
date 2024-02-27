using Newtonsoft.Json;

using System.IO;

namespace wjtool.Data
{
    public class GlobalData
    {
        public static void Init()
        {
            if (File.Exists(AppConfig.SavePath))
            {
                try
                {
                    var json = File.ReadAllText(AppConfig.SavePath);
                    Config = (string.IsNullOrEmpty(json) ? new AppConfig() : JsonConvert.DeserializeObject<AppConfig>(json)) ?? new AppConfig();
                }
                catch
                {
                    Config = new AppConfig();
                }
            }
            else
            {
                Config = new AppConfig();
                Save();
            }
        }

        public static void Save()
        {
            var json = JsonConvert.SerializeObject(Config);
            File.WriteAllText(AppConfig.SavePath, json);
        }

        public static AppConfig Config { get; set; }

        public static VueAppConfig VueAppConfig { get; set; }

        public static void InitVue()
        {
            if (File.Exists(VueAppConfig.SavePath))
            {
                try
                {
                    var json = File.ReadAllText(VueAppConfig.SavePath);
                    VueAppConfig = (string.IsNullOrEmpty(json) ? new VueAppConfig() : JsonConvert.DeserializeObject<VueAppConfig>(json)) ?? new VueAppConfig();
                }
                catch
                {
                    VueAppConfig = new VueAppConfig();
                }
            }
            else
            {
                VueAppConfig = new VueAppConfig();
                SaveVue();
            }
        }

        public static void SaveVue()
        {
            var json = JsonConvert.SerializeObject(VueAppConfig);
            File.WriteAllText(VueAppConfig.SavePath, json);
        }

        public static NewModuleConfig NewModuleConfig { get; set; }

        public static void InitNewModule()
        {
            if (File.Exists(NewModuleConfig.SavePath))
            {
                try
                {
                    var json = File.ReadAllText(NewModuleConfig.SavePath);
                    NewModuleConfig = (string.IsNullOrEmpty(json) ? new NewModuleConfig() : JsonConvert.DeserializeObject<NewModuleConfig>(json)) ?? new NewModuleConfig();
                }
                catch
                {
                    NewModuleConfig = new NewModuleConfig();
                }
            }
            else
            {
                NewModuleConfig = new NewModuleConfig();
                SaveNewModule();
            }
        }

        public static void SaveNewModule()
        {
            var json = JsonConvert.SerializeObject(NewModuleConfig);
            File.WriteAllText(NewModuleConfig.SavePath, json);
        }
    }
}