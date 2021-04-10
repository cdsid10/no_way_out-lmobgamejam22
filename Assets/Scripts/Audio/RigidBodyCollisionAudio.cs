using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class RigidBodyCollisionAudio : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] bool useDefaultSound = true;
    [SerializeField] [Range(.1f, 2f)] float defaultMinPitch = 0.8f, defaultMaxPitch = 1.2f;
    [SerializeField] [Range(0, 300)] float maxMagnitudeExpected = 50;
    [SerializeField] [Range(0, 256)] int soundPriority = 128;
    [SerializeField] bool simulateSoundWave = true;
    [SerializeField] List<string> physicsTags;
    [SerializeField] List<AudioClip> hitSounds;
    [SerializeField] List<Vector2> pitchRanges;

    [Header("Internal Data")]
    [SerializeField] Rigidbody rb;
    [SerializeField] AudioClip defaultSound;
    [SerializeField] AudioManager audioManager;

    #region Unity Methods
    void Awake() => SetUp();

    public void OnCollisionEnter(Collision collision)
    {
        UnityEngine.Profiling.Profiler.BeginSample(name + ".OnCollisionEnter",this);
        collision.gameObject.TryGetComponent(out MultiTag mt);      
        string physicsTag = mt == null ? "NoMultiTagComponent" : mt.physicsTag;      
        float magnitude = Mathf.Clamp(collision.relativeVelocity.sqrMagnitude, 0.01f, maxMagnitudeExpected);
        float volume = Mathf.Clamp(magnitude / maxMagnitudeExpected, 0.01f, 1f);
        float distance = rb.mass * 10;
        GameObject surface = collision.gameObject;
        GameObject go = new GameObject("Sound");
        Vector3 collisionPoint = collision.GetContact(collision.contactCount - 1).point;

        if (magnitude >= maxMagnitudeExpected / 10)
        {
            if (useDefaultSound)
            {
                Sound sound = new Sound(defaultSound, audioManager.soundMixerGroup, collisionPoint, distance, Random.Range(defaultMinPitch, defaultMaxPitch), volume, 1, soundPriority);
                sound.PlayAt(go);
                Destroy(go, defaultSound.length);
                if (simulateSoundWave) audioManager.SimulateSoundWave(collisionPoint, distance, defaultSound.length);
                return;
            }
            else
            {
                if (physicsTag == "NoMultiTagComponent")
                {
                    Debug.LogWarning("Contact Object doesn't have a 'MultiTag' component, using Default values.", surface);
                    Sound sound = new Sound(defaultSound, audioManager.soundMixerGroup, collisionPoint, distance, Random.Range(defaultMinPitch, defaultMaxPitch), volume, 1, soundPriority);
                    sound.PlayAt(go);
                    Destroy(go, defaultSound.length);
                    if (simulateSoundWave) audioManager.SimulateSoundWave(collisionPoint, distance, defaultSound.length);
                    return;
                }
                else if (physicsTag == "")
                {
                    Debug.LogWarning("Physics Tag at " + surface.name + " is empty, using Default values.", surface);
                    Sound sound = new Sound(defaultSound, audioManager.soundMixerGroup, collisionPoint, distance, Random.Range(defaultMinPitch, defaultMaxPitch), volume, 1, soundPriority);
                    sound.PlayAt(go);
                    Destroy(go, defaultSound.length);
                    if (simulateSoundWave) audioManager.SimulateSoundWave(collisionPoint, distance, defaultSound.length);
                    return;
                }
                else if (!physicsTags.Contains(physicsTag))
                {
                    Debug.LogWarning("Element '" + physicsTag + "' from " + surface.name + " at 'Physics Tags' list does not exist. Using Default values.", this);
                    Sound sound = new Sound(defaultSound, audioManager.soundMixerGroup, collisionPoint, distance, Random.Range(defaultMinPitch, defaultMaxPitch), volume, 1, soundPriority);
                    sound.PlayAt(go);
                    Destroy(go, defaultSound.length);
                    if (simulateSoundWave) audioManager.SimulateSoundWave(collisionPoint, distance, defaultSound.length);
                    return;
                }
                else
                {
                    int i = physicsTags.IndexOf(physicsTag);

                    if (hitSounds[i] == null)
                    {
                        Debug.LogWarning("Element " + i + " at 'hitSounds' list does not have a sound assigned. Using Default values.", this);
                        Sound sound = new Sound(defaultSound, audioManager.soundMixerGroup, collisionPoint, distance, Random.Range(defaultMinPitch, defaultMaxPitch), volume, 1, soundPriority);
                        sound.PlayAt(go);
                        Destroy(go, defaultSound.length);
                        if (simulateSoundWave) audioManager.SimulateSoundWave(collisionPoint, distance, defaultSound.length);
                        return;
                    }

                    float pitchMin;
                    float pitchMax;

                    if (pitchRanges[i].x == 0 || pitchRanges[i].y == 0)
                    {
                        pitchMin = defaultMinPitch; pitchMax = defaultMaxPitch;
                        Debug.LogWarning("One of the values at Element " + i + " at 'Pitch Ranges' is 0. Pitch values set to defaults (" + defaultMinPitch + ", " + defaultMaxPitch + ").", this);
                        Sound sound = new Sound(hitSounds[i], audioManager.soundMixerGroup, collisionPoint, distance, Random.Range(pitchMin, pitchMax), volume, 1, soundPriority);
                        sound.PlayAt(go);
                        Destroy(go, hitSounds[i].length);
                        if (simulateSoundWave) audioManager.SimulateSoundWave(collisionPoint, distance, hitSounds[i].length);
                        return;
                    }
                    else
                    {
                        pitchMin = pitchRanges[i].x; pitchMax = pitchRanges[i].y;
                        Sound sound = new Sound(hitSounds[i], audioManager.soundMixerGroup, collisionPoint, distance, Random.Range(pitchMin, pitchMax), volume, 1, soundPriority);
                        sound.PlayAt(go);
                        Destroy(go, hitSounds[i].length);
                        if (simulateSoundWave) audioManager.SimulateSoundWave(collisionPoint, distance, hitSounds[i].length);
                        return;
                    }
                }
            }
        }

        Destroy(go);
        UnityEngine.Profiling.Profiler.EndSample();
    }

    #endregion

    #region Internal Methods

    void SetUp()
    {
        // Internal Data Setup
        rb = GetComponent<Rigidbody>();
        audioManager = FindObjectOfType<AudioManager>();
        defaultSound = (AudioClip)Resources.Load("Audio/Sounds/DefaultHitSound");
        int hitSoundsSize = hitSounds.Count;
        int physicsTagsSize = physicsTags.Count;
        int pitchRangesSize = pitchRanges.Count;

        //Failsafes
        if (useDefaultSound && defaultSound == null)
        {
            Debug.LogError("'Use Default Sound' in '" + name + "' is true, but 'Audio/Sounds/DefaultHitSound' does not exist. Returning to Editor.", this);
            //UnityEditor.EditorApplication.isPlaying = false;
        }
        if (hitSoundsSize != physicsTagsSize || hitSoundsSize != pitchRangesSize || physicsTagsSize != pitchRangesSize)
        {
            Debug.LogError("Lists element count are inconsistent, (Hit Sounds: " + hitSoundsSize + ", Physics Tags: " + physicsTagsSize + ", Pitch Ranges: " + pitchRangesSize + "). Returning to Editor.", this);
            //UnityEditor.EditorApplication.isPlaying = false;
        }
    }
    #endregion
}
