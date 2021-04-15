using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationInstance : MonoBehaviour
{
    [SerializeField] ParticleSystem showerParticles;
    [SerializeField] GameObject soundsParent;

    public ParticleSystem GetParticles()
    {
        return showerParticles;
    }

    public void StopSounds()
    {
        soundsParent.SetActive(false);
    }
}
