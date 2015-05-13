using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace T5.Common.MyImage.Filters
{
    public class SepiaToneFilter : ImageFilterBase
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


            byte maxValue = 255;
            float r = 0;
            float g = 0;
            float b = 0;


            for (int k = 0; k < byteBuffer.Length; k += 4)
            {
                r = byteBuffer[k] * 0.189f + byteBuffer[k + 1] * 0.769f + byteBuffer[k + 2] * 0.393f;
                g = byteBuffer[k] * 0.168f + byteBuffer[k + 1] * 0.686f + byteBuffer[k + 2] * 0.349f;
                b = byteBuffer[k] * 0.131f + byteBuffer[k + 1] * 0.534f + byteBuffer[k + 2] * 0.272f;


                byteBuffer[k + 2] = (r > maxValue ? maxValue : (byte)r);
                byteBuffer[k + 1] = (g > maxValue ? maxValue : (byte)g);
                byteBuffer[k] = (b > maxValue ? maxValue : (byte)b);
            }


            Marshal.Copy(byteBuffer, 0, ptr, byteBuffer.Length);


            bmpNew.UnlockBits(bmpData);


            bmpData = null;
            byteBuffer = null;


            return bmpNew;

        }
        public override string ToString()
        {
            return "Sepia Tone";
        }
    }
}
