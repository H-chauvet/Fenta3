using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public LevelData lastLevel;
    
    public GameObject player;
    public GameObject light;
    public GameObject[] enemyArray;
    
    
    
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        //GameManager should only be present in playable levels
        //On initialization, set last played level to the current level, which would be the last level played
        lastLevel.levelIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    
    
    public void GameOver()
    {
        //TODO: Implement GameOver logic
        SceneManager.LoadScene("GameOver");
    }

    public void WinGame()
    {
        //TODO: Implement WinGame logic
    }

    //If level manager is only present on playable levels, this logic would only work for popup UI on player death
    //Might have to do this in a different, better way. For now, it exists
    public void ReloadLastLevel()
    {
        SceneManager.LoadScene(lastLevel.levelIndex);
    }
    
    
}
