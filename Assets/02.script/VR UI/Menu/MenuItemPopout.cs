using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Menu
{

    public class MenuItemPopout : MonoBehaviour
    {
        [SerializeField] private Transform m_Transform;         
        [SerializeField] private VRInteractiveItem m_Item;      
        [SerializeField] private float m_PopSpeed = 8f;         
        [SerializeField] private float m_PopDistance = 0.5f;    


        private Vector3 m_StartPosition;                        
        private Vector3 m_PoppedPosition;                       
        private Vector3 m_TargetPosition;                       


        private void Start ()
        {
            m_StartPosition = m_Transform.position;

            m_PoppedPosition = m_Transform.position - m_Transform.forward * m_PopDistance;
        }


        private void Update ()
        {
            m_TargetPosition = m_Item.IsOver ? m_PoppedPosition : m_StartPosition;

            m_Transform.position = Vector3.MoveTowards(m_Transform.position, m_TargetPosition, m_PopSpeed * Time.deltaTime);
        }
    }
}
