using System;
using UnityEngine;

namespace VRStandardAssets.Utils
{
    // In order to interact with objects in the scene
    // this class casts a ray into the scene and if it finds
    // a VRInteractiveItem it exposes it for other classes to use.
    // This script should be generally be placed on the camera.
    public class VREyeRaycaster : MonoBehaviour
    {
        public event Action<RaycastHit> OnRaycasthit;                   // This event is called every frame that the user's gaze is over a collider.
        private static VREyeRaycaster lay_instance = null;//instance »ý¼º
        public int hit_object = 0;

        [SerializeField] private Transform m_Camera;
        [SerializeField] private LayerMask m_ExclusionLayers;           // Layers to exclude from the raycast.
        [SerializeField] private Reticle m_Reticle;                     // The reticle, if applicable.
        [SerializeField] private VRInput m_VrInput;                     // Used to call input based events on the current VRInteractiveItem.
        [SerializeField] private bool m_ShowDebugRay;                   // Optionally show the debug ray.
        [SerializeField] private float m_DebugRayLength = 5f;           // Debug ray length.
        [SerializeField] private float m_DebugRayDuration = 1f;         // How long the Debug ray will remain visible.
        [SerializeField] private float m_RayLength = 500f;            
        [SerializeField] public bool OnLayState= false;             
        [SerializeField] public bool select_state= false;
        public event Action OnClick;
        public float layTime = 0.0f;
        

        private VRInteractiveItem m_CurrentInteractible;                //The current interactive item
        private VRInteractiveItem m_LastInteractible;                   //The last interactive item


        // Utility for other classes to get the current interactive item
        public VRInteractiveItem CurrentInteractible
        {
            get { return m_CurrentInteractible; }
        }
        public static VREyeRaycaster Instance
        {
            get
            {
                if (lay_instance == null)
                {//= new CGame();
                    lay_instance = GameObject.FindObjectOfType(typeof(VREyeRaycaster)) as VREyeRaycaster;
                }
                return lay_instance;
            }
        }
        private void OnEnable()
        {
            m_VrInput.OnClick += HandleClick;
            m_VrInput.OnDoubleClick += HandleDoubleClick;
            m_VrInput.OnUp += HandleUp;
            m_VrInput.OnDown += HandleDown;
        }


        private void OnDisable ()
        {
            m_VrInput.OnClick -= HandleClick;
            m_VrInput.OnDoubleClick -= HandleDoubleClick;
            m_VrInput.OnUp -= HandleUp;
            m_VrInput.OnDown -= HandleDown;
        }


        private void Update()
        {
            EyeRaycast();
        }

        private void Awake()
        {
            hit_object = 0;
        }
        private void EyeRaycast()
        {
            // Show the debug ray if required
            if (m_ShowDebugRay)
            {
                Debug.DrawRay(m_Camera.position, m_Camera.forward * m_DebugRayLength, Color.blue, m_DebugRayDuration);
            }

            // Create a ray that points forwards from the camera.
            Ray ray = new Ray(m_Camera.position, m_Camera.forward);
            RaycastHit hit;

            // Do the raycast forweards to see if we hit an interactive item
            if (Physics.Raycast(ray, out hit, m_RayLength, ~m_ExclusionLayers))
            {
                VRInteractiveItem interactible = hit.collider.GetComponent<VRInteractiveItem>(); //attempt to get the VRInteractiveItem on the hit object
                m_CurrentInteractible = interactible;
                
                if (hit.collider.gameObject.name == "Social")
                {
                    hit_object = 1;
                    Debug.Log("Social_Click");
                }
                else if (hit.collider.gameObject.name == "Dongsan")
                {
                    hit_object = 2;
                    Debug.Log("Dongsan_Click");

                }
                else if (hit.collider.gameObject.name == "play")
                {
                    hit_object = 3;
                    Debug.Log("play");

                }
                else
                {
                    hit_object = 0;
                }


                // If we hit an interactive item and it's not the same as the last interactive item, then call Over
                if (interactible && interactible != m_LastInteractible)
                {
                    interactible.Over();
                    m_VrInput.Input_lay = true;

                }

                // Deactive the last interactive item 
                if (interactible != m_LastInteractible)
                {
                    DeactiveLastInteractible();
                }

                m_LastInteractible = interactible;

                // Something was hit, set at the hit position.
                if (m_Reticle)
                {
                    //layTime += Time.deltaTime;
                    m_Reticle.SetPosition(hit);
                }

                if (OnRaycasthit != null)
                {
                    OnRaycasthit(hit);
                }
            }
            else
            {
                // Nothing was hit, deactive the last interactive item.
                DeactiveLastInteractible();
                m_CurrentInteractible = null;
                hit_object = 0;

                // Position the reticle at default distance.
                if (m_Reticle)
                    m_Reticle.SetPosition();
            }
        }


        private void DeactiveLastInteractible()
        {

            if (m_LastInteractible == null)
                return;
            m_VrInput.Input_lay = false;

            m_LastInteractible.Out();
            m_LastInteractible = null;
        }


        private void HandleUp()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Up();
        }


        private void HandleDown()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Down();
        }


        private void HandleClick()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Click();
        }


        private void HandleDoubleClick()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.DoubleClick();

        }
    }
}