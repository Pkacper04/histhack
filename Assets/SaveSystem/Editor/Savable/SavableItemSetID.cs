using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Experimental.SceneManagement;
using NaughtyAttributes.Editor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(SavableItem),true)]
public class SavableItemSetID : Editor
{
    SerializedProperty id;
    SerializedProperty guid;


    private void OnEnable()
    {
        if (PrefabStageUtility.GetCurrentPrefabStage() != null)
            return;

        SavableItem item = (SavableItem)target;

        int maxNumber = 0;
        List<int> numbersInBeetween = new List<int>();

        if(item.Guid.Length == 0)
        {
            item.Guid = Guid.NewGuid().ToString();
            foreach(SavableItem savable in GameObject.FindObjectsOfType<SavableItem>())
            {
                if(savable.Guid != string.Empty)
                {
                    maxNumber++;
                    numbersInBeetween.Add(savable.Id);
                }
            }

            numbersInBeetween.Sort();

            for(int i=0;i<numbersInBeetween.Count-1;i++)
            {
                if (numbersInBeetween[i] != numbersInBeetween[i+1]-1)
                {
                    item.Id = numbersInBeetween[i] + 1;
                    EditorUtility.SetDirty(target);
                    return;
                }
            }
            item.Id = maxNumber;
            EditorUtility.SetDirty(target);
        }
        else
        {
            if(CheckIfGuidExists(item.Guid,item))
            {
                item.Guid = string.Empty;
                item.Id = 0;
                OnEnable();
                return;
            }
        }

        id = serializedObject.FindProperty("id");
        guid = serializedObject.FindProperty("guid");
    }

    public override void OnInspectorGUI()
    {
        NaughtyEditorGUI.PropertyField_Layout(id, false);
        NaughtyEditorGUI.PropertyField_Layout(guid, false);
        NaughtyInspector.DrawPropertiesExcluding(serializedObject, new string[] {
            "id",
            "guid"
        });
    }


        private bool CheckIfGuidExists(string guid, SavableItem myItem)
    {
        foreach(SavableItem item in GameObject.FindObjectsOfType<SavableItem>())
        {
            if(item.Guid == guid && item.gameObject.GetInstanceID() != myItem.gameObject.GetInstanceID())
                return true;
        }
        return false;
    }


}
