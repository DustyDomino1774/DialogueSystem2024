using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView m_GraphView;
    private string m_fileName;

    [MenuItem("Graph/Dialogue Graph")]
    public static void CreateDialogueWIndow()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Narrative Graph");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    private void SaveLoadData(bool isSaving)
    {
        if (!string.IsNullOrEmpty(m_fileName)) 
        {
            var saveUtility = GraphSaveUtility.GetInstance(m_GraphView);
            if (isSaving)
            {
                saveUtility.SaveGraph(m_fileName);
            }
            else
            {
                saveUtility.LoadGraph(m_fileName);
            }
        }
        else
        {
            EditorUtility.DisplayDialog("Invalid Filename", "Please Enter a valid Name", "OK");
        }
    }

    

    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        var fileNameTextField = new TextField("File Name:");
        fileNameTextField.SetValueWithoutNotify(m_fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => m_fileName = evt.newValue);
        toolbar.Add(fileNameTextField);

        toolbar.Add(new Button(() => SaveLoadData(true)) {text = "Save Data"});

        toolbar.Add(new Button(() => SaveLoadData(false)) { text = "Load Data" });

        toolbar.Add(new Button(() => m_GraphView.AddElement(m_GraphView.CreateNode("Dialogue Node", 
            new Vector2(0,0))))
            {
                text = "New Node"
            });
        rootVisualElement.Add(toolbar);
    }

    private void ConstructGraphView()
    {
        m_GraphView = new DialogueGraphView()
        {
            name = "Narrative Graph"
        };
        m_GraphView.StretchToParentSize();
        rootVisualElement.Add(m_GraphView);
    }
}
