using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalHumanBehaviour : MonoBehaviour
{
    private StateMachineEngine fsm;
    private State deambulandoState, humanoState, heridoState, infectandoState;
    private StateMachineEngine fsmHumano;
    private State huyendoState, escalandoState, armandoState, rindiendoState;
    private StateMachineEngine fsmHerido;
    private State suplicandoState, aproximandoState, curandoState;
    // Start is called before the first frame update
    void Start()
    {
        //Máquinas de estados
        fsm = new StateMachineEngine();
        fsmHumano = new StateMachineEngine(true);
        fsmHerido = new StateMachineEngine(true);

        //Estados
        deambulandoState = fsm.CreateEntryState("Deambulando", Deambular);

        huyendoState = fsmHumano.CreateEntryState("Huyendo", Huir);

        humanoState = fsm.CreateSubStateMachine("Humano", fsmHumano);

        escalandoState = fsmHumano.CreateState("Escalando", Escalar);
        armandoState = fsmHumano.CreateState("Armando", Armar);
        rindiendoState = fsmHumano.CreateState("Rindiendo", Rendirse);

        suplicandoState = fsmHerido.CreateEntryState("Suplicando", Suplicar);

        heridoState = fsm.CreateSubStateMachine("Herido", fsmHerido);

        aproximandoState = fsmHerido.CreateState("Aproximando", Aproximar);
        curandoState = fsmHerido.CreateState("Curando", Curar);

        infectandoState = fsm.CreateState("Infectando", Infectar);

        //Percepciones
        Perception zombiesCerca = fsm.CreatePerception<ValuePerception>();

        //Transiciones

    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();
        if (fsm.GetCurrentState() == humanoState)
        {
            fsmHumano.Update();
        }
        if (fsm.GetCurrentState() == heridoState)
        {
            fsmHerido.Update();
        }
    }

    private void Deambular()
    {
        
    }

    private void Huir()
    {

    }

    private void Escalar()
    {

    }

    private void Armar()
    {

    }

    private void Rendirse()
    {

    }

    private void Suplicar()
    {

    }

    private void Aproximar()
    {

    }

    private void Curar()
    {

    }

    private void Infectar()
    {

    }
}
