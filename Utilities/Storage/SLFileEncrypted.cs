using System.IO;
using System.IO.IsolatedStorage;
using System.Text;

namespace MonoCross.Utilities.Storage
{
    //XmlBasicFile
    public class SLFileEncrypted : SLFile, IFile
    {
        IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();

        internal SLFileEncrypted()
        {
        }

        //public override Stream Create( string fileName )
        //{
        //    Delete(fileName);
        //    EnsureDirectoryExists(fileName);
        //    return store.CreateFile(fileName);
        //}

        //public override Stream Open( string fileName )
        //{
        //    return store.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        //}
        public override byte[] Read( string filename )
        {
            if ( !Exists( filename ) )
                return null;

            return null;
            //return store.OpenFile( fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite );
        }
        public override string ReadString( string filename )
        {
            if ( !Exists( filename ) )
                return null;

            return null;
            //return store.OpenFile( fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite );
        }

        public override void Save( string fileName, string contents )
        {
            EnsureDirectoryExists( fileName );
            
            byte[] byteArray = Encoding.UTF8.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);
            Save(fileName, stream);
        }
        public override void Save( string fileName, Stream contents )
        {
            EnsureDirectoryExists( fileName );

            if (contents.Length > store.AvailableFreeSpace && !IncreaseStorage(contents.Length + (store.Quota - store.AvailableFreeSpace)))
                return;

            byte[] bytes;
            using ( FileStream fileStream = new FileStream( fileName, FileMode.Create ) )
            using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
            using (BinaryReader binaryReader = new BinaryReader(contents))
            {
                try
                {
                    // process through stream in small chunks to keep peak memory usage down.
                    bytes = binaryReader.ReadBytes(6144);
                    while (bytes.Length > 0)
                    {
                        binaryWriter.Write(bytes);
                        bytes = binaryReader.ReadBytes(6144);
                    }
                }
                finally
                {
                    if (binaryWriter != null)
                        binaryWriter.Close();
                    if (fileStream != null)
                        fileStream.Close();
                    if (binaryReader != null)
                        binaryReader.Close();
                }
            }
        }
        public override void Save( string fileName, byte[] contents )
        {
            EnsureDirectoryExists( fileName );
            Delete( fileName );

            if (contents.Length > store.AvailableFreeSpace && !IncreaseStorage(contents.Length + (store.Quota - store.AvailableFreeSpace)))
                return;

            using ( FileStream fileStream = new FileStream( fileName, FileMode.Create ) )
            using ( BinaryWriter binaryWriter = new BinaryWriter( fileStream ) )
            {
                try
                {
                    binaryWriter.Write( contents );
                }
                finally
                {
                    if ( binaryWriter != null )
                        binaryWriter.Close();
                    if ( fileStream != null )
                        fileStream.Close();
                }
            }
        }

    }
}