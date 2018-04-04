using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace AuthApp.API.Helpers
{
    public static class ReadJsonFile
    {
        public static T ReadJsonFileAs<T>(string filename)
        {
            T result;

            try
            {
                var assembly = typeof(AuthApp.API.Approvals.Approvals).GetTypeInfo().Assembly;

                using (Stream stream = assembly.GetManifestResourceStream($"AuthApp.API.{filename}"))
                using (StreamReader r = new StreamReader(stream))
                {
                    string json = r.ReadToEnd();
                    result = JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
