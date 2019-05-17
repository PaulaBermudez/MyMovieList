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
    public class PeliculasProximamente : ViewModelBase
    {
        RepositoryMyMovieList repo;

        public PeliculasProximamente()
        {
            this.repo = new RepositoryMyMovieList();
            Task.Run(async () => {
                await this.CargarPeliculas();
            });
        }

        private async Task CargarPeliculas()
        {
            SearchContainer<SearchMovie> lista = await this.repo.GetPeliculasProximamente();
            this.PeliculasProx = new ObservableCollection<SearchMovie>(lista.Results);
        }

        private ObservableCollection<SearchMovie> _PeliculasProx;
        public ObservableCollection<SearchMovie> PeliculasProx
        {
            get { return this._PeliculasProx; }
            set
            {
                this._PeliculasProx = value;
                OnPropertyChanged("PeliculasProx");
            }
        }

        public Command DetallesPelicula
        {
            get
            {
                return new Command(async (pelicula) => {
                    DetallesPeliculaProximamente view = new DetallesPeliculaProximamente();
                    PeliculaProximamente viewmodel = new PeliculaProximamente();
                    viewmodel.Pelicula = pelicula as SearchMovie;
                    view.BindingContext = viewmodel;
                    await Application.Current.MainPage.Navigation.PushModalAsync(view);

                });
            }
        }
    }
}
