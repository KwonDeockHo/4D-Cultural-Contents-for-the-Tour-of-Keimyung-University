using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepAttack : MonoBehaviour {

    public GameObject target;
    public float len;
    Animator ani;
    public float dis = 0;

	void Start () {
        ani = gameObject.GetComponent<Animator>();
	}
	
	void Update () {

        dis = Vector3.Distance(gameObject.transform.position, target.transform.position);

        if (dis <= len)
            ani.SetBool("bSleep", true);
        else
            ani.SetBool("bSleep", false);

	}
}
