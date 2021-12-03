
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System;


public abstract class GenericZombie : MonoBehaviour
{
    //Manager
    [SerializeField] protected ApocalipsisManager apocalipsisManager;

    //Datos
    public NavMeshAgent agente;
    public Transform agenteHumano;
    [SerializeField] protected int vida;
    [SerializeField] protected int NUM_VIDAS;
    [SerializeField] protected float tiempoInfeccion;

    //Capas
    public LayerMask mundo;
    public LayerMask jugadores;
    public LayerMask colinas;

    //Deambular
    protected Vector3 puntoDestino;
    protected bool existeDestino;
    protected const float rangoAndar = 8f;
    protected bool haLLegado;


    //Perseguir
    protected const float proximidadHumano = 30f;
    protected const float proximidadCercanaHumano = 5f;
    

    //Estado
    [SerializeField] protected GameObject estado;

    //Vida
    [SerializeField] protected GameObject[] vida_iconos;

    //Arbol de comportamiento
    protected BehaviourTreeEngine arbol = new BehaviourTreeEngine(false);

    public GenericZombie( int NUM_VIDAS, int tiempoInfeccion){
        
        this.NUM_VIDAS = NUM_VIDAS;
        this.tiempoInfeccion = tiempoInfeccion;

    }

    private void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        apocalipsisManager = GameObject.FindGameObjectWithTag("ApocalipsisManager").GetComponent<ApocalipsisManager>();
        mundo = 1 << 6;
        jugadores = 1 << 8;
        colinas = 1 << 9;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        vida = NUM_VIDAS;
        vida_iconos = new GameObject[NUM_VIDAS];

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // *******ACCIONES*******
    //DEAMBULAR
    private void BuscarPuntoRandom()
    {

        //Calcula un punto random del mund dependiendo de un rango
        float randomZ = UnityEngine.Random.Range(-rangoAndar, rangoAndar);
        float randomX = UnityEngine.Random.Range(-rangoAndar, rangoAndar);
        puntoDestino = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //El raycast es necesario hacer la cooomprobacion para que no tome un
        //punto fuera del mundo. En el caso que se haga entonces no pasara al
        //siguiente paso, sino que en la siguiente iteracion del bucle vuelve
        //a calcular un nuevo punto de destino.
        if (Physics.Raycast(puntoDestino, -transform.up, 2f, mundo))
            existeDestino = true;
    }

    protected void Deambular()
    {
        if (!existeDestino) BuscarPuntoRandom();

        if (existeDestino)
        {
            agente.SetDestination(puntoDestino);
            estado.GetComponent<Image>().color = new Color(0.3470763f, 0.3470763f, 0.3470763f);
        }

        Vector3 distanciaADestino = puntoDestino - this.transform.position;

        if (distanciaADestino.magnitude < 1f) existeDestino = false;
    }


    //SEGUIR
    //Dirige el zombie hasta el huamano, para que aparente que lo persigue
    protected void PerseguirHumano()
    {

        agente.SetDestination(agenteHumano.position);
    }


    


    //ATACAR
    protected void Atacar()
    {
        //Iniciar la animacion de atacar
        Debug.Log("Ha atacado");
    }


    protected void HayHumanoCercaA()
    {
        Collider[] collisiones = Physics.OverlapSphere(this.transform.position, 2f);
        List<NormalHumanBehaviour> array_humanos = new List<NormalHumanBehaviour>();

        foreach (var hitCollider in collisiones)
        {
            if (hitCollider.GetComponent<NormalHumanBehaviour>() != null
                && hitCollider.CompareTag("HumanoBasico") && hitCollider != null)
            {
                if (!array_humanos.Equals(hitCollider.GetComponent<NormalHumanBehaviour>()))
                    array_humanos.Add(hitCollider.GetComponent<NormalHumanBehaviour>());
            }

        }
        

            foreach (var humano in array_humanos)
            {
            float distanciaHumanoCercano = (this.transform.position - agenteHumano.position).magnitude;
            float nuevaDistanciaHumanoCercano = (this.transform.position - humano.transform.position).magnitude;

            if (nuevaDistanciaHumanoCercano < distanciaHumanoCercano) agenteHumano = humano.transform;
            }

        
    }

    protected void HayHumanoMuyCercaA()
    {

    }

   

    // *******PERCEPCIONES*******
    protected ReturnValues HayHumanoCerca()
    {
        bool hayHumanoCerca = Physics.CheckSphere(this.transform.position, proximidadHumano, jugadores);
        //return (hayHumanoCerca) ? ReturnValues.Succeed : ReturnValues.Failed;
        if (hayHumanoCerca)
        {
            Debug.Log("Hay Humano Cerca? SI");
            return ReturnValues.Succeed;
        }
        else
        {
            Debug.Log("Hay Humano Cerca? NO");
            return ReturnValues.Failed;
        }
    }

    protected ReturnValues HayHumanoMuyCerca()
    {
        bool hayHumanoMuyCerca = Physics.CheckSphere(this.transform.position, proximidadCercanaHumano, jugadores);
        //return (hayHumanoMuyCerca) ? ReturnValues.Succeed : ReturnValues.Failed;
        if (hayHumanoMuyCerca)
        {
            Debug.Log("Hay Humano MUY Cerca? SI");
            return ReturnValues.Succeed;
        }
        else
        {
            Debug.Log("Hay Humano  MUY Cerca? NO");
            return ReturnValues.Failed;
        }
    }

    


    protected void estaVivo()
    {
    }

    protected ReturnValues tieneV()
    {
        if (vida != 0)
        {
            return ReturnValues.Succeed;
        }
        else
        {
            return ReturnValues.Failed;
        }
    }


    protected bool tieneVida()
    {
        return vida != 0;
    }


    protected ReturnValues haLlegadoADestino()
    {
        bool existeDestino = Physics.Raycast(puntoDestino, -transform.up, 2f, mundo);
        Vector3 distanciaADestino = puntoDestino - this.transform.position;
        if (!existeDestino)
        {

            Debug.Log("Ha llegado a destino? NO EXISTE");
            return ReturnValues.Failed;
        }
        else if (existeDestino && distanciaADestino.magnitude > 1f)
        {
            Debug.Log("Ha llegado a destino? NO de momento");
            return ReturnValues.Running;
        }
        else
        {
            Debug.Log("Ha llegado a destino? SI");

            return ReturnValues.Succeed;
        }
    }

    


    protected ReturnValues haAtacado()
    {
        return ReturnValues.Succeed;
    }

    protected ReturnValues HaAlacanzadoAHumano()
    {
        float distanciaHumano = (agenteHumano.position - this.transform.position).magnitude;
        /*if (distanciaHumano > 1f && !HaLLegadoAColinaA2())
        {
            Debug.Log("Ha alcanzado a humano? NO DE MOMENTO");
            return ReturnValues.;
        }
        else if (distanciaHumano > 1f && HaLLegadoAColinaA2())
        {
            return ReturnValues.Failed;
            Debug.Log("Ha alcanzado a humano? NO, COLINA");

        }
        else
        {
            Debug.Log("Ha alcanzado a humano? SI");
            return ReturnValues.Succeed;
        }*/
        return ReturnValues.Succeed;
    }



    //****COSAS****
    protected void RecibeDaño()
    {
        vida_iconos[vida - 1].SetActive(false);
        vida--;

        Debug.Log("AAAAAY");
        Morir();
    }


    protected void Morir()
    {
        if (vida == 0)
        {
            EliminarDelApocalipsis();
            Destroy(this.gameObject, 2);
        }
    }

    protected virtual void EliminarDelApocalipsis() { }
}
