using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [Header("Data")]
    [Range(0f,1f)]
    [SerializeField] float musicVolume = 1f;
    [SerializeField] AudioMixer audioMixer;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup soundMixerGroup;
    [SerializeField] bool loopTracks, playOnAwake;   

    [Header("Internal Data")]
    [SerializeField] AudioSource audioSrc;

    #region Unity Methods
    void Awake() => SetUp();
    #endregion

    #region Internal Methods
    public void PlayTrack(AudioClip track)
    {
        audioSrc.Stop();
        audioSrc.clip = track;
        audioSrc.Play();
    }

    public void PlaySound(AudioClip clip, Vector3 soundLocation, float maxDistance, bool randomPitch, float volume, float pitchMin, float pitchMax, float spatialBlend, int priority, bool simulateSoundWave)
    {
        float pitch = randomPitch ? Random.Range(pitchMin, pitchMax) : 1;
        GameObject go = new GameObject("Sound");
        Sound sound = new Sound(clip, soundMixerGroup, soundLocation, maxDistance, pitch, volume, spatialBlend, priority);
        sound.PlayAt(go);
        Destroy(go, clip.length);
        if (simulateSoundWave) SimulateSoundWave(soundLocation, maxDistance, clip.length);
    }
    public void SimulateSoundWave(Vector3 location, float radius, float duration)
    {
        GameObject simGO = new GameObject("Sound Sim");
        simGO.transform.position = location;

        Debug.DrawRay(simGO.transform.position, -simGO.transform.forward * radius, Color.red);

        Collider[] listeners = Physics.OverlapSphere(location, radius);
        for (int i = 0; i < listeners.Length; i++)
        {
            if (listeners[i].TryGetComponent(out SoundListener listener))
            {
                listener.HearSound();
            }
        }

        Destroy(simGO, duration);
    }
    public void StopTrack() => audioSrc.Stop();
    public void ChangeMasterVolume(float volume) => audioMixer.SetFloat("MasterVolume", volume);
    public void ChangeMusicVolume(float volume) => audioMixer.SetFloat("MusicVolume", volume);
    public void ChangeAudioVolume(float volume) => audioMixer.SetFloat("AudioVolume", volume);
    void SetUp()
    {
        //Initialization SetUp
        GameObject[] objs = GameObject.FindGameObjectsWithTag("AudioManager");
        if (objs.Length > 1) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        //AudioSource SetUp
        audioSrc = GetComponent<AudioSource>();
        audioSrc.outputAudioMixerGroup = musicMixerGroup;
        audioSrc.volume = musicVolume;
        audioSrc.spatialBlend = 0;
        audioSrc.playOnAwake = playOnAwake;
        audioSrc.loop = loopTracks;
    }
    #endregion
}
