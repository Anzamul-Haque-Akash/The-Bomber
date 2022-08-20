using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody _rigidbody; //Player Rigidbody
    [SerializeField] private FloatingJoystick _joystick; //On Screen Joystick
    public Animator _animator; //Player Animatior

    [SerializeField] private float _moveSpeed; //Player Moving Speed

    GameObject hasEnemy;

    bool attack; //PLayer Attack Or not.


    [SerializeField] AudioSource BGAudio;
    [SerializeField] AudioSource DeathAudio;

    GameManager gameManagerScript; //Game Manager Script

    [SerializeField] GameObject enemyBomb;

    [SerializeField] GameObject vectoryPanel; //vevtory panel
    [SerializeField] GameObject gameOverPanel; //Game Over Panel

    [SerializeField] GameObject joyStick; //Floating joystick

    bool move;

    private void Awake() //Awake Function
    {
        attack = false;
    }

    private void Start() //Start Function
    {
        BGAudio.Play(); //back ground audio play

        gameManagerScript = GetComponent<GameManager>();

        gameManagerScript.setPlayerDeath(false); //Set player death false in GameManager Script

        move = true;
    }

    private void OnTriggerEnter(Collider other) //Player On trigger enter function
    {
        if (other.tag == "EnemyBomb") //Player Death
        {
            joyStick.SetActive(false); // remove joystick;

            Destroy(other.gameObject); //Destroy enemy thrown bomb

            enemyBomb.SetActive(true); //Enemt bomb blust in the player foot position

            gameManagerScript.setPlayerDeath(true); //Player death set int GameManager Script

            BGAudio.Stop(); //back ground audio stop
            DeathAudio.Play(); //Death Audio Play

            _animator.SetBool("isRun", false);

            _animator.SetBool("isDeath", true);

            gameOverPanel.SetActive(true); //Game Over panel

            StartCoroutine(StartMenuScene(3.5f));

        }

        if (other.tag == "EnemyBombArea") //Player Death
        {
            //Destroy(other.gameObject); //Destroy enemy thrown bomb

            joyStick.SetActive(false); // remove joystick;

            gameManagerScript.setPlayerDeath(true); //Player death set int GameManager Script

            BGAudio.Stop(); //back ground audio stop
            DeathAudio.Play(); //Death Audio Play

            _animator.SetBool("isRun", false);

            _animator.SetBool("isDeath", true);

            gameOverPanel.SetActive(true); //Game Over panel

            StartCoroutine(StartMenuScene(3.5f));

        }
    }

    private void Update()
    {
        hasEnemy = GameObject.FindWithTag("Enemy"); //Find the enemy for player win or not

        if(hasEnemy == null && _animator.GetBool("isDeath") == false && _animator.GetBool("isRun") == false) //Player vectory
        {
            _animator.SetBool("isDance", true);

            vectoryPanel.SetActive(true);

            StartCoroutine(StartMenuScene(4f));
        }
    }

    private void FixedUpdate() //Fixed Update Function
    {
        if (move == true)
        {
            _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _joystick.Vertical * _moveSpeed); //Player move with joystick

            if (_joystick.Horizontal != 0 || _joystick.Vertical != 0) //Player Rotation
            {
                transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);

                _animator.SetBool("isRun", true); //Player run animation start

                attack = true; //player can attack; 
            }
            else
            {
                _animator.SetBool("isRun", false); //Player run animation stop

                if (attack) //Player Attack when he stop
                {
                    _animator.SetTrigger("isThrow");

                    attack = false; //Player can not atack

                    joyStick.SetActive(false); // remove joystick;
                    StartCoroutine(Wait(1));
                }
            }
        }

    }

    void PlayerMove() //Player move bool value true and false
    {
        if(move == false)
        {
            move = true;
        }
        else
        {
            move = false;
        }
    }
    //-------------------------------------------------------

    IEnumerator StartMenuScene(float t) //Scene change tyme
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene("MainMenuScene");
    }

    IEnumerator Wait(float t) //Scene change tyme
    {
        yield return new WaitForSeconds(t);

        joyStick.SetActive(true); //active joystick after throw.
    }

}//Class
