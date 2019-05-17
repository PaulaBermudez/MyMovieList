using MyMovieList.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace MyMovieList.Repositories
{
    public class RepositoryMyMovieList
    {
        String urlapi;

        public RepositoryMyMovieList()
        {
            this.urlapi = "https://mymovielistapi.azurewebsites.net/";
        }

        private HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
        public async Task<String> GetToken(String usuario
            , String password)
        {
            using (HttpClient client = this.GetHttpClient())
            {
                client.BaseAddress = new Uri(this.urlapi);
                LoginModel login = new LoginModel();
                login.NombreUsuario = usuario;
                login.Password = password;
                String json = JsonConvert.SerializeObject(login);
                StringContent content =
                    new StringContent(json
                    , System.Text.Encoding.UTF8, "application/json");
                String peticion = "Auth/Login";
                HttpResponseMessage response =
                    await client.PostAsync(peticion, content);
                if (response.IsSuccessStatusCode)
                {
                    String contenido =
                        await response.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(contenido);
                    return jObject.GetValue("response").ToString();
                }
                else
                {
                    return null;
                }
            }
        }
        public async Task CrearUsuario(Usuario user)
        {
            String jsonusuario = JsonConvert.SerializeObject(user, Formatting.Indented);
            byte[] bufferusuario = Encoding.UTF8.GetBytes(jsonusuario);
            ByteArrayContent content = new ByteArrayContent(bufferusuario);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            String peticion = "api/Usuarios/CrearUsuario";
            Uri uri = new Uri(this.urlapi + peticion);
            HttpClient client = this.GetHttpClient();
            HttpResponseMessage response = await client.PostAsync(uri, content);
        }
        public async Task<SearchContainer<SearchMovie>> GetPeliculasPopulares()
        {
            String peticion = "api/Peliculas/PeliculasPopulares";
            Uri uri = new Uri(this.urlapi + peticion);
            HttpClient client = this.GetHttpClient();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                String json = await response.Content.ReadAsStringAsync();
                SearchContainer<SearchMovie> listas = JsonConvert.DeserializeObject<SearchContainer<SearchMovie>>(json);
                return listas;
            }
            else
            {
                return null;
            }
        }

        public async Task<SearchContainer<SearchMovie>> GetPeliculasCartelera()
        {
            String peticion = "api/Peliculas/PeliculasCartelera";
            Uri uri = new Uri(this.urlapi + peticion);
            HttpClient client = this.GetHttpClient();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                String json = await response.Content.ReadAsStringAsync();
                SearchContainer<SearchMovie> listas = JsonConvert.DeserializeObject<SearchContainer<SearchMovie>>(json);
                return listas;
            }
            else
            {
                return null;
            }
        }

        public async Task<SearchContainer<SearchMovie>> GetPeliculasProximamente()
        {
            String peticion = "api/Peliculas/PeliculasProximamente";
            Uri uri = new Uri(this.urlapi + peticion);
            HttpClient client = this.GetHttpClient();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                String json = await response.Content.ReadAsStringAsync();
                SearchContainer<SearchMovie> listas = JsonConvert.DeserializeObject<SearchContainer<SearchMovie>>(json);
                return listas;
            }
            else
            {
                return null;
            }
        }
    }
}
