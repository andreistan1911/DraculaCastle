using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    private PlayerClass player;
    private TextMeshProUGUI text;

    private bool open;
    private Transform startingTransform;

    public bool Open { get => open; set => open = value; }
    public Transform StartingTransform => startingTransform;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerClass>();
        text = GameObject.Find("Text Key").GetComponent<TextMeshProUGUI>();

        open = false;
        startingTransform = GetComponent<Transform>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (player.IsInteracting() && player.DecrementNrKeys())
        {
            text.SetText(PlayerPrefs.GetInt("Keys") + " x");

            open = true;
            transform.SetPositionAndRotation(
                new Vector3(transform.position.x - .6f, transform.position.y, transform.position.z + .64f),
                Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.z)
                );
        }
    }

    public void ArtificialOpen()
    {
        open = true;
        transform.SetPositionAndRotation(
                new Vector3(transform.position.x - .6f, transform.position.y, transform.position.z + .64f),
                Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.z)
                );
    }
}
