using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using Inno_MotionController;

namespace VRStandardAssets.Utils
{
    // This class exists to setup the device on a per platform basis.
    // The class uses the singleton pattern so that only one object exists.
    public class VRDeviceManager : MonoBehaviour
    {
        [SerializeField] private float m_RenderScale = 1.4f;

        private Vector3 DefaultPosition;
        public int EquipNumber = 11;

        private static VRDeviceManager s_Instance;

        public static VRDeviceManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType<VRDeviceManager> ();
                    DontDestroyOnLoad (s_Instance.gameObject);
                }

                return s_Instance;
            }
        }


        private void Awake ()
        {
            if (s_Instance == null)
            {
                s_Instance = this;
                DontDestroyOnLoad (this);
            }
            else if (this != s_Instance)
            {
                Destroy (gameObject);
            }
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

        void OnApplicationQuit()
        {
            CInnoMotion_API.SetSettle();
            CInnoMotion_API.CloseDevice();
        }
    }
}