using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fadeinout : MonoBehaviour
{
    public UnityEngine.UI.Image fade;

    float fades = 0.0f;
    float time = 0;

    public bool fade_state = false;
    public static Fadeinout f_instance = null;

    public static Fadeinout Instance
    {
        get
        {
            if (f_instance == null)
            {//= new CGame();
                f_instance = FindObjectOfType(typeof(Fadeinout)) as Fadeinout;
            }
            return f_instance;
        }
    }
    private void Awake()
    {
        f_instance = this;
    }
    // Use this for initialization
    void Start()
    {
        fade_state = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("fade");
        time += Time.deltaTime;
        if (!fade_state)
        {
            Debug.Log("in");

            if (fades < 1.0f && time >= 0.1f)
            {
                fades += 0.1f;
                fade.color = new Color(0, 0, 0, fades);
                time = 0;
            }
            else if (fades >= 1.0f)
            {
                time = 0;
                //VideoController.Instance.circle_tr.SetActive(true);
                //VideoController.Instance.canvas_ui.SetActive(true);
                fade_state = true;
            }
        }
        else if (fade_state == true)
        {
            Debug.Log("out");

            if (fades > 0.0f && time >= 0.1f)
            {
                fades -= 0.1f;
                fade.color = new Color(0, 0, 0, fades);

                time = 0;
            }
            else if (fades <= 0.0f)
            {
                time = 0;
                fade_state = true;
            }
        }



    }
}
