using System.Text.Json.Serialization;

namespace Stazh.Core.Data.Models.Api
{
    public class ApiItemCreated
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Success { get; set; }
    }
}