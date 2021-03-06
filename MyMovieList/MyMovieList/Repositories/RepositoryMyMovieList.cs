﻿using MyMovieList.Models;
using MyMovieList.Services;
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
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using System.Linq;

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
                    client.DefaultRequestHeaders.Add("Authorization", "bearer "
                      + token);
                }
                HttpResponseMessage response = await client.GetAsync(peticion);

                if (response.IsSuccessStatusCode)
                {
                    String content = await response.Content.ReadAsStringAsync();
                    T datos = JsonConvert.DeserializeObject<T>(content);
                    return (T)Convert.ChangeType(datos, typeof(T));
                }
                else
                {
                    return default(T);
                }
            }
        }
        public async Task<List<Usuario>> GetUsuarios(String token)
        {
            List<Usuario> usuarios = await this.CallApi<List<Usuario>>("api/Usuarios/GetUsuarios/", token);
            return usuarios;
        }

        public async Task<Usuario> PerfilUsuario(String token)
        {
            Usuario usuario = await this.CallApi<Usuario>("api/Usuarios/PerfilUsuario", token);
            return usuario;
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


        public async Task EliminarUsuario(String username, String token)
        {
            using (HttpClient client = new HttpClient())
            {
                String peticion = "api/Usuarios/EliminarUsuario/" + username;
                client.BaseAddress = new Uri(this.urlapi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(headerjson);
                if (token != null)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "bearer "
                        + token);
                }
                await client.DeleteAsync(peticion);
            }
        }



        public async Task EditarUsuario(Usuario user)
        {
            using (HttpClient client = this.GetHttpClient())
            {
                SessionService session = App.Locator.SessionService;
                String peticion = "api/Usuarios/EditarUsuario";
                client.BaseAddress = new Uri(this.urlapi);
                if (session.Token != null)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "bearer "
                        + session.Token);
                }
                Usuario usuario = new Usuario();
                usuario.IdUsuario = user.IdUsuario;
                usuario.NombreUsuario = user.NombreUsuario;
                usuario.Email = user.Email;
                usuario.Password = user.Password;
                usuario.Password2 = user.Password2;
                String json = JsonConvert.SerializeObject(usuario);
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                await client.PutAsync(peticion, content);
            }
        }
        public async Task<String> GetListaUser(String token)
        {
            String lista = await
                this.CallApi<String>("api/Usuarios/ListaUsuario", token);
            return lista;
        }
        public async Task<Lista> GetListaUser2(String token)
        {
            Lista lista = await
                this.CallApi<Lista>("api/Usuarios/ListaUser", token);
            return lista;
        }
        public async Task<List<String>> ListaUsuario(String token)
        {
            List<String> lista = await
                this.CallApi<List<String>>("api/Usuarios/PeliculasUsuario", token);
            return lista;
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
        public async Task AñadirPelicula(int idpelicula, String token)
        {
            string lista = await this.GetListaUser(token);
            using (HttpClient client = new HttpClient())
            {
                String peticion = "api/Usuarios/AñadirPelicula/" + idpelicula;
                client.BaseAddress = new Uri(this.urlapi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(headerjson);
                if (token != null)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "bearer "
                        + token);
                }
                Lista listauser = new Lista();
                listauser.Peliculas = lista + "," + idpelicula.ToString();
                String json = JsonConvert.SerializeObject(listauser);
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, headerjson.ToString());
                await client.PutAsync(peticion, content);
            }
        }
        public async Task EliminarPelicula(int idpelicula, String token)
        {
            string lista = await this.GetListaUser(token);
            using (HttpClient client = new HttpClient())
            {
                String peticion = "api/Usuarios/EliminarPelicula/" + idpelicula;
                client.BaseAddress = new Uri(this.urlapi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(headerjson);
                if (token != null)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "bearer "
                        + token);
                }
                Lista listauser = await this.GetListaUser2(token);
                if (lista == "")
                {
                    lista = "";
                }
                else if (listauser.Peliculas.IndexOf(',') != -1)
                {
                    if (listauser.Peliculas.Split(',').Last() == idpelicula.ToString())
                    {
                        lista = lista.Replace("," + idpelicula.ToString(), "");
                    }
                    else
                    {
                        lista = lista.Replace(idpelicula.ToString() + ",", "");
                    }
                }
                else
                {
                    lista = "";
                }
                listauser.Peliculas = lista;
                String json = JsonConvert.SerializeObject(listauser);
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, headerjson.ToString());
                await client.PutAsync(peticion, content);
            }
        }
        public async Task<Movie> DetallesPelicula(int id)
        {
            String peticion = "api/Peliculas/DetallesPelicula/" + id;
            Uri uri = new Uri(this.urlapi + peticion);
            HttpClient client = this.GetHttpClient();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                String json = await response.Content.ReadAsStringAsync();
                Movie pelicula = JsonConvert.DeserializeObject<Movie>(json);
                return pelicula;
            }
            else
            {
                return null;
            }
        }
        public async Task<Credits> RepartoPelicula(int idpelicula)
        {
            String peticion = "api/Peliculas/RepartoPelicula/" + idpelicula;
            Uri uri = new Uri(this.urlapi + peticion);
            HttpClient client = this.GetHttpClient();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                String json = await response.Content.ReadAsStringAsync();
                Credits reparto = JsonConvert.DeserializeObject<Credits>(json);
                return reparto;
            }
            else
            {
                return null;
            }
        }
        public async Task<ImagesWithId> ImagenesPelicula(int idpelicula)
        {
            String peticion = "api/Peliculas/ImagenesPelicula/" + idpelicula;
            Uri uri = new Uri(this.urlapi + peticion);
            HttpClient client = this.GetHttpClient();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                String json = await response.Content.ReadAsStringAsync();
                ImagesWithId imagenes = JsonConvert.DeserializeObject<ImagesWithId>(json);
                return imagenes;
            }
            else
            {
                return null;
            }
        }
    }
}
