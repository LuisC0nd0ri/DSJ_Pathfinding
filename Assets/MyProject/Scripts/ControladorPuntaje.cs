using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControladorPuntaje : MonoBehaviour
{
    public static ControladorPuntaje Instance;

    [Header("Puntaje")]
    [SerializeField] private TextMeshProUGUI puntaje;
    private int contadorEnemigosMatados;
    [Header("Enemigos muertos para ganar la partida")]
    [SerializeField] private int numeroEnemigos = 5;
    [SerializeField] private GameObject panelGanar;

    [Header("Vida")]
    [SerializeField] private TextMeshProUGUI textvidaactual;
    private int vidaActual;
    [SerializeField] private int totalVidas = 3;
    [SerializeField] private GameObject panelPerder;

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
        puntaje.text = "0/" + numeroEnemigos.ToString();
        contadorEnemigosMatados = 0;

        vidaActual = totalVidas;
        textvidaactual.text = totalVidas.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void sumarPuntaje()
    {
        contadorEnemigosMatados++;
        puntaje.text = contadorEnemigosMatados.ToString() + "/" + numeroEnemigos.ToString();
        if(contadorEnemigosMatados >= numeroEnemigos)
        {
            panelGanar.SetActive(true);
            Time.timeScale = 0.0f; //parar el juego. El audio y UI funcionan
        }
    }

    public void perderUnaVida()
    {
        this.vidaActual -= 1;
        textvidaactual.text = vidaActual.ToString();
        if (vidaActual <= 0)
        {
            panelPerder.SetActive(true);
            Time.timeScale = 0.0f; //parar el juego. El audio y UI funcionan
        }
    }
}
