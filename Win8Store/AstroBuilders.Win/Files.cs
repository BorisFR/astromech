using AstroBuilders.Win;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(Files))]

namespace AstroBuilders.Win
{
    public class Files : IFiles
    {
        private StorageFolder theFolder = null;
        private StorageFolder TheFolder
        {
            get
            {
                if (theFolder != null)
                    return theFolder;
                theFolder = ApplicationData.Current.LocalFolder;
                System.Diagnostics.Debug.WriteLine("**** Folder: " + theFolder.Path);
                return theFolder;
            }
        }

        async Task<bool> IFiles.IsExit(string name)
        {
            bool isExit = false;
            IStorageItem file;
            try
            {
                System.Diagnostics.Debug.WriteLine("IsExit trying to get item: " + name);
                file = await TheFolder.TryGetItemAsync(name); // .GetFileAsync(name);
                if (file == null)
                {
                    isExit = false;
                    System.Diagnostics.Debug.WriteLine("RealIsExist:" + name + " => false");
                }
                else
                {
                    //RealLoadString(name);
                    //if (theString == null || theString.Length == 0)
                    //{
                    //    isExit = false;
                    //    System.Diagnostics.Debug.WriteLine("RealIsExist:" + name + " => false");
                    //}
                    isExit = true;
                    System.Diagnostics.Debug.WriteLine("RealIsExist:" + name + " => true");
                }
            }
            catch (Exception e)
            {
                isExit = false;
                System.Diagnostics.Debug.WriteLine("RealIsExist:" + name + " => " + e.Message);
                System.Diagnostics.Debug.WriteLine("RealIsExist:" + name + " => false");
            }
            file = null;
            return isExit;
        }

        async Task<string> IFiles.ReadFile(string name)
        {
            string theString;
            try
            {
                StorageFile file = await TheFolder.GetFileAsync(name);
                TextReader tr = new StreamReader(await file.OpenStreamForReadAsync(), Encoding.UTF8);
                theString = await tr.ReadToEndAsync();
                tr.Dispose();
                file = null;
                return theString;
            }
            catch (Exception ioe)
            {
                System.Diagnostics.Debug.WriteLine("RealLoadString:" + name + " => " + ioe.Message);
                theString = string.Empty;
            }
            return theString;
        }

        async Task<byte[]> IFiles.ReadFileBytes(string name)
        {
            byte[] theData;
            try
            {
                StorageFile file = await TheFolder.GetFileAsync(name);
                //XmlSerializer x = new XmlSerializer(typeof(byte[]));
                BinaryReader tr = new BinaryReader(await file.OpenStreamForReadAsync());
                theData = tr.ReadBytes(65535); // (byte[])x.Deserialize(tr);
                tr.Dispose();
                file = null;
                //tr = null;
                //x = null;
            }
            catch (Exception ioe)
            {
                System.Diagnostics.Debug.WriteLine("RealLoadBytes:" + name + " => " + ioe.Message);
                theData = null;
            }
            return theData;
        }

        async Task IFiles.SaveFile(string name, string data)
        { 
            try
            {
                var file = await TheFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
                //XmlSerializer x = new XmlSerializer(typeof(byte[]));
                TextWriter writer = new StreamWriter(await file.OpenStreamForWriteAsync(), Encoding.UTF8);
                //x.Serialize(writer, data);
                writer.Write(data);
                writer.Flush();
                writer.Dispose();
                writer = null;
                file = null;
                //x = null;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("eSave:" + e.Message + " : " + e.StackTrace);
            }  
        }

        async Task IFiles.SaveFile(string name, byte[] data)
        {
            try
            {
                var file = await TheFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
                BinaryWriter writer = new BinaryWriter(await file.OpenStreamForWriteAsync());
                writer.Write(data);
                writer.Flush();
                writer.Dispose();
                writer = null;
                file = null;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("eSave:" + e.Message + " : " + e.StackTrace);
            }
        }

        async Task IFiles.DeleteFile(string name)
        {
            
        }
    }
}
