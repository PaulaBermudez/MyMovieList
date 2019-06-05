using MyMovieList.Models;
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
	public partial class ListaUsuario : ContentPage
	{
		public ListaUsuario ()
		{
			InitializeComponent ();
		}

        private async void  ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PerfilView view = new PerfilView();
            UsuarioViewModel viewmodel = new UsuarioViewModel();
            viewmodel.Usuario = e.SelectedItem as Usuario;
            view.BindingContext = viewmodel;
            await Application.Current.MainPage.Navigation.PushModalAsync(view);
        }
    }
}