using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValues : MonoBehaviour
{
    public static KeyCode interactKey = KeyCode.O;
    public static KeyCode godModeKey1 = KeyCode.Y;
    public static KeyCode godModeKey2 = KeyCode.H;
    public static KeyCode godModeKey3 = KeyCode.N;
    public static int nrHitFlashes = 5;

    public static int EnemiesAwaken = 0;

    public static bool dragonAvailable = false;
    public static bool eyeShouldChase = false;

    public static Vector2 standardGravity = Physics.gravity;

    public static bool continueLoading = false;
    public static bool restoredDoors = false;

    public static float totalTImer = 0f;

    public static void SaveGame(int levelNumber, int checkpoint, int nrCoins, int nrKeys, int doorsData, int coinsData, int keysData, float totalTimer)
    {
        PlayerPrefs.SetInt("Level", levelNumber);
        PlayerPrefs.SetInt("Checkpoint", checkpoint);
        PlayerPrefs.SetInt("Coins", nrCoins);
        PlayerPrefs.SetInt("Keys", nrKeys);
        PlayerPrefs.SetInt("DoorsData", doorsData);
        PlayerPrefs.SetInt("CoinsData", coinsData);
        PlayerPrefs.SetInt("KeysData", keysData);
        PlayerPrefs.SetFloat("Timer", totalTimer);
    }

    public static void SaveGame(int levelNumber, int checkpoint, int doorsData, int coinsData, int keysData, float totalTimer)
    {
        PlayerPrefs.SetInt("Level", levelNumber);
        PlayerPrefs.SetInt("Checkpoint", checkpoint);
        PlayerPrefs.SetInt("DoorsData", doorsData);
        PlayerPrefs.SetInt("CoinsData", coinsData);
        PlayerPrefs.SetInt("KeysData", keysData);
        PlayerPrefs.SetFloat("Timer", totalTimer);
    }

    public static bool CheckIfThereIsAnyProgress()
    {
        return PlayerPrefs.GetInt("Level") > 1 && PlayerPrefs.GetInt("Checkpoint") > 0;
    }

    public static void LogAllPrefs()
    {
        Debug.Log("Level: " + PlayerPrefs.GetInt("Level")
                    + " | Checkpoint:" + PlayerPrefs.GetInt("Checkpoint")
                    + " | Coins: " + PlayerPrefs.GetInt("Coins")
                    + " | Keys: " + PlayerPrefs.GetInt("Keys")
                    + " | DoorsData: " + PlayerPrefs.GetInt("DoorsData")
                    + " | CoinsData: " + PlayerPrefs.GetInt("CoinsData")
                    + " | KeysData: " + PlayerPrefs.GetInt("KeysData"));
    }
}
