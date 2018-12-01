using Harvey.Notification.Api;
using Harvey.Notification.Application.Configs;
using Harvey.Notification.Application.Extensions.PagingExtensions;
using Harvey.Notification.Application.Models;
using System;
using System.Linq;

namespace Harvey.Notification.Application.Domains.Notifications.Queries
{
    public class GetNotificationsQuery : IGetNotificationsQuery
    {
        private readonly HarveyNotificationDbContext _dbContext;
        public GetNotificationsQuery(HarveyNotificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetNotificationsResponse Execute(GetNotificationsRequest request)
        {
            var query = _dbContext.Notifications
                .Join(_dbContext.NotificationStatus,
                        notifications => notifications.Status,
                        status => status.Id,
                        (notifications, status) => new { Notifications = notifications, Status = status })
                .Where(x => x.Notifications.NotificationTypeId == (int)NotificationTypeEnum.SMS)
                .Select(o => new NotificationModel
                {
                   Id = o.Notifications.Id,
                   Action = o.Notifications.Action.ToString(),
                   Receivers = o.Notifications.Receivers,
                   Content = o.Notifications.Content,
                   CreatedDate = o.Notifications.CreatedDate,
                   Status = o.Status.DisplayName
                })
                .OrderByDescending(x => x.CreatedDate).AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(x => (x.Receivers != null && x.Receivers.Contains(request.SearchText)));
            }

            if (!string.IsNullOrEmpty(request.DateFilter))
            {
                query = query.Where(x => x.CreatedDate.ToString("dd/MM/yyyy") == request.DateFilter);
            }
            var result = PagingExtensions.GetPaged<NotificationModel>(query, request.PageNumber, request.PageSize);
            var response = new GetNotificationsResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.ListNotification = result.Results.ToList();
            return response;
        }
    }
}
