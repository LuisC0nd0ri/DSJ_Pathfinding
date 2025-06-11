using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Define una clase llamada PathMarker que representa un nodo en el camino de búsqueda
public class PathMarker {

    public MapLocation location; //poiscion nodo mapa
    //G: Coste acumulado desde el inicio hasta este nodo.
    //H: Heurística estimada desde este nodo hasta el destino.
    //F: Coste total (F = G + H).
    public float G, H, F;
    //El objeto visual en Unity que representa este nodo.
    public GameObject marker;
    //El nodo anterior desde donde se llegó a este, para reconstruir el camino.
    public PathMarker parent;

    //Constructor
    public PathMarker(MapLocation l, float g, float h, float f, GameObject m, PathMarker p) {

        location = l;
        G = g;
        H = h;
        F = f;
        marker = m;
        parent = p;
    }

    //Sobrescribe el método Equals para que dos PathMarker
    //sean iguales si sus location son iguales.
    public override bool Equals(object obj) {

        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;
        else
            return location.Equals(((PathMarker)obj).location);
    }

    public override int GetHashCode() {

        return 0;
    }
}

//Clase que controla la búsqueda de camino usando A* en Unity.
public class FindPathAStar : MonoBehaviour {

    //Estructura del laberinto
    public Maze maze;
    //Materiales para visualizar nodos cerrados y abiertos.
    public Material closedMaterial;
    public Material openMaterial;
    //Objetos para los nodos
    public GameObject start;
    public GameObject end;
    public GameObject pathP;

    //Variables para gestionar los nodos y el estado de búsqueda.
    PathMarker startNode;
    PathMarker goalNode;
    PathMarker lastPos;
    bool done = false;
    bool hasStarted = false;

    //Listas abiertas y cerradas típicas en A*.
    List<PathMarker> open = new List<PathMarker>();
    List<PathMarker> closed = new List<PathMarker>();

    //Elimina todos los objetos marcadores en la escena.
    void RemoveAllMarkers() {

        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");

        foreach (GameObject m in markers) Destroy(m);
    }

    //resetea estados de busqueda
    //Recolecta todas las posiciones válidas del mapa(menos muros)
    //seleccionar aleatoriamente los puntos de inicio y fin.
    void BeginSearch() {

        done = false;
        RemoveAllMarkers();

        List<MapLocation> locations = new List<MapLocation>();

        for (int z = 1; z < maze.depth - 1; ++z) {
            for (int x = 1; x < maze.width - 1; ++x) {

                if (maze.map[x, z] != 1) {

                    locations.Add(new MapLocation(x, z));
                }
            }
        }
        locations.Shuffle();

        Vector3 startLocation = new Vector3(locations[0].x * maze.scale, 0.0f, locations[0].z * maze.scale);
        startNode = new PathMarker(new MapLocation(locations[0].x, locations[0].z),
            0.0f, 0.0f, 0.0f, Instantiate(start, startLocation, Quaternion.identity), null);

        Vector3 endLocation = new Vector3(locations[1].x * maze.scale, 0.0f, locations[1].z * maze.scale);
        goalNode = new PathMarker(new MapLocation(locations[1].x, locations[1].z),
            0.0f, 0.0f, 0.0f, Instantiate(end, endLocation, Quaternion.identity), null);

        open.Clear();
        closed.Clear();

        open.Add(startNode);
        lastPos = startNode;
    }

    //realiza un paso de la búsqueda A* en un nodo dado:
    void Search(PathMarker thisNode) {

        //Si llegamos al nodo destino, terminamos.
        if (thisNode.Equals(goalNode)) {

            done = true;
            // Debug.Log("DONE!");
            return;
        }

        //Itera sobre las posibles direcciones (arriba, abajo, izquierda, derecha)
        foreach (MapLocation dir in maze.directions) {

            MapLocation neighbour = dir + thisNode.location;

            //Salta si la posición es muro, fuera del mapa o ya está en la lista cerrada.
            if (maze.map[neighbour.x, neighbour.z] == 1) continue;
            if (neighbour.x < 1 || neighbour.x >= maze.width || neighbour.z < 1 || neighbour.z >= maze.depth) continue;
            if (IsClosed(neighbour)) continue;

            float g = Vector2.Distance(thisNode.location.ToVector(), neighbour.ToVector()) + thisNode.G;
            float h = Vector2.Distance(neighbour.ToVector(), goalNode.location.ToVector());
            float f = g + h;

            //Luego crea visualización y actualiza o añade el nodo a la lista
            GameObject pathBlock = Instantiate(pathP, new Vector3(neighbour.x * maze.scale, 0.0f, neighbour.z * maze.scale), Quaternion.identity);

            TextMesh[] values = pathBlock.GetComponentsInChildren<TextMesh>();

            values[0].text = "G: " + g.ToString("0.00");
            values[1].text = "H: " + h.ToString("0.00");
            values[2].text = "F: " + f.ToString("0.00");

            if (!UpdateMarker(neighbour, g, h, f, thisNode)) {

                open.Add(new PathMarker(neighbour, g, h, f, pathBlock, thisNode));
            }
        }

        //Selecciona el nodo con menor F, lo mueve a cerrados, y lo marca visualmente.
        open = open.OrderBy(p => p.F).ToList<PathMarker>();
        PathMarker pm = (PathMarker)open.ElementAt(0);
        closed.Add(pm);

        open.RemoveAt(0);
        pm.marker.GetComponent<Renderer>().material = closedMaterial;

        lastPos = pm;
    }

    //Actualiza un nodo en la lista abierta si ya existe y devuelve true.
    //Si no, devuelve false.
    bool UpdateMarker(MapLocation pos, float g, float h, float f, PathMarker prt) {

        foreach (PathMarker p in open) {

            if (p.location.Equals(pos)) {

                p.G = g;
                p.H = h;
                p.F = f;
                p.parent = prt;
                return true;
            }
        }
        return false;
    }

    //Verifica si una posición está en la lista cerrada.
    bool IsClosed(MapLocation marker) {

        foreach (PathMarker p in closed) {

            if (p.location.Equals(marker)) return true;
        }
        return false;
    }

    void Start() {

    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.P)) {
            //inicia la búsqueda
            BeginSearch();
            hasStarted = true;
        }

        //realiza un paso del algoritmo en el nodo actual.
        if (hasStarted)
            if (Input.GetKeyDown(KeyCode.C)) 
                Search(lastPos);

        if (Input.GetKeyDown(KeyCode.M))
            GetPath();
    }

    //para dar el camino escogido desde el punto de inicio al punto final
    void GetPath()
    {
        RemoveAllMarkers();

        PathMarker begin = lastPos;

        while (!startNode.Equals(begin) && begin != null)
        {
            Instantiate(pathP, new Vector3(begin.location.x * maze.scale, 0, 
                begin.location.z * maze.scale), Quaternion.identity);
            begin = begin.parent;
        }

        Instantiate(pathP, new Vector3(startNode.location.x * maze.scale, 0,
                startNode.location.z * maze.scale), Quaternion.identity);
    }
}
