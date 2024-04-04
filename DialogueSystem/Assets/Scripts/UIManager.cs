using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Canvas uiCanvas = null;
    public TMPro.TextMeshProUGUI dialogueText = null;
    public GameObject buttonContainer = null;
    public RawImage portrait = null;

    class ButtonData
    {
        public GameObject buttonObject;
        public Button uiButton;
        public TMPro.TextMeshProUGUI buttonText;
        public Image image;
    };

    public GameObject buttonPrefab = null;
    private const int queueSize = 5;
    private Queue<ButtonData> buttonQueue = new Queue<ButtonData>();
    private List<ButtonData> activeButtons = new List<ButtonData>();

    private ButtonData CreateButtonData(GameObject prefab)
    {
        ButtonData buttonData = new ButtonData();
        buttonData.buttonObject = Instantiate(buttonPrefab, buttonContainer.transform);

        buttonData.buttonText = 
            buttonData.buttonObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        buttonData.uiButton = buttonData.buttonObject.GetComponent<Button>();
        buttonData.image = buttonData.buttonObject.GetComponentInChildren<Image>();

        buttonData.buttonObject.SetActive(false);

        return buttonData;

    }

    private ButtonData GetNextButton()
    {
        ButtonData buttonData = null;
        if (buttonQueue.TryDequeue(out buttonData))
        {
            return buttonData;
        }
        else
        {
            return CreateButtonData(buttonPrefab);
        }
    }

    private void RecycleButton(ButtonData buttonData)
    {
        buttonData.buttonObject.transform.SetParent(null, false);
        buttonData.buttonObject.SetActive(false);
        buttonData.uiButton.onClick.RemoveAllListeners();
        buttonQueue.Enqueue(buttonData);
    }

    // Start is called before the first frame update
    void Start()
    {
        EventDispatcher.AddListener<ShowDialogueEvent>(ShowUI);
        EventDispatcher.AddListener<ShowResponses>(ShowButtons);
        EventDispatcher.AddListener<ResponseClicked>(DisableButtons);
        EventDispatcher.AddListener<HideUI>(HideDialogue);

        for (int i=0; i < queueSize; i++)
        {
            ButtonData data = CreateButtonData(buttonPrefab);
            buttonQueue.Enqueue(data);
            
        }
    }

    private void HideDialogue(HideUI evt)
    {
        uiCanvas.gameObject.SetActive(false);
        DisableButtons(null);
    }


    private void ShowUI(ShowDialogueEvent evt)
    {
        uiCanvas.gameObject.SetActive(true);
        dialogueText.text = evt.GetDialogue();
        portrait.texture = evt.portrait;
    }

    private void ShowButtons(ShowResponses evt)
    {
        CreateResponseButtons(evt.GetResponses());
    }

    private void CreateResponseButtons(List<UIResponseData> data)
    {
        ButtonData button = null;

        foreach(UIResponseData response in data)
        {
            button = GetNextButton();
            button.buttonObject.SetActive(true);
            activeButtons.Add(button);

            button.buttonText.text = response.response;
            button.uiButton.onClick.AddListener(response.buttonAction);
            ColorBlock color = button.uiButton.colors;
            if (response.karmaScore < 0)
            {
                color.highlightedColor = new Color(1f,0f,0f,0.1f);
                color.normalColor = new Color(1f,0f,0f,0.5f);
            }
            else
            {
                color.highlightedColor = new Color(0f,1f,0f,0.1f);
                color.normalColor = new Color(0f,1f,0f,0.5f);
            }
            button.uiButton.colors = color;
        }
    }

    private void DisableButtons(ResponseClicked data)
    {
        foreach(ButtonData button in activeButtons)
        {
            RecycleButton(button);
        }

        activeButtons.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
