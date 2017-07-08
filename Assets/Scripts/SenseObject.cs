using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class: SenseObject
 * An object which, when created, will trigger spidey sense.
 */
public class SenseObject : MonoBehaviour {
    public GameObject webCover; // When the object is hit by a WebShot, the webCover will be attached.

    private Rigidbody rb;
    private WaitForSeconds velocityEase = new WaitForSeconds(0.1f);

    private void Start() {
        rb = GetComponent<Rigidbody>();
        if (rb == null) Destroy(gameObject);
        SpideySense.Instance.SenseObject(gameObject);

        MoveTowardTarget(Camera.main.transform, rb);
        
    }

    private void MoveTowardTarget(Transform tar, Rigidbody rb) {
        transform.LookAt(tar);
        Vector3 dir = (tar.position - transform.position).normalized;
        rb.AddForce(dir * 200f);
    }

    private void OnCollisionEnter(Collision collision) {
        WebShot ws = collision.gameObject.GetComponent<WebShot>();
        if (rb == null || ws == null) return;

        Destroy(ws.gameObject);
        GameObject cover = Instantiate(webCover, transform.position, transform.rotation);
        cover.transform.parent = transform;
        rb.useGravity = true;
        StartCoroutine(EaseOutVelocity());
        Destroy(gameObject, 5f); // We mostly destroy the object because the web cover is graphically intensive (see WebEncasing.cs).
    }

    // Rather than immediately set the velocity to zero, ease the velocity to zero.
    private IEnumerator EaseOutVelocity() {
        while (rb.velocity.magnitude > 0.1f) {
            rb.velocity *= 0.5f;
            yield return velocityEase;
        }
        rb.velocity = Vector3.zero; // After velocity gets low enough, set to zero.
    }
}
