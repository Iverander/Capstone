using UnityEngine;

namespace Capstone
{
    public abstract class Modifier : ScriptableObject
    {
        [field: SerializeField] public Sprite icon { get; private set; }
        [field: SerializeField] public int cost { get; private set; }
        public bool active;

        public bool gained { get; protected set; } = false;
        public virtual void onGained(){ gained = true; }
        public virtual void onActive(){}
        public virtual void onRemoved(){}
    }
}
