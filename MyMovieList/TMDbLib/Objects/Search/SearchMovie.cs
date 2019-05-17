using System;
using Newtonsoft.Json;
using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Search
{
    public class SearchMovie : SearchMovieTvBase
    {
        public SearchMovie()
        {
            MediaType = MediaType.Movie;
        }

        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("release_date")]
        public DateTime? ReleaseDate { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("video")]
        public bool Video { get; set; }

        [JsonProperty("backdrop_path")]

        public String backdrop_path { get; set; }

        [JsonProperty("vote_average")]

        public String vote_average { get; set; }
        
        [JsonProperty("overview")]

        public String overview { get; set; }

        [JsonProperty("poster_path")]

        public String poster_path { get; set; }
    }
}