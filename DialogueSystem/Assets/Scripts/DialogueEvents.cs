using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIResponseData
{
    public string response;
    public UnityAction buttonAction;
    public int karmaScore;
}

public class PlayAudio : Event
{
    public AudioClip clip;
}

public class ResponseClicked : Event
{}

public class HideUI : Event
{}

public class ShowResponses : Event
{
    private List<UIResponseData> m_responses;

    public List<UIResponseData> GetResponses()
    {
        return m_responses;
    }

    public void Init(DialogueLine line, List<UnityAction> actions)
    {
        m_responses = new List<UIResponseData>(line.responses.Length);

        int i = 0;
        foreach (DialogueLine response in line.responses)
        {
            m_responses.Add(new UIResponseData
            {
                response = response.dialogue,
                buttonAction = actions[i],
                karmaScore = response.karmaScore
            });
            i++;
        }
    }
}

public class ShowDialogueEvent : Event
{
    private string dialogue;
    public Texture portrait;


    public string GetDialogue()
    {
        return dialogue;
    }



    public void Init(DialogueLine line)
    {
        dialogue = line.dialogue;
    }
}
