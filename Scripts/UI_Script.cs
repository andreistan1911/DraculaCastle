using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Script : MonoBehaviour
{
    private GameComplete gameComplete;
    private GameObject gameMenu;
    private LevelClass level;
    private Coin[] coins;
    private Key[] keys;

    private TextMeshProUGUI textKey;
    private TextMeshProUGUI textCoin;

    void Start()
    {
        GameObject gc = GameObject.Find("gamecomplete");
        gameMenu = GameObject.Find("GameMenu");
        gameMenu.SetActive(false);

        level = FindObjectOfType<LevelClass>();
        coins = Resources.FindObjectsOfTypeAll<Coin>();
        keys = Resources.FindObjectsOfTypeAll<Key>();

        textKey = GameObject.Find("Text Key").GetComponent<TextMeshProUGUI>();
        textCoin = GameObject.Find("Text Coin").GetComponent<TextMeshProUGUI>();
        textKey.SetText(PlayerPrefs.GetInt("Keys") + " x");
        textCoin.SetText(PlayerPrefs.GetInt("Coins") + " x");

        if (gc != null)
            gameComplete = gc.GetComponent<GameComplete>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            gameMenu.SetActive(!gameMenu.activeSelf);
    }

    public void Resume()
    {
        gameMenu.SetActive(false);
    }

    public void SaveExit()
    {
        Door[] doors = FindObjectsOfType<Door>();

        GlobalValues.SaveGame(level.levelNumber, level.CrCheckpoint, SaveDoorsData(), SaveCoinsData(), SaveKeysData(), GlobalValues.totalTImer);

        GlobalValues.LogAllPrefs();

        // Exit the application
        Application.Quit();

        // If you are in the Unity editor, stop playing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private int SaveDoorsData()
    {
        Door[] doors = FindObjectsOfType<Door>();
        int doorsData = PlayerPrefs.GetInt("DoorsData");

        // There should be less than 32 doors in a level
        for (int i = 0; i < doors.Length; i++)
            doorsData |= (doors[i].Open ? 1 : 0) << i;

        return doorsData;
    }

    private int SaveCoinsData()
    {
        int coinsData = 0;

        // There should be less than 32 coins in a level
        for (int i = 0; i < coins.Length; i++)
            coinsData |= (coins[i].gameObject.activeSelf ? 0 : 1) << i;

        return coinsData;
    }

    private int SaveKeysData()
    {
        int keysData = 0;

        // There should be less than 32 keys in a level
        for (int i = 0; i < keys.Length; i++)
        {
            keysData |= (keys[i].gameObject.activeSelf ? 0 : 1) << i;
        }

        return keysData;
    }

    public void Restart()
    {
        StartCoroutine(Hide_UI());
        
    }

    public IEnumerator Hide_UI()
    {
        
        foreach (Transform child in transform)
        {
            if (child != gameComplete)
            {
                child.gameObject.SetActive(false);
            }
        }

        yield return new WaitForSeconds(3.0f);

        foreach (Transform child in transform)
        {
            if (child.gameObject != gameComplete)
            {
                child.gameObject.SetActive(true);
            }
        }

        gameObject.SetActive(true);
    }
}
