using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicZombieBehaviour : MonoBehaviour
{
    private float speed;
    private int health;
    private int infectionTime;
    public Vector3 direction;
    public CharacterController player;
    // Start is called before the first frame update

    void Start()
    {
        speed = 5.0f;
        health = 1;
        infectionTime = 10;
        direction = newDirection();
        player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        wander();
    }

    public void wander()
    {
        if (Input.GetKeyDown("space"))
        {
            direction = newDirection();
        }

        player.Move(direction * speed * Time.deltaTime);
    }

    public void follow()
    {

    }

    public Vector3 newDirection()
    {
        Vector3 newVector = new Vector3(0, 0, 0);

        while (newVector.magnitude == 0) {
            newVector = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)).normalized;
        }

        return newVector;
    }

    public bool isAlive()
    {
        return health != 0;
    }

    public void attack()
    {
        //Iniciar la mision de ataque
        
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("BasicHuman"))
        {
            Debug.Log("Humano cerca");
            if (collision.gameObject.GetComponent<Transform>().position.magnitude <= 5)
            {
                Debug.Log("ALLÁ QUE VOY");
            }
        }else if (collision.gameObject.CompareTag("Hill"))
        {
            Debug.Log("Montaña");
        }
    }

    public int getInfectionTime()
    {
        return infectionTime;
    }

}

