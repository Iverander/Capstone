using UnityEngine;

namespace Capstone
{
    public class Obstacle : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            gameObject.SetActive(LevelSettings.CurrentMapSettings.obstacles);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
