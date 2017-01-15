using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XamarinAdvanceRe.Data
{
    class MSP
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "picUrl")]
        public string Image { get; set; } = string.Empty;
        [JsonIgnore]
        public Uri PicUrl { get { return new Uri(Image); } }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; } = string.Empty;
        [Microsoft.WindowsAzure.MobileServices.Version]
        public string Version { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "emotion")]
        public string emotion { get; set; } = string.Empty;
        [Microsoft.WindowsAzure.MobileServices.UpdatedAt]
        public DateTime updatedAt { get; set; }
        [JsonProperty(PropertyName = "personid")]
        public string Personid { get; set; } = string.Empty;
        [JsonIgnore]
        public string emotionImg { get; set; }
    }
}
