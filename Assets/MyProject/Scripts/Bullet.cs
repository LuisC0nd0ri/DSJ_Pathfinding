using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] private float speed = 10f;

    [Range(1, 10)]
    [SerializeField] private float lifeTime = 10f;

    [SerializeField] private AudioClip audioMuerteEnemigo;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //transform.position;
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rb.velocity = transform.up * speed; //en caso el obj mire a para el eje y+
        rb.velocity = transform.right * speed; //el obj esta mirando en right y no en up

    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.collider.CompareTag("Enemigo") && this.transform.CompareTag("Bala"))
        {
            print("Sumaste 1 punto");
            ControladorPuntaje.Instance.sumarPuntaje();
            ControladorSonido.Instance.EjecutarSonidoMuerteEnemigo(audioMuerteEnemigo); //sonido
            //destruye al enemigo
            Destroy(collision.gameObject);
        }
        //la colision al jugador esta en el script PlayerScript
        Destroy(gameObject);
    }
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo") && this.transform.CompareTag("Bala"))
        {
            print("Sumaste 1 punto");
            ControladorPuntaje.Instance.sumarPuntaje();
            ControladorSonido.Instance.EjecutarSonidoMuerteEnemigo(audioMuerteEnemigo); //sonido
            //destruye al enemigo
            Destroy(collision.gameObject);
        }
        //la colision al jugador esta en el script PlayerScript
        Destroy(gameObject);
    }
}
