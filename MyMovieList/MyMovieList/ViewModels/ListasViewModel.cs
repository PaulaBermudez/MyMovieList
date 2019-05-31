using MyMovieList.Base;
using MyMovieList.Models;
using MyMovieList.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using System.Linq;
using Xamarin.Forms;
using MyMovieList.Views;
using TMDbLib.Objects.Movies;
using System.Windows.Input;

namespace MyMovieList.ViewModels
{
    public class ListasViewModel : ViewModelBase
    {
        RepositoryMyMovieList repo;

        public ListasViewModel()
        {
            this.repo = new RepositoryMyMovieList();
            Task.Run(async () => {
                await this.CargarListas();
            });
        }

        public ListasViewModel(String s)
        {
            this.repo = new RepositoryMyMovieList();
            Task.Run(async () => {
                await this.Busqueda(s);
            });
        }

        private async Task CargarListas()
        {
            SearchContainer<SearchMovie> lista = await this.repo.GetPeliculasPopulares();
            this.Listas = new ObservableCollection<SearchMovie>(lista.Results);
        }

        public async Task Busqueda(String text)
        {
            SearchContainer<SearchMovie> lista = await this.repo.BuscarPeliculas(text);
            this.Listas = new ObservableCollection<SearchMovie>(lista.Results);
        }

        private ObservableCollection<SearchMovie> _Listas;
        public ObservableCollection<SearchMovie> Listas
        {
            get { return this._Listas; }
            set
            {
                this._Listas = value;
                OnPropertyChanged("Listas");
            }
        }


        public Command DetallesPelicula
        {
            get
            {
                return new Command(async (pelicula) => {
                    DetallesPeliculaPopular view = new DetallesPeliculaPopular();
                    PeliculaViewModel viewmodel = new PeliculaViewModel();
                    SearchMovie movie = pelicula as SearchMovie;
                    Movie peli = await this.repo.DetallesPelicula(movie.Id);
                    Credits actores = await this.repo.RepartoPelicula(movie.Id);
                    ImagesWithId imagenes = await this.repo.ImagenesPelicula(movie.Id);
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

        //private ICommand _searchCommand;
        //public ICommand SearchCommand
        //{
        //    get
        //    {
        //        return _searchCommand ?? (_searchCommand = new Command<string>(async (text) =>
        //        {
        //            ListasView listasView = new ListasView();
        //            ListasViewModel listasViewmodel = new ListasViewModel();
        //            SearchContainer<SearchMovie> lista = await this.repo.BuscarPeliculas(text);
        //            listasViewmodel.Listas = new ObservableCollection<SearchMovie>(lista.Results);
        //            listasView.BindingContext = listasViewmodel;
        //            await Application.Current.MainPage.Navigation.PushModalAsync(listasView);
        //        }));
        //    }
        //}
    }
}
