using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Do.Scripts.Tools.Editor
{
    public class ListSceneEditor : EditorWindow
    {
        [MenuItem("Tools/List Scene")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ListSceneEditor)).titleContent = new GUIContent("List Scene Panel");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Delete All Pref"))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }
            GUILayout.Label("List Scene", EditorStyles.boldLabel);
            var scenes = EditorBuildSettings.scenes;
            if (scenes.Length <= 0)
                return;
            foreach (var scene in scenes)
            {
                if (!GUILayout.Button(scene.path)) 
                    continue;
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(scene.path);
            }
        }
    }
}
