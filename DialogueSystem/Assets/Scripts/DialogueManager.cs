using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : Singleton<DialogueManager>
{

    public DialogueDatabase database;

    private Dictionary<string, DialogueLine> m_DialogueTable =
    new Dictionary<string, DialogueLine>();

    private const float kDefaultWaitTime = 1.0f;

    private DialogueLine m_currentLine = null;
    private float waitTime = 0.0f;
    private float currentTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        foreach(DialogueLine line in database.database )
        {
            m_DialogueTable.Add(line.GUID, line);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartDialogue("Hi");
        }

        if (m_currentLine != null && waitTime > 0.0f)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= waitTime)
            {
                currentTime = 0.0f;
                waitTime = 0.0f;

                if(m_currentLine.responses.Length == 0)
                {
                    m_currentLine = null;
                    EventDispatcher.Raise<HideUI>(new HideUI());
                }
                else
                {

                

                    //Tell UI to show the responses.
                    List<UnityAction> actions = new List<UnityAction>();
                    for(int i = 0; i < m_currentLine.responses.Length; i++)
                    {
                        int currentIndex = i;
                        actions.Add(() => { this.PlayResponse(currentIndex); });
                    }

                    ShowResponses data = new ShowResponses();
                    data.Init(m_currentLine, actions);
                    EventDispatcher.Raise<ShowResponses>(data);
                }
            }
        }
    }

    public void PlayResponse(int i)
    {
        if (m_currentLine != null)
        {
            EventDispatcher.Raise<ResponseClicked>(new ResponseClicked());

            if (i >= 0 && i < m_currentLine.responses.Length)
            {
                DialogueLine responseLine = m_currentLine.responses[i];
                StartDialogue(responseLine);
            }
        }
    }

    public void StartDialogue(string name)
    {
        StartDialogue(m_DialogueTable[name]);
    }

    public void StartDialogue(DialogueLine line)
    {
        m_currentLine = line;

        //Trigger the dialogue on the screen....
        ShowDialogueEvent data = new ShowDialogueEvent();
        data.Init(m_currentLine);
        data.portrait = m_currentLine.speaker.image;

        EventDispatcher.Raise<ShowDialogueEvent>(data);

        waitTime = kDefaultWaitTime;

        if (m_currentLine.audio != null)
        {
            waitTime = m_currentLine.audio.length;
            PlayAudio audioEvent = new PlayAudio { clip = m_currentLine.audio };
            EventDispatcher.Raise<PlayAudio>(audioEvent);
        }

    }


}
