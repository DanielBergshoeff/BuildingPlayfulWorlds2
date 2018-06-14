using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager {
    private static int points;

    private static int healthShrines;

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
            CheckPoints();
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

    public static int HealthShrines
    {
        get
        {
            return healthShrines;
        }
        set
        {
            healthShrines = value;
            if(healthShrines <= 0)
            {
                RestartLevel();
            }
        }
    }

    public static void NextLevel()
    {
        CurrentLevel++;
        SceneManager.LoadScene(currentLevel);
    }    

    public static void CheckPoints()
    {
        if(points > 0 && currentLevel == 0)
        {
            NextLevel();
        }
        else if(points > 1 && currentLevel == 1)
        {
            NextLevel();
        }
        else if(points > 4 && currentLevel == 2)
        {
            NextLevel();
        }
    }

    public static void RestartLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }
}
