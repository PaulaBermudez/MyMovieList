using MyMovieList.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TMDbLib.Objects.Movies;

namespace MyMovieList.Services
{
    public class SessionService
    {
        public String Token { get; set; }
        public Usuario Usuario { get; set; }
        public List<Movie> ListaPeliculas { get; set; } 
        public SessionService()
        {
            this.Usuario = new Usuario();
            this.ListaPeliculas = new List<Movie>();
        }
    }
}
