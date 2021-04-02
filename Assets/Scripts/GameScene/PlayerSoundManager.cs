using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public static PlayerSoundManager instance;

    // 싱글톤 생성
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(go, clip.length);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
