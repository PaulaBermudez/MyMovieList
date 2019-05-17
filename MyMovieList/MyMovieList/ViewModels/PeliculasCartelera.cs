using MyMovieList.Base;
using MyMovieList.Repositories;
using MyMovieList.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using Xamarin.Forms;

namespace MyMovieList.ViewModels
{
    public class PeliculasCartelera : ViewModelBase
    {
        RepositoryMyMovieList repo;

        public PeliculasCartelera()
        {
            this.repo = new RepositoryMyMovieList();
            Task.Run(async () => {
                await this.CargarPeliculas();
            });
        }

        private async Task CargarPeliculas()
        {
            SearchContainer<SearchMovie> lista = await this.repo.GetPeliculasCartelera();
            this.Peliculas = new ObservableCollection<SearchMovie>(lista.Results);
        }

        private ObservableCollection<SearchMovie> _Peliculas;
        public ObservableCollection<SearchMovie> Peliculas
        {
            get { return this._Peliculas; }
            set
            {
                this._Peliculas = value;
                OnPropertyChanged("Peliculas");
            }
        }

        public Command DetallesPelicula
        {
            get
            {
                return new Command(async (pelicula) => {
                    DetallesPeliculaCartelera view = new DetallesPeliculaCartelera();
                    PeliculaCartelera viewmodel = new PeliculaCartelera();
                    viewmodel.Pelicula = pelicula as SearchMovie;
                    view.BindingContext = viewmodel;
                    await Application.Current.MainPage.Navigation.PushModalAsync(view);

                });
            }
        }

    }
}
