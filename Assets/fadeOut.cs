using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fadeOut : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Coater"))
            fadeout();
    }
    
    void fadeout()
    {
        SceneManager.LoadScene("Outro");
    }
}
