using System;
using System.IO;
using Android.Graphics;
using Java.IO;

namespace Android.Dialog
{
    public class ImageUtility
    {
        public static void SaveImage(Bitmap bitmap, String fileName)
        {
            if (bitmap == null || fileName == null) return;
            MemoryStream stream = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            byte[] byteArray = stream.GetBuffer();
            using (FileOutputStream fo = new FileOutputStream(fileName, false))
                fo.Write(byteArray);
        }

        public static Bitmap LoadImage(String fileName)
        {
            return BitmapFactory.DecodeFile(fileName);
        }
    }
}