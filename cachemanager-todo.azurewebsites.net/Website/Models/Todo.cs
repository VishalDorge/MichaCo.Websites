using System;
using System.Linq;
using Newtonsoft.Json;

namespace Website.Models
{
    [Serializable]
    [JsonObject]
    public class Todo
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "completed")]
        public bool Completed { get; set; }
    }
}