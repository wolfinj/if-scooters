using if_scooters.core.Models;
using if_scooters.core.Services;
using if_scooters.data;

namespace if_scooters.services;

public class EntityService<T> : DbService, IEntityService<T> where T : Entity
{
    public EntityService(IScooterDbContext context) : base(context)
    {
    }

    public ServiceResult Create(T entity)
    {
        return Create<T>(entity);
    }

    public ServiceResult Delete(T entity)
    {
        return Delete<T>(entity);
    }

    public ServiceResult Update(T entity)
    {
        return Update<T>(entity);
    }

    public List<T> GetAll()
    {
        return GetAll<T>();
    }

    public T? GetById(int id)
    {
        return GetById<T>(id);
    }

    public IQueryable<T> Query()
    {
        return Query<T>();
    }
}
