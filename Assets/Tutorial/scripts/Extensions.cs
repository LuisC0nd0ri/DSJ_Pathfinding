using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este código implementa el famoso algoritmo de barajado Fisher-Yates
//para mezclar una lista aleatoriamente. La función Shuffle puede
//ser llamada sobre cualquier lista que implemente IList<T>
public static class Extensions
{
    //'rng' crea una instancia de generador de números aleatorios usado
    //para mezclar listas.
    private static System.Random rng = new System.Random();

    /// <summary>
    ///La palabra clave this delante de IList<T> list indica que es un
    ///método de extensión, es decir, que puede llamado directamente en
    ///cualquier objeto que implemente IList<T>
    ///T es un tipo genérico, lo que permite que este método funcione
    ///con listas de cualquier tipo (IList<T>).
    ///Esto funciona porque List<T> implementa IList<T> 
    /// 
    /// IList<T> es una interfaz que define un conjunto de métodos 
    /// y propiedades que una lista debe tener.
    /// 
    /// puedes usarlo tanto en List<T> como en otras clases 
    /// que implementen esa interfaz(IList).
    /// 
    /// Ejemplo: List<int> miLista = new List<int>();
    /// miLista.Shuffle();
    /// </summary>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            //Genera un número aleatorio k entre 0 y n inclusive.
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value; 
        }
    }

}
