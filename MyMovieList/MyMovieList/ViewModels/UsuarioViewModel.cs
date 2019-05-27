using MyMovieList.Base;
using MyMovieList.Models;
using MyMovieList.Repositories;
using MyMovieList.Services;
using MyMovieList.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyMovieList.ViewModels
{
    public class UsuarioViewModel: ViewModelBase
    {
        RepositoryMyMovieList repo;
        public UsuarioViewModel()
        {
            this.repo = new RepositoryMyMovieList();
            if (this.Usuario == null)
            {
                this.Usuario = new Usuario();
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
                        SessionService session = App.Locator.SessionService;
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
