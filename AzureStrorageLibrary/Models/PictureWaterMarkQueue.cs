using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStrorageLibrary.Models
{
    public class PictureWaterMarkQueue
    {
        public string? UserID { get; set; }
        public string? City { get; set; }
        public List<string>? Pictures { get; set; }
        public string? ConnectionID { get; set; }
        public string? WaterMarkText { get; set; }
    }
}
