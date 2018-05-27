﻿using Lands.Models;
using Lands.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        #endregion

        #region Properties  
        public ObservableCollection<Land> Lands
        {
            get { return this.lands; }
            set { SetValue(ref this.lands, value); }
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
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
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
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            var list = (List<Land>)response.Result;//como devuelve un object hay que castearlo como una lista de Land
            this.Lands = new ObservableCollection<Land>(list);//aquí el objeto ya está en memoria fuck yes!!!
        } 
        #endregion
    }
}
