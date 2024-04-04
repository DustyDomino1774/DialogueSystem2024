using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{

    public readonly Vector2 DefaultNodeSize = new Vector2(200, 150);

    public DialogueGraphView()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new FreehandSelector());
        
        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();
        grid.SendToBack();

        AddElement(GetStartNodeInstance());
    }

    public DialogueNode CreateNode(string nodeName, Vector2 position)
    {
        DialogueNode node = new DialogueNode()
        {
            title = nodeName,
            dialogueText = nodeName,
            GUID = Guid.NewGuid().ToString()
        };
        
        // Create port and change name
        Port port = GetPortInstance(node, Direction.Input, Port.Capacity.Multi);
        port.portName = "Input";

        //Add created port to node
        node.inputContainer.Add(port);
        node.RefreshExpandedState();
        node.RefreshPorts();
        node.SetPosition(new Rect(position, DefaultNodeSize));

        var textField = new TextField("Title");
        textField.value = nodeName;
        textField.RegisterValueChangedCallback(evt =>
        {
            node.dialogueText = evt.newValue;
            node.title = evt.newValue;
        });
        textField.SetValueWithoutNotify(node.title);
        node.mainContainer.Add(textField);

        //Create button with add choice function
        var button = new Button(() => { AddChoicePort(node); })
        {
            text = "Add Choice"
        };
        
        //add created button to node
        node.titleButtonContainer.Add(button);

        return node;
    }

    public void AddChoicePort(DialogueNode node, string overridePortName = "")
    {
        //Get port and current port count
        Port generatedPort = GetPortInstance(node, Direction.Output);

        var portLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(portLabel);

        int outputPortCount = node.outputContainer.Query("connector").ToList().Count;
        //If port was passsed with a name make port name choice {current port count + 1}
        string outPortName = string.IsNullOrEmpty(overridePortName) ?
            $"Choice {outputPortCount + 1}" : overridePortName;
        generatedPort.portName = outPortName;

        var textField = new TextField()
        {
            name = string.Empty,
            value = outPortName
        };
        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
        textField.style.minWidth = 60;
        textField.style.maxWidth = 60;
        generatedPort.contentContainer.Add(new Label(" "));
        generatedPort.contentContainer.Add(textField);
        
        //Create button with remove choice fucntion
        var button = new Button(() => { RemoveChoicePort(node, generatedPort); })
        {
            text = "X"
        };
        //add the remove button to the generated port
        generatedPort.contentContainer.Add(button);
        
        //add the generated port to the node
        node.outputContainer.Add(generatedPort);
        //refresh ports and nodes
        node.RefreshExpandedState();
        node.RefreshPorts();
    }

    private void RemoveChoicePort(DialogueNode node, Port generatedPort)
    {
         //Find the edges that are connected to this port.
        var targetEdges = edges.ToList();
        List<Edge> foundEdges = new List<Edge>();
        foreach (Edge edge in targetEdges) 
        {
            if (edge.output.portName == generatedPort.portName && 
                edge.output.node == generatedPort.node)
            {
                foundEdges.Add(edge);
            }
        }
        foreach (Edge edge in foundEdges) 
        {
            edge.input.Disconnect(edge);
            RemoveElement(edge);
        }

        //Remove the corresponding port and refresh the node
        node.outputContainer.Remove(generatedPort);
        node.RefreshPorts();
        node.RefreshExpandedState();
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter adapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        Port startPortView = startPort;

        foreach(Port port in ports)
        {
            var portView = port;
            if (startPortView != port && startPortView.node != port.node
                && startPortView.direction != portView.direction)
            {
                compatiblePorts.Add(port);
            }
        }

        return compatiblePorts;
    }

    private Port GetPortInstance(DialogueNode node, Direction nodeDirection,
        Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, nodeDirection, 
            capacity, typeof(float));
    }

    private DialogueNode GetStartNodeInstance()
    {
        DialogueNode node = new DialogueNode()
        {
            title = "START",
            GUID = Guid.NewGuid().ToString(),
            dialogueText = "ENTRYPOINT",
            EntryPoint = true
        };

        Port generatedPort = GetPortInstance(node, Direction.Output);
        generatedPort.portName = "Next";
        node.outputContainer.Add(generatedPort);

        node.RefreshExpandedState();
        node.RefreshPorts();
        node.SetPosition(new Rect(100,200, 100, 150));

        return node;
    }
}
