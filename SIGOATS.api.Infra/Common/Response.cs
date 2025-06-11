using Microsoft.AspNetCore.Mvc;

namespace SIGOATS.api.Infra.Common
{
    public class Response<T, E> : ControllerBase
    {
        public T? Data { get; set; }
        public E? DataError { get { return dataError; } set { dataError = value; Error = true; } }
        public bool Error { get; set; }

        private E? dataError;
    }
}
