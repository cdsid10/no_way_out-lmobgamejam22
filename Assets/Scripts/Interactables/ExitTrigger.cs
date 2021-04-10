using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    [Header("Internal Data")]
    [SerializeField] EndingManager endingManager;

    public void Awake() => SetUp();

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && SceneManager.GetActiveScene().buildIndex != 6) StartCoroutine(LoadNextScene());
        if(SceneManager.GetActiveScene().buildIndex == 6)
        {
            endingManager.StartEnding();
        }
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void SetUp()
    {
        endingManager = GameObject.FindGameObjectWithTag("EndingManager").GetComponent<EndingManager>();
    }

}
