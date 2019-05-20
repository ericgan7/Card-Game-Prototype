using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource AS;
    public AudioClip walk;
    public AudioClip useCard;
    public AudioClip mousePassCard;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        AS = GetComponent<AudioSource>();
        walk = Resources.Load("walk",typeof (AudioClip)) as AudioClip;
        useCard = Resources.Load("CardUse", typeof(AudioClip)) as AudioClip;
        mousePassCard = Resources.Load("mousePass", typeof(AudioClip)) as AudioClip;
        ASbgm = transform.GetChild(0).GetComponent<AudioSource>();
        ASbgm.Play();
    }
    public void passCard() {
        AS.PlayOneShot(mousePassCard);
    }
    public void usecard()
    {
        AS.PlayOneShot(useCard);
    }
    AudioSource ASbgm;
    public void playWalk() {
        AS.clip = walk;
        AS.Play();
    }

    public void stopPlaying() {
        AS.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
