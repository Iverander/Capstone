using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Capstone
{
    public class HitEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem effect;
        [SerializeField] private int impactTimeMS;
        
        public async void PlayEffect()
        {
            effect.Play();
            
            Time.timeScale = 0;
            await Task.Delay(impactTimeMS);
            Time.timeScale = 1;
        }
    }
}
