using SQLite;
using System;
using System.Collections.Generic;
using SystemZarzadzaniaAkademikiem.Data;
using SystemZarzadzaniaAkademikiem.Enums;
using SystemZarzadzaniaAkademikiem.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SystemZarzadzaniaAkademikiem.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TableDetailPage : ContentPage
	{
        TableDetailViewModel viewModel;
        AppDatabase database;
        public bool isLoading;
		public TableDetailPage (TableDetailViewModel tableDetailViewModel, AppDatabase database)
		{
			InitializeComponent ();
            this.database = database;
            BindingContext = viewModel = tableDetailViewModel;
        }
        protected override void OnAppearing()
        {
            if (Content == null)
            {
                List<SQLiteConnection.ColumnInfo> list = database.DatabaseNotAsync.GetTableInfo(viewModel.name);
                StackLayout main = new StackLayout { Orientation = StackOrientation.Vertical };
                Grid mainGrid = new Grid { MinimumHeightRequest = 80 };
                StackLayout s = new StackLayout();
                s.Orientation = StackOrientation.Horizontal;
                int k = 0;
                foreach (var a in list)
                {
                    var label = new Label { Text = a.Name };
                    mainGrid.Children.Add(label, k, 0);
                    k++;
                }
                int j = 0;
                var objects = RunSql("SELECT * FROM " + viewModel.name, false);
                for (int row = 0; row < objects.Count; row++)
                {
                    for (int column = 0; column < objects[row].Length; column++)
                    {
                        if (objects[row][column] == null)
                        {
                            var label = new Label { Text = "NULL" };
                            mainGrid.Children.Add(label, column, row + 1);
                            j = column;
                        }
                        else if (objects[row][column] != null)
                        {
                            if (objects[row][column].ToString().Length <= 1 && column != 18 && column != 19 && column!=0 && viewModel.name=="User")
                            {
                                if (objects[row][column].ToString() == "0")
                                {
                                    var label = new Label { Text = "NULL" };
                                    mainGrid.Children.Add(label, column, row + 1);
                                    j = column;
                                }
                                else
                                {

                                    var label = ChangeEnumIntToString(int.Parse(objects[row][column].ToString()),column);
                                    mainGrid.Children.Add(label, column, row + 1);
                                    j = column;
                                }
                            }
                            else
                            {
                                var label = new Label { Text = objects[row][column].ToString() };
                                mainGrid.Children.Add(label, column, row + 1);
                                j = column;
                            }
                        }
                    }
                    var buttonEdit = new Button { Text = "Edit", CommandParameter = objects[row][0] };
                    var buttonDelete = new Button { Text = "Delete", CommandParameter = objects[row][0] };
                    buttonDelete.Clicked += Remove_Record;
                    buttonEdit.Clicked += Edit_Record;
                    mainGrid.Children.Add(buttonEdit, j + 1, row + 1);
                    mainGrid.Children.Add(buttonDelete, j + 2, row + 1);
                }
                main.Children.Add(mainGrid);
                var addButton = new Button { Text = "Add", HorizontalOptions = LayoutOptions.Center, WidthRequest = 300 };
                addButton.Clicked += Add_Record;
                main.Children.Add(addButton);
                ScrollView sv = new ScrollView
                {
                    HorizontalOptions = LayoutOptions.Fill,
                    Orientation = ScrollOrientation.Both,
                    Content = main
                };
                Content = sv;
            }

        }
        async private void Add_Record(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddRecordPage(viewModel?.name));
        }
        public Label ChangeEnumIntToString(int enumInt, int column)
        {
            switch (column)
            {
                case 4:
                    Enum Sex = (Sex)enumInt;
                    return new Label { Text = Sex.ToString() };
                case 5:
                    Enum Floor = (Floor)enumInt;
                    return new Label { Text = Floor.ToString() };
                case 6:
                    Enum BedLocation = (BedLocation)enumInt;
                    return new Label { Text = BedLocation.ToString() };
                case 7:
                    Enum SleepTime = (SleepTime)enumInt;
                    return new Label { Text = SleepTime.ToString() };
                case 8:
                    Enum WakeUpTime = (WakeUpTime)enumInt;
                    return new Label { Text = WakeUpTime.ToString() };
                case 9:
                    Enum HotOrNot = (HotOrNot)enumInt;
                    return new Label { Text = HotOrNot.ToString() };
                case 10:
                    Enum Music = (Music)enumInt;
                    return new Label { Text = Music.ToString() };
                case 11:
                    Enum CleanUp = (YesNo)enumInt;
                    return new Label { Text = CleanUp.ToString() };
                case 12:
                    Enum Talkative = (Talkative)enumInt;
                    return new Label { Text = Talkative.ToString() };
                case 13:
                    Enum StudyField = (StudyField)enumInt;
                    return new Label { Text = StudyField.ToString() };
                case 14:
                    Enum Sporting = (YesNo)enumInt;
                    return new Label { Text = Sporting.ToString() };
                case 15:
                    Enum HomeBack = (HomeBack)enumInt;
                    return new Label { Text = HomeBack.ToString() };
                case 16:
                    Enum Smoking = (YesNo)enumInt;
                    return new Label { Text = Smoking.ToString() };
                case 17:
                    Enum Party = (Party)enumInt;
                    return new Label { Text = Party.ToString() };

            }
            return new Label { Text = "NULL2" };
        }
        private List<object[]> RunSql(string sqlString, bool includeColumnNamesAsFirstRow)
        {
            var lstRes = new List<object[]>();
            SQLitePCL.sqlite3_stmt stQuery = null;
            try
            {
                stQuery = SQLite3.Prepare2(database.DatabaseNotAsync.Handle, sqlString);
                var colLenght = SQLite3.ColumnCount(stQuery);

                if (includeColumnNamesAsFirstRow)
                {
                    var obj = new object[colLenght];
                    lstRes.Add(obj);
                    for (int i = 0; i < colLenght; i++)
                    {
                        obj[i] = SQLite3.ColumnName(stQuery, i);
                    }
                }

                while (SQLite3.Step(stQuery) == SQLite3.Result.Row)
                {
                    var obj = new object[colLenght];
                    lstRes.Add(obj);
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
                }
                return lstRes;
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
        private void Remove_Record(object sender, EventArgs e)
        {
            var button = sender as Button;
            viewModel?.DeleteRecordCommand.Execute(button.CommandParameter);
            Content = null;
            OnAppearing();
        }
        async private void Edit_Record(object sender, EventArgs e)
        {
            var button = sender as Button;
            await Navigation.PushAsync(new EditRecordPage(viewModel?.name,button.CommandParameter));
        }
    }
}