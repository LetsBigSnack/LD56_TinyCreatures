using System;
using System.Linq;
using Data;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using UnityEngine;

public class BodyPartHandler : JsonConverter
{
    public BodyPartHandler()
    {
    }

    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        BodyPart bodyPart = CreatureManager.Instance.BodyPartSets 
            .SelectMany(set => set.bodyPartEntries)
            .Select(partEntry => partEntry.bodyPart)
            .FirstOrDefault(bodyPart => bodyPart.bodyPartName ==  reader.Value as string);

        if (bodyPart == null)
        {
            return null;
        }
        else
        {
            return bodyPart;
        }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        BodyPart bodyPart = value as BodyPart;
        if (bodyPart != null) writer.WriteValue(bodyPart.bodyPartName);
    }
}