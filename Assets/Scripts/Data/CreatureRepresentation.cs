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

        private Color32[] _baseColor;
        private Color32[] _addOnColor;
        
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

        [JsonIgnore]
        public Sprite TopHeadSprite
        {
            get => _bodyParts.ContainsKey(BodyPartType.TopHead) ? _bodyParts[BodyPartType.TopHead].bodyPartSprite : null;

        }

        [JsonIgnore]
        public Sprite BackSprite
        {
            get => _bodyParts.ContainsKey(BodyPartType.Back) ? _bodyParts[BodyPartType.Back].bodyPartSprite : null;

        }

        [JsonIgnore]
        public Sprite TailSprite
        {
            get => _bodyParts.ContainsKey(BodyPartType.Tail) ? _bodyParts[BodyPartType.Tail].bodyPartSprite : null;

        }

        [JsonIgnore]
        public BodyPart HeadBodyPart
        {
            set => _bodyParts[BodyPartType.Head] = value;
        }

        [JsonIgnore]
        public BodyPart TopHeadBodyPart
        {
            set => _bodyParts[BodyPartType.TopHead] = value;
        }

        [JsonIgnore]
        public BodyPart BodyBodyPart
        {
            set => _bodyParts[BodyPartType.Body] = value;
        }

        [JsonIgnore]
        public BodyPart ArmsBodyPart
        {
            set => _bodyParts[BodyPartType.Arms] = value;
        }

        [JsonIgnore]
        public BodyPart LegsBodyPart
        {
            set => _bodyParts[BodyPartType.Legs] = value;
        }

        [JsonIgnore]
        public BodyPart TailBodyPart
        {
            set => _bodyParts[BodyPartType.Tail] = value;
        }

        [JsonIgnore]
        public BodyPart BackBodyPart
        {
            set => _bodyParts[BodyPartType.Back] = value;
        }

        [JsonConverter(typeof(ColorHandler))]
        public Color32[] BaseColor
        {
            get => _baseColor;
            set => _baseColor = value;
        }

        [JsonConverter(typeof(ColorHandler))]
        public Color32[] AddOnColor
        {
            get => _addOnColor;
            set => _addOnColor = value;
        }

        public CreatureRepresentation(Dictionary<BodyPartType, BodyPart> bodyParts, Color32[] baseColor, Color32[] addOnColor)
        {
            _bodyParts = bodyParts;
            _baseColor = baseColor;
            _addOnColor = addOnColor;
        }
        
    }
}