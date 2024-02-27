using Data.Abstractions.Entities;
using Data.Abstractions.Helper;

using HandyControl.Controls;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using System;
using System.Threading.Tasks;
using System.Windows.Input;

using wjtool.Data;
using wjtool.UserControl.Base;

namespace wjtool.ViewModel.Controls
{
    public class NewModuleContent : ObservableObject
    {
        public NewModuleContent()
        {
            GlobalData.InitNewModule();
            _module = GlobalData.NewModuleConfig?.Module ?? "";
            _saveCodePath = GlobalData.NewModuleConfig?.SaveCodePath ?? "";

            SelectCodePathCommand = new AsyncRelayCommand(SelectCodePathAsync);
            CreateCodeCommand = new AsyncRelayCommand(CreateCodeAsync);
            SaveConfigCommand = new AsyncRelayCommand(SaveConfigAsync);
            LoadConfigCommand = new AsyncRelayCommand(LoadConfigAsync);
        }

        #region 字段

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

        private bool _shared = true;

        /// <summary>
        /// 枚举
        /// </summary>
        public bool Shared
        {
            get => _shared;
            set => SetProperty(ref _shared, value);
        }

        #endregion 字段

        #region 事件

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

                    string basePath = SaveCodePath + @$"\{Module}";
                    if (Shared)
                    {
                        string savaPath = basePath + @$"\{moduleModel.Name}.Domain.Shared\";

                        string templatePath = path + @"\Template\NewModule\Shared\";

                        ModuleHelper.CreateFileCommon(templatePath + "csproj.txt", $"{savaPath}",
                            $"{moduleModel.Name}.Domain.Shared.csproj", moduleModel);

                        ModuleHelper.CreateFileCommon(templatePath + "AppModule.txt", $"{savaPath}",
                           $"{moduleModel.Name}DomainSharedModule.cs", moduleModel);
                    }

                    if (IRepository)
                    {
                        string savaPath = basePath + @$"\{moduleModel.Name}.Domain\";

                        string templatePath = path + @"\Template\NewModule\IRepository\";

                        ModuleHelper.CreateFileCommon(templatePath + "csproj.txt", $"{savaPath}",
                            $"{moduleModel.Name}.Domain.csproj", moduleModel);

                        ModuleHelper.CreateFileCommon(templatePath + "AppModule.txt", $"{savaPath}",
                           $"{moduleModel.Name}DomainModule.cs", moduleModel);

                        ModuleHelper.CreateFileCommon(templatePath + "DbContext.txt", $"{savaPath}",
                           $"{moduleModel.Name}DbContext.cs", moduleModel);
                    }

                    if (Repository)
                    {
                        string savaPath = basePath + @$"\{moduleModel.Name}.Domain.Sqlsugar\";

                        string templatePath = path + @"\Template\NewModule\Repository\";

                        ModuleHelper.CreateFileCommon(templatePath + "csproj.txt", $"{savaPath}",
                            $"{moduleModel.Name}.Domain.Sqlsugar.csproj", moduleModel);

                        ModuleHelper.CreateFileCommon(templatePath + "AppModule.txt", $"{savaPath}",
                           $"{moduleModel.Name}DomainSqlsugarModule.cs", moduleModel);
                    }
                    //if (IService)
                    //{
                    //    string savaPath = basePath + @$"\{moduleModel.Name}.Application.Contracts\";

                    //    string templatePath = path + @"\Template\NewModule\IService\";

                    //    ModuleHelper.CreateFileCommon(templatePath + "csproj.txt", $"{savaPath}",
                    //        $"{moduleModel.Name}.Application.Contracts.csproj", moduleModel);

                    //    ModuleHelper.CreateFileCommon(templatePath + "AppModule.txt", $"{savaPath}",
                    //       $"{moduleModel.Name}ApplicationContractsModule.cs", moduleModel);
                    //}

                    if (Service)
                    {
                        string savaPath = basePath + @$"\{moduleModel.Name}.Application\";

                        string templatePath = path + @"\Template\NewModule\Service\";

                        ModuleHelper.CreateFileCommon(templatePath + "csproj.txt", $"{savaPath}",
                            $"{moduleModel.Name}.Application.csproj", moduleModel);

                        ModuleHelper.CreateFileCommon(templatePath + "AppModule.txt", $"{savaPath}",
                           $"{moduleModel.Name}ApplicationModule.cs", moduleModel);
                    }

                    if (Controller)
                    {
                        string savaPath = basePath + @$"\{moduleModel.Name}.HttpApi\";

                        string templatePath = path + @"\Template\NewModule\Controller\";

                        ModuleHelper.CreateFileCommon(templatePath + "csproj.txt", $"{savaPath}",
                            $"{moduleModel.Name}.HttpApi.csproj", moduleModel);

                        ModuleHelper.CreateFileCommon(templatePath + "AppModule.txt", $"{savaPath}",
                           $"{moduleModel.Name}HttpApiModule.cs", moduleModel);

                        ModuleHelper.CreateFileCommon(templatePath + "BaseController.txt", $"{savaPath}",
                           $"BaseController.cs", moduleModel);
                    }

                    //修改解决方案

                    string slnPath = path + @"\Template\NewModule\sln.txt";
                    ModuleHelper.AddSln(slnPath, SaveCodePath, moduleModel);
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
                    NewModuleConfig appConfig = new NewModuleConfig();

                    appConfig.SaveCodePath = SaveCodePath;
                    appConfig.Module = Module;
                    GlobalData.NewModuleConfig = appConfig;
                    GlobalData.SaveNewModule();
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
                    GlobalData.InitNewModule();

                    SaveCodePath = GlobalData.NewModuleConfig.SaveCodePath;
                    Module = GlobalData.NewModuleConfig.Module;
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