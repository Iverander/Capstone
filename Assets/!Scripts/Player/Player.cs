using System;
using System.Collections;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Capstone
{
  [Flags]
  public enum State
  {
    None = 0,
    Sprinting = 1 << 0,
    Grounded = 1 << 1,
    Turning = 1 << 2,
    Falling = 1 << 3,
    Jumping = 1 << 4,
    Walking = 1 << 5,
  }
  [DefaultExecutionOrder(-10000)]
  public class Player : Creature
  {

    public static Player instance;
    public static InputReader input => instance.inputReader;
    public InputReader inputReader = new();

    public static CameraType cameraType => instance.cameraSettings.cameraType;

    [ReadOnly] public State playerState;
    public static State state
    {
      get { return instance.playerState; }
      set { instance.playerState = value; }
    }

    public Camera cam => cameraSettings.activeCamera;
    public CameraSettings cameraSettings { get; private set; }
    public PlayerMovement movement { get; private set; }
    public PlayerCombat combat { get; private set; }
    public PlayerModifier modifier { get; private set; }
    [field: SerializeField] public Animator animator { get; private set; }



    void Start()
    {
      instance = this;
      inputReader.Enable();

      cameraSettings = GetComponent<CameraSettings>();
      combat = GetComponent<PlayerCombat>();
      modifier = GetComponent<PlayerModifier>();
      cameraSettings.CameraChanged += CameraChanged;
    }

    private void CameraChanged()
    {
      if (movement != null) Destroy(movement);

      switch (cameraType)
      {
        case CameraType.ThirdPerson:
          movement = gameObject.AddComponent<ThirdpersonMovement>();
          break;
        case CameraType.Isometric:
          movement = gameObject.AddComponent<IsometricMovement>();
          break;
      }

      playerState = 0;
    }

    private void FixedUpdate()
    {
      Debug.Log((transform.rotation * rb.linearVelocity));
      animator.SetFloat("Speed", rb.linearVelocity.magnitude);
      animator.SetFloat("Direction", movement.moveDirection.x);

      Ray groundRay = new(transform.position + -Vector3.down * .1f, Vector3.down);
      Debug.DrawRay(groundRay.origin, groundRay.direction, Color.red);
      if (Physics.Raycast(groundRay, .2f))
      {
        AddState(State.Grounded);
      }
      else
      {
        RemoveState(State.Grounded);
      }
      if (!state.HasFlag(State.Falling))
      {
        if (rb.linearVelocity.y <= -.5)
          AddState(State.Falling);
      }
      else
      {
        if (rb.linearVelocity.y > -.5)
          RemoveState(State.Falling);
      }
    }

    private void OnDestroy()
    {
      inputReader.Disable();
    }

    public static void AddState(State state)
    {
      Player.state |= state;
    }
    public static void RemoveState(State state)
    {
      Player.state &= ~state;
    }

    public override IEnumerator Stun(float durationSeconds)
    {
      stunned = true;
      yield return new WaitForSeconds(durationSeconds);
      stunned = false;
      //rb.AddForce((transform.position - origin) * (knockback * 10), ForceMode.Force);
    }
  }
}
