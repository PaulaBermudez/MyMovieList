using MyMovieList.Base;
using MyMovieList.Models;
using MyMovieList.Repositories;
using MyMovieList.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.ViewModels
{
    public class ListaUsuarioViewModel :ViewModelBase
    {

        RepositoryMyMovieList repo;
        SessionService session;


        public ListaUsuarioViewModel()
        {
            this.repo = new RepositoryMyMovieList();

            session = App.Locator.SessionService;
            Task.Run(async () => {
                await this.CargarListas(session.Token);
            });
        }

        private async Task CargarListas(String token)
        {
            this.Usuarios = await this.repo.GetUsuarios(token);
        }

        private List<Usuario> _Usuarios;
        public List<Usuario> Usuarios
        {
            get { return this._Usuarios; }
            set
            {
                this._Usuarios = value;
                OnPropertyChanged("LUsuario");
            }
        }
    }
}
