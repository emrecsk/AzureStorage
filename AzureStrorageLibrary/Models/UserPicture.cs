using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

namespace AzureStrorageLibrary.Models
{
    public class UserPicture : TableEntity
    {
        public string? RawPaths { get; set; }
        [IgnoreProperty]
        public List<string> Paths { 
            get => RawPaths == null ? null : JsonConvert.DeserializeObject<List<string>>(RawPaths);
            set => RawPaths = JsonConvert.SerializeObject(value); 
        }
        public string? ProjectRawPaths { get; set; }
        [IgnoreProperty]
        public List<string> ProjectPaths
        {
            get => ProjectRawPaths == null ? null : JsonConvert.DeserializeObject<List<string>>(ProjectRawPaths);
            set => ProjectRawPaths = JsonConvert.SerializeObject(value);
        }
    }
}
