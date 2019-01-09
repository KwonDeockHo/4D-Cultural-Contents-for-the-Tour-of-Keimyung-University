using UnityEngine;
using FDRPConnect;

public class FDRP_Input : MonoBehaviour
{
    public FDRP SendMsg = new FDRP();
    string lastMessage;
    string PrevSend = null;
    int motion_state;


    private void OnTrriggerEnter(Collider other)
    {
        if(other.tag == "Right")
        {
            if (PrevSend == "FDRP.R") { return; }
            SendMsg.fnPacket("FDRP.R");
            PrevSend = "FDRP.R";
            Debug.Log("RIGHT");
        }
    }




    void Start()
    {
        Debug.Log(SendMsg.fnConnectResult("127.0.0.1", 61010));
        if (SendMsg.res != "")
        {
            Debug.Log(SendMsg.res);
        }
        motion_state = 0;
    }

    void Update()
    {
      
    }
}