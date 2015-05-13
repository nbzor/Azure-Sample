using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using T5.Common.Models;
using T5.Common.MyImage.Filters;

namespace T5.Common.MyImage
{
    public interface IImageService
    {
        List<IImageFilter> Filters { get; set; }        
        NewTaskImage NewTaskImage { get; set; }
        IImagePersistence ImagePersistance { get; }
        Task<TaskImage> ProcessImageAsync();
        Image Resize(int width, int height, Image img);
        void LoadDefaultFilters();                
    }
}
