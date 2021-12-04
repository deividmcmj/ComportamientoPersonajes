using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicZombieBehaviour : MonoBehaviour
{
    private float velocidad;
    private int vida;
    private int tiempoInfeccion;
    public Vector3 direccion;
    public CharacterController jugador;
 
    // Start is called before the first frame update

    void Start()
    {
        velocidad = 5.0f;
        vida = 1;
        tiempoInfeccion = 10;
        direccion = nuevaDireccion();
        jugador = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Deambular();
    }

    public void Deambular()
    {
        if (Input.GetKeyDown("space"))
        {
            direccion = nuevaDireccion();
        }

        jugador.Move(direccion * velocidad * Time.deltaTime);
    }

    public void Seguir()
    {

    }

    public Vector3 nuevaDireccion()
    {
        Vector3 newVector = new Vector3(0, 0, 0);

        while (newVector.magnitude == 0) {
            newVector = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)).normalized;
        }

        return newVector;
    }

    public bool estaVivo()
    {
        return vida != 0;
    }

    public void Atacar()
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

    public int getTiempoInfeccion()
    {
        return tiempoInfeccion;
    }

}

