using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosomoDb.Models
{
    public class Item
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}
