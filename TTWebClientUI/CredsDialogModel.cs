using System;
using System.Linq;

namespace TTWebClientUI
{
    public class CredsDialogModel : ObservableObject
    {
        private string _login;
        private string _password;
        private string _server;
        private string _secret;
        private string _account;

        public CredsDialogModel()
        {
            Title = "Add Credentials";
            CmdOk = new Command(OnClose);
        }

        public CredsDialogModel(CredsModel creds) : this()
        {
            Title = "Edit Credentials";
            Login = creds.WebApiId;
            Password = creds.WebApiKey;
            Server = creds.WebApiAddress;
            Secret = creds.WebApiSecret;
        }

        public string Title { get; private set; }

        public Command CmdOk { get; private set; }
        
        public CredsModel Creds { get; private set; }

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public string Server
        {
            get { return _server; }
            set
            {
                _server = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public string Secret
        {
            get { return _secret; }
            set
            {
                _secret = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public string Account
        {
            get { return _account; }
            set
            {
                _account = value;
                OnPropertyChanged();
            }
        }

        private void OnClose()
        {
            Creds = new CredsModel() { WebApiId = Login, WebApiKey = Password, WebApiAddress = Server, WebApiSecret = Secret, Account = Account};
            Closed();
        }

        private void Validate()
        {
            bool isValid = ValidateStringInput(_server); 
                //&& ValidateStringInput(_login)
                //&& ValidateStringInput(_password)
                //&& ValidateStringInput(_secret);

            CmdOk.IsEnabled = isValid;
        }

        private bool ValidateStringInput(string text)
        {
            return !string.IsNullOrEmpty(text) && !text.Contains(' ');
        }

        public Action Closed { get; set; }
    }

    public class CredsModel
    {
        public static CredsModel Parse(string credsStr)
        {
            if (string.IsNullOrWhiteSpace(credsStr))
                return null;

            var parts = credsStr.Split(' ');
            if (parts.Length < 4)
                return null;

            return new CredsModel()
            {
                WebApiAddress = parts[0],
                WebApiId = parts[1],
                WebApiKey = parts[2],
                WebApiSecret = parts[3],
                Account = parts.Length > 4 ? parts[4] : "",
            };
        }

        public string WebApiAddress { get; set; }
        public string WebApiId { get; set; }
        public string WebApiKey { get; set; }
        public string WebApiSecret { get; set; }
        public string Account { get; set; }

        public bool IsPublicOnly =>
            !string.IsNullOrEmpty(WebApiAddress) &&
            (string.IsNullOrEmpty(WebApiId) || string.IsNullOrEmpty(WebApiKey) || string.IsNullOrEmpty(WebApiSecret));

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as CredsModel;
            return other != null && other.WebApiId == WebApiId && other.WebApiKey == WebApiKey && other.WebApiAddress == WebApiAddress;
        }

        public string Serialize()
        {
            return WebApiAddress + ' ' + WebApiId + ' ' + WebApiKey + ' ' + WebApiSecret + ' ' + Account;
        }

        public override string ToString()
        {
            return WebApiAddress + " " + WebApiId + " " + (string.IsNullOrEmpty(Account)? "" : " (" + Account + ")");
        }
    }
}
