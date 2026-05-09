using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class AlignSceneView : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        AlignCamera(transform);
    }


    private static void AlignCamera(Transform target)
    {
#if UNITY_EDITOR
        var view = SceneView.lastActiveSceneView;
        if (view == null) return;
        var sceneCam = view.camera;
        if (sceneCam == null) return;
        sceneCam.transform.position = target.position;
        sceneCam.transform.rotation = target.rotation;
        view.AlignViewToObject(sceneCam.transform);
#endif
    }
}