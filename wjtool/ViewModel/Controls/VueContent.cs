using Data.Abstractions.Adapter;
using Data.Abstractions.Entities;
using Data.Abstractions.Helper;
using Data.Abstractions.Judge;
using Data.Adapters.Mysql;
using Data.Adapters.Oracle;
using Data.Adapters.SqlServer;

using HandyControl.Controls;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using wjtool.Data;
using wjtool.UserControl.Base;

namespace wjtool.ViewModel.Controls
{
    public class VueContent : ObservableObject
    {
        public VueContent()
        {
            GlobalData.InitVue();
            _dbType = GlobalData.VueAppConfig?.DbType ?? 0;
            _dbProvider = (DbProvider)(GlobalData.VueAppConfig?.DbType ?? 0);
            _dataConnection = GlobalData.VueAppConfig?.DataConnection ?? "";
            _module = GlobalData.VueAppConfig?.Module ?? "";
            _saveCodePath = GlobalData.VueAppConfig?.SaveCodePath ?? "";

            LoadTableCommand = new AsyncRelayCommand(LoadTableAsync);
            SelectCodePathCommand = new AsyncRelayCommand(SelectCodePathAsync);
            CreateCodeCommand = new AsyncRelayCommand(CreateCodeAsync);
            SaveConfigCommand = new AsyncRelayCommand(SaveConfigAsync);
            LoadConfigCommand = new AsyncRelayCommand(LoadConfigAsync);
        }

        #region 字段

        private int _dbType;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public int DbType
        {
            get => _dbType;
            set => SetProperty(ref _dbType, value);
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        private DbProvider _dbProvider;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbProvider DbProvider
        {
            get => _dbProvider;
            set => SetProperty(ref _dbProvider, value);
        }

        private List<OptionModel> _dbProviderEnum = DbProvider.MySql.ToResult();

        public List<OptionModel> DbProviderEnum
        {
            get => _dbProviderEnum;
            set => SetProperty(ref _dbProviderEnum, value);
        }

        private string _dataConnection;

        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        public string DataConnection
        {
            get => _dataConnection;
            set => SetProperty(ref _dataConnection, value);
        }

        private List<DbTable> _dbTables = null;

        /// <summary>
        /// 表集合
        /// </summary>
        public List<DbTable> DbTables
        {
            get => _dbTables;
            set => SetProperty(ref _dbTables, value);
        }

        private string _tableStr;

        /// <summary>
        /// 表集合
        /// </summary>
        public string TableStr
        {
            get => _tableStr;
            set => SetProperty(ref _tableStr, value);
        }

        private string _module;

        /// <summary>
        /// 模块名称
        /// </summary>
        public string Module
        {
            get => _module;
            set => SetProperty(ref _module, value);
        }

        private string _saveCodePath;

        /// <summary>
        /// 仓储层代码地址
        /// </summary>
        public string SaveCodePath
        {
            get => _saveCodePath;
            set => SetProperty(ref _saveCodePath, value);
        }

        private bool _api = true;

        /// <summary>
        /// 接口服务
        /// </summary>
        public bool Api
        {
            get => _api;
            set => SetProperty(ref _api, value);
        }

        private bool _list = true;

        /// <summary>
        /// 列表
        /// </summary>
        public bool List
        {
            get => _list;
            set => SetProperty(ref _list, value);
        }

        private bool _dto = true;

        /// <summary>
        /// Dto
        /// </summary>
        public bool Dto
        {
            get => _dto;
            set => SetProperty(ref _dto, value);
        }

        private bool _addUpdate = true;

        /// <summary>
        /// 新增
        /// </summary>
        public bool AddUpdate
        {
            get => _addUpdate;
            set => SetProperty(ref _addUpdate, value);
        }

        #endregion 字段

        #region 事件

        public ICommand LoadTableCommand { get; }

        private async Task LoadTableAsync()
        {
            var dialog = Dialog.Show(new ProLoadIngDialog("数据加载中"));
            try
            {
                await Task.Run(async () =>
                {
                    switch (DbProvider)
                    {
                        case DbProvider.SqlServer:
                            DbTables = await new SqlServerDbAdapter().GetDbTables(this.DataConnection);

                            break;

                        case DbProvider.MySql:
                            DbTables = await new MysqlDbAdapter().GetDbTables(this.DataConnection);

                            break;

                        case DbProvider.Oracle:
                            DbTables = await new OracleDbAdapter().GetDbTables(this.DataConnection);

                            break;
                    }
                    DbTables = DbTables.OrderBy(a => a.TableName).ToList();
                });
                dialog.Close();
                Growl.Success("数据加载成功");
            }
            catch (Exception ex)
            {
                dialog.Close();
                Growl.Error($"数据加载失败-{ex.Message}");
            }
            finally
            {
            }
            await Task.CompletedTask;
        }

        public ICommand SelectCodePathCommand { get; }

        private async Task SelectCodePathAsync()
        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
            {
                SaveCodePath = openFileDialog.SelectedPath;
            }
        }

