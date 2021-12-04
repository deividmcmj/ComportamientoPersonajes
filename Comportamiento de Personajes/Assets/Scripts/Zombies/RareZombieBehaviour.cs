using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RareZombieBehaviour : GenericZombie

{

    public RareZombieBehaviour() : base(2, 5) { }
    
    void Start()
    {

        NUM_VIDAS = 2;
        vida = 2;
        tiempoInfeccion = 5;

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
        SelectorNode selectorHumanoCerca = arbol.CreateSelectorNode("selectorHumanoCerca");
        secuenciaHumanoCerca.AddChild(selectorHumanoCerca);


        //Quinto nivel: NODO SELECTOR
        //Abuelo : nodo selector nivel 4
        //hijos:
        //      -nodo secuencia Humano muy cerca
        //      -nodo selector perseguir

        SequenceNode secuenciaHumanoMuyCerca = arbol.CreateSequenceNode("secuenciaHumanoMuyCerca", false);
        selectorHumanoCerca.AddChild(secuenciaHumanoMuyCerca);
        selectorHumanoCerca.AddChild(arbol.CreateLeafNode("perseguir", PerseguirHumano, HaAlacanzadoAHumano));


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

    void EliminarDelApocalipsis()
    {
        apocalipsisManager.RemoveZombieEspecial(this.gameObject);
    }




}
