#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System;

namespace UnityEditor.Extensions
{
    /// <summary>
    /// Class Contains Logic to show all scenes into new windows and navigate thourght that
    /// </summary>
    public class ScenesWindow : EditorWindow
    {
        Vector2 scrollPosition;
        string sceneName = string.Empty;

        public void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            this.titleContent = new GUIContent("All Scenes");
            GUILayout.BeginVertical();
            string[] assetguids = AssetDatabase.FindAssets("t:Scene");

            if (assetguids.Length <= 0)
            {
                EditorGUILayout.HelpBox("There is nothing to Show here..!", MessageType.Warning);
            }
            else
            {
                sceneName = EditorGUILayout.TextField("Enter Scene Name: ", sceneName);
            }

            foreach (var assetguid in assetguids)
            {
                string path = AssetDatabase.GUIDToAssetPath(assetguid);
                string assetname = path.Substring(path.LastIndexOf("/") + 1);

                if (!assetname.ToLower().Contains(sceneName.ToLower()))
                {
                    continue;
                }

                if (GUILayout.Button(new GUIContent(assetname, path)))
                {
                    if (!EditorSceneManager.GetActiveScene().isDirty)
                    {
                        EditorSceneManager.OpenScene(path);
                    }
                    else
                    {
                        int option = EditorUtility.DisplayDialogComplex("Alert", "You want to Save?", "Yes", "No", "Cancel");
                        switch (option)
                        {
                            case 0:
                                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                                EditorSceneManager.OpenScene(path);
                                break;
                            case 1:
                                EditorSceneManager.OpenScene(path);
                                break;
                            case 2:
                                break;
                        }
                    }
                }
            }
            GUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
        }
    }

    /// <summary>
    /// Class Contains Logic to show build scenes into new windows and navigate thourght that
    /// </summary>
    public class BuildScenesWindow : EditorWindow
    {
        Vector2 scrollPosition;
        string sceneName = string.Empty;

        public void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            this.titleContent = new GUIContent("All Build Scenes");

            GUILayout.BeginVertical();
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

            if (scenes.Length <= 0)
            {
                EditorGUILayout.HelpBox("There is nothing to Show here..!", MessageType.Warning);
            }
            else
            {
                sceneName = EditorGUILayout.TextField("Enter Scene Name: ", sceneName);
            }

            foreach (var scene in scenes)
            {
                string path = scene.path;
                string assetname = path.Substring(path.LastIndexOf('/') + 1);

                if (!assetname.ToLower().Contains(sceneName.ToLower()))
                {
                    continue;
                }

                if (GUILayout.Button(new GUIContent(assetname, path)))
                {
                    if (!EditorSceneManager.GetActiveScene().isDirty)
                    {
                        EditorSceneManager.OpenScene(path);
                    }
                    else
                    {
                        int option = EditorUtility.DisplayDialogComplex("Alert", "You want to Save?", "Yes", "No", "Cancel");
                        switch (option)
                        {
                            case 0:
                                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                                EditorSceneManager.OpenScene(path);
                                break;
                            case 1:
                                EditorSceneManager.OpenScene(path);
                                break;
                            case 2:
                                break;
                        }
                    }
                }
            }

            if (GUILayout.Button("Add Scenes To Build Setting"))
            {
                EditorWindow.GetWindow(Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
            }

            GUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
        }
    }
#endif
}