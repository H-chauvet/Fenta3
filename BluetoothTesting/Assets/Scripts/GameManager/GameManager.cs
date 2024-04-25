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
    public GameObject enemy;

    [SerializeField] private bool LevelIsLosable;
    [SerializeField]
    private float LoseDistance = 500f;
    
    
    
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

    private void Update()
    {
        
        if (LevelIsLosable)
        {
            float distance = Vector3.Distance(enemy.transform.position , player.transform.position);
            if (distance <= LoseDistance)
            {
                GameOver();
            }
        }
        
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("GameOver");
    }

    public void WinGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        LoadLevel(3);
    }

    private void OnDrawGizmos()
    {
        if (!LevelIsLosable) return;
        Gizmos.color = Color.red;
        Vector3 direction = player.transform.position - enemy.transform.position;
        direction = direction.normalized * Mathf.Min(LoseDistance, direction.magnitude);
        Gizmos.DrawLine(enemy.transform.position, enemy.transform.position + direction);
    }
}
