using MyMovieList.Base;
using MyMovieList.Models;
using MyMovieList.Repositories;
using MyMovieList.Services;
using MyMovieList.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyMovieList.ViewModels
{
    public class UsuarioViewModel: ViewModelBase
    {
        RepositoryMyMovieList repo;
        SessionService session;
        String oldpass;
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
                        PrincipalMaster view = new PrincipalMaster();
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
                        this.Error = "Usuario creado";
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
                        PerfilView view = new PerfilView();
                        UsuarioViewModel viewmodel = new UsuarioViewModel();
                        viewmodel.Usuario = session.Usuario;
                        view.BindingContext = viewmodel;
                        await Application.Current.MainPage.Navigation.PushModalAsync(view);
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
