using GDIPlusX.GDIPlus11.Effects;
using System.Drawing;

namespace T5.Common.MyImage.Filters
{
    public class BlurrFilter : IImageFilter
    {
        public Image Create(Image img)
        {
            var effect = new BlurEffect(25, true);
            Bitmap newImg = new Bitmap(img);
            newImg.ApplyEffect(effect, Rectangle.Empty);
            return newImg;
        }

        public override string ToString()
        {
            return "Blurr";
        }
    }
}
