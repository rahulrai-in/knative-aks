using System;
using System.Threading.Tasks;
using DurableTask.Core;

namespace Hello_Orchestrator
{
    public class CronOrchestration : TaskOrchestration<string, TimeSpan>
    {
        private int _jobNumber;

        public override async Task<string> RunTask(OrchestrationContext context, TimeSpan duration)
        {
            try
            {
                while (true)
                {
                    var currentTime = context.CurrentUtcDateTime;
                    var fireAt = currentTime.Add(duration);
                    _jobNumber += 1;
                    if (!context.IsReplaying)
                    {
                        Console.WriteLine(
                            $"{context.OrchestrationInstance.InstanceId}: Attempting to queue job {_jobNumber}.");
                    }

                    Console.WriteLine(
                        $"{context.OrchestrationInstance.InstanceId}: Job {_jobNumber} scheduled to run at {fireAt}.");
                    await context.CreateTimer(fireAt, _jobNumber.ToString());
                    Console.WriteLine(await context.ScheduleTask<string>(typeof(CronTask), _jobNumber.ToString()));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}