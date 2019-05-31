﻿using MyMovieList.Base;
using MyMovieList.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace MyMovieList.ViewModels
{
    public class PeliculaViewModel : ViewModelBase
    {
        RepositoryMyMovieList repo;

        public PeliculaViewModel()
        {
            this.repo = new RepositoryMyMovieList();
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
