using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SenseBar : MonoBehaviour {
    public GameObject bar;
	
	public void SetSense(Color c, float duration) {
        GameObject senseBar = Instantiate(bar, transform.position, transform.rotation);
        senseBar.transform.parent = transform;

        senseBar.GetComponent<Image>().color = c;
        Destroy(senseBar, duration);
    }
}
