using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class SamplesWindow : EditorWindow
{
    [InitializeOnLoadMethod]
    private static void Init()
    {
        EditorSceneManager.sceneOpened += SceneOpened;
    }


    private static void SceneOpened(Scene scene, OpenSceneMode openSceneMode)
    {
        var currentShowcase = (SamplesShowcase)FindFirstObjectByType(typeof(SamplesShowcase));
        if (currentShowcase != null)
            Selection.activeGameObject = currentShowcase.gameObject;
    }
}