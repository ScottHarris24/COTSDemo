using System.Linq.Expressions;
using COTSDemo.Abstractions.Enums;
using COTSDemo.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace COTSDemo.Repositories;

public class Repository<TEntity>(IServiceProvider serviceProvider) : IRepository<TEntity> 
    where TEntity : class
{
    #region Member Variables and Construction

    protected readonly COTSDemoDbContext _dbContext = serviceProvider.GetRequiredService<COTSDemoDbContext>();

    #endregion Member Variables and Construction

    #region Public Functions

    public async Task<IRepositoryResponse> GetAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
    {
        IRepositoryResponse? response;

        try
        {
            var entity = await _dbContext
                .Set<TEntity>()
                .FindAsync(id, cancellationToken)
                .ConfigureAwait(false);

            response = new RepositoryResponse()
            {
                ReturnStatus = entity == null
                    ? RepositoryReturnStatusType.NotFound
                    : RepositoryReturnStatusType.Success,

                Data = entity!,
            };
        }
        catch (Exception e)
        {
            response = CreateRepositoryResponse(e);
        }

        return response;
    }

    public async Task<IRepositoryResponse> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        IRepositoryResponse? response;

        try
        {
            var entity = await _dbContext
                .Set<TEntity>()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            response = new RepositoryResponse()
            {
                ReturnStatus = entity.Count == 0
                    ? RepositoryReturnStatusType.NotFound
                    : RepositoryReturnStatusType.Success,

                Data = entity!,
            };
        }
        catch (Exception e)
        {
            response = CreateRepositoryResponse(e);
        }

        return response;
    }

    public async Task<IRepositoryResponse> QueryAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
    {
        IRepositoryResponse? response;

        try
        {
            var entity = await _dbContext
                .Set<TEntity>()
                .Where(predicate)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            response = new RepositoryResponse()
            {
                ReturnStatus = entity.Count == 0
                    ? RepositoryReturnStatusType.NotFound
                    : RepositoryReturnStatusType.Success,

                Data = entity!,
            };
        }
        catch (Exception e)
        {
            response = CreateRepositoryResponse(e);
        }

        return response;
    }

    public async Task<IRepositoryResponse> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
    {
        IRepositoryResponse? response;

        try
        {
            var addedEntity = await _dbContext
                .Set<TEntity>()
                .AddAsync(entity, cancellationToken)
                .ConfigureAwait(false);

            var recordsEffected = await _dbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            response = new RepositoryResponse()
            {
                ReturnStatus = addedEntity == null
                    ? RepositoryReturnStatusType.NotFound
                    : RepositoryReturnStatusType.Success,

                RecordsEffected = recordsEffected,
                Data = addedEntity?.Entity!,
            };
        }
        catch (Exception e)
        {
            response = CreateRepositoryResponse(e);
        }

        return response;
    }

    public async Task<IRepositoryResponse> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
    {
        IRepositoryResponse? response;

        try
        {
            await _dbContext
                .Set<TEntity>()
                .AddRangeAsync(entities, cancellationToken)
                .ConfigureAwait(false);

            var recordsEffected = await _dbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            var count = entities.Count();
            response = new RepositoryResponse()
            {
                ReturnStatus = count == 0 
                    ? RepositoryReturnStatusType.NotFound
                    : RepositoryReturnStatusType.Success,

                RecordsEffected = recordsEffected,
                Data = entities
            };
        }
        catch (Exception e)
        {
            response = CreateRepositoryResponse(e);
        }

        return response;
    }

    public async Task<IRepositoryResponse> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
    {
        IRepositoryResponse? response;

        try
        {
            var deletedEntity = _dbContext
                .Set<TEntity>()
                .Remove(entity);

            var recordsEffected = await _dbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            response = new RepositoryResponse()
            {
                ReturnStatus = deletedEntity == null
                    ? RepositoryReturnStatusType.NotFound
                    : RepositoryReturnStatusType.Success,

                RecordsEffected = recordsEffected,
                Data = deletedEntity?.Entity!,
            };
        }
        catch (Exception e)
        {
            response = CreateRepositoryResponse(e);
        }

        return response;
    }

    public async Task<IRepositoryResponse> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
    {
        IRepositoryResponse? response;

        try
        {
            _dbContext
                .Set<TEntity>()
                .RemoveRange(entities);

            var recordsEffected = await _dbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            var count = entities.Count();
            response = new RepositoryResponse()
            {
                ReturnStatus = count == 0 
                    ? RepositoryReturnStatusType.NotFound
                    : RepositoryReturnStatusType.Success,

                RecordsEffected = recordsEffected,
                Data = entities
            };
        }
        catch (Exception e)
        {
            response = CreateRepositoryResponse(e);
        }

        return response;
    }

    #endregion Public Functions

    #region Protected Support Functions

    protected IRepositoryResponse CreateRepositoryResponse(Exception e)
    {
        var response = new RepositoryResponse()
        {
            ReturnStatus = RepositoryReturnStatusType.Exception,

            ErrorCode = e.HResult,
            ErrorMessage = e.Message,
            ErrorTypeName = e.GetType().Name,
        };

        return response;
    }

    #endregion Protected Support Functions

}