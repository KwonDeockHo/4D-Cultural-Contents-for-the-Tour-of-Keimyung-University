using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Scenemanager : MonoBehaviour {
    AsyncOperation async_operation;
    public GameObject frame;
    VideoPlayer video;
    void Start()
    {
        async_operation = SceneManager.LoadSceneAsync("tmp_2");
        async_operation.allowSceneActivation = false;
        video = frame.GetComponent<VideoPlayer>();
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(video.time);
        if (video.time >= 58.5f)
            async_operation.allowSceneActivation = true;
    }
}
