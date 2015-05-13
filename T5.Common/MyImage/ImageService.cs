using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using T5.Common.Models;
using T5.Common.MyImage.Filters;

namespace T5.Common.MyImage
{
    public class ImageService : IImageService
    {        
        public ImageService(bool defaultFilters = false)
        {
            if (defaultFilters)
                LoadDefaultFilters();
        }

        private Image CurrentImage { get; set; }

        public List<IImageFilter> Filters { get; set; } = new List<IImageFilter>();

        public IImagePersistence ImagePersistance { get; } = new ImagePersistence();


        private TaskImage CurrentTask { get; set; }
        public NewTaskImage NewTaskImage { get; set; }

        public async Task<TaskImage> ProcessImageAsync()
        {
            if (NewTaskImage == null)
                return null;
            CurrentTask = new TaskImage { GUID = NewTaskImage.GUID };
            CurrentTask.OriginalURL = NewTaskImage.OriginalURL;
            CurrentImage = await ImagePersistance.DownloadImageAsync(NewTaskImage.OriginalURL);
            if (Resize())
                CurrentTask.CurrentURL = await ImagePersistance.UploadImageAsync(CurrentImage);
            else
                CurrentTask.CurrentURL = NewTaskImage.OriginalURL;
            foreach (IImageFilter filter in Filters)
            {
                var img = filter.Create(CurrentImage);
                var url = await ImagePersistance.UploadImageAsync(img);
                CurrentTask.Filters.Add(new Filter() { Type = filter.ToString(), URL = url });
            }
            CurrentTask.GUID = NewTaskImage.GUID;
            return CurrentTask;
        }

        public Image Resize(int width, int height, Image img)
        {
            if (img == null)
                return null;

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(img.HorizontalResolution, img.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(img, destRect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private bool Resize()
        {
            if (CurrentImage == null)
                return false;
            if (NewTaskImage.Width != CurrentImage.Width && NewTaskImage.Height != CurrentImage.Height)
            {
                CurrentImage = Resize(NewTaskImage.Width, NewTaskImage.Height, CurrentImage);
                return true;
            }
            if (NewTaskImage.Width != CurrentImage.Width)
            {
                CurrentImage = Resize(NewTaskImage.Width, NewTaskImage.Height, CurrentImage);
                return true;
            }
            if (NewTaskImage.Height != CurrentImage.Height)
            {
                CurrentImage = Resize(NewTaskImage.Width, NewTaskImage.Height, CurrentImage);
                return true;
            }
            return false;
        }

        public void LoadDefaultFilters()
        {
            Filters.Add(new GrayscaleFilter());
            Filters.Add(new NegativeFilter());
            Filters.Add(new SepiaToneFilter());
            Filters.Add(new TransparencyFilter());
            Filters.Add(new TintFilter());
            Filters.Add(new BlurrFilter());
        }
    }
}
