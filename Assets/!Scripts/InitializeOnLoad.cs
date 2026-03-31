using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;

namespace Capstone
{
    [InitializeOnLoad]
    public static class InitializeOnLoad 
    {
    static InitializeOnLoad()
    {
        EditorApplication.playModeStateChanged += LoadStartUp;
    }

    private static void LoadStartUp(PlayModeStateChange change)
    {
        if (change == PlayModeStateChange.ExitingEditMode)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        if (change == PlayModeStateChange.EnteredPlayMode)
        {
            if(EditorSceneManager.GetActiveScene().buildIndex != 0)
            {
                EditorSceneManager.LoadScene(0);
            }
        }
    }
    }
}
