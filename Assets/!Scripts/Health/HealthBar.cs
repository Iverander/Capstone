using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Capstone
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health health;
        
        ProgressBar progressBar;

        private void Start()
        {
            health.healthChanged += UpdateProgress;
            health.maxHealthChanged += (f) => //changes the max of the progress bar when max hp is changed
            {
                progressBar.highValue = f;
            };
            progressBar = GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>();
            progressBar.highValue = health.maxHealth;
            progressBar.value = health.health;
        }

        private void OnDestroy()
        {
            health.healthChanged -= UpdateProgress;
        }

        ///Updates the progress:)
        public void UpdateProgress(float health)
        {
            progressBar.value = health;
        }

    }
}
