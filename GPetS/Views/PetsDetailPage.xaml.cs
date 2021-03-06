﻿using GPetS.Models;
using GPetS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GPetS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PetsDetailPage : ContentPage
    {
        public PetsDetailPage()
        {
            InitializeComponent();

            BindingContext = new PetsDetailViewModel();
        }

        public PetsDetailPage(PetModel petSelected)
        {
            InitializeComponent();

            BindingContext = new PetsDetailViewModel(petSelected);
        }
    }
}