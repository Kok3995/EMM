using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Data
{
    public class CustomJsonConverter : JsonConverter
    {
        public override bool CanRead => base.CanRead;

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IAction));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            var action = default(IAction);

            switch((BasicAction)(int)jObject["BasicAction"])
            {
                case BasicAction.Click:
                    action = new Click();
                    break;
                case BasicAction.Swipe:
                    action = new Swipe();
                    break;
                case BasicAction.Wait:
                    action = new Wait();
                    break;
                case BasicAction.AE:
                    action = new AE();
                    break;
                case BasicAction.ActionGroup:
                    action = new ActionGroup();
                    break;
            }

            serializer.Populate(jObject.CreateReader(), action);

            return action;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new InvalidOperationException("Use the default writer");
        }
    }
}
