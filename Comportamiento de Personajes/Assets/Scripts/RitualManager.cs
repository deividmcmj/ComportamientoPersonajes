
using System.Collections.Generic;
using UnityEngine;

public class RitualManager : MonoBehaviour
{

    [SerializeField]private ApocalipsisManager apocalipsisManager;
    private const int NUM_ZOMBIES_PARA_RITUAL = 4;

    private void Awake()
    {
        apocalipsisManager = GameObject.FindGameObjectWithTag("ApocalipsisManager").GetComponent<ApocalipsisManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        ComprobarArea();
    }

    private void ComprobarArea()
    {
        Collider[] collisiones = Physics.OverlapSphere(this.transform.position, 2f);
        List <BasicZombieBehaviour2>array_zombiesRitual = new List<BasicZombieBehaviour2>();
        int numZombies = 0;

        foreach (var hitCollider in collisiones)
        {
            if (hitCollider.GetComponent<GenericZombie>() != null && numZombies < NUM_ZOMBIES_PARA_RITUAL
                && hitCollider.CompareTag("ZombieBasico") && hitCollider != null)
            {
                if (!array_zombiesRitual.Equals(hitCollider.GetComponent<BasicZombieBehaviour2>())) 
                array_zombiesRitual.Add(hitCollider.GetComponent<BasicZombieBehaviour2>());
            }

            if (array_zombiesRitual.Count >= NUM_ZOMBIES_PARA_RITUAL)
            {
                break;
            }
            //hitCollider.SendMessage("recibeDaño");

        }
        if (array_zombiesRitual.Count == NUM_ZOMBIES_PARA_RITUAL)
        {
            
            foreach (var zombies in array_zombiesRitual)
            {
                apocalipsisManager.RemoveZombieBasico(zombies.gameObject);
                Destroy(zombies.gameObject, 1);
            }
         
        }
    }
}
