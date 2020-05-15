using GPetS.Models;
using GPetS.Services;
using GPetS.Views;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GPetS.ViewModels
{
    public class PetsDetailViewModel : BaseViewModel
    {
        Command saveCommand;
        public Command SaveCommand => saveCommand ?? (saveCommand = new Command(SaveAction));

        Command deleteCommand;
        public Command DeleteCommand => deleteCommand ?? (deleteCommand = new Command(DeleteAction));

        Command _mapCommand;
        public Command MapCommand => _mapCommand ?? (_mapCommand = new Command(MapAction));

        Command _GetLocationCommand;
        public Command GetLocationCommand => _GetLocationCommand ?? (_GetLocationCommand = new Command(GetLocationAction));

        Command cancelCommand;
        public Command CancelCommand => cancelCommand ?? (cancelCommand = new Command(CancelAction));

        Command _TakePictureCommand;
        public Command TakePictureCommand => _TakePictureCommand ?? (_TakePictureCommand = new Command(TakePictureAction));

        Command _SelectPictureCommand;
        public Command SelectPictureCommand => _SelectPictureCommand ?? (_SelectPictureCommand = new Command(SelectPictureAction));

        PetModel petSelected;
        public PetModel PetSelected
        {
            get => petSelected;
            set => SetProperty(ref petSelected, value);
        }

        ImageSource imageSource_;
        public ImageSource ImageSource_
        {
            get => imageSource_;
            set => SetProperty(ref imageSource_, value);
        }

        string _ImageBase64;
        public string ImageBase64
        {
            get => _ImageBase64;
            set => SetProperty(ref _ImageBase64, value);
        }

        string _ImageUrl;
        public string ImageUrl
        {
            get => _ImageUrl;
            set => SetProperty(ref _ImageUrl, value);
        }

        string _Name;
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        DateTime _PetDate;
        public DateTime PetDate
        {
            get => _PetDate;
            set => SetProperty(ref _PetDate, value);
        }

        string _Gender;
        public string Gender
        {
            get => _Gender;
            set => SetProperty(ref _Gender, value);
        }

        string _Race;
        public string Race
        {
            get => _Race;
            set => SetProperty(ref _Race, value);
        }

        string _Weight;
        public string Weight
        {
            get => _Weight;
            set => SetProperty(ref _Weight, value);
        }

        string _Comments;
        public string Comments
        {
            get => _Comments;
            set => SetProperty(ref _Comments, value);
        }

        double _Latitude;
        public double Latitude
        {
            get => _Latitude;
            set => SetProperty(ref _Latitude, value);
        }

        double _Longitude;
        public double Longitude
        {
            get => _Longitude;
            set => SetProperty(ref _Longitude, value);
        }

        public PetsDetailViewModel()
        {
            PetSelected = new PetModel();
        }



        public PetsDetailViewModel(PetModel petSelected)
        {
            /*if (!string.IsNullOrEmpty(petSelected.ImageBase64))
            {
                ImageSource_ = new ImageService().ConvertImageFromBase64ToImageSource(petSelected.ImageBase64);
            }*/
            PetSelected = petSelected;
            ImageBase64 = petSelected.ImageBase64;
        }

        private async void SaveAction()
        {
            /*if (!string.IsNullOrEmpty(petSelected.ImageUrl))
            {
                PetSelected.ImageBase64 = await new ImageService().DownloadImageAsBase64Async(petSelected.ImageUrl);
            }*/
            await App.PetsDatabase.SavePetAsync(PetSelected);
            PetsListViewModel.GetInstance().LoadPets();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void DeleteAction()
        {
            await App.PetsDatabase.DeletePetAsync(PetSelected);
            PetsListViewModel.GetInstance().LoadPets();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void CancelAction()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private void MapAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new PetMapPage(new PetModel
            {
                ID = PetSelected.ID,
                Name = PetSelected.Name,
                ImageUrl = PetSelected.ImageUrl,
                Gender = PetSelected.Gender,
                Race = PetSelected.Race,
                Comments = PetSelected.Comments,
                Latitude = PetSelected.Latitude,
                Longitude = PetSelected.Longitude,
            }));
        }

        private async void GetLocationAction()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                }
            }
            catch (Exception ex){}
        }

        private async void TakePictureAction()
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                await CrossMedia.Current.Initialize();
            }

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            PetSelected.ImageBase64 = ImageBase64 = await new ImageService().ConvertImageFileToBase64(file.Path);
            //ImageUrl = await new ImageService().ConvertImageFileToBase64(file.Path);
            //await Application.Current.MainPage.DisplayAlert("File Location", file.Path, "OK");
        }

        private async void SelectPictureAction()
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                await CrossMedia.Current.Initialize();
            }

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Not supported", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });

            if (file == null)
                return;

            PetSelected.ImageBase64 = ImageBase64 = await new ImageService().ConvertImageFileToBase64(file.Path);
        }
    }
}
