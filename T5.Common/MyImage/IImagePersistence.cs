using System.Drawing;
using System.Threading.Tasks;
using System.Web;

namespace T5.Common.MyImage
{
    public interface IImagePersistence
    {
        Task<Image> DownloadImageAsync(string url);
        Task<string> UploadImageAsync(Image img);
        Task<string> UploadImageAsync(HttpPostedFileBase file);
    }
}
