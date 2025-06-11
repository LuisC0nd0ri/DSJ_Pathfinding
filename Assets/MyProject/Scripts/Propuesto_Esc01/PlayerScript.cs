using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class PlayerScript : MonoBehaviour
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
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * Speed * Time.deltaTime;

            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * Speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * Speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * Speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, 270f);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            //Vector3 spawnPosition = transform.position + transform.up*2;
            Vector3 spawnPosition = transform.position + transform.right*2.5f; //el obj esta mirando en right y no en up
            GameObject bullet = Instantiate(objPrefabBullet, spawnPosition, transform.rotation);

        }


    }

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

    public void volverPosicionInicial()
    {
        transform.position = posicionInicial;
    }
}