using GDIPlusX.GDIPlus11.Effects;
using System.Drawing;

namespace T5.Common.MyImage.Filters
{
    public class TintFilter : IImageFilter
    {       
        public Image Create(Image img)
        {
            var effect = new TintEffect(90,30);
            Bitmap newImg = new Bitmap(img);
            newImg.ApplyEffect(effect, Rectangle.Empty);
            return newImg;
        }

        public override string ToString()
        {
            return "Tint";
        }
    }
}
