using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Services.SMSService
{
    internal class SMSService : ISMSService
    {
        private readonly IConfiguration _configuration;

        public SMSService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendToMultiplePhoneNumbers()
        {
            throw new NotImplementedException();
        }

        public async Task<PublishResponse> SendAsync(string phoneNumber,string title, string content)
        {
            if (bool.Parse(_configuration["Testing:IsTesting"]) == true)
            {
                phoneNumber = _configuration["Testing:SMSReceivers"];
            }

            AmazonSimpleNotificationServiceConfig config = new AmazonSimpleNotificationServiceConfig {
                UseHttp = false,
                RegionEndpoint = RegionEndpoint.APSoutheast1,
                BufferSize = 1024,
            };
           
            AmazonSimpleNotificationServiceClient snsclient = new AmazonSimpleNotificationServiceClient(_configuration["SMSService:AccessKey"], _configuration["SMSService:SecretKey"], config);
            var smsAttributes = new Dictionary<string, MessageAttributeValue>();

            MessageAttributeValue senderID = new MessageAttributeValue();
            senderID.DataType = "String";
            senderID.StringValue = _configuration["SMSService:SenderID"];
            smsAttributes.Add("AWS.SNS.SMS.SenderID", senderID);

            MessageAttributeValue snsType = new MessageAttributeValue();
            snsType.DataType = "String";
            snsType.StringValue = _configuration["SMSService:SMSType"];
            smsAttributes.Add("AWS.SNS.SMS.SMSType", snsType);

            MessageAttributeValue maxPrice = new MessageAttributeValue();
            maxPrice.DataType = "String";
            maxPrice.StringValue = _configuration["SMSService:MaxPrice"];
            smsAttributes.Add("AWS.SNS.SMS.MaxPrice", maxPrice);

          
            PublishRequest pr = new PublishRequest
            {
                Subject = title,
                Message = content,
                PhoneNumber = phoneNumber
            };
            pr.MessageAttributes = smsAttributes;
            var result = await snsclient.PublishAsync(pr);
            return result;
        }
    }
}