        public ICommand CreateCodeCommand { get; }

        private async Task CreateCodeAsync()
        {
            var dialog = Dialog.Show(new ProLoadIngDialog("代码生成中"));
            try
            {
                if (DbTables == null)
                {
                    Growl.Warning($"请先选择表数据");
                    return;
                }
                var createTables = DbTables.Where(a => a.Selected == true).ToList();
                if (createTables.Count == 0)
                {
                    Growl.Warning($"请先选择生成的表");
                    return;
                }
                await Task.Run(() =>
                {
                    var path = AppContext.BaseDirectory;
                    string id = Module.Split("_")[0];
                    string name = Module.Split("_")[1];
                    ModuleModel moduleModel = new ModuleModel
                    {
                        Id = id,
                        Name = name,
                        PrefixSpace = GlobalData.Config.PrefixSpace
                    };
                    foreach (var item in createTables)
                    {
                        item.ModuleModel = moduleModel;
                        if (item.PrimaryKeysNum < 2)
                        {
                            //if (Api)
                            //{
                            //    string apiBasePath = SaveCodePath +
                            //    @$"\api\{moduleModel.Name.ToLower()}\{EntityJudge.ToLower(item.EntityName)}\";

                            //    //api
                            //    string apiTemplatePath = path + @"\Template\VueTemplate\Api.txt";
                            //    ModuleHelper.CreateFileCommon(apiTemplatePath, $"{apiBasePath}",
                            //        $"index.ts", item);

                            //}

                            if (List)
                            {
                                string listBasePath = SaveCodePath +
                                  @$"\views\{moduleModel.Name.ToLower()}\{EntityJudge.ToLower(item.EntityName)}\";
                                string listTemplatePath = path + @"\Template\VueTemplate\List.txt";
                                ModuleHelper.CreateFileCommon(listTemplatePath, $"{listBasePath}",
                                    $"index.vue", item);
                            }

                            if (Dto)
                            {
                                string listBasePath = SaveCodePath +
                                  @$"\views\{moduleModel.Name.ToLower()}\{EntityJudge.ToLower(item.EntityName)}\dto\";
                                string listTemplatePath = path + @"\Template\VueTemplate\Dto.txt";
                                ModuleHelper.CreateFileCommon(listTemplatePath, $"{listBasePath}",
                                    $"index.ts", item);
                            }

                            if (AddUpdate)
                            {
                                string listBasePath = SaveCodePath +
                                  @$"\views\{moduleModel.Name.ToLower()}\{EntityJudge.ToLower(item.EntityName)}\component\";
                                string listTemplatePath = path + @"\Template\VueTemplate\AddUpdate.txt";
                                ModuleHelper.CreateFileCommon(listTemplatePath, $"{listBasePath}",
                                    $"add.vue", item);
                            }
                        }
                    }
                });
                dialog.Close();
                Growl.Success("生成代码");
            }
            catch (Exception ex)
            {
                dialog.Close();
                Growl.Error($"代码生成失败-{ex.Message}");
            }
            finally
            {
                //dialog.Close();
                //await Task.CompletedTask;
            }
            await Task.CompletedTask;
        }

        public ICommand SaveConfigCommand { get; }

        private async Task SaveConfigAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    VueAppConfig appConfig = new VueAppConfig();
                    appConfig.DbType = DbType;
                    appConfig.DataConnection = DataConnection;
                    appConfig.SaveCodePath = SaveCodePath;
                    appConfig.Module = Module;
                    GlobalData.VueAppConfig = appConfig;
                    GlobalData.SaveVue();
                });
                Growl.Success("配置保存成功");
            }
            catch (Exception ex)
            {
                Growl.Error($"配置保存失败-{ex.Message}");
            }
            finally
            {
            }
        }

        public ICommand LoadConfigCommand { get; }

        private async Task LoadConfigAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    GlobalData.InitVue();
                    DbType = GlobalData.VueAppConfig.DbType;
                    DataConnection = GlobalData.VueAppConfig.DataConnection;
                    SaveCodePath = GlobalData.VueAppConfig.SaveCodePath;
                    Module = GlobalData.VueAppConfig.Module;
                });
                Growl.Success("配置加载成功");
            }
            catch (Exception ex)
            {
                Growl.Error($"配置加载失败-{ex.Message}");
            }
            finally
            {
            }
        }

        #endregion 事件
    }
}