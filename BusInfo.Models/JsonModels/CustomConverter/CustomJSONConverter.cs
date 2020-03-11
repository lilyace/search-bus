using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BusInfo.Models.JsonModels.CustomConverter
{
    class CustomJSONConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var list = new List<RouteDirectionsModel>();
            JToken token = JToken.Load(reader);           
            foreach(var child in token.Children())
            {   
                var model = child.First.ToObject<RouteDirectionsModel>();
                model.Name = ((JProperty)child).Name;
                list.Add(model);
            }
            return list;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
