using MyMovieList.Base;
using MyMovieList.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Objects.Movies;

namespace MyMovieList.ViewModels
{
    public class PeliculaViewModel : ViewModelBase
    {
        RepositoryMyMovieList repo;

        public PeliculaViewModel()
        {
            this.repo = new RepositoryMyMovieList();
            if(Pelicula == null)
            {
                Pelicula = new Movie();
            }
            Task.Run(async () => {
                await this.CargarPelicula();
            });
        }
        private async Task CargarPelicula()
        {
            Movie pelicula = await this.repo.DetallesPelicula(this.Pelicula.Id);
            this.Pelicula = pelicula;
        }
        private Movie _Pelicula;
        public Movie Pelicula
        {
            get
            {
                return this._Pelicula;
            }
            set
            {
                this._Pelicula = value;
                OnPropertyChanged("Pelicula");
            }
        }
    }
}
