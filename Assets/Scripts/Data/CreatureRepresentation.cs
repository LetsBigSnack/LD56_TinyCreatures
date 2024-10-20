using System.Collections.Generic;
using UnityEngine;

namespace Data
{
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
        
        public Sprite HeadSprite
        {
            get => _bodyParts.ContainsKey(BodyPartType.Head) ? _bodyParts[BodyPartType.Head].bodyPartSprite : null;
        }

        public Sprite BodySprite
        {
            get => _bodyParts.ContainsKey(BodyPartType.Body) ? _bodyParts[BodyPartType.Body].bodyPartSprite : null;

        }

        public Sprite LegsSprite
        {
            get => _bodyParts.ContainsKey(BodyPartType.Legs) ? _bodyParts[BodyPartType.Legs].bodyPartSprite : null;

        }

        public Sprite ArmsSprite
        {
            get => _bodyParts.ContainsKey(BodyPartType.Arms) ? _bodyParts[BodyPartType.Arms].bodyPartSprite : null;

        }

        public Color HeadColor
        {
            get => _headColor;
            set => _headColor = value;
        }

        public Color BodyColor
        {
            get => _bodyColor;
            set => _bodyColor = value;
        }

        public Color LegsColor
        {
            get => _legsColor;
            set => _legsColor = value;
        }

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