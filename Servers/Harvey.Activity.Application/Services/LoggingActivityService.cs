using Harvey.Activity.Api;
using Harvey.Activity.Application.Entities;
using System;
using System.Threading.Tasks;

namespace Harvey.Activity.Application.Services
{
    public class LoggingActivityService : ILoggingActivityService
    {
        private readonly HarveyActivityDbContext _dbContext;

        public LoggingActivityService(HarveyActivityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ExecuteAsync(LoggingActivityRequest request)
        {
            var entity = new ActionActivity();
            entity.Description = request.Description;
            entity.Comment = request.Comment;
            entity.CreatedBy = request.UserId;
            entity.CreatedDate = DateTime.Now;
            entity.ActionAreaId = request.ActionAreaPath.ToString();
            entity.ActionTypeId = request.ActionType.ToString();
            entity.CreatedByName = request.CreatedByName;
            entity.Value = request.Value;
            _dbContext.Add(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
