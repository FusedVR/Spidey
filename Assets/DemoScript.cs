using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour {
    public GameObject[] toggles;

	// Use this for initialization
	void Start () {
        StartCoroutine(DemoTimer());
	}
	
	private IEnumerator DemoTimer() {
        yield return new WaitForSeconds(3f);
        toggles[0].SetActive(true);

        yield return new WaitForSeconds(2f);
        toggles[1].SetActive(true);
        toggles[2].SetActive(true);
        toggles[3].SetActive(true);
        toggles[4].SetActive(true);
    }
}
