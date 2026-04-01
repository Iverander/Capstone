using UnityEngine;

namespace Capstone
{
    public class Obstacle : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            gameObject.SetActive(Settings.mapSettings.obstacles);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
