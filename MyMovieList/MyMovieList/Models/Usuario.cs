using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMovieList.Models
{
    public class Usuario
    {
        [JsonProperty("IdUsuario")]
        public int IdUsuario { get; set; }
        [JsonProperty("NombreUsuario")]
        public String NombreUsuario { get; set; }
        [JsonProperty("Email")]
        public String Email { get; set; }
        [JsonProperty("Password")]
        public String Password { get; set; }
        [JsonProperty("Password2")]
        public String Password2 { get; set; }
        [JsonProperty("FotoPerfil")]
        public String FotoPerfil { get; set; }
        [JsonProperty("Lista")]
        public int Lista { get; set; }
    }
}
