using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Resources;

namespace WebHybrid.WindowsPhone
{
    public static class ISExtensions
    {
        public static void CopyTextFile(this IsolatedStorageFile isf, string filename, bool replace = false)
        {
            if (!isf.FileExists(filename) || replace == true) {
                StreamResourceInfo sr = Application.GetResourceStream(new Uri(filename, UriKind.Relative));
                if (sr != null) {
                    using (StreamReader stream = new StreamReader(sr.Stream))
                    {
                        IsolatedStorageFileStream outFile = isf.CreateFile(filename);

                        string fileAsString = stream.ReadToEnd();
                        byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(fileAsString);

                        outFile.Write(fileBytes, 0, fileBytes.Length);

                        stream.Close();
                        outFile.Close();
                    }
                }
            }
        }
    }
}
