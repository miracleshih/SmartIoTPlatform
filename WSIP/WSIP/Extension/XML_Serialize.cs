using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace WSIP
{
    public static class XML_Serialize
    {
        public static string SerializeObject<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                try
                {
                    xmlSerializer.Serialize(textWriter, toSerialize, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.ToString()}");
                }
                return textWriter.ToString();
            }
        }

        public static string ByteToString(this byte[] data)
        {
            return System.Text.Encoding.Default.GetString(data);
        }


        public static string Serialize(object dataToSerialize)
        {
            if (dataToSerialize == null) return null;

            using (StringWriter stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(dataToSerialize.GetType());
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
        }

        public static T Deserialize<T>(string xmlText)
        {
            try
            {
                var stringReader = new System.IO.StringReader(xmlText);
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
            catch
            {
                throw;
            }
        }


    }
}
