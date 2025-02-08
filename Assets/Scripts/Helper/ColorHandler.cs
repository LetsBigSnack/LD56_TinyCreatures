using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ColorHandler : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Color32) || objectType == typeof(Color32[]);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartArray)
        {
            List<Color32> colors = new List<Color32>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray) break;

                string colorString = reader.Value.ToString();
                if (ColorUtility.TryParseHtmlString("#" + colorString, out Color loadedColor))
                {
                    colors.Add(new Color32(
                        (byte)(loadedColor.r * 255),
                        (byte)(loadedColor.g * 255),
                        (byte)(loadedColor.b * 255),
                        (byte)(loadedColor.a * 255)
                    ));
                }
                else
                {
                    Debug.LogError($"Failed to parse color: #{colorString}");
                }
            }
            return colors.ToArray();
        }
        else
        {
            string colorString = reader.Value.ToString();
            if (ColorUtility.TryParseHtmlString("#" + colorString, out Color loadedColor))
            {
                return new Color32(
                    (byte)(loadedColor.r * 255),
                    (byte)(loadedColor.g * 255),
                    (byte)(loadedColor.b * 255),
                    (byte)(loadedColor.a * 255)
                );
            }
            Debug.LogError($"Failed to parse color: #{colorString}");
            return new Color32(255, 255, 255, 255);
        }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is Color32[] colorArray)
        {
            writer.WriteStartArray();
            foreach (Color32 color in colorArray)
            {
                string colorString = ColorUtility.ToHtmlStringRGB(color);
                writer.WriteValue(colorString);
            }
            writer.WriteEndArray();
        }
        else if (value is Color32 singleColor)
        {
            string colorString = ColorUtility.ToHtmlStringRGB(singleColor);
            writer.WriteValue(colorString);
        }
    }
}
