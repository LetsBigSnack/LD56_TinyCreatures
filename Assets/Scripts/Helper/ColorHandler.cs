using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Data;

public class ColorHandler : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(BaseColor) || objectType == typeof(AddOnColor);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var colorMap = new Dictionary<string, Color>();
        if (reader.TokenType == JsonToken.StartObject)
        {
            reader.Read();
            while (reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = reader.Value.ToString();
                reader.Read();

                string colorString = reader.Value.ToString();
                if (ColorUtility.TryParseHtmlString("#" + colorString, out Color loadedColor))
                {
                    colorMap[propertyName] = new Color(
                        loadedColor.r,
                        loadedColor.g,
                        loadedColor.b,
                        loadedColor.a
                    );
                }
                else
                {
                    Debug.LogError($"Failed to parse color: #{colorString}");
                }
                reader.Read();
            }
        }

        if (objectType == typeof(BaseColor))
        {
            return new BaseColor
            {
                BaseColor1 = colorMap.GetValueOrDefault("BaseColor1", Color.white),
                BaseColor2 = colorMap.GetValueOrDefault("BaseColor2", Color.white),
                BaseColor3 = colorMap.GetValueOrDefault("BaseColor3", Color.white),
                BaseColor4 = colorMap.GetValueOrDefault("BaseColor4", Color.white)
            };
        }
        else if (objectType == typeof(AddOnColor))
        {
            return new AddOnColor
            {
                addOnColor1 = colorMap.GetValueOrDefault("addOnColor1", Color.white),
                addOnColor2 = colorMap.GetValueOrDefault("addOnColor2", Color.white),
                addOnColor3 = colorMap.GetValueOrDefault("addOnColor3", Color.white)
            };
        }

        return null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        if (value is BaseColor baseColor)
        {
            WriteColorProperty(writer, "BaseColor1", baseColor.BaseColor1);
            WriteColorProperty(writer, "BaseColor2", baseColor.BaseColor2);
            WriteColorProperty(writer, "BaseColor3", baseColor.BaseColor3);
            WriteColorProperty(writer, "BaseColor4", baseColor.BaseColor4);
        }
        else if (value is AddOnColor addOnColor)
        {
            WriteColorProperty(writer, "addOnColor1", addOnColor.addOnColor1);
            WriteColorProperty(writer, "addOnColor2", addOnColor.addOnColor2);
            WriteColorProperty(writer, "addOnColor3", addOnColor.addOnColor3);
        }

        writer.WriteEndObject();
    }

    private void WriteColorProperty(JsonWriter writer, string propertyName, Color color)
    {
        writer.WritePropertyName(propertyName);
        writer.WriteValue(ColorUtility.ToHtmlStringRGBA(new Color32(
            (byte)(color.r * 255),
            (byte)(color.g * 255),
            (byte)(color.b * 255),
            (byte)(color.a * 255)
        )));
    }
}
