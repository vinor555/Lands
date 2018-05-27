using GalaSoft.MvvmLight.Command;
using Lands.Models;
using Lands.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Lands.ViewModels
{
    public class LandsViewModel : BaseViewModel
    {
        #region Services //los servicios se deben instanciar en el contructor
        private ApiService apiService;
        #endregion

        #region Atributes
        private ObservableCollection<Land> lands;
        private bool isRefreshing;
        private string filter;
        private List<Land> landsList;
        #endregion

        #region Properties  
        public ObservableCollection<Land> Lands //está "Lands" es la que vamos a mandar a la LandPage para mostrar en listview
        {
            get { return this.lands; }
            set { SetValue(ref this.lands, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public string Filter
        {
            get { return this.filter; }
            set
            {
                SetValue(ref this.filter, value);
                this.Search();
            }
        }
        #endregion

        #region Constructors
        public LandsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadLands();
        }
        #endregion

        #region Methods
        private async void LoadLands()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();//si no hay conexion lo regresa al login
                return;
            }

            var response = await this.apiService.GetList<Land>(
                "http://restcountries.eu",
                "/rest",
                "/v2/all");

            if (!response.IsSuccess)//si no devuelve la lista y pasa algo malo malo bien malo
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            this.landsList = (List<Land>)response.Result;//como devuelve un object hay que castearlo como una lista de Land y se usa para mantener todo el tiempo en memoria la lista original
            this.Lands = new ObservableCollection<Land>(this.landsList);//aquí el objeto ya está en memoria fuck yes!!!
            this.IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadLands);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }
        #endregion

        private void Search()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                this.Lands = new ObservableCollection<Land>(
                    this.landsList);
            }
            else
            {
                this.Lands = new ObservableCollection<Land>(
                    this.landsList.Where(
                        l => l.Name.ToLower().Contains(this.Filter.ToLower()) ||//.where va a filtrar el Filter de landview por paises
                             l.Capital.ToLower().Contains(this.Filter.ToLower())));//.where va a filtrar el Filter de landview por capitales
            }
        }
    }
}
