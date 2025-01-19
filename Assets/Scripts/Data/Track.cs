using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class Track
    {
        public string name;
        public AudioSource source;
        public bool isEnabled;
        public bool isUnlocked;
    }
}
