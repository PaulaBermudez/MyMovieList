using MyMovieList.Models;
using MyMovieList.Services;
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
        SessionService session;
        public PaginaMaestra()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            this.Paginas();

            this.lsvmenu.ItemSelected += Lsvmenu_ItemSelected;
            //this.btnMML.Clicked += BtnMML_Clicked;
        }
        public void Paginas()
        {
            this.MiMenu = new List<PaginaMenu>();
            session = App.Locator.SessionService;
            var home = new PaginaMenu()
            {
                Titulo = "Home",
                Icono = "Home.png",
                Pagina = typeof(Tabbed)
            };
            this.MiMenu.Add(home);
            if (session.Token == null || session.Token == "")
            {
                var page1 = new PaginaMenu()
                {
                    Titulo = "Login",
                    Icono = "Login.png",
                    Pagina = typeof(LoginView)
                };
                this.MiMenu.Add(page1);
            }
            else if(session.Usuario.NombreUsuario == "admin")
            {
                var perfil = new PaginaMenu()
                {
                    Titulo = "Perfil",
                    Icono = "Perfil.png",
                    Pagina = typeof(PerfilView)
                };
                this.MiMenu.Add(perfil);

                var lista = new PaginaMenu()
                {
                    Titulo = "Mi lista",
                    Icono = "Lista.png",
                    Pagina = typeof(ListaPeliculasUsuario)
                };
                this.MiMenu.Add(lista);
                var listausuarios = new PaginaMenu()
                {
                    Titulo = "Administración",
                    Icono = "Usuarios.png",
                    Pagina = typeof(ListaUsuario)
                };
                this.MiMenu.Add(listausuarios);
                var logout = new PaginaMenu()
                {
                    Titulo = "Logout",
                    Icono = "Logout.png",
                    Pagina = typeof(Tabbed)
                };
                this.MiMenu.Add(logout);
            }
            else
            {
                var perfil = new PaginaMenu()
                {
                    Titulo = "Perfil",
                    Icono = "Perfil.png",
                    Pagina = typeof(PerfilView)
                };
                this.MiMenu.Add(perfil);

                var lista = new PaginaMenu()
                {
                    Titulo = "Mi lista",
                    Icono = "Lista.png",
                    Pagina = typeof(ListaPeliculasUsuario)
                };
                this.MiMenu.Add(lista);

                var logout = new PaginaMenu()
                {
                    Titulo = "Logout",
                    Icono = "Logout.png",
                    Pagina = typeof(Tabbed)
                };
                this.MiMenu.Add(logout);
            }
            this.lsvmenu.ItemsSource = this.MiMenu;
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Tabbed)));
            IsPresented = false;
        }

        //private void BtnMML_Clicked(object sender, EventArgs e)
        //{
        //    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Tabbed)));

        //    IsPresented = false;          
        //}

        private void Lsvmenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PaginaMenu pagina = e.SelectedItem as PaginaMenu;
            Detail = new NavigationPage((Page)Activator.CreateInstance(pagina.Pagina));
            IsPresented = false;
            if (pagina.Titulo == "Logout")
            {
                session.Token = null;
                session.Usuario = null;
                session.ListaPeliculas = null;
                this.Paginas();
            }
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
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

