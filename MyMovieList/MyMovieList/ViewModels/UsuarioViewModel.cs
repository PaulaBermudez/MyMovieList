using MyMovieList.Base;
using MyMovieList.Models;
using MyMovieList.Repositories;
using MyMovieList.Services;
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
                        session.Usuario = Usuario;
                        this.Error = "Usuario/Password correctos";
                        //PerfilView view = new PerfilView();
                        //await Application.Current.MainPage.Navigation.PushModalAsync(view);
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
