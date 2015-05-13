using System.Drawing;
using System.Drawing.Imaging;

namespace T5.Common.MyImage.Filters
{
    public abstract class ImageFilterBase : IImageFilter
    {
        protected Image CurrentImage { get; set; }
        public Bitmap RGBA
        {
            get
            {
                if (CurrentImage != null)
                {
                    Bitmap argb = new Bitmap(CurrentImage.Width, CurrentImage.Height, PixelFormat.Format32bppArgb);
                    using (Graphics graphics = Graphics.FromImage(argb))
                    {
                        graphics.DrawImage(CurrentImage, new Rectangle(0, 0, argb.Width, argb.Height),
                            new Rectangle(0, 0, argb.Width, argb.Height),
                            GraphicsUnit.Pixel);

                        graphics.Flush();
                    }
                    return argb;
                }
                return null;
            }

        }
        public abstract Image Create(Image img);
    }

}
