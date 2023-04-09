using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{

    public AudioClip backGroundMusic;
    public AudioSource currAudioSource;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        currAudioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if(backGroundMusic)currAudioSource.clip = backGroundMusic;
        currAudioSource.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
