using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using DurableTask.AzureStorage;
using DurableTask.Core;

namespace Hello_Orchestrator
{
    internal class Program
    {
        [STAThread]
        private static async Task Main(string[] args)
        {
            foreach (DictionaryEntry env in Environment.GetEnvironmentVariables())
            {
                var name = (string) env.Key;
                var value = (string) env.Value;
                Console.WriteLine("{0}={1}", name, value);
            }

            var storageConnectionString = Environment.GetEnvironmentVariable("StorageConnectionString");
            var taskHubName = Environment.GetEnvironmentVariable("TaskHubName");
            var durationInSeconds = Environment.GetEnvironmentVariable("DurationInSeconds");
            var mre = new ManualResetEvent(false);

            var settings = new AzureStorageOrchestrationServiceSettings
            {
                StorageConnectionString = storageConnectionString,
                TaskHubName = taskHubName
            };
            var orchestrationServiceAndClient = new AzureStorageOrchestrationService(settings);

            var taskHubClient = new TaskHubClient(orchestrationServiceAndClient);
            var taskHub = new TaskHubWorker(orchestrationServiceAndClient);

            orchestrationServiceAndClient.CreateIfNotExistsAsync().Wait();
            try
            {
                await taskHub
                    .AddTaskOrchestrations(typeof(CronOrchestration))
                    .AddTaskActivities(new CronTask())
                    .StartAsync();

                var orchestrationInstance = await taskHubClient.CreateOrchestrationInstanceAsync(
                    typeof(CronOrchestration),
                    TimeSpan.FromSeconds(double.Parse(durationInSeconds ?? "5")));

                Console.WriteLine($"ExecutionId: {orchestrationInstance.ExecutionId}. Blocking main thread.");
                mre.WaitOne();
                await taskHub.StopAsync(true);
                Console.WriteLine("Done!!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"worker exception: {e}");
            }
        }
    }
}