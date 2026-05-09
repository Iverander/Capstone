using System.Collections.Generic;
using FMOD;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace FMODUnity
{
    [AddComponentMenu("FMOD Studio/FMOD Studio Listener")]
    public class StudioListener : MonoBehaviour
    {
        private static readonly List<StudioListener> listeners = new();

        [SerializeField] private bool nonRigidbodyVelocity;

        [SerializeField] private GameObject attenuationObject;

        private Vector3 lastFramePosition = Vector3.zero;

#if UNITY_PHYSICS_EXIST
        private Rigidbody rigidBody;
#endif
#if UNITY_PHYSICS2D_EXIST
        private Rigidbody2D rigidBody2D;
#endif

        public GameObject AttenuationObject
        {
            get => attenuationObject;
            set => attenuationObject = value;
        }

        public static int ListenerCount => listeners.Count;

        public int ListenerNumber => listeners.IndexOf(this);

        private void Update()
        {
            if (ListenerNumber < 0 || ListenerNumber >= CONSTANTS.MAX_LISTENERS) return;

            if (nonRigidbodyVelocity)
            {
                var velocity = Vector3.zero;
                var position = transform.position;

                if (Time.deltaTime != 0)
                {
                    velocity = (position - lastFramePosition) / Time.deltaTime;
                    velocity = Vector3.ClampMagnitude(velocity, 20.0f);
                }

                lastFramePosition = position;

                RuntimeManager.SetListenerLocation(ListenerNumber, gameObject, attenuationObject, velocity);
            }
            else
            {
#if UNITY_PHYSICS_EXIST
                if (rigidBody)
                    RuntimeManager.SetListenerLocation(ListenerNumber, gameObject, rigidBody, attenuationObject);
                else
#endif
#if UNITY_PHYSICS2D_EXIST
                if (rigidBody2D)
                    RuntimeManager.SetListenerLocation(ListenerNumber, gameObject, rigidBody2D, attenuationObject);
                else
#endif
                    RuntimeManager.SetListenerLocation(ListenerNumber, gameObject, attenuationObject);
            }
        }

        private void OnEnable()
        {
            RuntimeUtils.EnforceLibraryOrder();
#if UNITY_PHYSICS_EXIST
            rigidBody = gameObject.GetComponent<Rigidbody>();

            if (nonRigidbodyVelocity && rigidBody)
            {
                Debug.LogWarning(string.Format(
                    "[FMOD] Non-Rigidbody Velocity is enabled on Listener attached to GameObject \"{0}\", which also has a Rigidbody component attached - this will be disabled in favor of velocity from Rigidbody component.",
                    name));
                nonRigidbodyVelocity = false;
            }
#endif
#if UNITY_PHYSICS2D_EXIST
            rigidBody2D = gameObject.GetComponent<Rigidbody2D>();

            if (nonRigidbodyVelocity && rigidBody2D)
            {
                Debug.LogWarning(string.Format(
                    "[FMOD] Non-Rigidbody Velocity is enabled on Listener attached to GameObject \"{0}\", which also has a Rigidbody2D component attached - this will be disabled in favor of velocity from Rigidbody2D component.",
                    name));
                nonRigidbodyVelocity = false;
            }
#endif
            AddListener(this);

            lastFramePosition = transform.position;
        }

        private void OnDisable()
        {
            RemoveListener(this);
        }

        public static float DistanceToNearestListener(Vector3 position)
        {
            var result = float.MaxValue;
            for (var i = 0; i < listeners.Count; i++)
                if (listeners[i].attenuationObject == null)
                    result = Mathf.Min(result, Vector3.Distance(position, listeners[i].transform.position));
                else
                    result = Mathf.Min(result,
                        Vector3.Distance(position, listeners[i].attenuationObject.transform.position));

            return result;
        }

        public static float DistanceSquaredToNearestListener(Vector3 position)
        {
            var result = float.MaxValue;
            for (var i = 0; i < listeners.Count; i++)
                if (listeners[i].attenuationObject == null)
                    result = Mathf.Min(result, (position - listeners[i].transform.position).sqrMagnitude);
                else
                    result = Mathf.Min(result,
                        (position - listeners[i].attenuationObject.transform.position).sqrMagnitude);

            return result;
        }

        private static void AddListener(StudioListener listener)
        {
            // Is the listener already in the list?
            if (listeners.Contains(listener))
            {
                Debug.LogWarning(string.Format("[FMOD] Listener has already been added at index {0}.",
                    listener.ListenerNumber));
                return;
            }

            // If already at the max numListeners
            if (listeners.Count >= CONSTANTS.MAX_LISTENERS)
                Debug.LogWarning(
                    string.Format("[FMOD] Max number of listeners reached : {0}.", CONSTANTS.MAX_LISTENERS));

            listeners.Add(listener);
            RuntimeManager.StudioSystem.setNumListeners(Mathf.Clamp(listeners.Count, 1, CONSTANTS.MAX_LISTENERS));
        }

        private static void RemoveListener(StudioListener listener)
        {
            listeners.Remove(listener);
            RuntimeManager.StudioSystem.setNumListeners(Mathf.Clamp(listeners.Count, 1, CONSTANTS.MAX_LISTENERS));
        }
    }
}