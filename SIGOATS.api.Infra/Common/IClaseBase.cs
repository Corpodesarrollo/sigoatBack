namespace SIGOATS.api.Infra.Common
{
    public interface IClaseBase<T>
    {
        public IQueryable<T> GetSelectBase(string? search = null);
        public Task<Response<T[], ResponseError>> GetAll();
        public Task<Response<T, ResponseError>> GetByID(long id);
        public Task<Response<long, ResponseError>> Create(T data);
        public Task<Response<bool, ResponseError>> Update(T data);
        public Task<Response<bool, ResponseError>> Delete(long id);
        Task<Response<ODataServiceResult<T>, ResponseError>> OnDemandAsync(int page, int pageSize, IQueryable<T> selectQuery);
        Task<Response<bool, ResponseError>> ActivateToDeactivate(long id);
    }
}
