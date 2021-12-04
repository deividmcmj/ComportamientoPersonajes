using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ArmoredZombieBehaviour : GenericZombie
{

    [SerializeField]GameObject flecha;
    private const float TIEMPO_ENTRE_DISPAROS = 8;
    private bool haDisparadoYa;
    ArmoredZombieBehaviour() : base(3, 0) { }

    // Start is called before the first frame update
   


    void Start()
    {
        NUM_VIDAS = 3;
        vida = 3;
        tiempoInfeccion = 0;
       

        //***CREACION DEL ARBOL DE COMPORTAMIENTO***;
        arbol = new BehaviourTreeEngine(false);

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
        SequenceNode secuenciaHumanoCerca2 = arbol.CreateSequenceNode("secuenciaHumanoCerca2", false);
        secuenciaHumanoCerca.AddChild(secuenciaHumanoCerca2);

        secuenciaHumanoCerca2.AddChild(arbol.CreateLeafNode("Disparar", Disparar, haDisparado));
        SelectorNode selectorDisparar = arbol.CreateSelectorNode("selectorDisparar");
        secuenciaHumanoCerca2.AddChild(selectorDisparar);

        //Quinto nivel: NODO SELECTOR
        //Abuelo : nodo selector nivel 4
        //hijos:
        //      -nodo secuencia Humano muy cerca
        //      -nodo selector perseguir

        SequenceNode secuenciaHumanoMuyCerca = arbol.CreateSequenceNode("secuenciaHumanoMuyCerca", false);
        //SelectorNode selectorPerseguir = arbol.CreateSelectorNode("selectoPerseguir");
        selectorDisparar.AddChild(secuenciaHumanoMuyCerca);
        //selectorHumanoCerca.AddChild(selectorPerseguir);
        selectorDisparar.AddChild(arbol.CreateLeafNode("perseguir", PerseguirHumano, HaAlacanzadoAHumano));


        // Sexto nivel: NODO SECUENCIA
        //Abuelo : nodo selector nivel nivel 5
        //hijos:
        //      -nodo hoja humano muy cerca
        //      -nodo hoja atacar

        secuenciaHumanoMuyCerca.AddChild(arbol.CreateLeafNode("ComprobarHumanoMuyCerca", HayHumanoMuyCercaA, HayHumanoMuyCerca));
        secuenciaHumanoMuyCerca.AddChild(arbol.CreateLeafNode("atacar", Atacar, HayHumanoCerca));



        //Asignar el nodo root
        nodoRaiz = arbol.CreateLoopNode("hastaMorir", secuenciaVivo);

        arbol.SetRootNode(nodoRaiz);

    }

    // Update is called once per frame
    void Update()
    {

        arbol.Update();


    }

    private void Disparar()
    {
        if (!haDisparadoYa)
        {
            transform.LookAt(agenteHumano);
            Debug.Log("DISPARO");
            GameObject flechaObj = Instantiate(flecha, transform.position, Quaternion.identity);
            flechaObj.GetComponent<Rigidbody>().AddForce(transform.forward * 320f, ForceMode.Impulse);
            flechaObj.GetComponent<Rigidbody>().AddForce(transform.up * 8f, ForceMode.Impulse);
            haDisparadoYa = true;
        }
    }

    // *******PERCEPCIONES*******

    private ReturnValues haDisparado()
    {
        return ReturnValues.Succeed;
    }


    void EliminarDelApocalipsis()
    {
        apocalipsisManager.RemoveZombieArmado(this.gameObject);
    }
}

