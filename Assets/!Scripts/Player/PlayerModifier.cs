using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Capstone
{
    public class PlayerModifier : MonoBehaviour
    {
        [Expandable] public List<Modifier> modifiers = new();

        private void Start()
        {
            foreach (var modifier in modifiers) modifier.onGained();
        }

        public void AddModifier(Modifier modifier)
        {
            if (modifiers.Contains(modifier)) throw new Exception("modifier already exists!");
            modifiers.Add(modifier);
            modifier.onGained();
        }

        public void RemoveModifier(Modifier modifier)
        {
            if (!modifiers.Contains(modifier)) throw new Exception("modifier doesn't exist!");
            modifiers.Remove(modifier);
        }
    }
}