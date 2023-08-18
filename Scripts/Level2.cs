using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : LevelClass
{
    new private void Awake()
    {
        base.Awake();

        levelNumber = 2;

        Checkpoints = new Vector3[]
        {
            StartingPosition,
            StartingPosition + new Vector3(10, 0, 0)
        };

        NrCheckpoints = Checkpoints.Length;

        
    }

    
}
