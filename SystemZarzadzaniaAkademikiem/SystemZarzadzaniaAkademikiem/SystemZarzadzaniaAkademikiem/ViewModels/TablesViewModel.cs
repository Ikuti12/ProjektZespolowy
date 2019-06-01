﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using SystemZarzadzaniaAkademikiem.Models;
using SystemZarzadzaniaAkademikiem.Services;
using Xamarin.Forms;

namespace SystemZarzadzaniaAkademikiem.ViewModels
{
    public class TablesViewModel : BaseViewModel
    {
        public ObservableCollection<Table> Tables { get; set; }
        public Command LoadTablesCommand { get; set; }
        public TablesViewModel()
        {
            Title = "Browse Tables";
            Tables = new ObservableCollection<Table>();
            LoadTablesCommand = new Command(()=> ExecuteLoadTablesCommand());
        }
        void ExecuteLoadTablesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Tables.Clear();
                var tables = GetAllTablesAsync().Result;
                foreach (var table in tables)
                {
                    if (table.name != "sqlite_sequence")
                    {
                        Tables.Add(table);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        async Task<List<Table>> GetAllTablesAsync()
        {
            string queryString = $"SELECT name FROM sqlite_master WHERE type = 'table'";
            return await App.Database.Database.QueryAsync<Table>(queryString).ConfigureAwait(false);
        }
    }
}
