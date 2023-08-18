using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static readonly string path = Application.persistentDataPath + "/save.txt";

    public static void SaveCheckpoints(LevelClass level)
    {
        if (level != null)
        {
            string content = $"level={level.levelNumber}\ncheckpoint={level.CrCheckpoint}";
            File.WriteAllText(path, content);
        }
    }

    public static bool LoadCheckpoints()
    {
        CheckpointData.LevelNumber = 0;
        CheckpointData.Checkpoint = 0;

        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                var splitLine = line.Split('=');
                if (splitLine.Length == 2)
                {
                    if (splitLine[0] == "level")
                    {
                        CheckpointData.LevelNumber = int.Parse(splitLine[1]);
                    }
                    else if (splitLine[0] == "checkpoint")
                    {
                        CheckpointData.Checkpoint = int.Parse(splitLine[1]);
                    }
                }
            }
            return true;
        }
        return false;
    }

    public static bool HasSavedCheckpoints()
    {
        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                var splitLine = line.Split('=');
                if (splitLine.Length == 2 && splitLine[0] == "level")
                {
                    // Return true if the level is not 0
                    return int.Parse(splitLine[1]) != 0;
                }
            }
        }

        // Return false if the file doesn't exist or the level is 0
        return false;
    }

}
