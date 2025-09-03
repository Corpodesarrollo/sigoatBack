namespace SIGOATS.api.Infra.Common
{
    public class Response<T, E>
    {
        public T? Data { get { return data; } set { data = value; Ok = true; Error = false; } }
        public E? DataError { get { return dataError; } set { dataError = value; Error = true; Ok = false; } }
        public bool Error { get; set; }
        public bool Ok { get; set; }

        private E? dataError;
        private T? data;
    }
}
