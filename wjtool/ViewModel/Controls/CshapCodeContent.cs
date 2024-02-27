using Data.Abstractions.Adapter;
using Data.Abstractions.Entities;
using Data.Abstractions.Helper;
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
    public class CshapCodeContent : ObservableObject
    {
        public CshapCodeContent()
        {
            GlobalData.Init();
            _dbType = GlobalData.Config?.DbType ?? 0;
            _dbProvider = (DbProvider)(GlobalData.Config?.DbType ?? 0);
            _dataConnection = GlobalData.Config?.DataConnection ?? "";
            _module = GlobalData.Config?.Module ?? "";
            _saveCodePath = GlobalData.Config?.SaveCodePath ?? "";
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

        private bool _iservice = true;

        /// <summary>
        /// 接口服务
        /// </summary>
        public bool IService
        {
            get => _iservice;
            set => SetProperty(ref _iservice, value);
        }

        private bool _service = true;

        /// <summary>
        /// 服务
        /// </summary>
        public bool Service
        {
            get => _service;
            set => SetProperty(ref _service, value);
        }

        private bool _irepository = true;

        /// <summary>
        /// 领域
        /// </summary>
        public bool IRepository
        {
            get => _irepository;
            set => SetProperty(ref _irepository, value);
        }

        private bool _repository = true;

        /// <summary>
        /// 仓储
        /// </summary>
        public bool Repository
        {
            get => _repository;
            set => SetProperty(ref _repository, value);
        }

        private bool _controller = true;

        /// <summary>
        /// 控制器
        /// </summary>
        public bool Controller
        {
            get => _controller;
            set => SetProperty(ref _controller, value);
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
            //finally
            //{
            //    await Task.CompletedTask;
            //}
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
                List<DbTable>? createTables = DbTables.Where(a => a.Selected == true).OrderBy(a => a.TableName).ToList();
                var sql = new OracleDbAdapter();
                foreach (var item in createTables)
                {
                    item.DbColumns = await sql.GetDbColumns(this.DataConnection,item.TableName,item); 
                    
                }


                if (createTables.Count == 0)
                {
                    dialog.Close();
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

                        if (IRepository)
                        {
                            string domainBasePath = SaveCodePath + @$"\{Module}\{moduleModel.Name}.Domain\";

                            //Entity
                            string entityTemplatePath = path + @"\Template\CshapCodeTemplate\Domain\Entity.txt";
                            ModuleHelper.CreateFileCommon(entityTemplatePath, $"{domainBasePath}{item.EntityName}",
                                $"{item.EntityName}Entity.cs", item);

                            //Entity.Extend
                            string entityExtendTemplatePath = path + @"\Template\CshapCodeTemplate\Domain\Entity.Extend.txt";
                            ModuleHelper.CreateFileCommon(entityExtendTemplatePath, $"{domainBasePath}{item.EntityName}",
                              $"{item.EntityName}Entity.Extend.cs", item);

                            //IRepository
                            string irepositoryTemplatePath = path + @"\Template\CshapCodeTemplate\Domain\IRepository.txt";
                            ModuleHelper.CreateFileCommon(irepositoryTemplatePath, $"{domainBasePath}{item.EntityName}",
                              $"I{item.EntityName}Repository.cs", item);
                        }
                        if (Repository)
                        {
                            string domainBasePath = SaveCodePath + @$"\{Module}\{moduleModel.Name}.Domain.Sqlsugar\";
                            //Repository
                            string repositoryTemplatePath = path + @"\Template\CshapCodeTemplate\Domain.Sqlsugar\Repository.txt";
                            ModuleHelper.CreateFileCommon(repositoryTemplatePath, $"{domainBasePath}{item.EntityName}",
                              $"{item.EntityName}Repository.cs", item);
                        }
                        if (item.PrimaryKeysNum < 2)
                        {
                            //if (IService)
                            //{
                            //    string iserviceBasePath = SaveCodePath + @$"\{Module}\{moduleModel.Name}.Application.Contracts\";

                            //    //InSearchDto
                            //    string searchTemplatePath = path + @"\Template\CshapCodeTemplate\Application.Contracts\Dto\InSearchDto.txt";
                            //    ModuleHelper.CreateFileCommon(searchTemplatePath, $@"{iserviceBasePath}{item.EntityName}\Dto",
                            //        $"In{item.EntityName}SearchDto.cs", item);
                            //    //InAddUpdateDto
                            //    string addUpdateTemplatePath = path + @"\Template\CshapCodeTemplate\Application.Contracts\Dto\InAddUpdateDto.txt";
                            //    ModuleHelper.CreateFileCommon(addUpdateTemplatePath, @$"{iserviceBasePath}{item.EntityName}\Dto",
                            //        $"In{item.EntityName}AddUpdateDto.cs", item);
                            //    //OutDto
                            //    string dtoTemplatePath = path + @"\Template\CshapCodeTemplate\Application.Contracts\Dto\OutDto.txt";
                            //    ModuleHelper.CreateFileCommon(dtoTemplatePath, @$"{iserviceBasePath}{item.EntityName}\Dto",
                            //        $"Out{item.EntityName}Dto.cs", item);
                            //    //OutTableDto
                            //    string tableTemplatePath = path + @"\Template\CshapCodeTemplate\Application.Contracts\Dto\OutTableDto.txt";
                            //    ModuleHelper.CreateFileCommon(tableTemplatePath, @$"{iserviceBasePath}{item.EntityName}\Dto",
                            //        $"Out{item.EntityName}TableDto.cs", item);
                            //    //IServiceiserviceTemplatePath
                            //    string iserviceTemplatePath = path + @"\Template\CshapCodeTemplate\Application.Contracts\IService.txt";
                            //    ModuleHelper.CreateFileCommon(iserviceTemplatePath, $"{iserviceBasePath}{item.EntityName}",
                            //        $"I{item.EntityName}Service.cs", item);
                            //}

                            if (Service)
                            {
                                string serviceBasePath = SaveCodePath + @$"\{Module}\{moduleModel.Name}.Application\";


                                //InSearchDto
                                string searchTemplatePath = path + @"\Template\CshapCodeTemplate\Application\Dto\InSearchDto.txt";
                                ModuleHelper.CreateFileCommon(searchTemplatePath, $@"{serviceBasePath}{item.EntityName}\Dto",
                                    $"In{item.EntityName}SearchDto.cs", item);
                                //InAddUpdateDto
                                string addUpdateTemplatePath = path + @"\Template\CshapCodeTemplate\Application\Dto\InAddUpdateDto.txt";
                                ModuleHelper.CreateFileCommon(addUpdateTemplatePath, @$"{serviceBasePath}{item.EntityName}\Dto",
                                    $"In{item.EntityName}Dto.cs", item);
                                //OutDto
                                string dtoTemplatePath = path + @"\Template\CshapCodeTemplate\Application\Dto\OutDto.txt";
                                ModuleHelper.CreateFileCommon(dtoTemplatePath, @$"{serviceBasePath}{item.EntityName}\Dto",
                                    $"Out{item.EntityName}Dto.cs", item);
                                //OutTableDto
                                string tableTemplatePath = path + @"\Template\CshapCodeTemplate\Application\Dto\OutTableDto.txt";
                                ModuleHelper.CreateFileCommon(tableTemplatePath, @$"{serviceBasePath}{item.EntityName}\Dto",
                                    $"Out{item.EntityName}TableDto.cs", item);


                                //Service
                                string serviceTemplatePath = path + @"\Template\CshapCodeTemplate\Application\Service.txt";
                                ModuleHelper.CreateFileCommon(serviceTemplatePath, $"{serviceBasePath}{item.EntityName}",
                                    $"{item.EntityName}Service.cs", item);

                                //MapsterConfig
                                string mapsterTemplatePath = path + @"\Template\CshapCodeTemplate\Application\MapsterConfig.txt";
                                ModuleHelper.CreateFileCommon(mapsterTemplatePath, $"{serviceBasePath}{item.EntityName}",
                                    $"_MapsterConfig.cs", item);
                            }

                            //if (Controller)
                            //{
                            //    string controllerBasePath = SaveCodePath + @$"\{Module}\{moduleModel.Name}.HttpApi\";
                            //    //Service
                            //    string controllerTemplatePath = path + @"\Template\CshapCodeTemplate\Controllers\Controller.txt";
                            //    ModuleHelper.CreateFileCommon(controllerTemplatePath, $@"{controllerBasePath}\Controllers",
                            //        $"{item.EntityName}Controller.cs", item);
                            //}
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
            await Task.CompletedTask;
            //Growl.Success("生成代码");
        }

        public ICommand SaveConfigCommand { get; }

        private async Task SaveConfigAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    AppConfig appConfig = new AppConfig();
                    appConfig.DbType = DbType;
                    appConfig.DataConnection = DataConnection;
                    appConfig.SaveCodePath = SaveCodePath;
                    appConfig.Module = Module;
                    GlobalData.Config = appConfig;
                    GlobalData.Save();
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
                    GlobalData.Init();
                    DbType = GlobalData.Config.DbType;
                    DataConnection = GlobalData.Config.DataConnection;
                    SaveCodePath = GlobalData.Config.SaveCodePath;
                    Module = GlobalData.Config.Module;
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