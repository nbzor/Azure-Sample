using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace T5.Common.MyImage
{
    public class ImagePersistence : IImagePersistence
    {
        public async Task<Image> DownloadImageAsync(string url)
        {
            using (var webClient = new WebClient())
            {
                var data = await webClient.DownloadDataTaskAsync(new Uri(url));
                using (MemoryStream ms = new MemoryStream(data))
                {
                    return Image.FromStream(ms);
                }
            }
        }

        public async Task<string> UploadImageAsync(Image img)
        {
            if (img == null)
                return null;
            string imageName = RandomGuidByImgType(img);
            string url = null;
            try
            {
                var blockBlob = StorageUtils.BlobContainer.GetBlockBlobReference(imageName);

                blockBlob.Properties.ContentType = "image/jpeg";
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, ImageFormat.Jpeg);
                    var data = ms.ToArray();
                    await blockBlob.UploadFromByteArrayAsync(data, 0, data.Length);
                }

                url = blockBlob.Uri.ToString();
            }
            catch (Exception)
            {
                throw;
            }
            return url;
        }

        public async Task<string> UploadImageAsync(HttpPostedFileBase photoToUpload)
        {
            if (photoToUpload == null || photoToUpload.ContentLength == 0)
            {
                return null;
            }
            string imageName = RandomGUID(photoToUpload.FileName);
            string url = null;
            try
            {
                var blockBlob = StorageUtils.BlobContainer.GetBlockBlobReference(imageName);
                blockBlob.Properties.ContentType = photoToUpload.ContentType;
                await blockBlob.UploadFromStreamAsync(photoToUpload.InputStream);
                url = blockBlob.Uri.ToString();
            }
            catch (Exception)
            {
                throw;
            }
            return url;
        }

        private string RandomGuidByImgType(Image img)
        {
            string ss = "qweq.";
            if (ImageFormat.Jpeg.Equals(img.RawFormat))
                return RandomGUID(ss + "jpeg");

            if (ImageFormat.Png.Equals(img.RawFormat))
                return RandomGUID(ss + "png");
            if (ImageFormat.Bmp.Equals(img.RawFormat))
                return RandomGUID(ss + "bmp");

            return RandomGUID(ss + "jpeg");
        }


        private string RandomGUID(string file)
        {
            return string.Format("{0}{1}", Guid.NewGuid().ToString(),
                    Path.GetExtension(file));
        }
    }
}
