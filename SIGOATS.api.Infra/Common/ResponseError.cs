namespace SIGOATS.api.Infra.Common
{
    public class ResponseError(string message)
    {
        public string Message { get; set; } = message;
    }
}
