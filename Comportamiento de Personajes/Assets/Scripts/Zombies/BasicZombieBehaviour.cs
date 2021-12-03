
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BasicZombieBehaviour : GenericZombie
{
    //Ritual
    private Vector3 destinoRitual;
    private bool seDirigeARitual;

    //Perseguir
    private float proximidadColina;


    public BasicZombieBehaviour() : base(1, 10)
    {
        
    }
    

    void Start()
    {
     
        destinoRitual = GameObject.FindGameObjectWithTag("DestinoRitual").GetComponent<Transform>().position;

        NUM_VIDAS = 1;
        vida = 1;

        proximidadColina = 5f;
       

        //***CREACION DEL ARBOL DE COMPORTAMIENTO***;
        this.arbol = new BehaviourTreeEngine(false);

        //Primer nivel: NODO DECORADOR
        LoopDecoratorNode nodoRaiz;

        //Segundo nivel: NODO SECUENCIA,
        //Abuelo : nodo decorador
        //hijos:
        //      -hoja coprobar vida
        //      -nodo selector
        //Padre
        SequenceNode secuenciaVivo = arbol.CreateSequenceNode("secuenciaVivo", false);

        //Hijos
        secuenciaVivo.AddChild(arbol.CreateLeafNode("comprobarVida", estaVivo, tieneV));

        SelectorNode selectorDeambular = arbol.CreateSelectorNode("selectorDeambular");
        secuenciaVivo.AddChild(selectorDeambular);

        
        //Tercer nivel: NODO SELECTOR
        //Abuelo : nodo selector secuenciaVivo
        //hijos:
        //      -nodo secuencia
        //      -nodo hoja deambular
        SequenceNode secuenciaHumanoCerca = arbol.CreateSequenceNode("secuenciaHumanoCerca", false);
        selectorDeambular.AddChild(secuenciaHumanoCerca);

        selectorDeambular.AddChild(arbol.CreateLeafNode("Deambular", Deambular, haLlegadoADestino));
        
        //Cuarto nivel: NODO SECUENCIA
        //Abuelo : nodo selector secuenciaVivo
        //hijos:
        //      -nodo hoja hay humano cerca
        //      -nodo selector
        secuenciaHumanoCerca.AddChild(arbol.CreateLeafNode("ComprobarHumanoCerca", HayHumanoCercaA, HayHumanoCerca));
        SelectorNode selectorHumanoCerca = arbol.CreateSelectorNode("selectorHumanoCerca");
        secuenciaHumanoCerca.AddChild(selectorHumanoCerca);
        
        
        //Quinto nivel: NODO SELECTOR
        //Abuelo : nodo selector nivel 4
        //hijos:
        //      -nodo secuencia Humano muy cerca
        //      -nodo selector perseguir

        SequenceNode secuenciaHumanoMuyCerca = arbol.CreateSequenceNode("secuenciaHumanoMuyCerca", false);
        SelectorNode selectorPerseguir = arbol.CreateSelectorNode("selectoPerseguir");
        selectorHumanoCerca.AddChild(secuenciaHumanoMuyCerca);
        selectorHumanoCerca.AddChild(selectorPerseguir);

        
        // Sexto nivel: NODO SECUENCIA
        //Abuelo : nodo selector nivel nivel 5
        //hijos:
        //      -nodo hoja humano muy cerca
        //      -nodo hoja atacar

        secuenciaHumanoMuyCerca.AddChild(arbol.CreateLeafNode("ComprobarHumanoMuyCerca", HayHumanoMuyCercaA, HayHumanoMuyCerca));
        secuenciaHumanoMuyCerca.AddChild(arbol.CreateLeafNode("atacar", Atacar, HayHumanoCerca));

        // Sexto nivel: NODO SECUENCIA
        //Abuelo : nodo selector nivel nivel 5
        //hijos:
        //      -nodo secuencia ritual
        //      -nodo hoja perseguir
        SequenceNode secuenciaRitual = arbol.CreateSequenceNode("secuenciaRitual", false);
        selectorPerseguir.AddChild(secuenciaRitual);
        selectorPerseguir.AddChild(arbol.CreateLeafNode("perseguir", PerseguirHumano, HaAlacanzadoAHumano));
        
        // Septimo nivel: NODO SECUENCIA
        //Abuelo : nodo selector nivel nivel 5
        //hijos:
        //      -nodo hoja ha llegado a colina
        //      -nodo hoja hacer ritual

        secuenciaRitual.AddChild(arbol.CreateLeafNode("HaLLegadoAColina", HaLLegadoAColinaA, HayLLegadoAColina));
        secuenciaRitual.AddChild(arbol.CreateLeafNode("hacerRitual", HacerRitual ,haLlegadoARitual));
        
        //Asignar el nodo root
        nodoRaiz = arbol.CreateLoopNode("hastaMorir", secuenciaVivo);

        arbol.SetRootNode(nodoRaiz);

    }

    // Update is called once per frame
    void Update()
    {

        arbol.Update();
        
        
    }

    // *******ACCIONES*******
    //RITUAL
    //Dirigirse a la aglomeracion de zombies
    protected void irAAglomeracion()
    {
        agente.SetDestination(destinoRitual);
    }

    protected void HacerRitual()
    {
        seDirigeARitual = true;
        irAAglomeracion();
        Vector3 distanciaARitual = destinoRitual - this.transform.position;
        if (distanciaARitual.magnitude < 1f)
        {
            //Iniciar la animacion del ritual
        }

    }

    private void HaLLegadoAColinaA()
    {

    }

    protected bool HaLLegadoAColinaA2()
    {
        return Physics.CheckSphere(this.transform.position, proximidadColina, colinas);
    }


    // *******PERCEPCIONES*******
    private ReturnValues HayLLegadoAColina()
    {
        bool haLLegadoAColina = Physics.CheckSphere(this.transform.position, proximidadColina, colinas);
        //return (haLLegadoAColina) ? ReturnValues.Succeed : ReturnValues.Failed;
        if (haLLegadoAColina)
        {
            Debug.Log("Ha llegado a colina? SI");
            return ReturnValues.Succeed;
        }
        else
        {
            Debug.Log("Ha llegado a colina? NO");
            return ReturnValues.Failed;
        }
    }

    private ReturnValues haLlegadoARitual()
    {
        Vector3 distanciaARitual = destinoRitual - this.transform.position;
        if (distanciaARitual.magnitude > 1f)
        {
            Debug.Log("Ha llegado a ritual? NO DE MOMENTO");
            return ReturnValues.Running;
        }
        else
        {
            Debug.Log("Ha llegado a destino? SI, ESPERANDO");
            return ReturnValues.Running;
        }

    }

    void EliminarDelApocalipsis() {
        apocalipsisManager.RemoveZombieBasico(this.gameObject);
    }





}
