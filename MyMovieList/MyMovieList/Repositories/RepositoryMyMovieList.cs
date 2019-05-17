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
        private MediaTypeWithQualityHeaderValue headerjson;

        public RepositoryMyMovieList()
        {
            this.urlapi = "https://mymovielistapi.azurewebsites.net/";
            this.headerjson = new MediaTypeWithQualityHeaderValue("application/json");
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
        public async Task<SearchContainer<SearchMovie>> BuscarPeliculas(String busqueda)
        {
            SearchContainer<SearchMovie> resultados = await this.CallApi<SearchContainer<SearchMovie>>("api/Peliculas/" + busqueda, null);
            return resultados;
        }

        private async Task<T> CallApi<T>(String peticion, String token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.urlapi);
                client.DefaultRequestHeaders.Accept.Clear();
                MediaTypeWithQualityHeaderValue headerjson = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(headerjson);

                if (token != null)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                }

                HttpResponseMessage response = await client.GetAsync(peticion);

                if (response.IsSuccessStatusCode)
                {
                    String content = await response.Content.ReadAsStringAsync();
                    //DESERIALIZAR LOS DATOS CON JSONCONVERT
                    T datos = JsonConvert.DeserializeObject<T>(content);
                    //T datos = await response.Content.ReadAsStringAsync();
                    return (T)Convert.ChangeType(datos, typeof(T));

                }
                else
                {
                    return default(T);
                }
            }
        }
    }
}
