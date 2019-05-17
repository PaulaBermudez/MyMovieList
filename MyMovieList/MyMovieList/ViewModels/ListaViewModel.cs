using MyMovieList.Base;
using MyMovieList.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using TMDbLib.Objects.Search;

namespace MyMovieList.ViewModels
{
    public class ListaViewModel:ViewModelBase
    {
        RepositoryMyMovieList repo;

        public ListaViewModel()
        {
            this.repo = new RepositoryMyMovieList();
            if(Lista == null)
            {
                Lista = new SearchMovie();
            }
        }

        private SearchMovie _Lista;
        public SearchMovie Lista
        {
            get
            {
                return this._Lista;
            }

            set
            {
                this._Lista = value;
                OnPropertyChanged("Lista");
            }
        }
    }
}
