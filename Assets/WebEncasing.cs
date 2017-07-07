using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebEncasing : MonoBehaviour {
    public GameObject webQuad;

	// Use this for initialization
	void Start () {
        int numQuads = Random.Range(200, 400);
        SpawnQuads(numQuads);
	}
	
	private void SpawnQuads(int numQuads) {
        for(int i = 0; i < numQuads; i++) {
            GameObject quad = Instantiate(webQuad, transform.position, Random.rotation);
            quad.transform.parent = transform;
            Vector3 randPoint = Random.onUnitSphere * 0.5f;
            quad.transform.localPosition = randPoint;
        }
    }
}
