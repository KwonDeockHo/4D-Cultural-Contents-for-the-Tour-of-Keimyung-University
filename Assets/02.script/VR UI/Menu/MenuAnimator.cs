using System.Collections;
using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Menu
{

    public class MenuAnimator : MonoBehaviour
    {
        [SerializeField] private int m_FrameRate = 30;                  
        [SerializeField] private MeshRenderer m_ScreenMesh;             
        [SerializeField] private VRInteractiveItem m_VRInteractiveItem; 
        [SerializeField] private Texture[] m_AnimTextures;              


        private WaitForSeconds m_FrameRateWait;                         
        private int m_CurrentTextureIndex;                            
        private bool m_Playing;                                       


        private void Awake ()
        {
            
            m_FrameRateWait = new WaitForSeconds (1f / m_FrameRate);
        }


        private void OnEnable ()
        {
            m_VRInteractiveItem.OnOver += HandleOver;
            m_VRInteractiveItem.OnOut += HandleOut;
        }


        private void OnDisable ()
        {
            m_VRInteractiveItem.OnOver -= HandleOver;
            m_VRInteractiveItem.OnOut -= HandleOut;
        }


        private void HandleOver ()
        {
            m_Playing = true;
            StartCoroutine (PlayTextures ());
        }


        private void HandleOut ()
        {
            m_Playing = false;
        }


        private IEnumerator PlayTextures ()
        {
            while (m_Playing)
            {
                m_ScreenMesh.material.mainTexture = m_AnimTextures[m_CurrentTextureIndex];

                m_CurrentTextureIndex = (m_CurrentTextureIndex + 1) % m_AnimTextures.Length;

                yield return m_FrameRateWait;
            }
        }
    }
}
