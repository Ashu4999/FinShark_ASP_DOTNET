using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using LearningDotnet.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace LearningDotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpPost("CreateBackgroundJob")]
        public ActionResult CreateBackgroundJob() {
            // BackgroundJob.Enqueue(() => Console.WriteLine("Background Job Triggered!!!"));
            BackgroundJob.Enqueue<CustomJobLogger>(x => x.WriteLog("Background Job Triggered!!!"));
            return Ok();
        }

        [HttpPost("CreateScheduleJob")]
        public ActionResult CreateScheduleJob() {
            var scheduleDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffset = new DateTimeOffset(scheduleDateTime);
            // BackgroundJob.Schedule(() => Console.WriteLine("Schedule Job Triggered!!!"), dateTimeOffset);
            BackgroundJob.Schedule<CustomJobLogger>(x => x.WriteLog("Schedule Job Triggered!!!"), dateTimeOffset);
            return Ok();
        }

        [HttpPost("CreateContinuationJob")]
        public ActionResult CreateContinuationJob() {
            var scheduleDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffset = new DateTimeOffset(scheduleDateTime);
            // var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Schedule Job Triggered!!!"), dateTimeOffset);

            // var jobId2 = BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine($"Continuation Job 1 Triggered after {jobId}"));
            // var jobId3 = BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine($"Continuation Job 2 Triggered after {jobId2}"));
            // var jobId4 = BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine($"Continuation Job 3 Triggered after {jobId3}"));

             var jobId = BackgroundJob.Schedule<CustomJobLogger>(x => x.WriteLog("Schedule Job Triggered!!!"), dateTimeOffset);

            var jobId2 = BackgroundJob.ContinueJobWith<CustomJobLogger>(jobId, x => x.WriteLog($"Continuation Job 1 Triggered after {jobId}"));
            var jobId3 = BackgroundJob.ContinueJobWith<CustomJobLogger>(jobId, x => x.WriteLog($"Continuation Job 2 Triggered after {jobId2}"));
            var jobId4 = BackgroundJob.ContinueJobWith<CustomJobLogger>(jobId, x => x.WriteLog($"Continuation Job 3 Triggered after {jobId3}"));
            return Ok();
        }

        [HttpPost("CreateRecurringJob")]
        public ActionResult CreateRecurringJob() {
            // RecurringJob.AddOrUpdate("RecurringJob1", () => Console.WriteLine("Recurring Job Triggered!!!"), "*/2 * * * * *");
            RecurringJob.AddOrUpdate<CustomJobLogger>("RecurringJob1", x => x.WriteLog("Recurring Job Triggered!!!"), "*/2 * * * * *");
            return Ok();
        }
    }
}