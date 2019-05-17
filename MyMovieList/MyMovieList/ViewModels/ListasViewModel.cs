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

        private async Task CargarListas()
        {
            SearchContainer<SearchMovie> lista = await this.repo.GetPeliculasPopulares();
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
                    ListaViewModel viewmodel = new ListaViewModel();
                    viewmodel.Lista = pelicula as SearchMovie;
                    view.BindingContext = viewmodel;
                    await Application.Current.MainPage.Navigation.PushModalAsync(view);

                });
            }
        }
    }
}
