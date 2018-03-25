using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class FindReferencesWindow : EditorWindow
{
    UnityEngine.Object obj;

    List<Component> searchResultList;

    private void OnEnable()
    {
        searchResultList = new List<Component>();
    }

    private void OnGUI()
    {
        this.titleContent = new GUIContent("Find References");

        obj = EditorGUILayout.ObjectField("Find Reference:", obj, typeof(object), true);

        if (GUILayout.Button("Search"))
        {
            searchResultList.Clear();

            if (obj == null)
            {
                Debug.Log("There is nothing selected");
                return;
            }

            Component[] components = GameObject.FindObjectsOfType<MonoBehaviour>();

            GameObject selected = Selection.activeGameObject;

            string retString = String.Empty;

            Component component;

            for (int i = 0; i < components.Length; i++)
            {
                component = components[i];

                FieldInfo[] fields = component.GetType().GetFields();

                foreach (FieldInfo field in fields)
                {
                    if (obj.Equals(field.GetValue(component)))
                    {
                        searchResultList.Add(component);
                        Debug.LogWarning("GameObject Name: " + component.name + " \nClass Name: " + component.GetType().ToString() + " \nField Name: " + field.Name, component.gameObject);
                    }
                }
            }
        }

        foreach(Component component in searchResultList)
        {
            EditorGUILayout.ObjectField(component.name, component, component.GetType(), true);
        }
    }


}
