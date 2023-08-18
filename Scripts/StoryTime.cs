using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryTime : MonoBehaviour
{
    public enum StoryState
    {
        Story1,
        Transition1to2,
        Story2,
        Transition2to3,
        Story3,
        TransitionToBlack,
    }

    public CanvasGroup story1, story2, story3, blackScreen;
    public float stay1, stay2, stay3;
    public float fade12, fade23, fade3Black;

    private float timer;
    private StoryState currentState;

    void Start()
    {
        currentState = StoryState.Story1;
        story1.alpha = 1;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (currentState)
            {
                case StoryState.Story1:
                    story1.alpha = 0;
                    story2.alpha = 1;
                    timer = 0;
                    currentState = StoryState.Transition1to2;
                    break;
                case StoryState.Story2:
                    story2.alpha = 0;
                    story3.alpha = 1;
                    timer = 0;
                    currentState = StoryState.Transition2to3;
                    break;
                case StoryState.Story3:
                case StoryState.TransitionToBlack:
                    SceneManager.LoadScene("Scenes/Level_1");
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Scenes/Level_1");
        }

        switch (currentState)
        {
            case StoryState.Story1:
                if (timer >= stay1)
                {
                    timer -= stay1;
                    currentState = StoryState.Transition1to2;
                }
                break;
            case StoryState.Transition1to2:
                story1.alpha = 1 - (timer / fade12);
                story2.alpha = timer / fade12;
                if (timer >= fade12)
                {
                    timer -= fade12;
                    currentState = StoryState.Story2;
                }
                break;
            case StoryState.Story2:
                if (timer >= stay2)
                {
                    timer -= stay2;
                    currentState = StoryState.Transition2to3;
                }
                break;
            case StoryState.Transition2to3:
                story2.alpha = 1 - (timer / fade23);
                story3.alpha = timer / fade23;
                if (timer >= fade23)
                {
                    timer -= fade23;
                    currentState = StoryState.Story3;
                }
                break;
            case StoryState.Story3:
                if (timer >= stay3)
                {
                    timer -= stay3;
                    currentState = StoryState.TransitionToBlack;
                }
                break;
            case StoryState.TransitionToBlack:
                story3.alpha = 1 - (timer / fade3Black);
                blackScreen.alpha = timer / fade3Black;
                if (timer >= fade3Black)
                {
                    SceneManager.LoadScene("Scenes/Level_1");
                }
                break;
        }
    }
}
