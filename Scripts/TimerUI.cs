using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    private TextMeshProUGUI text;
    private GameComplete gameComplete;
    private GameObject gameMenu;

    private float timeElapsed;
    private bool visible;

    private void Start()
    {
        text = GameObject.Find("Text Timer").GetComponent<TextMeshProUGUI>();
        gameComplete = FindObjectOfType<GameComplete>();
        gameMenu = GameObject.Find("GameMenu");

        visible = true;
    }

    private void Update()
    {
        if (visible && !gameMenu.activeSelf)
        {
            GlobalValues.totalTImer += Time.deltaTime;

            int minutes = Mathf.FloorToInt(GlobalValues.totalTImer / 60f);
            int seconds = Mathf.FloorToInt(GlobalValues.totalTImer % 60f);
            int milliseconds = Mathf.FloorToInt((GlobalValues.totalTImer * 100f) % 100f);

            text.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
        }
        else
        {
            //StartCoroutine(Waiter());
        }
        
    }

    private IEnumerator Waiter()
    {
        Color textColor = text.color;

        textColor.a = 1.0f;
        text.color = textColor;

        yield return new WaitForSeconds(gameComplete.ScreenTime);

        textColor.a = 1.0f;
        text.color = textColor;

        visible = true;
    }

    public void Restart()
    {
        visible = false;
    }
}
