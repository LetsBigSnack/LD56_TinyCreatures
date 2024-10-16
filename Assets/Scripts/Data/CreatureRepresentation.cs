using UnityEngine;

namespace Data
{
    public class CreatureRepresentation
    {

        //TODO: change that for scriptable objects
        private Sprite _headSprite;
        private Sprite _bodySprite;
        private Sprite _legsSprite;
        private Sprite _armsSprite;
    
        private Color _headColor;
        private Color _bodyColor;
        private Color _legsColor;
        private Color _armsColor;
        
        public Sprite HeadSprite
        {
            get => _headSprite;
            set => _headSprite = value;
        }

        public Sprite BodySprite
        {
            get => _bodySprite;
            set => _bodySprite = value;
        }

        public Sprite LegsSprite
        {
            get => _legsSprite;
            set => _legsSprite = value;
        }

        public Sprite ArmsSprite
        {
            get => _armsSprite;
            set => _armsSprite = value;
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

        public CreatureRepresentation(Sprite headSprite, Sprite bodySprite, Sprite legsSprite, Sprite armsSprite,
            Color headColor, Color bodyColor, Color legsColor, Color armsColor)
        {
            _headSprite = headSprite;
            _bodySprite = bodySprite;
            _legsSprite = legsSprite;
            _armsSprite = armsSprite;
            
            _headColor = headColor;
            _bodyColor = bodyColor;
            _legsColor = legsColor; 
            _armsColor = armsColor;
            
        }
        
    }
}