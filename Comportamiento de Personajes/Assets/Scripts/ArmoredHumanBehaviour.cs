using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredHumanBehaviour : MonoBehaviour
{
    private StateMachineEngine fsm;
    private State deambulandoState, humanoState, heridoState, infectandoState;
    private StateMachineEngine fsmHumano;
    private State preparandoState, disparandoState, curandoHeridoState, huyendoState, suicidandoState;
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

        preparandoState = fsmHumano.CreateEntryState("Preparando", Preparar);

        humanoState = fsm.CreateSubStateMachine("Humano", fsmHumano);

        disparandoState = fsmHumano.CreateState("Disparar", Disparar);
        curandoHeridoState = fsmHumano.CreateState("Curando Herido", CurarHerido);
        huyendoState = fsmHumano.CreateState("Huyendo", Huir);
        suicidandoState = fsmHumano.CreateState("Suicidando", Suicidarse);

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

    private void Preparar()
    {

    }

    private void Huir()
    {

    }

    private void Disparar()
    {

    }

    private void CurarHerido()
    {

    }

    private void Suicidarse()
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
