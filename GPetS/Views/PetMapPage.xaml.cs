using GPetS.Models;
using GPetS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace GPetS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PetMapPage : ContentPage
    {
        public PetMapPage(PetModel petSelected)
        {
            InitializeComponent();

            MapPet.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Position(petSelected.Latitude, petSelected.Longitude),
                    Distance.FromMiles(.5)
            ));

            string imagePath = new ImageService().SaveImageFromBase64(petSelected.ImageBase64, petSelected.ID);
            petSelected.ImageBase64 = imagePath;
            //petSelected.ImageBase64 = new ImageService().SaveImageFromBase64(petSelected.ImageBase64);
            MapPet.Pet = petSelected;

            MapPet.Pins.Add(
                new Pin
                {
                    Type = PinType.Place,
                    Label = petSelected.Name,
                    Position = new Position(petSelected.Latitude, petSelected.Longitude)
                }
            );

            Name.Text = petSelected.Name;
            Race.Text = petSelected.Race;
            Gender.Text = petSelected.Gender;
            Date.Text = petSelected.PetDate.ToShortDateString();
            Comments.Text = petSelected.Comments;
        }
    }
}