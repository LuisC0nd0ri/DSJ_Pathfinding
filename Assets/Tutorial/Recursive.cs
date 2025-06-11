using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recursive : Maze
{
    public override void Generate()
    {
        Generate(5, 5);
    }

    //Esto inicia la generación desde la posición (x,z).
    void Generate(int x, int z)
    {
        //Si en la posición actual hay 2 o más vecinos abiertos,
        //la función termina en esa rama, evitando crear caminos
        //demasiado conectados o creando ciclos grandes. Esto ayuda a mantener
        //el laberinto con corredores y paredes.
        if (CountSquareNeighbours(x, z) >= 2) return;
        //Marca la posición (x, z) como paso libre o corredor
        map[x, z] = 0;

        //Mezcla aleatoriamente la lista directions
        //Esta definido en clase padre
        directions.Shuffle();

        //Llama recursivamente a Generate() en las cuatro direcciones 
        Generate(x + directions[0].x, z + directions[0].z);
        Generate(x + directions[1].x, z + directions[1].z);
        Generate(x + directions[2].x, z + directions[2].z);
        Generate(x + directions[3].x, z + directions[3].z);
    }

}
