using MyMovieList.Models;
using MyMovieList.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyMovieList.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaginaMaestra : MasterDetailPage
	{
        public List<PaginaMenu> MiMenu { get; set; }
        public PaginaMaestra()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            MiMenu = new List<PaginaMenu>();
            PaginaMenu pag1 = new PaginaMenu() { Titulo = "Perfil", Pagina = typeof(LoginView) };
            MiMenu.Add(pag1);

            PaginaMenu pag2 = new PaginaMenu() { Titulo = "Lista Usuarios", Pagina = typeof(ListaUsuario) };
            MiMenu.Add(pag2);

            this.lsvmenu.ItemsSource = MiMenu;
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Tabbed)));

            this.lsvmenu.ItemSelected += Lsvmenu_ItemSelected;
            this.btnMML.Clicked += BtnMML_Clicked;
        }

        private void BtnMML_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Tabbed)));
            IsPresented = false;
        }

        private void Lsvmenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PaginaMenu pagina = e.SelectedItem as PaginaMenu;
            Detail = new NavigationPage((Page)Activator.CreateInstance(pagina.Pagina));
            IsPresented = false;
        }

        private void  SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            String Parametro = this.sbBuscar.Text;

            ListasView listasView = new ListasView();
            ListasViewModel listasViewmodel = new ListasViewModel(Parametro);
            listasView.BindingContext = listasViewmodel;
            Detail = new NavigationPage(listasView);

            IsPresented = false;
        }
    }
}

