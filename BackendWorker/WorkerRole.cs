using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using T5.Common;
using T5.Common.Models;
using T5.Common.MyImage;

namespace BackendWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private ManagerQueue<NewTaskImage> managerQueue = new ManagerQueue<NewTaskImage>();
        private ManageTable<AzureTableEntity<TaskImage>> manageTable = new ManageTable<AzureTableEntity<TaskImage>>();
        private ImageService imageService = new ImageService(true);

        public override void Run()
        {
            Trace.TraceInformation("BackendWorker is running");

            try
            {
                RunAsync(cancellationTokenSource.Token).Wait();
            }
            finally
            {
                runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("BackendWorker has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("BackendWorker is stopping");

            cancellationTokenSource.Cancel();
            runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("BackendWorker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                var message = await managerQueue.GetMessageAsync();
                if(message != null)
                {
                    imageService.NewTaskImage = message;
                    var respose = await imageService.ProcessImageAsync();
                    var entity = new AzureTableEntity<TaskImage>(respose.GUID) { Entity = respose };
                    await manageTable.AddMessage(entity, respose.GUID);
                }                
                await Task.Delay(1000);
            }
        }
    }
}
