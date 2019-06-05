using MyMovieList.Services;
using MyMovieList.ViewModels;
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
	public partial class PerfilView : ContentPage
	{
		public PerfilView ()
		{
			InitializeComponent ();
            SessionService session = App.Locator.SessionService;
            if(session.ListaPeliculas.First() == null)
            {
                
            }
            else
            {
                this.lstPelis.ItemsSource = session.ListaPeliculas;
            }
            
		}
	}
}