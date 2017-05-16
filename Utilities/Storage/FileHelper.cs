using System;
using System.Text;
using System.IO;

namespace MonoCross.Utilities.Storage
{
    internal static class FileHelper
    {
        private static object _padLock = new object();

        /// <summary>
        /// Appends text to the FileStream using a StreamWriter.
        /// StreamWriter similar to FileStream shall be fine for all platforms.
        /// </summary>
        /// <param name="fs">A <see cref="FileStream"/> representing the Fs value.</param>
        /// <param name="value">A <see cref="String"/> representing the Value value.</param>
        internal static void AppendText(FileStream fs, string value)
        {
            fs.Seek(0, SeekOrigin.End);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.WriteLine(value);
            sw.Close();
            sw.Dispose();
        }

        /// <summary>
        /// Appends text to the FileStream using a StreamWriter.
        /// StreamWriter similar to FileStream shall be fine for all platforms.
        /// </summary>
        /// <param name="filename">A <see cref="String"/> representing the file location.</param>
        /// <param name="value">A <see cref="String"/> representing the Value value.</param>
        internal static void AppendText(string filename, string value)
        {
            lock (_padLock)
            {

                FileStream fs = null;
                StreamWriter sw = null;
                try
                {
                    fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Seek(0, SeekOrigin.End);
                    sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine(value);
                }
                catch (Exception)
                {
                    //Swallow any errors here... If we tried logging our error, we might end up back here.
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Close();
                        sw.Dispose();
                    }
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }
            }
        }

        //internal static void AddText(FileStream fs, string value)
        //{
        //    byte[] info = new UTF8Encoding(true).GetBytes(value);
        //    fs.Write(info, 0, info.Length);
        //}
    }
}
