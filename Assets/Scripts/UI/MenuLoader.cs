using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoader : MonoBehaviour
{
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
}
