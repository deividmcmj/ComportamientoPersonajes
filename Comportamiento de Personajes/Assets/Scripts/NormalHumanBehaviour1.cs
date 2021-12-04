using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NormalHumanBehaviour : MonoBehaviour
{
    //Manager
    [SerializeField] protected ApocalipsisManager apocalipsisManager;

    //Datos
    public UnityEngine.AI.NavMeshAgent agente;
    [SerializeField] protected int vida;
    [SerializeField] protected int NUM_VIDAS;

    //Capas
    public LayerMask mundo;
    public LayerMask jugadores;
    public LayerMask colinas;

    //Deambular
    protected Vector3 puntoDestino;
    protected bool existeDestino;
    protected const float rangoAndar = 8f;
    protected bool haLLegado;


    //Estado
    [SerializeField] protected GameObject estado;

    //Vida
    [SerializeField] protected GameObject[] vida_iconos;

    //Arbol de comportamiento
    protected BehaviourTreeEngine arbol = new BehaviourTreeEngine(false);


    private void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        //jugador = GameObject.FindGameObjectWithTag("HumanoBasico").GetComponent<Transform>();
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

        this.arbol = new BehaviourTreeEngine(false);
        LoopDecoratorNode nodoRaiz;
        SequenceNode secuenciaDeambular = arbol.CreateSequenceNode("secuenciaDeambular", false);

        secuenciaDeambular.AddChild(arbol.CreateLeafNode("Deambular", Deambular, haLlegadoADestino));

        nodoRaiz = arbol.CreateLoopNode("hastaMorir", secuenciaDeambular);

        arbol.SetRootNode(nodoRaiz);
    }

    // Update is called once per frame
    void Update()
    {
        arbol.Update();
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


    private void BuscarPuntoRandom()
    {

        //Calcula un punto random del mund dependiendo de un rango
        float randomZ = Random.Range(-rangoAndar, rangoAndar);
        float randomX = Random.Range(-rangoAndar, rangoAndar);
        puntoDestino = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //El raycast es necesario hacer la cooomprobacion para que no tome un
        //punto fuera del mundo. En el caso que se haga entonces no pasara al
        //siguiente paso, sino que en la siguiente iteracion del bucle vuelve
        //a calcular un nuevo punto de destino.
        if (Physics.Raycast(puntoDestino, -transform.up, 2f, mundo))
            existeDestino = true;
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
}
