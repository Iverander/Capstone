using UnityEngine;

namespace Capstone
{
    public class CameraLock : MonoBehaviour
    {
        private void Start()
        {
            transform.parent = null;
            transform.eulerAngles = Vector3.zero;
        }

        private void Update()
        {
            if (Player.instance != null)
                transform.localPosition = Player.instance.cam.transform.position;
        }
    }
}