using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace TPM
{
    public static class JsonUtil
    {
        private static readonly MasterJsonConverter _masterJsonConverter = new();

        private static readonly Newtonsoft.Json.Converters.StringEnumConverter _stringEnumConverter = new();
        
        static JsonUtil()
        {
            //MasterJsonConverter.AddWriteFormatter(typeof(Vector3), Vector3WriteFormatter);
            //MasterJsonConverter.AddWriteFormatter(typeof(Grid3), Grid3WriteFormatter);
            //MasterJsonConverter.AddWriteFormatter(typeof(ProtobufResultContainer.ResultType), ProtobufResultTypeWriteFormatter);
        }
        
        public static T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static string SerializeObjectWithIndentation(object value)
        {
            var stringWriter = new StringWriter(new StringBuilder(256), CultureInfo.InvariantCulture);
            using (var jsonTextWriter = new JsonTextWriter(stringWriter))
            {
                jsonTextWriter.Formatting = Formatting.Indented;
                jsonTextWriter.IndentChar = ' ';
                jsonTextWriter.Indentation = 4;

                var serializer = new JsonSerializer();
                serializer.Converters.Add(_masterJsonConverter);
                serializer.Converters.Add(_stringEnumConverter);
                serializer.Serialize(jsonTextWriter, value);
            }

            return stringWriter.ToString();
        }
    }

    public class MasterJsonConverter : JsonConverter
    {
        private static readonly Dictionary<Type, Func<object, string>> WriteFormatter = new();

        public static void AddWriteFormatter(Type type, Func<object, string> formatter)
        {
            WriteFormatter[type] = formatter;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) return;

            var formatter = WriteFormatter[value.GetType()];
            writer.WriteRawValue(formatter(value));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException(
                "Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return WriteFormatter.ContainsKey(objectType);
        }
    }
}