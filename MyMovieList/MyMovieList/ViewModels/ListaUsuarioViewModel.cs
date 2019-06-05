using MyMovieList.Base;
using MyMovieList.Models;
using MyMovieList.Repositories;
using MyMovieList.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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
            List<Usuario> lista = await this.repo.GetUsuarios(token);
            this.Usuarios = new ObservableCollection<Usuario>(lista);
        }

        private ObservableCollection<Usuario> _Usuarios;
        public ObservableCollection<Usuario> Usuarios
        {
            get { return this._Usuarios; }
            set
            {
                this._Usuarios = value;
                OnPropertyChanged("Usuarios");
            }
        }


        public Command EliminarUsuario
        {
            get
            {
                return new Command(async (user) =>
                {
                    Usuario usuario = user as Usuario;
                    await this.repo.EliminarUsuario(usuario.NombreUsuario, session.Token);
                    await this.CargarListas(session.Token);
                });
            }
        }
    }
}
