using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        EventDispatcher.AddListener<PlayAudio>(StartAudio);   
    }

    private void StartAudio(PlayAudio evtData)
    {
        if (source != null)
        {
            source.Stop();
            source.PlayOneShot(evtData.clip);
        }
    }
}
