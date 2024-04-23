using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class videoStops : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private int LevelToLoad;

    // Update is called once per frame
    void LateUpdate()
    {
        StartCoroutine(playVideo());
        
    }

    IEnumerator playVideo()
    {
        yield return new WaitForSeconds(3f);
        if (!videoPlayer.isPlaying)
        {
            
                GameManager.Instance.LoadLevel(LevelToLoad);
            
        }
    }
}
