using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace T5.Common.MyImage.Filters
{
    public class TransparencyFilter : ImageFilterBase
    {
        public byte AlphaComponent { get; set; } = 100;

        public override Image Create(Image img)
        {
            CurrentImage = img;
            Bitmap bmpNew = RGBA;
            BitmapData bmpData = bmpNew.LockBits(new Rectangle(0, 0, CurrentImage.Width, CurrentImage.Height), 
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            IntPtr ptr = bmpData.Scan0;

            byte[] byteBuffer = new byte[bmpData.Stride * bmpNew.Height];

            Marshal.Copy(ptr, byteBuffer, 0, byteBuffer.Length);

            for (int k = 3; k < byteBuffer.Length; k += 4)
            {
                byteBuffer[k] = AlphaComponent;
            }

            Marshal.Copy(byteBuffer, 0, ptr, byteBuffer.Length);

            bmpNew.UnlockBits(bmpData);

            bmpData = null;
            byteBuffer = null;

            

            return bmpNew;
        }

        public override string ToString()
        {
            return "Transparency";
        }
    }
}
