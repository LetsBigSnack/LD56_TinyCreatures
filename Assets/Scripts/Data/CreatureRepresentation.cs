using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class CreatureRepresentation
    {

        private Dictionary<BodyPartType, BodyPart> _bodyParts;

        public Dictionary<BodyPartType, BodyPart> BodyParts
        {
            get => _bodyParts;
            set => _bodyParts = value;
        }
        
        private Color _headColor;
        private Color _bodyColor;
        private Color _legsColor;
        private Color _armsColor;
        
        [JsonIgnore]
        public Sprite HeadSprite
        {
            get => _bodyParts.ContainsKey(BodyPartType.Head) ? _bodyParts[BodyPartType.Head].bodyPartSprite : null;
        }
        
        [JsonIgnore]
        public Sprite BodySprite
        {
            get => _bodyParts.ContainsKey(BodyPartType.Body) ? _bodyParts[BodyPartType.Body].bodyPartSprite : null;

        }
        
        [JsonIgnore]
        public Sprite LegsSprite
        {
            get => _bodyParts.ContainsKey(BodyPartType.Legs) ? _bodyParts[BodyPartType.Legs].bodyPartSprite : null;

        }
        
        [JsonIgnore]
        public Sprite ArmsSprite
        {
            get => _bodyParts.ContainsKey(BodyPartType.Arms) ? _bodyParts[BodyPartType.Arms].bodyPartSprite : null;

        }

        [JsonConverter(typeof(ColorHandler))]
        public Color HeadColor
        {
            get => _headColor;
            set => _headColor = value;
        }

        [JsonConverter(typeof(ColorHandler))]
        public Color BodyColor
        {
            get => _bodyColor;
            set => _bodyColor = value;
        }

        [JsonConverter(typeof(ColorHandler))]
        public Color LegsColor
        {
            get => _legsColor;
            set => _legsColor = value;
        }
        
        [JsonConverter(typeof(ColorHandler))]
        public Color ArmsColor
        {
            get => _armsColor;
            set => _armsColor = value;
        }

        public CreatureRepresentation(Dictionary<BodyPartType, BodyPart> bodyParts, Color headColor, Color bodyColor, Color legsColor, Color armsColor)
        {
            _bodyParts = bodyParts;
            
            _headColor = headColor;
            _bodyColor = bodyColor;
            _legsColor = legsColor; 
            _armsColor = armsColor;
        }
        
    }
}