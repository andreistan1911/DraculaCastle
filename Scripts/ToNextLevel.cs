using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToNextLevel : MonoBehaviour
{
    private string[] scenePaths;

    public int nextLevel = 0;

    void Start()
    {
        scenePaths = new string[]
        {
            "dummy_string",     // 0
            "Scenes/Level_1",   // 1
            "Scenes/Level_2",   // 2
            "Scenes/Level_3",   // 3
            "Scenes/Level_4"    // 4
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(scenePaths[nextLevel]);
    }

}
