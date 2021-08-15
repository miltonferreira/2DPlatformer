using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // singleton da classe
    public static AudioManager instance;
    
    // Sound FX
    public AudioClip sfx_jump, sfx_hurt, sfx_cherry, sfx_end, sfx_boss, sfx_hurt_boss, sfx_gate;
    // Music
    public AudioClip music_tiktok;
    // Current Music Object
    public GameObject currentMusicObj;
    // Sound Object
    public GameObject soundObject;

    void Awake() {
        instance = this;
    }

    public void PlaySFX(string sfxName){

        switch(sfxName){
            case "jump":
                SoundObjectCreate(sfx_jump);
                break;
            case "cherry":
                SoundObjectCreate(sfx_cherry);
                break;
            case "end":
                SoundObjectCreate(sfx_end);
                break;
            case "boss":
                SoundObjectCreate(sfx_boss);
                break;
            case "hurt":
                SoundObjectCreate(sfx_hurt);
                break;
            case "hurt_boss":
                SoundObjectCreate(sfx_hurt_boss);
                break;
            case "gate":
                SoundObjectCreate(sfx_gate);
                break;
            default:
                break;
        }
    }

    void SoundObjectCreate(AudioClip clip){
        // cria obj do sound
        GameObject newObj = Instantiate(soundObject, transform);
        // obj recebe o som para tocar
        newObj.GetComponent<AudioSource>().clip = clip;
        // toca o som
        newObj.GetComponent<AudioSource>().Play();

    }

    public void PlayMusic(string musicName){

        switch(musicName){
            case "tiktok":
                MusicObjectCreate(music_tiktok);
                break;
            case "bobo":
                MusicObjectCreate(sfx_hurt);
                break;
            default:
                break;
        }
    }

    void MusicObjectCreate(AudioClip clip){
        // checa se obj existe, se true destroy
        if(currentMusicObj){
            Destroy(currentMusicObj);
        }
        // cria obj do sound
        currentMusicObj = Instantiate(soundObject, transform);
        // obj recebe o som para tocar
        currentMusicObj.GetComponent<AudioSource>().clip = clip;
        // cria o recurso de som em loop
        currentMusicObj.GetComponent<AudioSource>().loop = true;
        // toca o som
        currentMusicObj.GetComponent<AudioSource>().Play();

    }


}
