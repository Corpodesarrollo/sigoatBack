namespace SIGOATS.api.Infra.Common
{
    public interface IClaseBase<T>
    {
        public Task<Response<List<T>, ResponseError>> GetAll();
        public Task<Response<T, ResponseError>> GetByID(int id);
        public Task<Response<int, ResponseError>> Create(T data);
        public Task<Response<bool, ResponseError>> Update(T data);
        public Task<Response<bool, ResponseError>> Delete(int id);
    }
}
