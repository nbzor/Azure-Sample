using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using T5.Common;
using T5.Common.Models;
using T5.Common.MyImage;

namespace FrontendWebRole.Controllers
{
    public class HomeController : Controller
    {
        static IImagePersistence imagePersistance = new ImagePersistence();
        static ManageTable<AzureTableEntity<TaskImage>> table = new ManageTable<AzureTableEntity<TaskImage>>();
        static ManagerQueue<NewTaskImage> queue = new ManagerQueue<NewTaskImage>();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase file, int width, int height)
        {
            if (file == null)
                return View("Error");
            NewTaskImage newTask = new NewTaskImage { Height = height, Width = width };
            newTask.OriginalURL = await imagePersistance.UploadImageAsync(file);
            newTask.GUID = Guid.NewGuid().ToString();
            await queue.SendMessageAsync(newTask);
            var taskImage = await table.GetMessage(newTask.GUID);
            object data = null;
            if (taskImage != null)
                if (taskImage.Data != null && taskImage.Data.Length > 0)
                    data = JsonConvert.DeserializeObject<TaskImage>(taskImage.Data);
            return View("Done", data);
        }
    }
}