using GPetS.Models;
using GPetS.Views;
using GPetS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace GPetS.ViewModels
{
    public class PetsListViewModel : BaseViewModel
    {
        static PetsListViewModel instance;

        Command refreshCommand;
        public Command RefreshCommand => refreshCommand ?? (refreshCommand = new Command(LoadPets));

        Command _newCommand;
        public Command NewCommand => _newCommand ?? (_newCommand = new Command(NewAction));

        Command _selectCommand;
        public Command SelectCommand => _selectCommand ?? (_selectCommand = new Command(SelectAction));

        List<PetModel> pets;
        public List<PetModel> Pets
        {
            get => pets;
            set => SetProperty(ref pets, value);
        }

        PetModel petSelected;
        public PetModel PetSelected
        {
            get => petSelected;
            set
            {
                if (SetProperty(ref petSelected, value))
                {
                    SelectAction();
                }
            }
        }

        public PetsListViewModel()
        {
            instance = this;

            LoadPets();
        }

        public static PetsListViewModel GetInstance()
        {
            if (instance == null) instance = new PetsListViewModel();
            return instance;
        }

        public async void LoadPets()
        {
            Pets = await App.PetsDatabase.GetAllPetsAsync();
            IsBusy = false;
        }

        private void NewAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new PetsDetailPage());
        }

        private void SelectAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new PetsDetailPage(PetSelected));
        }
    }
}
