using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class VideoController : MonoBehaviour
{
    MovieTexture MOVIE_TEXTURE;
    AudioSource movieAudio;

    public GameObject circle_tr;
    public GameObject canvas_ui;
    public GameObject fade;

    public float videoTime;
    // Use this for initialization
    public static VideoController instance = null;
    private float time = 0.0f;
    
    public static VideoController Instance
    {
        get
        {
            if (instance == null)
            {//= new CGame();
                instance = FindObjectOfType(typeof(VideoController)) as VideoController;
            }
            return instance;
        }
    }


    void Start()
    {
        MOVIE_TEXTURE = ((MovieTexture)GetComponent<Renderer>().material.mainTexture);
        movieAudio = GetComponent<AudioSource>();
        time = 0.0f;
        videoTime = MotionController.Instance.time;

        circle_tr = GameObject.FindGameObjectWithTag("CameraUi");
        canvas_ui = GameObject.FindGameObjectWithTag("Menu");
        fade = GameObject.FindGameObjectWithTag("fade");
        
        circle_tr.SetActive(false);
        canvas_ui.SetActive(false);
        fade.SetActive(false);

        VRStandardAssets.Utils.Reticle.Instance.Hide();

        movieAudio.clip = MOVIE_TEXTURE.audioClip;

        MOVIE_TEXTURE.Play();
        movieAudio.Play();
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        //Debug.Log("Time = " + time);
        videoTime = MotionController.Instance.time;
        if (86 <= videoTime && videoTime < 86.5 && MotionController.Instance.state == true)
        {
            fade.SetActive(true);

            Invoke("show", 1.2f);

        }
        else if (VRStandardAssets.Utils.VREyeRaycaster.Instance.hit_object == 3 && MotionController.Instance.state == false)
        {
            Fadeinout.Instance.fade_state = false;

            Invoke("Hide", 1.2f);
        }

    }
    void show()
    {
        MOVIE_TEXTURE.Pause();
        movieAudio.Pause();

        VideoController.Instance.circle_tr.SetActive(true);
        VideoController.Instance.canvas_ui.SetActive(true);
        VRStandardAssets.Utils.Reticle.Instance.Show();

        MotionController.Instance.state = false;
    }
    void Hide()
    {
        MOVIE_TEXTURE.Play();
        movieAudio.Play();

        VideoController.Instance.circle_tr.SetActive(false);
        VideoController.Instance.canvas_ui.SetActive(false);
        VRStandardAssets.Utils.Reticle.Instance.Hide();

        MotionController.Instance.state = true;
    }
}
