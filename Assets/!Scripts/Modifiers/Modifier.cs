using System;
using UnityEngine;

namespace Capstone
{
    public abstract class Modifier : ScriptableObject
    {
        [field: SerializeField] public Sprite icon { get; private set; }
        [field: SerializeField] public int cost { get; private set; }
        public bool active;
        public bool gained { get; private set; } = false;

        public static Action reset;
        
        public virtual void onGained(){ gained = true; reset += ResetMod; }
        public virtual void onActive(){}
        public virtual void onRemoved(){}

        void ResetMod()
        {
            gained = false;
            reset -= ResetMod;
        }
    }
}
