using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMovieList.Models
{
    public class Lista
    {
        [JsonProperty("ID_LISTA")]
        public int IdLista { get; set; }
        [JsonProperty("PELICULAS")]
        public String Peliculas { get; set; }
        [JsonProperty("USUARIO")]
        public String Usuario { get; set; }
    }
}
