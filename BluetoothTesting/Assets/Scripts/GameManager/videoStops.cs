using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class videoStops : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    // Update is called once per frame
    void LateUpdate()
    {
        StartCoroutine(playVideo());
        
    }

    IEnumerator playVideo()
    {
        yield return new WaitForSeconds(0.5f);
        if (!videoPlayer.isPlaying)
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
