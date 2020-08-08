using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Cogito.Autofac;

using Quartz;

namespace Cogito.Components.MassTransit.Scheduler
{

    /// <summary>
    /// Schedules a set of periodic Pulse messages on the bus.
    /// </summary>
    [RegisterAs(typeof(IRunnable))]
    public class PeriodicScheduler : IRunnable
    {

        const string JOB_GROUP = "Cogito.Components.MassTransit.Scheduler";
        const string JOB_NAME = "PeriodicScheduler";
        const string TRIGGER_GROUP = "Cogito.Components.MassTransit.Scheduler";
        const string TRIGGER_NAME = "PeriodicScheduler";

        static readonly JobKey JOB_KEY = new JobKey(JOB_NAME, JOB_GROUP);
        static readonly TriggerKey TRIGGER_KEY = new TriggerKey(TRIGGER_NAME, TRIGGER_GROUP);

        readonly IScheduler scheduler;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="scheduler"></param>
        public PeriodicScheduler(IScheduler scheduler)
        {
            this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        }

        /// <summary>
        /// Periodically refreshes the configured pulse schedules.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                await InitializeJobSchedulerJob();
                await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
            }
        }

        /// <summary>
        /// Configures the various static schedules.
        /// </summary>
        /// <returns></returns>
        async Task InitializeJobSchedulerJob()
        {
            // find or create job
            var job = await scheduler.GetJobDetail(JOB_KEY) ??
                JobBuilder.Create<PeriodicSchedulerJob>()
                    .WithIdentity(JOB_KEY)
                    .StoreDurably(true)
                    .Build();

            // ensure job is added
            if (await scheduler.CheckExists(JOB_KEY) == false)
                await scheduler.AddJob(job, false);

            // all currently registered triggers
            var triggers = await scheduler.GetTriggersOfJob(JOB_KEY);

            // remove any unnecessary triggers
            foreach (var t in triggers.Where(i => i.Key != TRIGGER_KEY))
                await scheduler.UnscheduleJob(t.Key);

            // remove any additional triggers
            foreach (var t in triggers.Skip(1))
                await scheduler.UnscheduleJob(t.Key);

            // find existing matching trigger
            var trigger = triggers.FirstOrDefault();
            if (trigger == null)
            {
                trigger = TriggerBuilder.Create()
                    .ForJob(JOB_KEY)
                    .WithIdentity(TRIGGER_KEY)
                    .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromMinutes(5)).RepeatForever())
                    .StartNow()
                    .Build();

                // schedule new trigger
                await scheduler.ScheduleJob(trigger);
            }
        }

    }

}
