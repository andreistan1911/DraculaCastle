using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{
    private PlayerClass player;
    private TextMeshProUGUI text;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerClass>();
        text = GameObject.Find("Text Coin").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        player.IncrementNrCoins();
        text.SetText(PlayerPrefs.GetInt("Coins") + " x");

        gameObject.SetActive(false);
    }
}
