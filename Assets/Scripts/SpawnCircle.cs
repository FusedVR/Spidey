using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class: SpawnCircle
 * Spawns items from the spawnItems array on a sphere of radius 5 with random
 * vertical offsets.
 */
public class SpawnCircle : MonoBehaviour {
    public float secondDelay;
    public GameObject[] spawnItems;

    private float spawnSphereRadius = 8f;
    private WaitForSeconds spawnDelay; 

	// Use this for initialization
	void Start () {
        spawnDelay = new WaitForSeconds(secondDelay);
        StartCoroutine(StartSpawnCycle());
	}
	
	private IEnumerator StartSpawnCycle() {
        while (true) {
            Vector3 spawnPos = GetSpawnPosition();
            Spawn(spawnPos);

            yield return spawnDelay;
        }
    }

    private void Spawn(Vector3 pos) {
        GameObject item = spawnItems[Random.Range(0, spawnItems.Length)];
        
        GameObject spawnObj = Instantiate(item, pos, Random.rotation);
    }

    private Vector3 GetSpawnPosition() {
        Vector2 planarOffset = Random.insideUnitCircle.normalized * spawnSphereRadius;
        float verticalOffset = Random.Range(0.5f, 2.5f);
        Vector3 spawnOffset = new Vector3(planarOffset.x, verticalOffset, planarOffset.y);

        return Camera.main.transform.position + spawnOffset;
    }
}
