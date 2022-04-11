using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform TilePrefab;
    public Transform ObstaclePrefab;
    public Vector2Int MapSize;
    public Transform ParentContainer;

    [Range(0f, 1f)]
    public float outLineScale;
    [Range(0f, 1f)]
    public float obstaclePercent;
    public int Seed=10;
    List<Coord> coordList;
    Queue<Coord> shuffledcoordList;
    void Start()
    {
        StartCoroutine(Generate());    
    }
    public IEnumerator Generate()
    {
        
        yield return SearchAndDestroy();
        coordList = new List<Coord>();

        for (int x = 0; x < MapSize.x; x++)
        {
            for (int y = 0; y < MapSize.y; y++)
            {
                coordList.Add(new Coord(x, y));
            }
        }

        shuffledcoordList = new Queue<Coord>(Utility.ShuffleArray(coordList.ToArray(),Seed));

        for (int x = 0; x < MapSize.x; x++)
        {
            for( int y = 0; y < MapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x,y);
                Transform newTile = Instantiate(TilePrefab, ParentContainer);
                newTile.transform.position = tilePosition;
                newTile.transform.rotation = Quaternion.Euler(Vector3.right * 90);
                newTile.localScale = Vector3.one * (1 - outLineScale);
            }
        }


        bool[,] obstacleMap = new bool[MapSize.x, MapSize.y];
        int obstacleCount = (int)(MapSize.x * MapSize.y * obstaclePercent);
        for (int i=0; i<obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            Vector3 obstaclePosition = CoordToPosition(randomCoord.x,randomCoord.y);


            Transform newObstacle = Instantiate(ObstaclePrefab, ParentContainer);
            newObstacle.position = obstaclePosition + Vector3.up *.5f;
            newObstacle.rotation = Quaternion.identity;
        }

    }

    public Task SearchAndDestroy()
    {
        int a = ParentContainer.childCount;
        for (int i = 0; i < a; i++)
        {
            Destroy(ParentContainer.GetChild(i).GetComponent<Transform>().gameObject);
        }
        ParentContainer.position = Vector3.zero;
        return Task.CompletedTask;
    }

    Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-MapSize.x / 2 + 0.5f + x, 0, -MapSize.y / 2 + 0.5f + y);
    }

    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffledcoordList.Dequeue();
        shuffledcoordList.Enqueue(randomCoord);
        return randomCoord;
    }

    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x; 
            y = _y; 
        }
    }

}
