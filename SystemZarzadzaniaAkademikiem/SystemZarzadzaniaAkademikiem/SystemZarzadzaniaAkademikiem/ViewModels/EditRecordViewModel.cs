using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using SQLite;
using SystemZarzadzaniaAkademikiem.Data;
using SystemZarzadzaniaAkademikiem.Models;
using SystemZarzadzaniaAkademikiem.Services;
using Xamarin.Forms;

namespace SystemZarzadzaniaAkademikiem.ViewModels
{
    public class EditRecordViewModel : BaseViewModel
    {
        private UserRepo userRepo;
        private AdminRepo adminRepo;
        private RoomRepo roomRepo;
        private AppDatabase database;
        public ObservableCollection<Column> Columns { get; set; }
        public Command EditRecordCommand { get; set; }
        private string tableName;
        private object id;
        public EditRecordViewModel(string tableName,object id, AppDatabase database)
        {
            Title = "Edit Column";
            this.tableName = tableName;
            this.id = id;
            Columns = new ObservableCollection<Column>();
            this.database = database;
            userRepo = new UserRepo(database);
            adminRepo = new AdminRepo(database);
            roomRepo = new RoomRepo(database);
            EditRecordCommand = new Command(() => ExecuteEditRecordCommand());

        }
        public Command LoadColumnsCommand
        {
            get
            {
                return new Command((id) =>
                {
                    Columns.Clear();
                    List<SQLiteConnection.ColumnInfo> list = database.DatabaseNotAsync.GetTableInfo(tableName);
                    foreach (var a in list)
                    {
                        if (a.Name.ToLower() != "id" &&a.Name.ToLower() != "salt")
                        {
                            Column c = new Column { Name = a.Name,Value="" };
                            Columns.Add(c);
                        }
                    }
                    var o =RunSql("SELECT * FROM "+tableName+" WHERE Id="+id);
                    if (tableName == "SuperUser")
                    {
                        for (int i = 1; i < o.Length-1; i++)
                        {
                            if (o[i] != null)
                            {
                                Columns[i - 1].Value = o[i].ToString();
                            }
                        }
                    }
                    else
                    {
                        for (int i = 1; i < o.Length; i++)
                        {
                            if (o[i] != null)
                            {
                                Columns[i - 1].Value = o[i].ToString();
                            }
                        }
                    }

                });
            }
        }
        void ExecuteEditRecordCommand()
        {
            if (IsBusy)
            {
                return;
            }
                

            IsBusy = true;

            try
            {
                int result = 0;
                switch (tableName)
                {
                    case "User":

                        List<string> temp = new List<string>();
                        for (int i = 0; i < Columns.Count; i++)
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
                        int.TryParse(id.ToString(),out result);
                        newUser.Id = result;
                        userRepo.SaveUserAsync(newUser);
                        break;
                    case "SuperUser":
                        SuperUser newSuperUser = new SuperUser
                        {
                            Login = Columns[0].Value,
                            Password = Columns[1].Value
                        };
                        int.TryParse(id.ToString(), out result);
                        newSuperUser.Id = result;
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
                        int.TryParse(id.ToString(), out result);
                        room.Id = result;
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
        private object[] RunSql(string sqlString)
        {
            
            SQLitePCL.sqlite3_stmt stQuery = null;
            try
            {
                stQuery = SQLite3.Prepare2(database.DatabaseNotAsync.Handle, sqlString);
                var colLenght = SQLite3.ColumnCount(stQuery);
                while (SQLite3.Step(stQuery) == SQLite3.Result.Row)
                {
                    var obj = new object[colLenght];
                    for (int i = 0; i < colLenght; i++)
                    {
                        var colType = SQLite3.ColumnType(stQuery, i);
                        switch (colType)
                        {
                            case SQLite3.ColType.Blob:
                                obj[i] = SQLite3.ColumnBlob(stQuery, i);
                                break;
                            case SQLite3.ColType.Float:
                                obj[i] = SQLite3.ColumnDouble(stQuery, i);
                                break;
                            case SQLite3.ColType.Integer:
                                obj[i] = SQLite3.ColumnInt(stQuery, i);
                                break;
                            case SQLite3.ColType.Null:
                                obj[i] = null;
                                break;
                            case SQLite3.ColType.Text:
                                obj[i] = SQLite3.ColumnString(stQuery, i);
                                break;
                        }
                    }
                    return obj;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (stQuery != null)
                {
                    SQLite3.Finalize(stQuery);
                }
            }
        }
    }
}
