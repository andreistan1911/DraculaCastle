using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelClass : MonoBehaviour
{
    public int levelNumber;
    public static bool isLoadingComplete = false;

    private GameObject checkpointParent;
    private PlayerClass playerScript;

    private int nrCheckpoints;
    private int crCheckpoint;

    private Vector3 startingPosition;
    private Vector3[] checkpoints;

    private int enemiesCount;

    public int NrCheckpoints { get => nrCheckpoints; set => nrCheckpoints = value; }
    public int CrCheckpoint { get => crCheckpoint; set => crCheckpoint = value; }
    public Vector3 StartingPosition { get => startingPosition; set => startingPosition = value; }
    public Vector3[] Checkpoints { get => checkpoints; set => checkpoints = value; }

    public void Awake()
    {
        PlayerPrefs.SetInt("Level", levelNumber);

        if (GlobalValues.continueLoading)
        {
            crCheckpoint = PlayerPrefs.GetInt("Checkpoint");
        }
        else
        {
            PlayerPrefs.SetInt("DoorsData", 0);
            PlayerPrefs.SetInt("CoinsData", 0);
            PlayerPrefs.SetInt("KeysData", 0);
            PlayerPrefs.SetInt("Keys", 0);
            PlayerPrefs.SetInt("Checkpoint", 0);
            crCheckpoint = 0;
        }

        GlobalValues.LogAllPrefs();

        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerClass>();
        Physics.gravity = GlobalValues.standardGravity;
        StartingPosition = new Vector3(0, 0.1f, 0);
        enemiesCount = CountEnemies();

        GlobalValues.continueLoading = false;
        GlobalValues.restoredDoors = false;

        StartCoroutine(AwakeEnemies());
        RestoreDoors();
        DestroyInactiveCoins();
        DestroyInactiveKeys();
        SetCheckpoints();
        MovePlayerToCurrentCheckpoint();
    }

    private void MovePlayerToCurrentCheckpoint()
    {
        playerScript.transform.position = GetCrCheckpointPosition();
    }

    private void SetCheckpoints()
    {
        checkpointParent = GameObject.Find("Checkpoints");

        if (checkpointParent != null)
        {
            int numCheckpoints = checkpointParent.transform.childCount;
            checkpoints = new Vector3[numCheckpoints];

            for (int i = 0; i < numCheckpoints; i++)
            {
                Vector3 childPosition = checkpointParent.transform.GetChild(i).position;
                checkpoints[i] = new Vector3(childPosition.x, childPosition.y, 0);
            }
        }
        else
        {
            Debug.LogWarning("Checkpoints GameObject not found in the scene!");
        }

        NrCheckpoints = checkpoints.Length;
    }

    private IEnumerator AwakeEnemies()
    {
        while (GlobalValues.EnemiesAwaken < enemiesCount)
            yield return null;

        isLoadingComplete = true;
    }

    private void RestoreDoors()
    {
        int doorsData = PlayerPrefs.GetInt("DoorsData");
        Door[] doors = FindObjectsOfType<Door>();

        for (int i = 0; i < doors.Length; i++)
            if ((doorsData & (1 << i)) != 0)
            {
                doors[i].ArtificialOpen();
            }

        
        GlobalValues.restoredDoors = true;
    }

    private void DestroyInactiveCoins()
    {
        int coinsData = PlayerPrefs.GetInt("CoinsData");
        Coin[] coins = FindObjectsOfType<Coin>();

        for (int i = 0; i < coins.Length; i++)
            if ((coinsData & (1 << i)) != 0)
                coins[i].gameObject.SetActive(false);
    }

    private void DestroyInactiveKeys()
    {
        int keysData = PlayerPrefs.GetInt("KeysData");
        Key[] keys = FindObjectsOfType<Key>();

        for (int i = 0; i < keys.Length; i++)
            if ((keysData & (1 << i)) != 0)
                keys[i].gameObject.SetActive(false);
    }

    public void CheckToIncreaseCrCheckpoint()
    {
        while (crCheckpoint < nrCheckpoints - 1 && playerScript.Rb.position.x >= Checkpoints[crCheckpoint + 1].x)
        {
            ++crCheckpoint;
            PlayerPrefs.SetInt("Checkpoint", crCheckpoint);
        }
    }

    public Vector3 GetCrCheckpointPosition()
    {
        return checkpoints[crCheckpoint];
    }

    public void RestartLevel()
    {
        crCheckpoint = 0;
        RestartAtCheckpoint();
    }

    public void RestartAtCheckpoint()
    {
        playerScript.GoToCheckpoint();

        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        Hazard[] hazards = GameObject.FindObjectsOfType<Hazard>();

        foreach (Enemy enemy in enemies)
            enemy.RestartPosition();

        foreach (Hazard hazard in hazards)
            hazard.Delete();
    }

    private int CountEnemies()
    {
        return FindObjectsOfType<Enemy>().Length;
    }
}
