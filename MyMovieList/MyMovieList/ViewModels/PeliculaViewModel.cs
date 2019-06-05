using MyMovieList.Base;
using MyMovieList.Repositories;
using MyMovieList.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using Xamarin.Forms;

namespace MyMovieList.ViewModels
{
    public class PeliculaViewModel : ViewModelBase
    {
        RepositoryMyMovieList repo;
        SessionService session;
        public PeliculaViewModel()
        {
            this.repo = new RepositoryMyMovieList();
            session = App.Locator.SessionService;
            if (Pelicula == null)
            {
                Pelicula = new Movie();
            }
            if (Actores == null)
            {
                Actores = new Credits();
            }
            if (Imagenes == null)
            {
                Imagenes = new ImagesWithId();
            }
        }
        public Command AñadirPelicula
        {
            get
            {
                return new Command(async () =>
                {
                        session.ListaPeliculas.Add(this.Pelicula);
                        await this.repo.AñadirPelicula(this.Pelicula.Id, session.Token);
                        MessagingCenter.Send<UsuarioViewModel>
                        (App.Locator.UsuarioViewModel, "UPDATE");
                });
            }
        }
        public Command EliminarPelicula
        {
            get
            {
                return new Command(async () =>
                {
                        await this.repo.EliminarPelicula(this.Pelicula.Id, session.Token);
                        MessagingCenter.Send<UsuarioViewModel>
                        (App.Locator.UsuarioViewModel, "UPDATE");
                        session.ListaPeliculas.Remove(this.Pelicula);                
                });
            }
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
        private Credits _Actores;
        public Credits Actores
        {
            get
            {
                return this._Actores;
            }
            set
            {
                this._Actores = value;
                OnPropertyChanged("Actores");
            }
        }
        private ImagesWithId _Imagenes;
        public ImagesWithId Imagenes
        {
            get
            {
                return this._Imagenes;
            }
            set
            {
                this._Imagenes = value;
                OnPropertyChanged("Imagenes");
            }
        }
        private List<Genre> _Generos;
        public List<Genre> Generos
        {
            get
            {
                return this._Generos;
            }
            set
            {
                this._Generos = value;
                OnPropertyChanged("Generos");
            }
        }
    }
}
