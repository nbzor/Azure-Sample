using System.Drawing;
using GDIPlusX.GDIPlus11.Effects;

namespace T5.Common.MyImage.Filters
{
    public class ColorMatrixFilter : IImageFilter
    {
        public Image Create(Image img)
        {
            var effect = new ColorMatrixEffect();            
            Bitmap newImg = new Bitmap(img);
            newImg.ApplyEffect(effect, Rectangle.Empty);
            return newImg;
        }

        public override string ToString()
        {
            return "ColorMatrix";
        }
    }
}
