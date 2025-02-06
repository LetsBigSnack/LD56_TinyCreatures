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

        private Color _topHeadColor;
        private Color _headColor;
        private Color _bodyColor;
        private Color _legsColor;
        private Color _armsColor;
        private Color _backColor;
        private Color _tailColor;
        
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

        [JsonConverter(typeof(ColorHandler))]
        public Color TopHeadColor
        {
            get => _topHeadColor;
            set => _topHeadColor = value;
        }

        [JsonConverter(typeof(ColorHandler))]
        public Color BackColor
        {
            get => _backColor;
            set => _backColor = value;
        }

        [JsonConverter(typeof(ColorHandler))]
        public Color tailColor
        {
            get => _tailColor;
            set => _tailColor = value;
        }

        public CreatureRepresentation(Dictionary<BodyPartType, BodyPart> bodyParts, Color headColor, Color bodyColor, Color legsColor, Color armsColor, Color topHeadColor, Color backColor, Color tailColor)
        {
            _bodyParts = bodyParts;
            
            _headColor = headColor;
            _bodyColor = bodyColor;
            _legsColor = legsColor; 
            _armsColor = armsColor;
            _backColor = backColor;
            _tailColor = tailColor;
            _topHeadColor = topHeadColor;
        }
        
    }
}