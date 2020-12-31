namespace Stazh.Core.Data.Models.Api
{
    public class ApiItemCreation<T> : ApiItem
    {
        public int childId { get; set; }
        public bool Success { get; set; }
        public T Files { get; set; }

    }
}