using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using SystemZarzadzaniaAkademikiem.Data;
using SystemZarzadzaniaAkademikiem.Models;
using SystemZarzadzaniaAkademikiem.Services;
using Xamarin.Forms;

namespace SystemZarzadzaniaAkademikiem.ViewModels
{
    public class AddRecordViewModel : BaseViewModel
    {
        private UserRepo userRepo;
        private AdminRepo adminRepo;
        private RoomRepo roomRepo;
        private AppDatabase database;
        public ObservableCollection<Column> Columns { get; set; }
        public Command LoadColumnsCommand { get; set; }
        public Command AddRecordCommand { get; set; }
        private readonly string tableName;
        public AddRecordViewModel(string tableName, AppDatabase database)
        {
            Title = "Add Column";
            this.tableName = tableName;
            Columns = new ObservableCollection<Column>();
            LoadColumnsCommand = new Command(() => ExecuteLoadColumnsCommand());
            AddRecordCommand = new Command(() => ExecuteAddRecordCommand());
            this.database = database;
            userRepo = new UserRepo(database);
            adminRepo = new AdminRepo(database);
            roomRepo = new RoomRepo(database);
        }
        void ExecuteLoadColumnsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Columns.Clear();
                List<SQLiteConnection.ColumnInfo> list = database.DatabaseNotAsync.GetTableInfo(tableName);
                foreach (var a in list)
                {
                    if (a.Name.ToLower() != "id" && a.Name.ToLower() != "salt")
                    {
                        Column c = new Column { Name = a.Name, Value = "" };
                        Columns.Add(c);
                        
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
        void ExecuteAddRecordCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                switch (tableName)
                {
                    case "User":
                        
                        List<string> temp = new List<string>();
                        for(int i=0;i<Columns.Count;i++)
                        {
                            if (Columns[i].Value == "")
                            {
                                temp.Add(null);
                            }
                            else
                            {
                                temp.Add(Columns[i].Value);
                            }
                        }
                        
                        User newUser = new User(temp);
                        userRepo.SaveUserAsync(newUser);
                        break;
                    case "SuperUser":
                        SuperUser newSuperUser = new SuperUser
                        {
                            Login = Columns[0].Value,
                            Password = Columns[1].Value 
                        };
                        adminRepo.SaveAdminAsync(newSuperUser);
                        break;
                    case "Room":
                        List<string> tmp = new List<string>();
                        for (int i = 0; i < Columns.Count; i++)
                        {
                            if (Columns[i].Value == "")
                            {
                                tmp.Add(null);
                            }
                            else
                            {
                                tmp.Add(Columns[i].Value);
                            }
                        }

                        Room room = new Room(tmp);
                        roomRepo.SaveRoomAsync(room);
                        break;
                    default:
                        Debug.WriteLine("Error, uknown table");
                        break;
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

    }
}
