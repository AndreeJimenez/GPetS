using System;
using GPetS.Data;
using GPetS.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GPetS
{
    public partial class App : Application
    {
        static PetsDatabase petsDatabase;
        public static PetsDatabase PetsDatabase
        {
            get
            {
                if (petsDatabase == null) petsDatabase = new PetsDatabase();
                return petsDatabase;
            }
        }

        public App()
        {
            InitializeComponent();

            var nav = new NavigationPage(new PetsListPage());
            nav.BarBackgroundColor = (Color)App.Current.Resources["primaryColor"];
            nav.BarTextColor = Color.White;
            MainPage = nav;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
