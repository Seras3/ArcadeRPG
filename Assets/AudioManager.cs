using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    // Start is called before the first frame update
    void Awake()
    {


        if (!PlayerPrefs.HasKey("soundVolume"))
        {
            PlayerPrefs.SetFloat("soundVolume", (float)1.0);
        }


        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * PlayerPrefs.GetFloat("soundVolume");
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        Play("theme");
        //Play("running2");
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Nu exista " + name);
            return;
        }
        s.source.Play();
    }

    public void ChangeVolume()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * PlayerPrefs.GetFloat("soundVolume");
            //Debug.Log("volum " + PlayerPrefs.GetFloat("soundVolume"));
        }
    }

    public void Died()
    {
        Sound theme = Array.Find(sounds, sound => sound.name == "theme");
        theme.source.volume = theme.source.volume * 0.3f;
        //Play("fook");
        //Play("death");
        Play("gameover");
    }
}
