using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Inno_MotionController;

public class MotionController : MonoBehaviour
{
    private Vector3 DefaultPosition;

    private float moveSpeed = 10.0f;
    public int EquipNumber = 11;

    public float Heave = 0.0f;
    public float Roll = 0.0f;
    public float Pitch = 0.0f;

    public float FHeave = 0.0f;
    public float FRoll = 0.0f;
    public float FPitch = 0.0f;
    private float rotation_z = 0.0f;
    private float rotation_x = 0.0f;

    void Init()
    {
        Heave = 0.0f;
        Roll = 0.0f;
        Pitch = 0.0f;

        FHeave = 0.0f;
        FRoll = 0.0f;
        FPitch = 0.0f;


    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Right")
        {
            Roll += 0.4f;
            Debug.Log("Right");
        }
        else if (other.tag == "FRONT")
        {
            Pitch = 10f;
            Debug.Log("FRONT");
        }
        else if (other.tag == "Attack")
        {
            FRoll = 6f;
            Debug.Log("Attack");
        }

        CInnoMotion_API.SetOperation(Heave, Roll, Pitch, FHeave, FRoll, FPitch);

    }
    private void OnTriggerEnd(Collider other)
    {
        if (other.tag == "Right")
        {
            Init();
        }
        else if (other.tag == "FRONT")
        {
            Init();
        }
        else if (other.tag == "Attack")
        {
            Init();
        }






        CInnoMotion_API.SetOperation(Heave, Roll, Pitch, FHeave, FRoll, FPitch);

    }
    // Update is called once per frame
    void Update()
    {

        rotation_z = gameObject.transform.rotation.eulerAngles.z;
        rotation_x = gameObject.transform.rotation.eulerAngles.x;

        ////AmplitudeRoll = 좌 우 기울기 변환
        if (270 <= rotation_z && rotation_z <= 360)
        {
            Roll = ((360 - rotation_z) * -1)/(float)6.0;
            Debug.Log("Rotation_Z" + (int)((360 - rotation_z) * -1));
        }
        else
        {
            Roll = rotation_z / (float)5.2;
            Debug.Log("Rotation_Z" + (int)(rotation_z));
        }
        Heave = 3;
        ////Pitch 앞 뒤 기울기 변환
        //if (0 <= (rotation_x))
        //{
        //    Pitch = (rotation_x) /(float)-5.2;
        //    Debug.Log("Rotation_Y" + (int)(rotation_x));
        //}

        if (270 <= rotation_x && rotation_x <= 360)
        {
            Pitch = ((360 - rotation_x)) / (float)4.8;
            Debug.Log("Rotation_X" + (int)((360 - rotation_x) * -1));
        }
        else
        {
            Pitch = rotation_x / (float)-4.8;
            Debug.Log("Rotation_X" + (int)(rotation_x));
        }
            //AmplitudeHeave = 위 아래 / FrequencyHeave = 간격 - max 20
            //AmplitudeRoll = 좌 우 / FrequencyRoll = s - max 10
            //AmplitudePitch = 앞 뒤 / FrequencyPitch = 간격 - max 10
            FHeave = Random.Range(2, 7)*2 - 3;
            
            //좌 우 각도 이동
            CInnoMotion_API.SetOperation(Heave, Roll, Pitch, FHeave, FRoll, FPitch);

     }
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
