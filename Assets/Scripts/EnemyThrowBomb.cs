using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrowBomb : MonoBehaviour
{
    [SerializeField] float enemyBombSpeed;
    [SerializeField] GameObject enemyBomb;
    [SerializeField] GameObject enemyPlayerBomb;
    [SerializeField] Transform enemyBombPos;

    EnemyController enemyController; //For excess enemy become player friend or not

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ENEMYTHROW() //Player Throw Bomb
    {
        if (enemyController.enemy == true)
        {
            GameObject bombSpawn = Instantiate(enemyBomb, enemyBombPos.position, enemyBombPos.rotation);

            bombSpawn.GetComponent<Rigidbody>().velocity = enemyBombPos.transform.up * enemyBombSpeed;
        }
        else
        {
            enemyBombSpeed = 10f;

            GameObject bombSpawn = Instantiate(enemyPlayerBomb, enemyBombPos.position, enemyBombPos.rotation);

            bombSpawn.GetComponent<Rigidbody>().velocity = enemyBombPos.transform.up * enemyBombSpeed;
        }
    }
}//Class
