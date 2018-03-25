using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
public class CustomTransform : Editor
{
    private Transform _transform;
    private Vector2 _scrollPosition;
    private string _prefabName;

    private void OnEnable()
    {
        _transform = target as Transform;
        _prefabName = _transform.name;
    }

    public override void OnInspectorGUI()
    {
        DrawCustomInspector();
    }

    private void DrawCustomInspector()
    {

        EditorGUILayout.BeginVertical();

        // Draw Local data
        _transform.localPosition = EditorGUILayout.Vector3Field("Local Position", _transform.localPosition);
        _transform.localEulerAngles = EditorGUILayout.Vector3Field("Local Rotation", _transform.localEulerAngles);
        _transform.localScale = EditorGUILayout.Vector3Field("Local Scale", _transform.localScale);

        EditorGUILayout.Space();

        // Draw Global Data
        _transform.position = EditorGUILayout.Vector3Field("Global Position", _transform.position);
        _transform.eulerAngles = EditorGUILayout.Vector3Field("Global Rotation", _transform.eulerAngles);
        _transform.localScale = EditorGUILayout.Vector3Field("Global Scale", _transform.lossyScale);

        EditorGUILayout.Space();

        MakePrefab();

        EditorGUILayout.EndVertical();
    }

    private void MakePrefab()
    {
        // if this is already prefab then skip next code
        if (IsPrefabInstance)
        {
            return;
        }

        _prefabName = EditorGUILayout.TextField("Prefab Name", _prefabName);
        _prefabName = _prefabName.Trim();

        // Make prefab button (when clicked, this object become prefabs unders "Assets/Prefabs" folder with name of this gameobject name
        if (GUILayout.Button("Make Prefab"))
        {

            if (string.IsNullOrEmpty(_prefabName))
            {
                Debug.LogError("Prefab name should not be empty");
                return;
            }

            // assign prefab name based on gameobject name
            string prefabName = _prefabName + ".prefab";


            // check if directory don't exists then create new one
            DirectoryInfo directory = new DirectoryInfo("Assets/Prefabs");
            if (!directory.Exists)
            {
                directory.Create();
            }

            FileInfo file = new FileInfo("Assets/Prefabs/" + prefabName);
            if (file.Exists)
            {
                if (!EditorUtility.DisplayDialog("Alert", prefabName + " is already exists. Are you want to overwrite?", "Yes", "No"))
                {
                    return;
                }
            }

            // create prefab
            GameObject obj = PrefabUtility.CreatePrefab("Assets/Prefabs/" + prefabName, _transform.gameObject);
            GameObject newObj = PrefabUtility.InstantiatePrefab(obj) as GameObject;

            newObj.transform.parent = _transform.parent;

            newObj.transform.localPosition = _transform.localPosition;
            newObj.transform.localEulerAngles = _transform.localEulerAngles;
            newObj.transform.localScale = _transform.localScale;
            DestroyImmediate(_transform.gameObject);
        }
    }

    bool IsPrefabInstance
    {
        get
        {
            return PrefabUtility.GetPrefabParent(_transform) != null && PrefabUtility.GetPrefabObject(_transform) != null;
        }
    }

}
