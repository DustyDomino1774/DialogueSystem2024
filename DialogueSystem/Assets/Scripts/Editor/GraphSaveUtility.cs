using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor;

public class GraphSaveUtility
{
    private List<Edge> Edges
    {
        get
        {
            return m_graphView.edges.ToList();
        }
    }

    private List<Node> Nodes => m_graphView.nodes.ToList();

    private DialogueGraphView m_graphView;

    public static GraphSaveUtility GetInstance(DialogueGraphView graphView)
    {
        return new GraphSaveUtility
        {
            m_graphView = graphView
        };
    }

    private void SaveNodes(string fileName, DialogueDatabase database)
    {
        var connectedSockets = Edges.Where(x => x.input.node != null).ToArray();
        for (int i = 0; i < connectedSockets.Length; i++) 
        {
            var outNode = connectedSockets[i].output.node as DialogueNode;
            var inputNode = connectedSockets[i].input.node as DialogueNode;
            database.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGUID = outNode.GUID,
                PortName = connectedSockets[i].output.portName,
                TargetNodeGUID = inputNode.GUID,
            });
        }

        foreach(var node in Nodes)
        {
            var outNode = node as DialogueNode;
            if (!outNode.EntryPoint)
            {
                database.database.Add(new DialogueLine
                {
                    GUID = outNode.GUID,
                    dialogue = outNode.dialogueText,
                    position = outNode.GetPosition().position,
                });
            }
        }
    }
    public void SaveGraph(string fileName)
    {
        var dialogueDatabase = ScriptableObject.CreateInstance<DialogueDatabase>();
        SaveNodes(fileName, dialogueDatabase);

        if (!AssetDatabase.IsValidFolder("Assets/Dialogue"))
        {
            AssetDatabase.CreateFolder("Assets", "Dialogue");
        }

        UnityEngine.Object loadAsset =
            AssetDatabase.LoadAssetAtPath($"Assets/Dialogue/{fileName}.asset", 
            typeof(DialogueDatabase));

        if (loadAsset == null || !AssetDatabase.Contains(loadAsset))
        {
            AssetDatabase.CreateAsset(dialogueDatabase, $"Assets/Dialogue/{fileName}.asset");
        }
        else
        {
            DialogueDatabase container = loadAsset as DialogueDatabase;
            container.database = dialogueDatabase.database;
            EditorUtility.SetDirty(container);
        }
        AssetDatabase.SaveAssets();

    }

    public void LoadGraph(string fileName) 
    { 
    }
}