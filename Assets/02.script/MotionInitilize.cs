using UnityEngine;
using System.Collections;
using Inno_MotionController;


public class MotionInitilize : MonoBehaviour
{

    /// <summary>
    /// operating sequence
    /// 1. Init(only once)
    /// SetEquipNumber -> OpenDevice
    /// 2. start 
    /// SetSettle -> SetNeutral -> SetOperation
    /// 3. stop
    /// Stop -> SetSettle
    /// 4. end
    /// Stop -> SetSettle -> CloseDevice
    /// 
    /// loop by 2, 3
    /// </summary>

    public int EquipNumber = 11;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public int MotionConnect()
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

    void MotionDisConnect()
    {
        CInnoMotion_API.CloseDevice();
    }

    void SetSettle()
    {
        CInnoMotion_API.SetSettle();
    }

    void SetNeutral()
    {
        CInnoMotion_API.SetNeutral();
    }

}
