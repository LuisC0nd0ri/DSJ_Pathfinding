using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorSonido : MonoBehaviour
{
    [SerializeField] AudioSource audioMuerteEnemigo;
    [SerializeField] AudioSource audioMuerteJugador;
    [SerializeField] AudioSource audioRecibirSuministros;

    public static ControladorSonido Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EjecutarSonidoMuerteEnemigo(AudioClip sonido)
    {
        audioMuerteEnemigo.PlayOneShot(sonido);
    }
    public void EjecutarSonidoMuerteJugador(AudioClip sonido)
    {
        audioMuerteJugador.PlayOneShot(sonido);
    }
    public void EjecutarSonidoRecibirSuminsitros(AudioClip sonido)
    {
        audioRecibirSuministros.PlayOneShot(sonido);
    }
}
