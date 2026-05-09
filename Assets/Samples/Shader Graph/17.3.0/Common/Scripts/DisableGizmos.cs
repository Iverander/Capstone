using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class DisableGizmos : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
#if UNITY_EDITOR
        var view = SceneView.lastActiveSceneView;
        if (view != null) view.drawGizmos = false;
#endif
    }
}