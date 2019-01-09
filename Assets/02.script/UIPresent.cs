using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStandardAssets.Utils
{
    public class UIPresent : MonoBehaviour
    {
        public GameObject social;
        public GameObject dongsan;

        public static UIPresent instance = null;
        public static int state;
        public static UIPresent Instance
        {
            get
            {
                if (instance == null)
                {//= new CGame();
                    instance = FindObjectOfType(typeof(UIPresent)) as UIPresent;
                }
                return instance;
            }
        }
        // Use this for initialization
        void Start()
        {
            state = VREyeRaycaster.Instance.hit_object;

            social = GameObject.FindGameObjectWithTag("social");
            dongsan = GameObject.FindGameObjectWithTag("dongsan");

            social.SetActive(false);
            dongsan.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            state = VREyeRaycaster.Instance.hit_object;

            if (state==1)
            {
                social.SetActive(true);
                Debug.Log("active = " +state);
            }
            else if (state == 2)
            {
                dongsan.SetActive(true);
                Debug.Log("active = " + state);

            }
            else if (state == 3)
            {
                
                Debug.Log("active = " + state);

            }
            else if (state == 0)
            {
                social.SetActive(false);
                dongsan.SetActive(false);
            }
        }


    }
}