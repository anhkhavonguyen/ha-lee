using System;
using System.Net.Http;

namespace Harvey.Job.Jobs.Customers
{
    public static class CustomerSettle
    {
        //[Hangfire.Queue("Settlement")]
        public static void Execute()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://harvey.crmloyalty.api:80/settle-all-customer");
        }
    }
}
