using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace WindowsForms_LogBook
{
    #region JSON        

    public static class JsonFileHelper
    {
        public static void JSONSerialization(string data, string fileName)
        {
            if ((!string.IsNullOrWhiteSpace(data)) && (!string.IsNullOrWhiteSpace(fileName)))
            {
                var serializer = new JsonSerializer();

                if (!Directory.Exists("Database"))
                {
                    Directory.CreateDirectory("Database");
                }

                using (var sw = new StreamWriter($"Database/{fileName}"))
                {
                    using (var jw = new JsonTextWriter(sw))
                    {
                        jw.Formatting = Newtonsoft.Json.Formatting.Indented;
                        serializer.Serialize(jw, data);
                    }
                }
            }
        }
        public static void JSONDeSerialization(ref string data, string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var serializer = new JsonSerializer();

                using (var sr = new StreamReader($"Database/{fileName}"))
                {
                    using (var jr = new JsonTextReader(sr))
                    {
                        data = serializer.Deserialize<string>(jr);
                    }
                }
            }
        }


        public static void JSONSerialization<T>(List<T> datas, string fileName)
        {
            var serializer = new JsonSerializer();

            if (!Directory.Exists("Database"))
            {
                Directory.CreateDirectory("Database");
            }

            using (var sw = new StreamWriter($"Database/{fileName}"))
            {
                using (var jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Newtonsoft.Json.Formatting.Indented;
                    serializer.Serialize(jw, datas);
                }
            }
        }

        public static void JSONDeSerialization<T>(ref List<T> datas, string fileName)
        {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader($"Database/{fileName}"))
            {
                using (var jr = new JsonTextReader(sr))
                {
                    datas = serializer.Deserialize<List<T>>(jr);
                }
            }
        }
    }

    #endregion
}
