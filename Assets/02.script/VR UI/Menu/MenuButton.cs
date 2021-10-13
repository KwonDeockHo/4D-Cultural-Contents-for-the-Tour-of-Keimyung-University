using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRStandardAssets.Utils;


namespace VRStandardAssets.Menu
{

    public class MenuButton : MonoBehaviour
    {
        private static MenuButton m_instance = null; //instance 

        public event Action<MenuButton> OnButtonSelected;                   
        [SerializeField] public string m_SceneToLoad;                      
        [SerializeField] private VRCameraFade m_CameraFade;                 
        [SerializeField] private SelectionRadial m_SelectionRadial;         
        [SerializeField] private VRInteractiveItem m_InteractiveItem;       
        [SerializeField] private VREyeRaycaster m_ray;

        public int s_state = 0;

        public static bool m_GazeOver = false;                              

        public static MenuButton Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = GameObject.FindObjectOfType(typeof(MenuButton)) as MenuButton;
                }
                return m_instance;
            }
        }

        private void OnEnable (){
            m_InteractiveItem.OnOver += HandleOver;
            m_InteractiveItem.OnOut += HandleOut;
            m_SelectionRadial.OnSelectionComplete += HandleSelectionComplete;
        }
        
        private void OnDisable () {
            m_InteractiveItem.OnOver -= HandleOver;
            m_InteractiveItem.OnOut -= HandleOut;
            m_SelectionRadial.OnSelectionComplete -= HandleSelectionComplete;
        }

        public VREyeRaycaster RayCasting {
            get {
                return m_ray;
            }
        }

        private void HandleOver()
        {
            m_SelectionRadial.Show();

            m_GazeOver = true;
        }


        private void HandleOut()
        {

            m_SelectionRadial.Hide();

            m_GazeOver = false;
        }


        private void HandleSelectionComplete()
        {

            if (m_GazeOver)
                StartCoroutine (ActivateButton());
        }


        private IEnumerator ActivateButton()
        {


            if (OnButtonSelected != null)
                OnButtonSelected(this);


            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                if (m_CameraFade.IsFading)
                    yield break;
                yield return StartCoroutine(m_CameraFade.BeginFadeOut(true));

                SceneManager.LoadScene("Asite", LoadSceneMode.Single);
            }
            else if (SceneManager.GetActiveScene().buildIndex == 1)
            {

            }
        }
    }
}
