using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//clase utilizada para representar una posición en el mapa (coordenadas x y z).
public class MapLocation       
{
    public int x;
    public int z;

    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }

    public Vector2 ToVector()
    {
        return new Vector2(x, z);
    }

    //Sobrecarga del operador + para sumar dos MapLocation.
    public static MapLocation operator +(MapLocation a, MapLocation b)
       => new MapLocation(a.x + b.x, a.z + b.z);

    //Sobrescribe el método Equals para comparar si dos objetos MapLocation son iguales
    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;
        else
            return x == ((MapLocation)obj).x && z == ((MapLocation)obj).z;
    }

    public override int GetHashCode()
    {
        return 0;
    }

}

//clase para gestionar el laberinto
public class Maze : MonoBehaviour
{
    //directions es una lista de objetos MapLocation que representan las
    //cuatro direcciones básicas (arriba, abajo, izquierda, derecha).
    public List<MapLocation> directions = new List<MapLocation>() {
                                            new MapLocation(1,0),
                                            new MapLocation(0,1),
                                            new MapLocation(-1,0),
                                            new MapLocation(0,-1) };
    //tamanio mapa
    public int width = 30; //x length
    public int depth = 30; //z length
    //Matriz bidimensional de bytes que representa el mapa (muros y pasajes).
    //La usarás para marcar las paredes (1) y los pasajes (0).
    public byte[,] map;
    public int scale = 6;

    // Start is called before the first frame update
    void Start()
    {
        InitialiseMap();
        Generate();
        DrawMap();
    }

    //Crea la matriz map del tamaño definido. La llena con 1 (muros),
    //creando inicialmente un área cerrada.
    void InitialiseMap()
    {
        map = new byte[width,depth];
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                    map[x, z] = 1;     //1 = wall  0 = corridor
            }
    }

    //Método virtual que llena aleatoriamente la matriz con pasajes o muros.
    //Puede ser sobrescrito en clases derivadas.
    public virtual void Generate()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
               if(Random.Range(0,100) < 50)
                 map[x, z] = 0;     //1 = wall  0 = corridor
            }
    }

    //Recorre toda la matriz map.
    //Cuando encuentra un 1 (muro), crea un cubo en Unity en la posición
    //correspondiente multiplicada por la escala para posicionar
    //correctamente en el espacio 3D.
    void DrawMap()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 1)
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(scale, scale, scale);
                    wall.transform.position = pos;
                }
            }
    }

    //Cuenta cuántos vecinos cardinales (arriba, abajo, izquierda, derecha)
    //adyacentes a la posición (x, z) están abiertos (0).
    //Si la posición está en los bordes, devuelve 5 para indicar que no se
    //deben procesar esas posiciones, o que están en los límites.
    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z] == 0) count++;
        if (map[x + 1, z] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        if (map[x, z - 1] == 0) count++;
        return count;
    }

    //Cuenta las celdas diagonales(esquinas) adyacentes en las diagonales
    //(arriba-izquierda, abajo-derecha, arriba-derecha, abajo-izquierda)
    //que están abiertas(0).
    public int CountDiagonalNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z - 1] == 0) count++;
        if (map[x + 1, z + 1] == 0) count++;
        if (map[x - 1, z + 1] == 0) count++;
        if (map[x + 1, z - 1] == 0) count++;
        return count;
    }

    //Suma las conexiones ortogonales y diagonales, retornando el total de
    //vecinos abiertos en todas las direcciones alrededor de (x, z).
    public int CountAllNeighbours(int x, int z)
    {
        return CountSquareNeighbours(x,z) + CountDiagonalNeighbours(x,z);
    }
}
