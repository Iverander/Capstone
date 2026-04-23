using UnityEngine;

namespace Capstone
{
    public class SettingsRandomizer : MonoBehaviour
    {
        void Start()
        {
            Settings.active.Randomize();
        }

    }
}
