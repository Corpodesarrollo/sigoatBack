using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace SIGOATS.api.Infra.Common
{
    public class ClaseBaseService<TEntity, TDto>(ApplicationDbContext db) : IClaseBase<TDto>
    where TEntity : class
    where TDto : class
    {
        private readonly Mapper _mapper = MapperConfig.InitializeAutomapper();

        public virtual IQueryable<TDto> GetSelectBase(string? search = null)
        {
            return Enumerable.Empty<TDto>().AsQueryable();
        }

        public virtual async Task<Response<TDto[], ResponseError>> GetAll()
        {
            try
            {
                var entities = await db.Set<TEntity>().ToListAsync();
                var dtos = _mapper.Map<TDto[]>(entities);
                return new() { Data = dtos };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public virtual async Task<Response<TDto, ResponseError>> GetByID(long id)
        {
            try
            {
                var entity = await db.Set<TEntity>().FindAsync(id);
                var dto = _mapper.Map<TDto>(entity);
                return new() { Data = dto };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public virtual async Task<Response<long, ResponseError>> Create(TDto data)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(data);
                await db.Set<TEntity>().AddAsync(entity);
                await db.SaveChangesAsync();
                var idProperty = typeof(TEntity).GetProperty("id") ?? typeof(TEntity).GetProperty("Id");
                var id = idProperty.GetValue(entity);
                return new() { Data = (long)id };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public virtual async Task<Response<bool, ResponseError>> Update(TDto data)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(data);
                db.Set<TEntity>().Update(entity);
                await db.SaveChangesAsync();
                return new Response<bool, ResponseError> { Data = true };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public virtual async Task<Response<bool, ResponseError>> Delete(long id)
        {
            try
            {
                var entity = await db.Set<TEntity>().FindAsync(id);
                if (entity != null)
                {
                    db.Set<TEntity>().Remove(entity);
                    await db.SaveChangesAsync();
                    return new Response<bool, ResponseError> { Data = true };
                }

                return new Response<bool, ResponseError> { DataError = new("Registro no encontrado") };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public virtual async Task<Response<bool, ResponseError>> ActivateToDeactivate(long id)
        {
            try
            {
                var entity = await db.Set<TEntity>().FindAsync(id);
                if (entity != null)
                {
                    //cambiar Estado 
                    var estadoProperty = typeof(TEntity).GetProperty("Estado");
                    if (estadoProperty != null)
                    {
                        var currentState = (bool)estadoProperty.GetValue(entity);
                        estadoProperty.SetValue(entity, !currentState);
                        db.Set<TEntity>().Update(entity);
                        await db.SaveChangesAsync();
                        return new Response<bool, ResponseError> { Data = true };
                    }
                    return new Response<bool, ResponseError> { DataError = new("Propiedad 'Estado' no encontrada en la entidad") };
                }
                return new Response<bool, ResponseError> { DataError = new("Registro no encontrado") };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public virtual async Task<Response<ODataServiceResult<TDto>, ResponseError>> OnDemandAsync(int page, int pageSize, IQueryable<TDto> selectQuery)
        {
            try
            {
                var take = pageSize > 0 ? pageSize : 10;
                var skip = page > 0 ? (page - 1) * take : 0;
                var Query = selectQuery;
                var resultado = await Query.Take(take).Skip(skip).ToArrayAsync();

                var count = await Query.CountAsync();

                return new() { Data = new() { Count = count, Value = resultado } };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public ResponseError ProcessException(Exception ex)
        {
            var errorMessage = ex.InnerException != null
                    ? $"{ex.Message} | InnerException: {ex.InnerException.Message}"
                : ex.Message;

            return new(errorMessage);
        }
    }
}
