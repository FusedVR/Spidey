using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class: SpideySense
 * Receives spidey sense requests via SenseObject. Determines the axes (up/down/left/right) for the
 * senses in ToggleSenses, then triggers the senses on screen using the appropriate reference(s) from
 * horizontalSenses and verticalSenses.
 */
public class SpideySense : MonoBehaviour {
    public static SpideySense Instance;
    public float senseTime;
    public SenseBar[] horizontalSenses;
    public SenseBar[] verticalSenses;

    private float behindThreshold = -0.9f;
    private float boundsThreshold = Mathf.Sqrt(3f) / 2f;

    private AudioSource src;
    private int sensesProcessing;
    private WaitForSeconds senseWait;
    private Camera cam;
    private Plane[] planes;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        src = gameObject.GetComponent<AudioSource>();
        senseWait = new WaitForSeconds(senseTime);
        cam = Camera.main;
    }
	
	public void SenseObject(GameObject obj) {
        Collider co = obj.GetComponent<Collider>();
        if (co == null) return;

        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        if (GeometryUtility.TestPlanesAABB(planes, co.bounds)) // A very rough test to check if the collider is in view of the camera.
            Debug.Log(obj.name + " is in view!");
        else
            //Removed the following because the time slow down did not seem obvious enough or worthwhile. 
            //StartCoroutine(SlowTime());
            src.Play();
            ToggleSenses(obj);
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
        // Get a normalized, local space vector pointing toward the object which triggered the spidey sense.
        Vector3 objPosition = transform.InverseTransformPoint(go.transform.position);
        Vector3 objDirection = (objPosition - transform.localPosition).normalized;

        // The vector is normalized, which means (x^2 + y^2 + z^2)^(1/2) is equal to 1. So, the x/y/z coordinates are constrained.
        // For example: the largest magnitude of a coordinate is 1 and, if a coordinate has a magnitude of 1, the others must be 0.
        // 
        // We're in local space, so recall positive x means right, positive y means up, and positive z means forward relative to the player.
        // 
        // Concrete Example: if x has very high magnitude, the y and z components must have small magnitude. We use that as a quick and dirty way to decide
        // the object is mostly coming along the x axis. The sign of x determines left or right. If x is +1 the object is coming EXACTLY from the right.
        //
        // MATH TO SOLVE REAL (VIRTUAL?) WORLD PROBLEMS WOOT WOOT!
        if (objDirection.z < behindThreshold) { // Along z (the forward/behind) axis, we only trigger spidey sense for behind.
            Debug.Log("Behind");
            ToggleAll();
        } else if (AboveMagnitudeThreshold(objDirection.x)) {
            Debug.Log("Horizontal");
            ToggleSenses(horizontalSenses, objDirection.x);
        } else if (AboveMagnitudeThreshold(objDirection.y)) {
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

    private bool AboveMagnitudeThreshold(float val) {
        float magnitude = Mathf.Abs(val);
        return magnitude > boundsThreshold;
    }
}
