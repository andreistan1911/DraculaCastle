using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Key : MonoBehaviour
{
    private PlayerClass player;
    private TextMeshProUGUI text;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerClass>();
        text = GameObject.Find("Text Key").GetComponent<TextMeshProUGUI>();
        text.SetText(PlayerPrefs.GetInt("Keys") + " x");
    }

    private void OnTriggerEnter(Collider other)
    {
        player.IncrementNrKeys();
        text.SetText(PlayerPrefs.GetInt("Keys") + " x");

        gameObject.SetActive(false);
    }
}
