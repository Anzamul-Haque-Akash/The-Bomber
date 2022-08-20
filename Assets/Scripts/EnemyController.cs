using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] AudioSource mainPlayerAttackAudio;

    Transform player;

    [SerializeField] GameObject playerBomb;
    float enemyDistWithPlayer;
    [SerializeField] float howCloase;

    [SerializeField] float rotationSpeed;

    public bool enemy;
    bool followThePlayer;

    bool enemyMove; //Enemy can move or not

    bool attack;

    bool friendEnemyThrowBomb;

    int enemyAttackOrNot; //Enemy Attack Or Not

    int randomPositionIndex;
    float enemyDistanceWithRandomPosition;


    public GameObject enemyMaterial; //For Enemy Color Change
    [SerializeField] Color playerColor;


    //--------------------

    //NavMesh--------
    public GameObject[] positions;
    NavMeshAgent agent;
    //NavMesh--------

    GameManager gameManagerScript; //Game Manager Script


    //New Tag---------
    public string playerFriendTag; //player friend tag
    //----------------

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        enemy = true;
        followThePlayer = false;

        enemyMove = true;

        attack = true;
        friendEnemyThrowBomb = false;

        gameManagerScript = GetComponent<GameManager>();

        agent = GetComponent<NavMeshAgent>();

        anim.SetBool("isRun", true); //At First Enemy run animation start

        randomPositionIndex = Random.Range(0, 54); //Random index at first

    }

    private void OnTriggerEnter(Collider other) //Enemy Ontigger enter function
    {
        if (other.tag == "Bomb" && enemy == true)
        {
            Destroy(other.gameObject); //Destroy Thrown Bomb
            playerBomb.SetActive(true); //Eneble Enemy Foot position Bomb

            mainPlayerAttackAudio.Play(); //player hit audio

            anim.SetBool("isDeath", true); //Enemy Death Animation Start

           // StartCoroutine(DestroyEnemy()); //Destroy enemy after death

        }

        if (other.tag == "BombArea" && enemy == true)
        {
            //Destroy(other.gameObject); //Destroy Thrown Bomb

            //mainPlayerAttackAudio.Play(); //player hit audio

            anim.SetBool("isDeath", true); //Enemy Death Animation Start

            //StartCoroutine(DestroyEnemy()); //Destroy enemy after death

        }

        if(other.tag == "Bomb" && enemy == false) //Only bomb get distroy friend enemy
        {
            Destroy(other.gameObject);
        }

        if (other.tag == "Wall") //Enemy rotate when collide with wall.
        {
            transform.Rotate(transform.up * Time.deltaTime * (rotationSpeed + 100));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Enemy AI Code Start--------

        enemyDistWithPlayer = Vector3.Distance(player.position, transform.position); //Find The distance with Player

        
        if (enemy == true && enemyDistWithPlayer > 10f) 
        {

            if (enemyMove == true && anim.GetBool("isDeath") == false && enemy) //Enemy move code 
            {
                anim.SetBool("isRun", true);
                agent.speed = 1; //nevMesh Spped 1

                agent.SetDestination(positions[randomPositionIndex].transform.position);

                enemyDistanceWithRandomPosition = Vector3.Distance(positions[randomPositionIndex].transform.position, transform.position); //Random Inxed generate base on this condition 
                if (enemyDistanceWithRandomPosition <= howCloase)
                {
                    randomPositionIndex = Random.Range(0, 54); //Enemy move random position index
                }
            }

        }
        //------------------------------------------------------------

        //Enemy follow the player with in a range--------------------

        else if (enemy == true && enemyDistWithPlayer <= 10f) //Player Range Become true
        {
            enemyAttackOrNot = Random.Range(0, 50); //if (1 <-> 2) enemy attack

            if (enemyDistWithPlayer <= howCloase && (enemyAttackOrNot >= 1 && enemyAttackOrNot <= 2) && gameManagerScript.getPlayerDeath() == false && attack == true && enemy)
            {
                agent.speed = 0; //nevMesh Spped 0

                StartCoroutine(EnemyAttack()); //Enemy Attack IEnumarator
            }
            else if (enemyDistWithPlayer <= howCloase && enemyAttackOrNot == 5 && gameManagerScript.getPlayerDeath() == false && attack == true && enemy) //If random value is 5 enemy become player friend
            {
                agent.speed = 0;
                anim.SetBool("isRun", false);
                anim.SetTrigger("isPraying");
                enemy = false;
                gameObject.tag = playerFriendTag; //Change enemy tag
                StartCoroutine(WaitForPray());
            }
            else if (enemyMove == true && anim.GetBool("isDeath") == false && enemy) //Enemy move code 
            {

                if (enemyDistWithPlayer > howCloase)
                {
                    anim.SetBool("isRun", true);
                    agent.speed = 1.5f; //nevMesh Spped 1.5
                    agent.SetDestination(player.transform.position);
                }
                else
                {
                    agent.speed = 0;
                    anim.SetBool("isRun", false);
                }
                /*
                enemyDistanceWithRandomPosition = Vector3.Distance(positions[randomPositionIndex].transform.position, transform.position); //Random Inxed generate base on this condition 
                if (enemyDistanceWithRandomPosition <= howCloase)
                {
                    randomPositionIndex = Random.Range(0, 54); //Enemy move random position index
                }
                */
            }
        }

        //-----------------------------------------------------------
        

        //Enemy Become Player friend code-----------------------------
        else
        {
            if (enemyDistWithPlayer <= howCloase)
            {
                anim.SetBool("isRun", false);
                agent.speed = 0;


                if (friendEnemyThrowBomb == true)
                {
                    friendEnemyThrowBomb = false;
                    anim.SetTrigger("isAttack");
                    StartCoroutine(WaitForThrowBomb());
                }

            }
            else if (followThePlayer == true && enemyDistWithPlayer > howCloase+0.5f) 
            {
                agent.speed = 2.8f;
                agent.SetDestination(player.transform.position);

                anim.SetBool("isRun", true);
                friendEnemyThrowBomb = true;

            }

        }
        //------------------------------------------------------------


        if(anim.GetBool("isDeath") == true) //After death at running tyme enemy move stop.
        {
            agent.speed = 0;
            enemyMove = false;
        }
        //------------------------------------------------------------
    }

    IEnumerator WaitForPray() //Wait for pray
    {
        yield return new WaitForSeconds(4f);
        enemyMaterial.GetComponent<Renderer>().material.color = playerColor; //Enemy Change The color
        yield return new WaitForSeconds(3f);
        followThePlayer = true;
    }
    IEnumerator WaitForThrowBomb() //wait for throw bomb
    {
        followThePlayer = false;
        yield return new WaitForSeconds(2f);
        followThePlayer = true;
    }



    IEnumerator EnemyAttack()
    {
        enemyMove = false; //Enemy move false

        attack = false; //Enemy Attack false

        anim.SetBool("isRun", false); //Enemy Run animation stop

        transform.LookAt(player); //Enemy look at the player

        anim.SetTrigger("isAttack");

        yield return new WaitForSeconds(5f); //Enemy wait 5 second after attack

        randomPositionIndex = Random.Range(0, 54); //Enemy move random position index
        enemyMove = true;

        yield return new WaitForSeconds(2f); //Enemy wait 1 second after attack

        attack = true;
    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(3f); //Enemy destroy after 3 seconds
        Destroy(this.gameObject);
    }


}//Class
