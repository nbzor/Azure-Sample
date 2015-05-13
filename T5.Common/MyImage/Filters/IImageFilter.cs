using System.Drawing;

namespace T5.Common.MyImage.Filters
{
    public interface IImageFilter
    {
        Image Create(Image img);
    }
}
