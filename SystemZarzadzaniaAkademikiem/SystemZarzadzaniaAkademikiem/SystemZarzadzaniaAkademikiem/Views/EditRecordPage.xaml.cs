﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemZarzadzaniaAkademikiem.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SystemZarzadzaniaAkademikiem.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditRecordPage : ContentPage
	{

        EditRecordViewModel viewModel;
        string tableName;
        object id;
        public EditRecordPage (string tableName,object id)
		{
            this.tableName = tableName;
            this.id = id;
            InitializeComponent ();
            BindingContext = viewModel = new EditRecordViewModel(tableName,id);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadColumnsCommand.Execute(id);
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}