using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    



    //Singlton Pattern 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(instance);
        }

       
        

    }

    public AudioClip[] audioClips;
    public Dictionary<string, AudioClip> audioClipDictonary;

    private void Start()
    {
        audioClipDictonary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in audioClips)
        {
            
        }
    }

}
