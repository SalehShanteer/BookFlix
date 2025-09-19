using BookFlix.Core.Models;
using BookFlix.Web.Dtos.UserLog;

namespace BookFlix.Web.Mapper_Interfaces
{
    public interface IUserLogMapper
    {
        IList<UserLogDto> ToUserLogDtos(IReadOnlyCollection<UserLog> userLogs);
    }
}
