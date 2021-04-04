using UnityEngine;
using UnityEngine.Audio;

public struct Sound
{
    //Values
    public AudioClip sound;
    public AudioMixerGroup mixerGroup;
    public Vector3 location;
    public float distance, pitch, volume, spatialBlend;
    public int priority;

    public Sound(AudioClip sound, AudioMixerGroup mixerGroup, Vector3 location, float distance, float pitch, float volume, float spatialBlend, int priority)
    {
        this.sound = sound;
        this.mixerGroup = mixerGroup;
        this.location = location;
        this.distance = distance;
        this.pitch = pitch;
        this.volume = volume;
        this.spatialBlend = spatialBlend;
        this.priority = priority;
    }

    public void PlayAt(GameObject gameObject)
    {
        UnityEngine.Profiling.Profiler.BeginSample(this+"Play", gameObject);
        AudioSource soundAS = gameObject.AddComponent<AudioSource>();
        gameObject.transform.position = location;

        soundAS.outputAudioMixerGroup = mixerGroup;
        soundAS.clip = sound;
        soundAS.minDistance = 1;
        soundAS.maxDistance = distance;
        soundAS.pitch = pitch;
        soundAS.volume = volume;
        soundAS.spatialBlend = spatialBlend;
        soundAS.priority = priority;

        soundAS.PlayOneShot(sound);

        UnityEngine.Profiling.Profiler.EndSample();
    }

}
