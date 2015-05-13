using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace T5.Common.MyImage.Filters
{
    class GrayscaleFilter : ImageFilterBase
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


            float rgb = 0;


            for (int k = 0; k < byteBuffer.Length; k += 4)
            {
                rgb = byteBuffer[k] * 0.11f;
                rgb += byteBuffer[k + 1] * 0.59f;
                rgb += byteBuffer[k + 2] * 0.3f;


                byteBuffer[k] = (byte)rgb;
                byteBuffer[k + 1] = byteBuffer[k];
                byteBuffer[k + 2] = byteBuffer[k];


                byteBuffer[k + 3] = 255;
            }


            Marshal.Copy(byteBuffer, 0, ptr, byteBuffer.Length);


            bmpNew.UnlockBits(bmpData);


            bmpData = null;
            byteBuffer = null;

            return bmpNew;
        }

        public override string ToString()
        {
            return "Grayscale";
        }
    }
}
