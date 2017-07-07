using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCircle : MonoBehaviour {
    public float secondDelay;
    public GameObject[] spawnItems;

    private float spawnSphereRadius = 5f;
    private WaitForSeconds spawnDelay;

	// Use this for initialization
	void Start () {
        spawnDelay = new WaitForSeconds(secondDelay);
        StartCoroutine(StartSpawnCycle());
	}
	
	private IEnumerator StartSpawnCycle() {
        while (true) {
            Vector3 spawnPos = GetSpawnPosition();
            Vector3 spawnForce = GetSpawnForce(spawnPos);
            Spawn(spawnPos, spawnForce);

            yield return spawnDelay;
        }
    }

    private void Spawn(Vector3 pos, Vector3 dir) {
        GameObject item = spawnItems[Random.Range(0, spawnItems.Length)];
        
        GameObject spawnObj = Instantiate(item, pos, Random.rotation);
        Rigidbody rb = spawnObj.GetComponent<Rigidbody>();
        if (rb == null) {
            Destroy(spawnObj);
            return;
        } else {
            rb.AddForce(dir * 200f);
            rb.AddTorque(Random.onUnitSphere);
        }
    }

    private Vector3 GetSpawnPosition() {
        Vector2 planarOffset = Random.onUnitSphere * spawnSphereRadius;
        float verticalOffset = Random.Range(0.5f, 2.5f);

        return new Vector3(planarOffset.x, verticalOffset, planarOffset.y);
    }

    private Vector3 GetSpawnForce(Vector3 spawnPos) {
        return (transform.position - spawnPos).normalized;
    }
}
