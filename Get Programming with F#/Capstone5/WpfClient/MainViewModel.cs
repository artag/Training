using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Capstone5;
using Capstone5.Domain;

namespace WpfClient
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private RatedAccount _account;
        private int _balance;

        public MainViewModel()
        {
            Owner = new Customer("isaac");
            Transactions = new ObservableCollection<Transaction>();

            LoadTransactions();
            UpdateAccount(Api.LoadAccount(Owner));

            DepositCommand = new Command<int>(
                amount =>
                {
                    UpdateAccount(Api.Deposit(amount, Owner));
                    WithdrawCommand.Refresh();
                },
                TryParseInt);

            WithdrawCommand = new Command<int>(
                amount => UpdateAccount(Api.Withdraw(amount, Owner)),
                TryParseInt,
                () => _account.IsInCredit);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Customer Owner { get; }

        public Command<int> DepositCommand { get; }

        public Command<int> WithdrawCommand { get; }

        public int Balance
        {
            get => _balance;
            private set
            {
                _balance = value;
                OnPropertyChanged(nameof(Balance));
            }
        }

        public ObservableCollection<Transaction> Transactions { get; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadTransactions()
        {
            Transactions.Clear();
            foreach (var txn in Api.LoadTransactionHistory(Owner))
                Transactions.Add(txn);
        }

        private void UpdateAccount(RatedAccount newAccount)
        {
            _account = newAccount;
            LoadTransactions();
            Balance = (int)_account.Balance;
        }

        private Tuple<bool, int> TryParseInt(object value)
        {
            var parsed = int.TryParse(value as string, out var output);
            return Tuple.Create(parsed, output);
        }
    }
}
