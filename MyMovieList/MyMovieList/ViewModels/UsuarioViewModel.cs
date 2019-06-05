using MyMovieList.Base;
using MyMovieList.Models;
using MyMovieList.Repositories;
using MyMovieList.Services;
using MyMovieList.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using Xamarin.Forms;

namespace MyMovieList.ViewModels
{
    public class UsuarioViewModel : ViewModelBase
    {
        RepositoryMyMovieList repo;
        SessionService session;
        String oldpass;
        String APIKey = "c2b4a384b10d259a72d4671a10065bc6";
        public UsuarioViewModel()
        {
            this.repo = new RepositoryMyMovieList();
            session = App.Locator.SessionService;
            this.Usuario = session.Usuario;
            if (this.Usuario == null)
            {
                this.Usuario = new Usuario();
            }
            if (session.Usuario != null)
            {
                oldpass = session.Usuario.Password;
            }
        }
        public async Task CargarUsuario()
        {
            Usuario usuario = await this.repo.PerfilUsuario(session.Token);
            this.Usuario = usuario;
        }
        public async Task GetListaPeliculas()
        {
            List<String> lista = await this.repo.ListaUsuario(session.Token);
            foreach (String id in lista)
            {
                TMDbClient client = new TMDbClient(APIKey);
                Movie peli = client.GetMovieAsync(id, "es").Result;
                session.ListaPeliculas.Add(peli);
            }
            this.ListaUsuario = session.ListaPeliculas;
        }
        public Command DetallesPelicula
        {
            get
            {
                return new Command(async (pelicula) =>
                {
                    DetallesPeliculaPopular view = new DetallesPeliculaPopular();
                    PeliculaViewModel viewmodel = new PeliculaViewModel();
                    Movie peli = pelicula as Movie;
                    //Movie peli = await this.repo.DetallesPelicula(movie.Id);
                    Credits actores = await this.repo.RepartoPelicula(peli.Id);
                    ImagesWithId imagenes = await this.repo.ImagenesPelicula(peli.Id);
                    List<Genre> generos = peli.Genres;
                    viewmodel.Pelicula = peli as Movie;
                    viewmodel.Actores = actores as Credits;
                    viewmodel.Imagenes = imagenes as ImagesWithId;
                    viewmodel.Generos = generos as List<Genre>;
                    view.BindingContext = viewmodel;
                    await Application.Current.MainPage.Navigation.PushModalAsync(view);
                    
                });
            }
        }
        public Command ListaPeliculas
        {
            get
            {
                return new Command(async () =>
                {
                    ListaPeliculasUsuario view = new ListaPeliculasUsuario();
                    await Application.Current.MainPage.Navigation.PushModalAsync(view);
                    MessagingCenter.Subscribe<UsuarioViewModel>
                    (this, "UPDATE", async (sender) =>
                    {
                        await this.GetListaPeliculas();
                    });
                });
            }
        }
        private Usuario _Usuario;
        public Usuario Usuario
        {
            get { return this._Usuario; }
            set
            {
                this._Usuario = value;
                OnPropertyChanged("Usuario");
            }
        }
        private List<Movie> _ListaUsuario;
        public List<Movie> ListaUsuario
        {
            get { return this._ListaUsuario; }
            set
            {
                this._ListaUsuario = value;
                OnPropertyChanged("ListaUsuario");
            }
        }
        public Command Registro
        {
            get
            {
                return new Command(async () =>
                {
                    RegistroView view = new RegistroView();
                    await Application.Current.MainPage.Navigation.PushModalAsync(view);
                });
            }
        }
        public Command EditarUsuario
        {
            get
            {
                return new Command(async () =>
                {
                    EditarUsuarioView view = new EditarUsuarioView();
                    UsuarioViewModel viewmodel = new UsuarioViewModel();
                    viewmodel.Usuario = session.Usuario;
                    view.BindingContext = viewmodel;
                    await Application.Current.MainPage.Navigation.PushModalAsync(view);
                    MessagingCenter.Subscribe<UsuarioViewModel>(this, "UPDATE", async (sender) =>
                    {
                        await this.CargarUsuario();
                    });
                });
            }
        }
        public Command ValidarEdicion
        {
            get
            {
                return new Command(async () =>
                {
                    if (this.Usuario.Password == oldpass && this.Usuario.Password == this.Usuario.Password2)
                    {
                        await this.repo.EditarUsuario(this.Usuario);
                        EditarUsuarioView view = new EditarUsuarioView();
                        UsuarioViewModel viewmodel = new UsuarioViewModel();
                        viewmodel.Usuario = session.Usuario;
                        view.BindingContext = viewmodel;
                        await Application.Current.MainPage.Navigation.PopModalAsync(true);
                        MessagingCenter.Send<UsuarioViewModel>(App.Locator.UsuarioViewModel, "UPDATE");
                    }
                    else if (this.Usuario.Password != this.Usuario.Password2)
                    {
                        this.Error = "Las contraseñas no coinciden";
                    }
                    else
                    {
                        await this.repo.EditarUsuario(this.Usuario);
                        session.Usuario = null;
                        session.Token = null;
                        PaginaMaestra view = new PaginaMaestra();
                        await Application.Current.MainPage.Navigation.PushModalAsync(view);
                    }
                });
            }
        }

        public Command NuevoUsuario
        {
            get
            {
                return new Command(async () =>
                {
                    if (this.Usuario.Password == this.Usuario.Password2 && this.Usuario.Email.Contains("@"))
                    {
                        await this.repo.CrearUsuario(this.Usuario);
                        PaginaMaestra view = new PaginaMaestra();
                        await Application.Current.MainPage.Navigation.PushModalAsync(view);
                    }
                    else if (this.Usuario.Password != this.Usuario.Password2)
                    {
                        this.Error = "Las contraseñas no coinciden";
                    }
                    else
                    {
                        this.Error = "El email no es válido";
                    }
                });
            }
        }
        public Command IniciarSesion
        {
            get
            {
                return new Command(async () =>
                {
                    String token = await this.repo.GetToken(this.Usuario.NombreUsuario, this.Usuario.Password);
                    if (token == null)
                    {
                        this.Error = "Usuario/Password incorrectos";
                    }
                    else
                    {
                        session.Token = token;
                        session.Usuario = await this.repo.PerfilUsuario(session.Token);
                        PaginaMaestra view = new PaginaMaestra();
                        await Application.Current.MainPage.Navigation.PushModalAsync(view);
                        await this.GetListaPeliculas();
                    }
                });
            }
        }
        private String _Error;
        public String Error
        {
            get { return this._Error; }
            set
            {
                this._Error = value;
                OnPropertyChanged("Error");
            }
        }
    }
}
