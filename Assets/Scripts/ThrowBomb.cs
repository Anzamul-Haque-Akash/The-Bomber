using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBomb : MonoBehaviour
{
    [SerializeField] float bombSpeed;
    [SerializeField] GameObject bomb;
    [SerializeField] Transform bombPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void THROW() //Player Throw Bomb
    {
        GameObject bombSpawn = Instantiate(bomb, bombPos.position, bombPos.rotation);

        bombSpawn.GetComponent<Rigidbody>().velocity = bombPos.transform.up * bombSpeed;
    }

}
