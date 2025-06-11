namespace SIGOATS.api.Infra.Common
{
    public class ODataServiceResult<T>
    {
        public int Count { get; set; }
        public IEnumerable<T> Value { get; set; }
    }
}
