#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

namespace UnityEditor.Extensions
{
    /// <summary>
    /// Class contains Utility methods that will be shown in Tools Menu in Editors Menubar 
    /// </summary>
    public class Extensions
    {
        static string[] _folders = new string[] { "AnimationClip", "AudioClip", "AudioMixer", "Font", "GUISkin", "Material", "Mesh", "Model", "PhysicsMaterial", "Prefab", "Scene", "Script", "Shader", "Sprite", "Texture" };
        static List<string> folders = new List<string>(_folders);
        static string backUpPath = @"D:\Kashyap\Unity_Backup";

        /// <summary>
        /// Create all Types of Folders like Scenes, Scripts, Textures etc...
        /// </summary>
        [MenuItem("Tools/Create Folders")]
        public static void CreateFolders()
        {
            string[] subDirectories = AssetDatabase.GetSubFolders("Assets");
            folders.ForEach(folder =>
            {
                if (!subDirectories.Contains("Assets/" + folder))
                {
                    AssetDatabase.CreateFolder("Assets", folder);
                }
            });
        }

        /// <summary>
        /// Take Backup At BackUp location
        /// </summary>
        [MenuItem("Tools/Backup")]
        public static void Backup()
        {
            string pathName = EditorUtility.SaveFolderPanel("Backup Project", "", "");
            if (string.IsNullOrEmpty(pathName))
            {
                Debug.Log("Save Canceled");
                return;
            }

            backUpPath = pathName;
            string projectPath = Application.dataPath;
            projectPath = projectPath.Substring(0, projectPath.LastIndexOf("/"));
            string projectName = projectPath.Substring(projectPath.LastIndexOf("/") + 1);
            projectPath = projectPath.Substring(0, projectPath.LastIndexOf("/"));
            string date = " " + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + " " + DateTime.Now.Hour + "-" + DateTime.Now.Minute;
            CopyDirectory(projectPath, backUpPath);
            string project = Path.Combine(backUpPath, projectName);
            string newproject = Path.Combine(backUpPath, projectName + date);
            Directory.Move(project, newproject);
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            //p.StartInfo = new System.Diagnostics.ProcessStartInfo(string.Format("explorer.exe {0}", backUpPath));
            p.StartInfo = new System.Diagnostics.ProcessStartInfo(string.Format("explorer.exe"));
            p.Start();
        }

        /// <summary>
        /// Toggle Shot/Hide selected GameObject
        /// </summary>
        [MenuItem("Tools/Utility/Toggle Active %U")]
        public static void ToggleShowHide()
        {
            foreach (var gameObject in Selection.gameObjects)
            {
                gameObject.SetActive(!gameObject.activeSelf);
            }
        }

        /// <summary>
        /// Toggle Lock Inspector 
        /// </summary>
        [MenuItem("Tools/Utility/Toggle Lock %I")]
        public static void ToggleLockUnlock()
        {
            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        }

        /// <summary>
        /// Create basic materials like red, white etc...
        /// </summary>
        [MenuItem("Tools/Basic Material")]
        public static void CreateMaterials()
        {
            Dictionary<string, Color> colors = new Dictionary<string, Color>();
            colors.Add("red", Color.red);
            colors.Add("green", Color.green);
            colors.Add("blue", Color.blue);
            colors.Add("cyan", Color.cyan);
            colors.Add("yellow", Color.yellow);
            colors.Add("black", Color.black);
            colors.Add("magenta", Color.magenta);
            colors.Add("white", Color.white);

            foreach (KeyValuePair<string, Color> color in colors)
            {
                Material mat = new Material(Shader.Find("Standard"));
                mat.color = color.Value;
                string matName = @"Assets\Material\" + color.Key + ".mat";
                AssetDatabase.CreateAsset(mat, matName);
            }
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Opens window with all scenes into the project
        /// </summary>
        [MenuItem("Tools/Scenes/All Scenes %L")]
        public static void Scenes()
        {
            EditorWindow.GetWindow(typeof(ScenesWindow));
        }

        /// <summary>
        /// Opens window with all build scenes means all scenes that are in Build Setting
        /// </summary>
        [MenuItem("Tools/Scenes/Build Scenes")]
        public static void BuildScenes()
        {
            EditorWindow.GetWindow(typeof(BuildScenesWindow));
        }

        #region Helper Functions
        private static void CopyDirectory(string source, string destination)
        {
            DirectoryInfo dirSource = new DirectoryInfo(source);
            DirectoryInfo targetSource = new DirectoryInfo(destination);

            CopyDirectoryContents(dirSource, targetSource);
        }
        private static void CopyDirectoryContents(DirectoryInfo dirSource, DirectoryInfo targetSource)
        {
            try
            {
                Directory.CreateDirectory(targetSource.FullName);
                foreach (var file in dirSource.GetFiles())
                {
                    Debug.Log(string.Format(@"Coping : {0}\{1}", targetSource.FullName, file.Name));
                    file.CopyTo(Path.Combine(targetSource.FullName, file.Name), true);
                }
                foreach (var directory in dirSource.GetDirectories())
                {
                    DirectoryInfo nextTargetDirectory = targetSource.CreateSubdirectory(directory.Name);
                    CopyDirectoryContents(directory, nextTargetDirectory);
                }
            }
            catch (Exception)
            {


            }

        }
        #endregion
    }
#endif
}
