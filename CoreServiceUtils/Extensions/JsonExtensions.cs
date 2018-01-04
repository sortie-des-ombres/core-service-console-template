using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CoreServiceUtils.Extensions
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = "yyyy-MM-ddTHH:mm:ssZ"
        };

        /// <summary>
        /// Convert to Json
        /// </summary>
        /// <param name="data"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string ToJson(this object data, JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(data, settings ?? DefaultSettings);
        }

        /// <summary>
        /// Convert from Json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="settings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FromJson<T>(this string json, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject<T>(json, settings ?? DefaultSettings);
        }


        public static T LoadFromFile<T>(this string path)
        {
            var json = File.ReadAllText(path);
            return json.FromJson<T>();
        }
    }
}
