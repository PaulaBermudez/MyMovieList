using MyMovieList.Repositories;
using MyMovieList.Services;
using MyMovieList.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Objects.Movies;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyMovieList.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetallesPeliculaPopular : ContentPage
	{
		public DetallesPeliculaPopular()
		{
			InitializeComponent ();
            //this.botonañadir.Clicked += Botonañadir_Clicked;
            //this.botoneliminar.Clicked += Botoneliminar_Clicked;
		}

        //private void Botoneliminar_Clicked(object sender, EventArgs e)
        //{
        //    this.botoneliminar.IsEnabled = false;
        //}

        //private void Botonañadir_Clicked(object sender, EventArgs e)
        //{
        //    this.botonañadir.IsEnabled = false;
        //}
    }
}