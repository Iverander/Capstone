using UnityEngine;
using UnityEngine.SceneManagement;

namespace Capstone
{
    public class ShopScene : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 3)
            {
                SceneManager.LoadScene("ShopScene");
            }
        }
    }
}