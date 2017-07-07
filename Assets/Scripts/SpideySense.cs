using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpideySense : MonoBehaviour {
    public static SpideySense Instance;
    public float senseTime;
    public SenseBar[] horizontalSenses;
    public SenseBar[] verticalSenses;

    private float behindThreshold = -0.9f;
    private float boundsThreshold = Mathf.Sqrt(3f) / 2f;

    private int sensesProcessing;
    private WaitForSeconds senseWait;
    private Camera cam;
    private Plane[] planes;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        senseWait = new WaitForSeconds(senseTime);
        cam = Camera.main;
    }
	
	public void SenseObject(GameObject go) {
        Collider co = go.GetComponent<Collider>();
        if (co == null) return;

        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        if (GeometryUtility.TestPlanesAABB(planes, co.bounds))
            Debug.Log(go.name + " is in view!");
        else
            //StartCoroutine(SlowTime());
            ToggleSenses(go);
    }

    private IEnumerator SlowTime() {
        Time.timeScale = 0.2f;
        sensesProcessing++;
        yield return senseWait;
        sensesProcessing--;

        if (sensesProcessing == 0) {
            Time.timeScale = 1f;
        }
    }

    private void ToggleSenses(GameObject go) {
        // get a normalized vector pointing to go
        Vector3 objPosition = transform.InverseTransformPoint(go.transform.position);
        Vector3 objDirection = (objPosition - transform.localPosition).normalized;

        if (objDirection.z < behindThreshold) {
            Debug.Log("Behind");
            ToggleAll();
        } else if (InBounds(objDirection.x)) {
            Debug.Log("Horizontal");
            ToggleSenses(horizontalSenses, objDirection.x);
        } else if (InBounds(objDirection.y)) {
            Debug.Log("Vertical");
            ToggleSenses(verticalSenses, objDirection.y);
        } else {
            Debug.Log("Multi");
            ToggleSenses(horizontalSenses, objDirection.x);
            ToggleSenses(verticalSenses, objDirection.y);
        }
    }

    private void ToggleAll() {
        ToggleSenses(horizontalSenses, 1);
        ToggleSenses(horizontalSenses, -1);
        ToggleSenses(verticalSenses, 1);
        ToggleSenses(verticalSenses, -1);
    }

    // Note the first entry in senses always denotes the negative of the senses axis
    private void ToggleSenses(SenseBar[] senses, float val) {
        int senseIndex = (val > 0) ? 1 : 0;
        senses[senseIndex].SetSense(Color.red, 1f);
    }

    private bool InBounds(float val) {
        return val > boundsThreshold || val < (boundsThreshold * -1f);
    }
}
