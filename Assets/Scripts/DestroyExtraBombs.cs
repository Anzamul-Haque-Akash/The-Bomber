using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExtraBombs : MonoBehaviour
{

    [SerializeField] GameObject blastEffect;

    [SerializeField] GameObject bombArea;

    [SerializeField] AudioSource blust;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DESTROY());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DESTROY()
    {
        yield return new WaitForSeconds(1.6f); //Extra bomb destroy after 1 seconds.

        bombArea.SetActive(true); //Bomb are active

        blastEffect.SetActive(true);
        blust.Play(); //Blust Sound Play

        yield return new WaitForSeconds(1f); //Extra bomb destroy after 1 seconds.

        Destroy(this.gameObject);
    }
}//Class
