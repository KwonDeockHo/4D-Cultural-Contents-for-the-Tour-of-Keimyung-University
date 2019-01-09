using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRStandardAssets.Utils;


namespace VRStandardAssets.Menu
{
    // This script is for loading scenes from the main menu.
    // Each 'button' will be a rendering showing the scene
    // that will be loaded and use the SelectionRadial.
    public class MenuButton : MonoBehaviour
    {
        private static MenuButton m_instance = null;//instance »ý¼º

        public event Action<MenuButton> OnButtonSelected;                   // This event is triggered when the selection of the button has finished.
        [SerializeField] public string m_SceneToLoad;                      // The name of the scene to load.
        [SerializeField] private VRCameraFade m_CameraFade;                 // This fades the scene out when a new scene is about to be loaded.
        [SerializeField] private SelectionRadial m_SelectionRadial;         // This controls when the selection is complete.
        [SerializeField] private VRInteractiveItem m_InteractiveItem;       // The interactive item for where the user should click to load the level.
        [SerializeField] private VREyeRaycaster m_ray;

        public int s_state = 0;

        public static bool m_GazeOver = false;                                            // Whether the user is looking at the VRInteractiveItem currently.

        public static MenuButton Instance
        {
            get
            {
                if (m_instance == null)
                {//= new CGame();
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
            // When the user looks at the rendering of the scene, show the radial.
            m_SelectionRadial.Show();
           // m_ray.select_state;

            m_GazeOver = true;
        }


        private void HandleOut()
        {

            // When the user looks away from the rendering of the scene, hide the radial.
            m_SelectionRadial.Hide();

            m_GazeOver = false;
        }


        private void HandleSelectionComplete()
        {

            // If the user is looking at the rendering of the scene when the radial's selection finishes, activate the button.
            if (m_GazeOver)
                StartCoroutine (ActivateButton());
        }


        private IEnumerator ActivateButton()
        {

            // If the camera is already fading, ignore.

            // If anything is subscribed to the OnButtonSelected event, call it.
            if (OnButtonSelected != null)
                OnButtonSelected(this);

           // Debug.Log(m_InteractiveItem);

            // Load the level.
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                // Wait for the camera to fade out.
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