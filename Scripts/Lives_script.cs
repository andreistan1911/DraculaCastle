using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lives_script : MonoBehaviour
{
    private DemonBossController boss;

    RawImage image;
    public int nthHeart;
    public Texture2D fullHeart;
    public Texture2D emptyHeart;

    GameObject b;

    void Start()
    {
        image = GetComponent<RawImage>();
        b = GameObject.Find("Boss");
        if (b == null)
            return;
        boss = b.GetComponent<DemonBossController>();
    }

    void Update()
    {
        if (b == null)
            return;

        if ((boss.CrHP.x >= nthHeart && image.color.r > .95) ||
            (boss.CrHP.y >= nthHeart && image.color.g > .95) ||
            (boss.CrHP.z >= nthHeart && image.color.b > .95)
            )
            image.texture = fullHeart;
        else
            image.texture = emptyHeart;
    }
}
