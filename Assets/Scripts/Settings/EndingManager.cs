using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [Header("Internal Data")]
    [SerializeField] AudioManager audioManager;
    [SerializeField] GlitchEffect glitchScript;
    [SerializeField] float curDuration, restartDuration = 1f;
    [SerializeField] int timesReset;

    public void Awake() => SetUp();

    public void StartEnding()
    {
        Ending();
    }

    void Ending()
    {
        if(timesReset <= 10)
        {
            StartCoroutine(RestartLevel());
            Invoke(nameof(Ending), Random.Range(2f, 10f) - restartDuration);
        }
        else
        {
            SceneManager.LoadScene(7);
        }

    }

    public IEnumerator RestartLevel()
    {
        glitchScript = FindObjectOfType<GlitchEffect>();
        audioManager.PlaySound(Resources.Load<AudioClip>("Audio/Sounds/Glitch01"), transform.position, 100f, true, 1, 0.8f, 1.2f, 0, 128, false);
        while (curDuration < 1f)
        {
            curDuration += Time.deltaTime;
            glitchScript.intensity = 1;
            glitchScript.flipIntensity = 1;
            glitchScript.colorIntensity = 1;
            yield return null;
        }
        glitchScript.intensity = 0;
        glitchScript.flipIntensity = 0;
        glitchScript.colorIntensity = 0;
        restartDuration += 0.5f;
        timesReset++;
        SceneManager.LoadScene(Random.Range(1,6));
    }

    void SetUp()
    {
        //Initialization SetUp
        GameObject[] objs = GameObject.FindGameObjectsWithTag("EndingManager");
        if (objs.Length > 1) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        //Data Setup
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        glitchScript = FindObjectOfType<GlitchEffect>();

    }
}
