using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTWebClient;
using TTWebClientUI.Properties;

namespace TTWebClientUI
{
    internal class MainWindowModel : ObservableObject
    {
        //private LoadModel load = new LoadModel();
        private readonly IWndService _service;
        private CredsModel _selectedCreds;
        private bool _isRunning;
        private bool _isStopping;
        private bool _isValid;
        //private string _countStr;
        //private string _depthStr;
        //private string _sleepStr;
        //private int? _count;
        //private int? _depth;
        //private int? _sleep;
        private string _error;

        public Command StartCommand { get; private set; }
        public Command StopCommand { get; private set; }
        public Command AddCommand { get; private set; }
        public Command EditCommand { get; private set; }
        public Command DeleteCommand { get; private set; }

        public ObservableCollection<CredsModel> CredsList { get; private set; }

        public CredsModel SelectedCreds
        {
            get { return _selectedCreds; }
            set
            {
                _selectedCreds = value;
                OnPropertyChanged();
                Validate();
                UpdateCredsButtonState();
            }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsInputsEnabled));
                UpdateButtonState();
            }
        }

        public bool IsInputsEnabled { get { return !_isRunning; } }

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged();
            }
        }

        public MainWindowModel(IWndService wndService)
        {
            _service = wndService;
            StartCommand = new Command(Start);
            StopCommand = new Command(Stop);
            AddCommand = new Command(AddCreds);
            DeleteCommand = new Command(DeleteCreds);
            EditCommand = new Command(EditCreds);

            CredsList = new ObservableCollection<CredsModel>();

            if (Settings.Default.Creds != null)
            {
                foreach (var credsStr in Settings.Default.Creds)
                {
                    var credsItem = CredsModel.Parse(credsStr);
                    if (credsItem != null && !CredsList.Contains(credsItem))
                        CredsList.Add(credsItem);
                }
            }

            var savedSelected = CredsModel.Parse(Settings.Default.LastCred);
            if (savedSelected != null)
            {
                if (CredsList.Contains(savedSelected))
                    SelectedCreds = savedSelected;
            }
            if (SelectedCreds == null)
                SelectedCreds = CredsList.FirstOrDefault();

            UpdateCredsButtonState();
            Validate();
        }

        private async void Start()
        {
            Error = "";
            IsRunning = true;

            try
            {
                // Create instance of the TickTrader Web API client
                var clientModel = new ClientModel(_selectedCreds.WebApiAddress, _selectedCreds.WebApiId, _selectedCreds.WebApiKey, _selectedCreds.WebApiSecret);
                //---Public Web API methods---
                clientModel.GetPublicTradeSession();

                clientModel.GetPublicCurrencies();
                clientModel.GetPublicSymbols();
                clientModel.GetPublicTicks();
                clientModel.GetPublicTicksLevel2();

                //--- Web API client methods ---
                clientModel.GetAccount();
                clientModel.GetTradeSession();

                clientModel.GetCurrencies();
                clientModel.GetSymbols();
                clientModel.GetTicks();
                clientModel.GetTicksLevel2();

                clientModel.GetAssets();
                clientModel.GetPositions();
                clientModel.GetTrades();
                clientModel.GetTradeHistory();

                clientModel.LimitOrder();
            }
            catch (Exception ex)
            {
                //    Error = ex.Message;
                //    IsRunning = false;
            }
        }

        private async void Stop()
        {
            //if (isStopping)
            //    return;

            //isStopping = true;
            //UpdateButtonState();

            //await load.Stop();

            //isStopping = false;
            //IsRunning = false;

            //if (closeEvent != null)
            //    closeEvent.SetResult(null);
        }

        private void AddCreds()
        {
            CredsDialogModel dlgModel = new CredsDialogModel();
            _service.ShowCredsDialog(dlgModel);
            if (dlgModel.Creds != null)
            {
                CredsList.Add(dlgModel.Creds);
                if (SelectedCreds == null)
                    SelectedCreds = CredsList.First();
                SaveCredsCollection();
            }
        }

        private void DeleteCreds()
        {
            if (_selectedCreds != null)
            {
                if (CredsList.Remove(_selectedCreds))
                {
                    SelectedCreds = CredsList.FirstOrDefault();
                    SaveCredsCollection();
                }
            }
        }

        private void EditCreds()
        {
            int index = CredsList.IndexOf(_selectedCreds);
            CredsDialogModel dlgModel = new CredsDialogModel(_selectedCreds);
            _service.ShowCredsDialog(dlgModel);
            if (dlgModel.Creds != null)
            {
                CredsList[index] = dlgModel.Creds;
                SelectedCreds = dlgModel.Creds;
                SaveCredsCollection();
            }
        }

        private void SaveCredsCollection()
        {
            var serializedCreds = new StringCollection();

            foreach (var credsItem in CredsList)
                serializedCreds.Add(credsItem.Serialize());

            Settings.Default.Creds = serializedCreds;
            Settings.Default.Save();
        }

        private void Validate()
        {
            _isValid = _selectedCreds != null;
            UpdateButtonState();
        }

        private void UpdateCredsButtonState()
        {
            DeleteCommand.IsEnabled = _selectedCreds != null;
            EditCommand.IsEnabled = _selectedCreds != null;
        }

        private void UpdateButtonState()
        {
            StartCommand.IsEnabled = _isValid && !_isRunning;
            StopCommand.IsEnabled = _isRunning && !_isStopping;
        }

    }
}
