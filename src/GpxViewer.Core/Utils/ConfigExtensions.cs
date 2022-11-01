using System;
using System.IO;
using Newtonsoft.Json;

namespace GpxViewer.Core.Utils
{
    public static class ConfigExtensions
    {
        public static void WriteJsonAndClose<T>(this Stream outStream, T objToWrite)
        {
            using var sWriter = new StreamWriter(outStream);
            using var jsonWriter = new JsonTextWriter(sWriter);

            var serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.Serialize(jsonWriter, objToWrite);
        }

        public static T ReadJsonAndClose<T>(this Stream? inStream, bool returnDefaultOnError)
            where T : class, new()
        {
            if (inStream == null) { return new T(); }

            try
            {
                using var sReader = new StreamReader(inStream);
                using var jsonReader = new JsonTextReader(sReader);

                var serializer = new JsonSerializer();
                return serializer.Deserialize<T>(jsonReader) ?? new T();
            }
            catch (Exception)
            {
                if (returnDefaultOnError) { return new T(); }
                throw;
            }
        }
    }
}
