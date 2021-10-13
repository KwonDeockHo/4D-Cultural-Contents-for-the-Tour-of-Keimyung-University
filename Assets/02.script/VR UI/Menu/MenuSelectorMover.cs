using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Menu
{

    public class MenuSelectorMover : MonoBehaviour
    {
        [SerializeField] private float m_PopSpeed = 8f;         
        [SerializeField] private float m_PopDistance = 0.5f;    
        [SerializeField] private float m_MoveSpeed = 7f;        
        [SerializeField] private Transform m_ParentTransform;   
        [SerializeField] private Transform m_ChildTransform;    
        [SerializeField] private VRInteractiveItem[] m_Items;   


        private Quaternion m_TargetRotation;                    
        private Vector3 m_StartPosition;                        
        private Vector3 m_PoppedPosition;                       
        private Vector3 m_TargetPosition;                       


        void Awake ()
        {
            // start position.
            m_StartPosition = m_ChildTransform.localPosition;

            // Calculate the popped position.
            m_PoppedPosition = m_ChildTransform.localPosition - Vector3.forward * m_PopDistance;
        }

	    
        void Update ()
        {
            m_TargetPosition = m_StartPosition;

	        for (int i = 0; i < m_Items.Length; i++)
	        {
	            if (!m_Items[i].IsOver)
                    continue;

	            m_TargetRotation = m_Items[i].transform.rotation;
	            m_TargetPosition = m_PoppedPosition;
	            break;
	        }

            m_ChildTransform.localPosition = Vector3.MoveTowards (m_ChildTransform.localPosition, m_TargetPosition, m_PopSpeed * Time.deltaTime);

            m_ParentTransform.rotation = Quaternion.Slerp(m_ParentTransform.rotation, m_TargetRotation, m_MoveSpeed * Time.deltaTime);
	    }
    }
}
