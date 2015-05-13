using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace T5.Common.MyImage.Filters
{
    public class NegativeFilter : ImageFilterBase
    {
        public override Image Create(Image img)
        {
            CurrentImage = img;
            Bitmap bmpNew = RGBA;

            BitmapData bmpData = bmpNew.LockBits(new Rectangle(0, 0, CurrentImage.Width, CurrentImage.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            IntPtr ptr = bmpData.Scan0;


            byte[] byteBuffer = new byte[bmpData.Stride * bmpNew.Height];


            Marshal.Copy(ptr, byteBuffer, 0, byteBuffer.Length);
            byte[] pixelBuffer = null;


            int pixel = 0;


            for (int k = 0; k < byteBuffer.Length; k += 4)
            {
                pixel = ~BitConverter.ToInt32(byteBuffer, k);
                pixelBuffer = BitConverter.GetBytes(pixel);


                byteBuffer[k] = pixelBuffer[0];
                byteBuffer[k + 1] = pixelBuffer[1];
                byteBuffer[k + 2] = pixelBuffer[2];
            }

            Marshal.Copy(byteBuffer, 0, ptr, byteBuffer.Length);

            bmpNew.UnlockBits(bmpData);

            bmpData = null;
            byteBuffer = null;

            return bmpNew;
        }

         public override string ToString()
        {
            return "Negative";
        }
    }
}
