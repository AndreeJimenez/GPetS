using System;
using System.Collections.Generic;
using GPetS.Models;
using GPetS.Views;
using Xamarin.Forms;

namespace GPetS.ViewModels
{
    public class PetsListViewModel : BaseViewModel
    {
        static PetsListViewModel instance;

        Command newPetCommand;
        public Command NewPetCommand => newPetCommand ?? (newPetCommand = new Command(NewPetAction));

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
                    SelectPetAction();
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
        }

        private void NewPetAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new PetsDetailPage());
        }

        private void SelectPetAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new PetsDetailPage(PetSelected));
        }
    }
}
