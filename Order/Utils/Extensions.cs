using Newtonsoft.Json;

namespace Order.Utils
{
    internal static class StringExtensions
    {
        internal static bool HasValue(this string input)
        {
            return !string.IsNullOrEmpty(input);
        }

        internal static string ReadAllText(this string input)
        {
            return !input.HasValue() ? "" : File.ReadAllText(input);
        }
    }

    internal static class SerializationExtensions
    {
        internal static T? DeserializeTo<T>(this string self) where T : class
        {
            return !self.HasValue() ? null : JsonConvert.DeserializeObject<T>(self);
        }

        internal static T? GetData<T>(this string fileName) where T : class
        {
            return fileName.ReadAllText() == null ? null : JsonConvert.DeserializeObject<T>(fileName.ReadAllText());
        }

        internal static void WriteData<T>(this string fileName, T model)
        {
            using (StreamWriter writer = File.CreateText(fileName))
            {
                string output = JsonConvert.SerializeObject(model);
                writer.Write(output);
            }
        }
    }
}
