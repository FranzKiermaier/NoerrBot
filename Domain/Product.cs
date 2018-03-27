using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleEchoBot.Domain
{
    [Serializable]
    public class Product
    {
        [JsonProperty("ProductID")]
        public string ProductID { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Color")]
        public string Color { get; set; }
        [JsonProperty("ListPrice")]
        public double ListPrice { get; set; }
        [JsonProperty("Size")]
        public string Size { get; set; }

        public override string ToString()
        {
            return $"{Name} (${ListPrice})";
        }

        public Product()
        {

        }
        public Product(string name)
        {
            this.Name = name;
        }
    }
}