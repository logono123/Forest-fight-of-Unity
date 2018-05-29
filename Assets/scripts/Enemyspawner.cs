using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Enemyspawner : NetworkBehaviour
{

    public GameObject enemyPrefab;
    public int numberOfEnemies;


    public override void OnStartServer()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 position = new Vector3(Random.Range(-180f, 100f), 0, Random.Range(-100f, 200f));

            Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            GameObject enemy = Instantiate(enemyPrefab, position, rotation) as GameObject;

            NetworkServer.Spawn(enemy);
        }
    }


}