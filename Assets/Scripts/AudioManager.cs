using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

    private void OnEnable()
    {
        Card.onFlip += PlayLetterSound;
        DraggableObject.onSelect += PlayLetterSound;
    }

    private void OnDisable()
    {
        Card.onFlip -= PlayLetterSound;
        DraggableObject.onSelect -= PlayLetterSound;
    }

    void PlayLetterSound(string letter)
    {
        Debug.Log("Playing sound for " + letter);
        PlaySound(letter.ToLower());
    }

    private void PlaySound(string sound)
    {
        if (oneShotAudioSource == null)
        {
            oneShotGameObject = new GameObject("One shot sound");
            oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
        }

        oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
    }

    private AudioClip GetAudioClip(string sound)
    {
        foreach (Sound soundRef in sounds)
        {
            if (soundRef.name == sound)
            {
                return soundRef.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found");
        return null;
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;
}
