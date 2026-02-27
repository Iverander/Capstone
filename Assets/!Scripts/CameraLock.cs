using UnityEngine;

namespace Capstone
{
    public class CameraLock : MonoBehaviour
    {
        
        void Start()
        {
            transform.parent = null;
            transform.eulerAngles = Vector3.zero;
        }

        void Update()
        {
            if(Player.instance != null)
                transform.localPosition = Player.instance.cam.transform.position;
        }
    }
}
