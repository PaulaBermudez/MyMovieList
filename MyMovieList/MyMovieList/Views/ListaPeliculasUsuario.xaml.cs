using MyMovieList.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyMovieList.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaPeliculasUsuario : ContentPage
	{
		public ListaPeliculasUsuario ()
		{
			InitializeComponent ();
            SessionService session = App.Locator.SessionService;
            this.lstPelis.ItemsSource = session.ListaPeliculas;
        }
	}
}