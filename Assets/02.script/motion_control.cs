using UnityEngine;
using System.Collections;
using Inno_MotionController;

public class MotionControl : MonoBehaviour
{

    /// <summary>
    /// Amplitude limit 
    /// 30 > Heave > -30
    /// 10 > Roll  > -10
    /// 10 > Pitch > -10
    ///
    /// Caution !!
    /// SetOperation must not be invoked in less than 20ms.
    /// </summary>
    /// 
    private Vector3 DefaultPosition;


    private int EquipNumber = 11;
    private double moveSpeed = 10.0f;

    public double AmplitudeHeave = 0.0f;
    public double AmplitudeRoll = 0.0f;
    public double AmplitudePitch = 0.0f;

    public double FrequencyHeave = 0.0f;
    public double FrequencyRoll = 0.0f;
    public double FrequencyPitch = 0.0f;
    private double roll = 0.0f;
    private double heave = 0.0f;
    private double pitch = 0.0f;

    // Use this for initialization
    void Start()
    {
        DefaultPosition.Set(0.0f, 0.0f, 0.0f);

        if (MotionOpen() != 0)
        {
            Debug.LogError("Failed to connect to the motion");
        }
        else
        {
            StartCoroutine(MotionInit());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //AmplitudeHeave = 위 아래 / FrequencyHeave = 간격
        //AmplitudeRoll = 좌 우 / FrequencyRoll = 간격
        //AmplitudePitch = 앞 뒤 / FrequencyPitch = 간격

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (roll <= 10)
            {
                roll += (Time.deltaTime * moveSpeed);
                AmplitudeRoll = roll * -1;
            }
            else
                return;

            Debug.Log("LEFT");
            CInnoMotion_API.SetOperation(AmplitudeHeave, AmplitudeRoll, AmplitudePitch, FrequencyHeave, FrequencyRoll, FrequencyPitch);

        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (-10 <= roll)
            {
                roll -= (Time.deltaTime * moveSpeed);
                AmplitudeRoll = roll * -1;
            }
            Debug.Log("Right");

            CInnoMotion_API.SetOperation(AmplitudeHeave, AmplitudeRoll, AmplitudePitch, FrequencyHeave, FrequencyRoll, FrequencyPitch);

        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (pitch <= 10)
            {
                pitch += (Time.deltaTime * moveSpeed);
                AmplitudePitch = pitch;
            }
            Debug.Log("Down");

            CInnoMotion_API.SetOperation(AmplitudeHeave, AmplitudeRoll, AmplitudePitch, FrequencyHeave, FrequencyRoll, FrequencyPitch);

        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (-10 <= pitch)
            {
                pitch -= (Time.deltaTime * moveSpeed);
                AmplitudePitch = pitch;
            }
            Debug.Log("Up");

            CInnoMotion_API.SetOperation(AmplitudeHeave, AmplitudeRoll, AmplitudePitch, FrequencyHeave, FrequencyRoll, FrequencyPitch);

        }
        if (Input.GetKey(KeyCode.PageDown))
        {
            if (-10 <= heave)
            {
                heave -= (Time.deltaTime * moveSpeed);
                AmplitudeHeave = heave;
            }
            Debug.Log("Jump");

            CInnoMotion_API.SetOperation(AmplitudeHeave, AmplitudeRoll, AmplitudePitch, FrequencyHeave, FrequencyRoll, FrequencyPitch);

        }
        if (Input.GetKey(KeyCode.PageUp))
        {
            if (heave <= 10)
            {
                heave += (Time.deltaTime * moveSpeed);
                AmplitudeHeave = heave;
            }
            Debug.Log("Fall");

            CInnoMotion_API.SetOperation(AmplitudeHeave, AmplitudeRoll, AmplitudePitch, FrequencyHeave, FrequencyRoll, FrequencyPitch);

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            heave = 0.0f;
            pitch = 0.0f;
            roll = 0.0f;

            CInnoMotion_API.SetOperation(0, 0, 0, 0, 0, 0);
        }
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
