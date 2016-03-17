using System;
using System.IO.IsolatedStorage;
using System.IO;
using System.Runtime.Serialization;

namespace BenScharbach.WP7.Helpers.Storage
{
    /// <summary>
    /// My File processor helper class for Windows Phone 7.1 devices.
    /// </summary>
    /// <remarks>
    /// The use of WP7 IsolatedStorage;
    /// http://www.geekchamp.com/tips/all-about-wp7-isolated-storage---read-and-save-xml-files-using-xmlwriter/ 
    /// The use of WCF DataContract / DataMember attributes for WP7 IsolatedStorage Serialization;
    /// http://www.codeproject.com/Articles/156254/Simple-Data-Serializatoin-on-WP/
    /// </remarks>
    public static class FileProcessor
    {
        /// <summary>
        /// Save some class as a XML serialized file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        public static void SaveXmlSerializedStream<T>(T data, string fileName)
        {
            // open WP7 IsolatedStorageFile.
            using (var isolatedFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // open WP7 IsolatedStorageFile Stream.
                using (var isolatedFileStream = new IsolatedStorageFileStream(fileName, FileMode.Create, isolatedFile))
                {
                    // Note: Ran into serlization issue on WP7; use WCF's DataContract / DataMember attributes.
                    // Convert the object to XML data and put it in the stream.
                    //var serializer = new XmlSerializer(typeof(T));
                    //serializer.Serialize(isolatedFileStream, data);

                    // Convert the object to XML data and put it in the stream.
                    var serializer = new DataContractSerializer(typeof (T));
                    serializer.WriteObject(isolatedFileStream, data);
                }
            }
        }

        /// <summary>
        /// Load a XML file and deserialized back into a class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool LoadXmlSerializedStream<T>(string fileName, out T data)
        {
            // Set default value for out param.
            data = default(T);

            try
            {              

                // open WP7 IsolatedStorageFile
                using (var isolatedFile = IsolatedStorageFile.GetUserStoreForApplication())
                {
                     // check if file exist.                
                    if (!isolatedFile.FileExists(fileName))
                        return false;

                    // open WP7 IsolatedStorageFile Stream.
                    using (var isolatedFileStream = isolatedFile.OpenFile(fileName, FileMode.Open))
                    {
                        // Note: Ran into serlization issue on WP7; use WCF's DataContract / DataMember attributes.
                        // Read back the XML data and deserialize
                        //var serializer = new XmlSerializer(typeof(T));
                        //data = (T)serializer.Deserialize(isolatedFileStream);

                        // Read back the XML data and deserialize
                        var serializer = new DataContractSerializer(typeof(T));
                        data = (T)serializer.ReadObject(isolatedFileStream);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
