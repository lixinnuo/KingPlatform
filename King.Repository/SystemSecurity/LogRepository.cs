using King.Domain.Context;
using King.Domain.Entity.SystemSecurity;
using King.Domain.IRepository.SystemSecurity;

namespace King.Repository.SystemSecurity
{
    public class LogRepository : RepositoryBase<LogEntity>, ILogRepository
    {
    }
}
