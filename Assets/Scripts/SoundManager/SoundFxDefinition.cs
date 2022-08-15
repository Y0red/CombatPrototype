using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SoundFxDefinition
{
    public SoundEffects Effects;
    public AudioClip clip;
}

[System.Serializable]
public enum SoundEffects
{
    HeroHit,
    LevelUp,
    MobDamage,
    MobDeath,
    NextWave
}
