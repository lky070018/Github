using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, 5);
	}
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
