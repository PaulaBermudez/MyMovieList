﻿using MyMovieList.Base;
using MyMovieList.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using TMDbLib.Objects.Search;

namespace MyMovieList.ViewModels
{
    public class PeliculaProximamente:ViewModelBase
    {
        RepositoryMyMovieList repo;

        public PeliculaProximamente()
        {
            this.repo = new RepositoryMyMovieList();
            if(Pelicula == null)
            {
                Pelicula = new SearchMovie();
            }
        }

        private SearchMovie _Pelicula;
        public SearchMovie Pelicula
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
