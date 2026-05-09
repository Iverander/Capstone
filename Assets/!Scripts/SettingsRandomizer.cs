using UnityEngine;

namespace Capstone
{
    public class SettingsRandomizer : MonoBehaviour
    {
        private void Start()
        {
            Settings.active.Randomize();
        }
    }
}