using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager {
    private static int points;

    private static int currentLevel;

    private static Dictionary<int, int> scenes;

    public static int Points
    {
        get
        {
            return points;
        }
        set
        {
            points = value;
        }
    }

    public static int CurrentLevel
    {
        get
        {
            return currentLevel;
        }
        set
        {
            currentLevel = value;
        }
    }

    public static Dictionary<int, int> Scenes
    {
        get
        {
            return scenes;
        }
        set
        {
            scenes = value;
        }
    }
}
