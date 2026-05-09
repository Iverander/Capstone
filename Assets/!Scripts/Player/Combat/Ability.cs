using System;
using System.Collections;
using FMODUnity;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Capstone
{
    [Serializable]
    public abstract class Ability
    {
        [field: SerializeField] public string name { get; private set; }

        [Space] [SerializeField] public float cooldown = .2f;
        [field: SerializeField] public bool onCooldown { get; private set; }
        [field: SerializeField] public Color color { get; private set; } = Color.red;

        [Header("Gizmos")] public bool ShowGizmos;

        [Header("Art")] [SerializeField] protected ParticleSystem effectPrefab;

        [SerializeField] private EventReference sfx;
        [SerializeField] private string[] animationTriggers;
        protected Creature origin;
        public Action<float> performed;


        public void Initialize(Creature origin)
        {
            this.origin = origin;
        }

        public void Perform()
        {
            if (onCooldown || origin.isActing) return;

            origin.StartCoroutine(origin.ActionCooldown());
            if (animationTriggers.Length > 0)
                origin.animator.SetTrigger(animationTriggers[Random.Range(0, animationTriggers.Length)]);
            performed?.Invoke(cooldown);
            if (cooldown > 0)
                origin.StartCoroutine(Cooldown());

            Action();
            if (effectPrefab)
                Effect(Vector3.zero);
        }


        protected abstract void Action();

        protected virtual void Effect(Vector3 offset)
        {
            var effect = Object.Instantiate(effectPrefab, origin.transform);
            effect.transform.position = offset;
            var main = effect.main;
            main.startColor = color;
            Object.Destroy(effect.gameObject, effect.main.duration);
            AudioManager.PlayOneShot(sfx, origin.transform.position);
            effect.Play();
        }

        protected IEnumerator Cooldown()
        {
            onCooldown = true;

            float timer = 0;
            while (timer < cooldown)
            {
                timer += Time.deltaTime * Time.timeScale;
                yield return null;
            }

            onCooldown = false;
        }


        public virtual void Gizmos(Transform origin)
        {
            if (origin == null) return;
            UnityEngine.Gizmos.matrix = origin.localToWorldMatrix;
            UnityEngine.Gizmos.color = color;
        }
    }
}