﻿namespace InfConstractions.ViewModels
{
    using System.Threading.Tasks;
    using System.Data.Entity;
    using System.Linq;
    using System.Data.SqlClient;
    using System.Windows;
    using Models;
    using System.Data.Entity.Core.EntityClient;
    using System;
    using DevExpress.Xpf.Docking;
    using DevExpress.Mvvm;
    using DevExpress.Mvvm.ViewModel;
    using DevExpress.Mvvm.POCO;
    using DevExpress.Mvvm.DataAnnotations;
    using Services;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class MainWindowViewModel : ViewModelBase, IDialogService
    {
        #region Fields
        protected Guid proverkaGU_key = new Guid("99B84C49-D814-4028-8889-6ED5E7023FF5");
        //protected formLoginViewModel vm =  new formLoginViewModel();
        #endregion
        #region Services
        IOpenFileDialogService OpenFileDialogService { get { return GetService<IOpenFileDialogService>(); } }
        ISaveFileDialogService SaveFileDialogService { get { return GetService<ISaveFileDialogService>(); } }
        IDialogService DialogService { get { return GetService<IDialogService>(); } }
        IMessageBoxService MessageService { get { return GetService<IMessageBoxService>(); } }
        public IDocumentManagerService DocumentManagerService { get { return GetService<IDocumentManagerService>(); } }
        #endregion
        #region Constructors
        public MainWindowViewModel():base()
        {
            try
            {
                
                //vmVisibility = Visibility.Hidden;
                sqlConnection = new SqlConnection();
                efConnection = new EntityConnection();
                //var u = this.GetDependencyResolver().Resolve<IUIVisualizerService>();
                //u.ShowDialogAsync(vm, completeLogin);
            }
            catch (Exception e)
            { MessageService.ShowMessage(e.Message); }
        }
        #endregion
        #region Properties
        public string Title { get { return "InfConstractions"; } }
        public Entities mainContext
        {
            get { return GetProperty<Entities>(() => mainContext); }
            private set { SetProperty<Entities>(() => mainContext, value); }
        }
        public SqlConnection sqlConnection
        {
            get { return GetProperty<SqlConnection>(()=> sqlConnection); }
            private set { SetProperty<SqlConnection>(()=> sqlConnection, value); }
        }
        public EntityConnection efConnection
        {
            get { return GetProperty<EntityConnection>(() => efConnection); }
            private set { SetProperty<EntityConnection>(() => efConnection, value); }
        }
        public Visibility vmVisibility
        {
            get { return GetProperty(() => vmVisibility); }
            set { SetProperty<Visibility>(() => vmVisibility, value); }
        }
        #endregion
        #region Methods
        void Update_mainContext()
        {
            RaisePropertyChanged(() => mainContext);
        }
        /*private void completeLogin(object sender, UICompletedEventArgs e)
        {
            if (e.Result == true)
            {
                try
                {
                    sqlConnection = vm.sqlConnection;
                    efConnection = vm.efConnection;
                    mainContext = new Entities(efConnection);
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                finally
                { vmVisibility = Visibility.Visible; }

            }
            else { Application.Current.Shutdown(-1); }
        }*/
        #endregion
        #region Commands

        [Command(CanExecuteMethodName = "CancmShowLoginForm",
            Name = "cmShowLoginForm",
            UseCommandManager = true)]

        public void ShowLoginForm()
        {
            dxwLoginViewModel LoginViewModel = null;
            if (LoginViewModel == null)
                LoginViewModel = ViewModelSource.Create(() => new dxwLoginViewModel());
            UICommand registerCommand = new UICommand()
            {
                Caption = "Вход",
                IsCancel = false,
                IsDefault = true,
                Command = new DelegateCommand<CancelEventArgs>(
                    x => { LoginViewModel.ConnectionStringConstructExecute(); },
                    x => LoginViewModel.CancmConnectionStringConstructExecute(), true),              
            };
            UICommand cancelCommand = new UICommand()
            {
                Id = DevExpress.Mvvm.MessageResult.Cancel,
                Caption = "Отмена",
                IsCancel = true,
                IsDefault = false,
            };
            UICommand result = DialogService.ShowDialog(
                dialogCommands: new List<UICommand>() { registerCommand, cancelCommand },
                title: "Соединение с сервером",
                viewModel: LoginViewModel);
            if (result == registerCommand)
            {
                efConnection = LoginViewModel.efConnection;
                sqlConnection = LoginViewModel.sqlConnection;
                mainContext = new Entities(efConnection);
                App.mainConnection= LoginViewModel.efConnection;
            }
        }

        public bool CancmShowLoginForm()
        {
            return true;
        }
        [Command(CanExecuteMethodName = "CancmExit",
            Name = "cmExit",
            UseCommandManager = true)]
        public void Exit()
        {
            Application.Current.MainWindow.Close();
        }
        public bool CancmExit()
        {
            return true;
        }

        [Command(CanExecuteMethodName = "CancmProverkaGU",
            Name = "cmProverkaGU",
            UseCommandManager = true)]
        public void ProverkaGU()
        {
             
            DocumentManagerService.FindDocumentByIdOrCreate(proverkaGU_key, (ds) =>
            {
                ProverkaGUViewModel vm = ViewModelSource.Create(() => new ProverkaGUViewModel(mainContext, DocumentManagerService));
                IDocument _docCreated = ds.CreateDocument("ProverkaGUView",vm );
                _docCreated.Id = proverkaGU_key;
                _docCreated.Title = vm.Title;
                return _docCreated;
            }).Show();
        }
        public bool CancmProverkaGU()
        {
            if (efConnection!=null && efConnection.State != System.Data.ConnectionState.Closed)
            { return true; }
            else
            { return false; }
        }
        [Command(CanExecuteMethodName = "CancmOpenRichEdit",
            Name = "cmOpenRichEdit",
            UseCommandManager = true)]
        public void OpenRichEdit()
        {
            DialogService.ShowDialog(null, "dsfdsfds", ViewModelSource.Create(() => new dxwLoginViewModel()));
            DocumentManagerService.FindDocumentByIdOrCreate(proverkaGU_key, (ds) =>
            {
                IDocument doc1 = ds.CreateDocument("ucWord", ViewModelSource.Create(() => new ucWordViewModel()));
                doc1.Id = proverkaGU_key.ToString();
                doc1.Title = "Документ Word";
                return doc1;
            }).Show();
        }
        public bool CancmOpenRichEdit()
        {
            if (efConnection!=null && efConnection.State != System.Data.ConnectionState.Closed)
            { return true; }
            else
            { return false; }
        }

        public UICommand ShowDialog(IEnumerable<UICommand> dialogCommands, string title, string documentType, object viewModel, object parameter, object parentViewModel)
        {
            return DialogService.ShowDialog(dialogCommands, title, documentType, viewModel, parameter, parentViewModel);
        }

        #endregion
    }
}
