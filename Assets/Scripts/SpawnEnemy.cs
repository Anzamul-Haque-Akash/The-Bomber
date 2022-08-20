using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefabs;
    public int number;

    public float spawnRadius;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<number; i++)
        {
            Vector3 randomPoint = this.transform.position + Random.insideUnitSphere * spawnRadius;

            Instantiate(enemyPrefabs, randomPoint, Quaternion.identity);

        }
    }
}//Class
