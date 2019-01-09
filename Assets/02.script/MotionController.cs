using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Inno_MotionController;

public class MotionController : MonoBehaviour
{
    private static MotionController s_instance = null;

    public bool actionbool = false;

    private Vector3 DefaultPosition;

    private float moveSpeed = 10.0f;
    public int EquipNumber = 11;

    public float Heave = 0.0f;
    public float Roll = 0.0f;
    public float Pitch = 0.0f;

    public float FHeave = 0.0f;
    public float FRoll = 0.0f;
    public float FPitch = 0.0f;

    public float time = 0.0f;

    public bool state = false;

    Animator anim;


    public static MotionController Instance
    {
        get
        {
            if (s_instance == null)
            {//= new CGame();
                s_instance = FindObjectOfType(typeof(MotionController)) as MotionController;
            }
            return s_instance;
        }
    }
    void Init()
    {
        Heave = 0.0f;
        Roll = 0.0f;
        Pitch = 0.0f;

        FHeave = 0.0f;
        FRoll = 0.0f;
        FPitch = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //AmplitudeHeave = 위 아래 / FrequencyHeave = 간격 - max 30
        //AmplitudeRoll = 좌 우 / FrequencyRoll = s - max 10
        //AmplitudePitch = 앞 뒤 / FrequencyPitch = 간격 - max 10
        if (state == true)
        {
            if (actionbool == true)
            {
                anim.SetFloat("Speed", 1);

                CInnoMotion_API.SetOperation(Heave, Roll, Pitch, FHeave, FRoll, FPitch);
                time += Time.deltaTime;
                //pause
                if (time >= 130f)
                    SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);

            }
            else
                Init();
        }
        else if (state == false)
        {
            Init();
            anim.SetFloat("Speed", 0);
        }


    }
    // Use this for initialization
    void Start()
    {
        DefaultPosition.Set(0.0f, 0.0f, 0.0f);
        anim = gameObject.GetComponent<Animator>();
        if (MotionOpen() != 0)
        {
            Debug.LogError("Failed to connect to the motion");
        }
        else
        {
            StartCoroutine(MotionInit());
        }
        state = true;
        actionbool = true;
        time = 0.0f;
    }

    int MotionOpen()
    {
        int nOpen;
        CInnoMotion_API.SetEquipNumber(EquipNumber);
        nOpen = CInnoMotion_API.OpenDevice();

        if (nOpen == 0)
        {
            CInnoMotion_API.SetServoOnOff(0, CInnoMotion_API.ON);
            CInnoMotion_API.SetServoOnOff(1, CInnoMotion_API.ON);
            CInnoMotion_API.SetServoOnOff(2, CInnoMotion_API.ON);

            CInnoMotion_API.SetAlarmOnOff(0, CInnoMotion_API.ON);
            CInnoMotion_API.SetAlarmOnOff(1, CInnoMotion_API.ON);
            CInnoMotion_API.SetAlarmOnOff(2, CInnoMotion_API.ON);

            CInnoMotion_API.SetAlarmOnOff(0, CInnoMotion_API.OFF);
            CInnoMotion_API.SetAlarmOnOff(1, CInnoMotion_API.OFF);
            CInnoMotion_API.SetAlarmOnOff(2, CInnoMotion_API.OFF);
        }
        return nOpen;
    }
    public IEnumerator MotionInit()
    {
        CInnoMotion_API.SetSettle();
        yield return new WaitForSeconds(1.0f);  //while motion move

        CInnoMotion_API.SetNeutral();
        yield return new WaitForSeconds(3.0f);  //while motion move      
    }

    void SetSettle()
    {
        CInnoMotion_API.SetSettle();
    }

    void SetNeutral()
    {
        CInnoMotion_API.SetNeutral();
    }

    void OnApplicationQuit()
    {
        CInnoMotion_API.SetSettle();
        CInnoMotion_API.CloseDevice();
    }

}
