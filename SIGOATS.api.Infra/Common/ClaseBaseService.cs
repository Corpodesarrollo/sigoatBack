using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace SIGOATS.api.Infra.Common
{
    public class ClaseBaseService<TEntity, TDto>(ApplicationDbContext db) : IClaseBase<TDto>
    where TEntity : class
    where TDto : class
    {
        private readonly Mapper _mapper = MapperConfig.InitializeAutomapper();

        public virtual IQueryable<TDto> GetSelectBase()
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
                return new() { DataError = new(ex.Message) };
            }
        }

        public virtual async Task<Response<TDto, ResponseError>> GetByID(int id)
        {
            try
            {
                var entity = await db.Set<TEntity>().FindAsync(id);
                var dto = _mapper.Map<TDto>(entity);
                return new() { Data = dto };
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }

        public virtual async Task<Response<int, ResponseError>> Create(TDto data)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(data);
                await db.Set<TEntity>().AddAsync(entity);
                await db.SaveChangesAsync();
                var idProperty = typeof(TEntity).GetProperty("id") ?? typeof(TEntity).GetProperty("Id");
                var id = idProperty.GetValue(entity);
                return new() { Data = (int)id };
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
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
                return new Response<bool, ResponseError> { DataError = new(ex.Message) };
            }
        }

        public virtual async Task<Response<bool, ResponseError>> Delete(int id)
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
                return new Response<bool, ResponseError> { DataError = new(ex.Message) };
            }
        }
    }
}
