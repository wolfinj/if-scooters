using if_scooters.core.Models;
using if_scooters.core.Services;
using if_scooters.data;
using Microsoft.EntityFrameworkCore;

namespace if_scooters.services;

public class DbService : IDbService
{
    protected readonly IScooterDbContext Context;

    public DbService(IScooterDbContext context)
    {
        Context = context;
    }

    public ServiceResult Create<T>(T entity) where T : Entity
    {
        Context.Set<T>().Add(entity);
        Context.SaveChanges();

        return new ServiceResult(true).SetEntity(entity);
    }

    public ServiceResult Delete<T>(T entity) where T : Entity
    {
        try
        {
            Context.Set<T>().Remove(entity);
            Context.SaveChanges();
        }
        catch (Exception e)
        {
            return new ServiceResult(false).AddError(e.Message);
        }

        return new ServiceResult(true);
    }

    public ServiceResult Update<T>(T entity) where T : Entity
    {
        Context.Entry(entity).State = EntityState.Modified;
        Context.SaveChanges();

        return new ServiceResult(true).SetEntity(entity);
    }

    public List<T> GetAll<T>() where T : Entity
    {
        return Context.Set<T>().ToList();
    }

    public T? GetById<T>(int id) where T : Entity
    {
        return Context.Set<T>().SingleOrDefault(e => e.Id == id);
    }

    public IQueryable<T> Query<T>() where T : Entity
    {
        return Context.Set<T>().AsQueryable();
    }
}
