using System.Diagnostics.CodeAnalysis;
using AirlineManagement.Data;
using AirlineManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace AirlineManagement.Repositories.Implementations
{
    internal sealed class RepositoryOperations<TEntity> where TEntity : class
    {
        private readonly IDbContextFactory<DataContext> _contextFactory;
        private readonly ILogger _logger;
        private readonly Func<TEntity, int> _idOf;

        public RepositoryOperations(IDbContextFactory<DataContext> contextFactory, ILogger logger, Func<TEntity, int> idOf)
        {
            _contextFactory = contextFactory;
            _logger = logger;
            _idOf = idOf;
        }

        public TResult Run<TResult>(string operation, int? id, Func<DataContext, TResult> execute)
        {
            try
            {
                _logger.LogInformation("Выполняется {Operation}; идентификатор {EntityId}", operation, id);
                using var context = _contextFactory.CreateDbContext();
                return execute(context);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                LogFailure(operation, id, exception, null);
                throw new InvalidOperationException("Запись уже изменена или удалена. Обновите список и повторите операцию.", exception);
            }
            catch (Exception exception) when (exception is DbUpdateException or NpgsqlException)
            {
                var postgres = exception as PostgresException ?? exception.InnerException as PostgresException;
                LogFailure(operation, id, exception, postgres?.SqlState);
                var message = postgres?.SqlState switch
                {
                    PostgresErrorCodes.ForeignKeyViolation or PostgresErrorCodes.RestrictViolation => "Операция невозможна: связанная запись отсутствует или на эту запись ссылаются другие записи.",
                    PostgresErrorCodes.UniqueViolation => "Запись с такими уникальными значениями уже существует.",
                    PostgresErrorCodes.CheckViolation or PostgresErrorCodes.NotNullViolation => "Данные не удовлетворяют ограничениям базы данных.",
                    PostgresErrorCodes.SerializationFailure or PostgresErrorCodes.DeadlockDetected => "Произошёл конфликт с другой операцией. Обновите список и повторите операцию.",
                    _ => "Не удалось выполнить операцию в PostgreSQL. Проверьте подключение и журнал приложения."
                };
                throw new InvalidOperationException(message, exception);
            }
            catch (Exception exception)
            {
                LogFailure(operation, id, exception, null);
                throw;
            }
        }

        public IReadOnlyList<TEntity> ReadAll(string operation, Func<DataContext, IQueryable<TEntity>>? query = null)
        {
            return Run(operation, null, context =>
            {
                var entities = (query?.Invoke(context) ?? context.Set<TEntity>())
                    .AsNoTracking().OrderBy(x => EF.Property<int>(x, "Id")).ToList();
                _logger.LogInformation("{Operation}: получено записей {Count}", operation, entities.Count);
                return entities;
            });
        }

        public TEntity? ReadById(string operation, int id, Func<DataContext, IQueryable<TEntity>>? query = null)
        {
            return Run(operation, id, context =>
            {
                var entity = (query?.Invoke(context) ?? context.Set<TEntity>())
                    .AsNoTracking().SingleOrDefault(x => EF.Property<int>(x, "Id") == id);
                _logger.LogInformation("{Operation}: идентификатор {EntityId}, запись найдена {Found}", operation, id, entity is not null);
                return entity;
            });
        }

        public void Create(string operation, TEntity entity)
        {
            Run(operation, entity is null ? null : _idOf(entity), context =>
            {
                ValidateNew(entity);
                LogInput(operation, entity);
                context.Entry(entity).State = EntityState.Added;
                var count = context.SaveChanges();
                LogWrite(operation, _idOf(entity), count);
                return count;
            });
        }

        public void Update(string operation, TEntity entity)
        {
            Run(operation, entity is null ? null : _idOf(entity), context =>
            {
                ArgumentNullException.ThrowIfNull(entity);
                LogInput(operation, entity);
                var id = _idOf(entity);
                var existing = FindRequired(context, id);
                context.Entry(existing).CurrentValues.SetValues(entity);
                var count = context.SaveChanges();
                LogWrite(operation, id, count);
                return count;
            });
        }

        public void Delete(string operation, int id)
        {
            Run(operation, id, context =>
            {
                context.Set<TEntity>().Remove(FindRequired(context, id));
                var count = context.SaveChanges();
                LogWrite(operation, id, count);
                return count;
            });
        }

        public void ValidateNew([NotNull] TEntity? entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            if (_idOf(entity) != 0)
                throw new ArgumentException("Для новой записи укажите идентификатор 0: его назначает база данных.", nameof(entity));
        }

        public static InvalidOperationException Missing(int id) =>
            new($"Запись с идентификатором {id} не найдена. Обновите список.");

        public void LogInput(string operation, TEntity entity)
        {
            // Only known domain fields are logged. Employee names and connection details stay out of Logs.
            object input = entity switch
            {
                Airplane a => new { a.Model, a.Capacity },
                Employee e => new { e.Post },
                Flight f => new { f.FlightNumber, f.Destination, f.DepartureTime, f.ArrivalTime },
                FlightExecution x => new
                {
                    x.FlightId, x.AirplaneId, x.EmployeeId, x.OccupiedSeats, x.FlightDate, x.Days,
                    CrewEmployeeIds = x.CrewMembers.Select(m => m.EmployeeId).Order().ToArray()
                },
                TicketSaleItem t => new { t.FlightId, t.EmployeeId, t.TicketsSold, t.PricePerTicket },
                _ => new { Id = _idOf(entity) }
            };
            _logger.LogInformation("{Operation}: входные данные {@Input}", operation, input);
        }

        public void LogWrite(string operation, int id, int count) =>
            _logger.LogInformation("{Operation}: идентификатор {EntityId}, сохранено изменений {Count}", operation, id, count);

        private static TEntity FindRequired(DataContext context, int id) =>
            context.Set<TEntity>().Find(id) ?? throw Missing(id);

        // Database exception messages can contain parameter values or connection details.
        private void LogFailure(string operation, int? id, Exception exception, string? sqlState) =>
            _logger.LogError("Сбой {Operation}; идентификатор {EntityId}, тип ошибки {ErrorType}, SQLSTATE {SqlState}",
                operation, id, exception.GetType().Name, sqlState);
    }
}
