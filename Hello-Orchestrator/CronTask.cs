using System;
using System.Net.Http;
using System.Net.Mime;
using CloudNative.CloudEvents;
using DurableTask.Core;
using Newtonsoft.Json;

namespace Hello_Orchestrator
{
    public sealed class CronTask : TaskActivity<string, string>
    {
        public static int Result = 100;
        private readonly HttpClient _httpClient = new HttpClient();

        protected override string Execute(TaskContext context, string input)
        {
            try
            {
                Result++;

                var cloudEvent = new CloudEvent("com.hello.cron-event", new Uri("urn:hello-com:cron-source"))
                {
                    DataContentType = new ContentType(MediaTypeNames.Application.Json),
                    Data = JsonConvert.SerializeObject(new
                    {
                        Id = context.OrchestrationInstance.InstanceId,
                        JobId = input,
                        Result
                    })
                };

                var content = new CloudEventContent(cloudEvent, ContentMode.Structured, new JsonEventFormatter());
                Console.WriteLine($"Going to post data: {JsonConvert.SerializeObject(cloudEvent.Data)} to Url: {Environment.GetEnvironmentVariable("SINK")}");
                var result = _httpClient.PostAsync(Environment.GetEnvironmentVariable("SINK"), content).Result;
                return
                    $"Cron Job '{input}' Completed... @{DateTime.UtcNow} Response: {result} Event: {JsonConvert.SerializeObject(cloudEvent.Data)}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}