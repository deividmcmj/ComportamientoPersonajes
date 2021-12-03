using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ApocalipsisManager : MonoBehaviour
{

    //***LAYER MASK***
    [SerializeField] private LayerMask zombieMask;
    [SerializeField] private LayerMask humanoMask;
    [SerializeField] private LayerMask armaMask;


    //***AGENTES SPAWNEABLES***
    [SerializeField] private GameObject zombieBasico;//Zombie Basico
    [SerializeField] private GameObject zombieEspecial;//Zombie Especial
    [SerializeField] private GameObject zombieArmado;//Zombie Armado
    [SerializeField] private GameObject humanoBasico;//Humano Basico
    [SerializeField] private GameObject humanoArmado;//Humano Armado
    [SerializeField] private GameObject arma;//Arma

    //***MAXIMO OBJETOS***
    [SerializeField] private const int MAX_ZOMBIES = 4; //Maximo de zombies quue puede haber en el mapa simultaneamente
    [SerializeField] private const int MAX_HUMANOS = 4; //Maximo de humanos que pueden haber en el mapa simultaneamente
    [SerializeField] private const int MAX_ARMAS = 2;//Maximo de armas que pueden haber en el mapa simultaneamente

    //***TIEMPOS DE SPAWN***
    [SerializeField] private const float TIEMPO_ENTRE_SPAWN_ZOMBIE = 2f;
    [SerializeField] private const float TIEMPO_ENTRE_SPAWN_HUMANO = 2f;
    [SerializeField] private const float TIEMPO_ENTRE_SPAWN_ARMA = 4f;

    [SerializeField] private float TIEMPO_SPAWN_ULTIMA_ARMA;
    [SerializeField] private float TIEMPO_SPAWN_ULTIMO_ZOMBIE;
    [SerializeField] private float TIEMPO_SPAWN_ULTIMO_HUMANO;

    //***PUNTOS DE SPAWN***
    [SerializeField] List<Transform> zombiePuntosSpawn = new List<Transform>();//Spawn de zombies
    [SerializeField] List<Transform> huamanoPuntosSpawn = new List<Transform>();//Spawn de humanos
    [SerializeField] List<Transform> armaPuntosSpawn = new List<Transform>();//Armas

    //***ARRAYS DE OBJETOS***
    [SerializeField] List<GameObject> zombieBasico_lista = new List<GameObject>();//Array de zombies basicos
    [SerializeField] List<GameObject> zombieEspecial_lista = new List<GameObject>();//Array de zombies especiales
    [SerializeField] List<GameObject> zombieArmado_lista = new List<GameObject>();//Array de zombies armados
    [SerializeField] List<GameObject> humanoBasico_lista = new List<GameObject>();//Array de  humanos basicos
    [SerializeField] List<GameObject> humanoArmado_lista = new List<GameObject>();//Array de  humanos armados
    [SerializeField] List<GameObject> arma_lista = new List<GameObject>();//Array de  armas


    private void Awake()
    {
        zombieMask = 1 << 7;
        humanoMask = 1 << 8;
        armaMask = 1 << 10;
    }
    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("ControlSpawnZombies",0,  TIEMPO_ENTRE_SPAWN_ZOMBIE);
        InvokeRepeating("ControlSpawnArmas", 0,  TIEMPO_ENTRE_SPAWN_ARMA);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //***CONTROL DE ZOMBIES***

    void ControlSpawnZombies()
    {
        //Controla el numero de zombies que ya hay en la escena
        //Si hay menos entonces tiene que comporbar que zombies
        //pueden spawnear.
        if (zombieBasico_lista.Count +zombieEspecial_lista.Count + zombieArmado_lista.Count < MAX_ZOMBIES)
        {
            if (zombieArmado_lista.Count == 0)
            {
                SpawnZombieArmado();
                return;
            }

            if  (zombieEspecial_lista.Count == 0)
            {
                SpawnZombieEspecial();
                return;
            }

                SpawnZombieBasico();
                return;
            
        }
    }

    void ControlSpawnHumano()
    {
        //Controla el numero de humanos que ya hay en la escena
        //Si hay menos entonces tiene que comporbar que zombies
        //pueden spawnear.
       
    }

    void ControlSpawnArmas()
    {
        //Controla el numero de zombies que ya hay en la escena
        //Si hay menos entonces tiene que comporbar que zombies
        //pueden spawnear.
        if (arma_lista.Count <= MAX_ARMAS)
        {

            SpawnArma();

        }
    }


    //***SPAWNEOS***
    void SpawnZombieBasico()
    {
        int puntoRandom = Random.Range(0, zombiePuntosSpawn.Count);
        bool hayEspacio = Physics.CheckSphere(zombiePuntosSpawn[puntoRandom].position, 0.5f, zombieMask);
        while (hayEspacio) puntoRandom = Random.Range(0, zombiePuntosSpawn.Count);
        GameObject zombie =  Instantiate(zombieBasico, zombiePuntosSpawn[puntoRandom].position, Quaternion.identity);
        zombieBasico_lista.Add(zombie);
    }


    void SpawnZombieEspecial(){
        int puntoRandom = Random.Range(0, zombiePuntosSpawn.Count);
        bool hayEspacio = Physics.CheckSphere(zombiePuntosSpawn[puntoRandom].position, 0.5f, zombieMask);
        while (hayEspacio) puntoRandom = Random.Range(0, zombiePuntosSpawn.Count);
        GameObject zombie = Instantiate(zombieEspecial, zombiePuntosSpawn[puntoRandom].position, Quaternion.identity);
        zombieEspecial_lista.Add(zombie);
    }

    void SpawnZombieArmado()
    {
        int puntoRandom = Random.Range(0, zombiePuntosSpawn.Count);
        bool hayEspacio = Physics.CheckSphere(zombiePuntosSpawn[puntoRandom].position, 0.5f, zombieMask);
        while (hayEspacio) puntoRandom = Random.Range(0, zombiePuntosSpawn.Count);
        GameObject zombie = Instantiate(zombieArmado, zombiePuntosSpawn[puntoRandom].position, Quaternion.identity);
        zombieArmado_lista.Add(zombie);
    }

    void SpawnZombieArmado(Transform ritual)
    {
        GameObject zombie = Instantiate(zombieArmado, ritual.position, Quaternion.identity);
        zombieArmado_lista.Add(zombie);
    }


    void SpawnHumanoBasico()
    {
        int puntoRandom = Random.Range(0, huamanoPuntosSpawn.Count);
        GameObject humano = Instantiate(humanoBasico, zombiePuntosSpawn[puntoRandom].position, Quaternion.identity);
        humanoBasico_lista.Add(humano);
    }
    void SpawnHumanoArmado()
    {
        int puntoRandom = Random.Range(0, huamanoPuntosSpawn.Count);
        GameObject humano = Instantiate(humanoArmado, zombiePuntosSpawn[puntoRandom].position, Quaternion.identity);
        humanoArmado_lista.Add(humano);
    }

    void SpawnArma()
    {
        int puntoRandom = Random.Range(0, armaPuntosSpawn.Count);
        GameObject armaObj = Instantiate(arma, armaPuntosSpawn[puntoRandom].position, Quaternion.identity);
        arma_lista.Add(armaObj);
    }


    //***ELIMINAR DE LAS LISTAS***
    public void RemoveZombieBasico(GameObject zombieBasico)
    {
        zombieBasico_lista.Remove(zombieBasico);
    }

    public void RemoveZombieEspecial(GameObject zombieEspecial)
    {
        zombieEspecial_lista.Remove(zombieEspecial);
    }

    public void RemoveZombieArmado(GameObject zombieArmado)
    {
        zombieArmado_lista.Remove(zombieArmado);
    }

    public void RemoveHumanoBasico(GameObject humanoBasico)
    {
        humanoBasico_lista.Remove(humanoBasico);
    }

    public void RemoveHumanoArmado(GameObject humanoArmado)
    {
        humanoArmado_lista.Remove(humanoArmado);
    }

    public void RemoveArma(GameObject arma)
    {
        arma_lista.Remove(arma);
    }


}
