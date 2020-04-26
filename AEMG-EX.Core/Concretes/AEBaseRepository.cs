using Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEMG_EX.Core
{
    public class AEBaseRepository<T> : IAEGenericRepository<T> where T : class
    {
        protected List<T> internalStorage;

        protected virtual string databaseLocation => AEMGStatic.SAVED_AEACTION_FILEPATH;

        public virtual void Add(T aEAction)
        {
            if (internalStorage == null)
                internalStorage = FindAll();

            internalStorage.Add(aEAction);
        }

        public virtual List<T> FindAll()
        {
            if (internalStorage == null)
                internalStorage = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(databaseLocation), new CustomIAEActionJsonConverter());

            return internalStorage;
        }

        public virtual void Remove(T aEAction)
        {
            if (internalStorage == null)
                internalStorage = FindAll();

            internalStorage.Remove(aEAction);
        }

        public virtual bool SaveChange()
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(internalStorage, Formatting.Indented);

                File.WriteAllText(databaseLocation, jsonString);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public class CustomIAEActionJsonConverter : JsonConverter
    {
        public override bool CanRead => base.CanRead;

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IAEAction);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var action = default(IAEAction);

            switch ((AEAction)(int)jObject["AEAction"])
            {
                case AEAction.BossBattle:
                case AEAction.EXPBattle:
                case AEAction.TrashMobBattle:
                    action = new Battle();
                    break;
                case AEAction.FoodAD:
                case AEAction.ReFoodAD:
                    action = new Food();
                    break;
                case AEAction.Wait:
                    action = new AEWait();
                    break;
            }

            serializer.Populate(jObject.CreateReader(), action);

            return action;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
