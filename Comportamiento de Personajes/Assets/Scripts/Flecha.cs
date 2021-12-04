using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flecha : MonoBehaviour
{

    //Capas
    public LayerMask collisiones;

    private void Awake()
    {
        collisiones = (1 << 6) | (1 << 8) | (1 << 9);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        comprobarColision();
    }

    private void comprobarColision()
    {
        if (Physics.CheckSphere(this.transform.position, 1f, collisiones))
        {
            Destroy(this.gameObject);
        }
    }
}
