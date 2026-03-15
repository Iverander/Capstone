using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Capstone
{
  public class FPSSetter : MonoBehaviour
  {
    [SerializeField] private int fps = 60;
    private void Start()
    {
      Application.targetFrameRate = fps;
      transform.localPosition = Vector3.zero;
    }
  }
}
