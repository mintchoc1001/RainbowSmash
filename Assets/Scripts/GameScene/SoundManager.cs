using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource Buttonsource;

    public void OnSfx()
    {
        Buttonsource.Play();
    }
}
