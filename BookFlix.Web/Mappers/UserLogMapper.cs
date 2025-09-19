using BookFlix.Core.Models;
using BookFlix.Web.Dtos.UserLog;
using BookFlix.Web.Mapper_Interfaces;

namespace BookFlix.Web.Mappers
{
    public class UserLogMapper : IUserLogMapper
    {
        public IList<UserLogDto> ToUserLogDtos(IReadOnlyCollection<UserLog> userLogs)
        {
            var userLogDtos = userLogs.Select(log => new UserLogDto
            {
                Id = log.Id,
                UserId = log.UserId,
                EventType = log.EventType,
                Timestamp = log.Timestamp,
                IpAddress = log.IpAddress,
                Success = log.Success
            }).ToList();

            return userLogDtos;
        }
    }
}
