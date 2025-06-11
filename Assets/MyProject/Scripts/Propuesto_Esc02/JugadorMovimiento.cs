using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorMovimiento : MonoBehaviour
{
    [SerializeField] private float Speed = 5f;
    [SerializeField] private GameObject objPrefabBullet;

    [Header("Audios")]
    [SerializeField] private AudioClip audioMuerteJugador;
    [SerializeField] private AudioClip audioRecibirSuministros;

    private Vector3 posicionInicial;

    // Start is called before the first frame update
    void Start()
    {
        posicionInicial = transform.position;
    }
    // Update is called once per frame
    //se le agrego rotacion
    void Update()
    {
        Vector3 movimiento = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
            movimiento = Vector3.left;
        else if (Input.GetKey(KeyCode.RightArrow))
            movimiento = Vector3.right;
        else if (Input.GetKey(KeyCode.UpArrow))
            movimiento = Vector3.up;
        else if (Input.GetKey(KeyCode.DownArrow))
            movimiento = Vector3.down;

        // Movimiento
        transform.position += movimiento * Speed * Time.deltaTime;

        // Rotación solo si hay movimiento
        if (movimiento != Vector3.zero)
        {
            float angle = Mathf.Atan2(movimiento.y, movimiento.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            //Vector3 spawnPosition = transform.position + transform.up*2;
            Vector3 spawnPosition = transform.position + transform.right * 2.5f; //el obj esta mirando en right y no en up
            GameObject bullet = Instantiate(objPrefabBullet, spawnPosition, transform.rotation);

        }


    }

    //la puerta y ele enemigo no tiene el isTrigger del collider activado
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Puerta"))
        {
            print("Recibiste suministros de supervivencia...");
            ControladorSonido.Instance.EjecutarSonidoRecibirSuminsitros(audioRecibirSuministros); //sonido

        }

        if (collision.collider.CompareTag("Enemigo"))
        {
            print("Perdiste una vida");
            ControladorPuntaje.Instance.perderUnaVida();
            volverPosicionInicial();
            ControladorSonido.Instance.EjecutarSonidoMuerteJugador(audioMuerteJugador); //sonido

        }
    }
    //la bala funciona con trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BalaEnemigo"))
        {
            print("Perdiste una vida");
            ControladorPuntaje.Instance.perderUnaVida();
            volverPosicionInicial();
            ControladorSonido.Instance.EjecutarSonidoMuerteJugador(audioMuerteJugador); //sonido

        }
    }

    public void volverPosicionInicial()
    {
        transform.position = posicionInicial;
    }
}