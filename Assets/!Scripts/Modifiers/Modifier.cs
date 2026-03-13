using UnityEngine;

namespace Capstone
{
    public abstract class Modifier : ScriptableObject
    {
        public bool active;
        public virtual void onGained(){}
        public virtual void onActive(){}
        public virtual void onRemoved(){}
    }
}
