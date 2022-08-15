using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Manager<SoundManager>
{
    public List<SoundFxDefinition> SoundFx;
    public AudioSource SoundFxSource;

    public void PlaySoundEffect(SoundEffects soundEffects)
    {
        AudioClip effect = SoundFx.Find(sfx => sfx.Effects == soundEffects).clip;

        SoundFxSource.PlayOneShot(effect);
    }
   
}
