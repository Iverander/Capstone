using UnityEngine;

namespace Capstone
{
    public abstract class Modifier : ScriptableObject
    {
        [field: SerializeField] public Sprite icon { get; private set; }
        [field: SerializeField] public int cost { get; private set; }
        public bool active;
        public virtual void onGained(){}
        public virtual void onActive(){}
        public virtual void onRemoved(){}
    }
}
