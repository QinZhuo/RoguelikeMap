using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridMapCreat : MonoBehaviour
{
    public Transform player;
    [Range(10,99999)]
    public int mapSize = 20;
    [Range(1, 30)]
    public int cubeSize = 1;
    [Range(1, 10)]
    public int roomSizeRate = 1;
    Node current;
    [Range(0, 50)]
    public int doorRate = 10;
    public GameObject planeCube;
    public GameObject[] wallPrefab;

    int[,] grid;
    List<Node>[,] doors;
    public Vector3 RandomPosition()
    {
        int x = Random.Range(1, mapSize);
        int y = Random.Range(2, mapSize);
        if(InMap(new int[] { x, y }, 0, false))
        {
            return new Vector3(x * cubeSize, 0.5f, y * cubeSize);
        }
        else
        {
            return RandomPosition();
        }
    }
    int teamCount = 1;
    // Use this for initialization
    void Start()
    {
        current = null;
        grid = new int[mapSize, mapSize]; //obj = new GameObject[size, size];
        //nodeGrid = new Node[size, size];
        for (int i = 0; i < 2000; i++)
        {
            RandomRoom();
        }
        PrimAll();
        doors = new List<Node>[teamCount, teamCount];
        for (int i = 1; i < teamCount; i++)
        {
            for (int j = 1; j < teamCount; j++)
            {
                doors[i, j] = new List<Node>();
            }
        }

        FindDoor();

        OpenDoor();
        Cut();
        ShowMap();
        if (player)
        {
            player.transform.position = RandomPosition();
        }
    }


    public void RandomRoom()
    {


        List<Node> doorlist = new List<Node>();
        int[] pos = new int[] { Random.Range(0, mapSize / 2) * 2 + 1, Random.Range(0, mapSize / 2) * 2 + 1 };
        int[] roomSize = new int[] { Random.Range(1, 2) * 2*roomSizeRate, Random.Range(1, 2) * 2 * roomSizeRate };
        bool canCreat = true;
        for (int i = pos[0] - roomSize[0] - 1; i <= pos[0] + roomSize[0] + 1; i++)
        {
            for (int j = pos[1] - roomSize[1] - 1; j <= pos[1] + roomSize[1] + 1; j++)
            {
                if (!InMap(new int[] { i, j }))
                {
                    canCreat = false;

                    break;
                }
            }
        }
        if (canCreat)
        {
            int team = teamCount++;
            for (int i = pos[0] - roomSize[0]; i <= pos[0] + roomSize[0]; i++)
            {
                for (int j = pos[1] - roomSize[1]; j <= pos[1] + roomSize[1]; j++)
                {

                    grid[i, j] = team;
                    //   Instantiate(cube, new Vector3(i, 0, j), Quaternion.identity);
                }
            }



        }

        //  Debug.Log(canCreat);
    }



    public void Cut()
    {
        int num = 0;
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                if (grid[i, j] != 0)
                {
                    if (GetNeib(new int[] { i, j }, 0, false).Count <= 1)
                    {
                        //    Debug.Log(GetNeib(new int[] { i, j }, 0).Count);
                        grid[i, j] = 0;

                       // Destroy(obj[i, j]);
                        num++;
                        //return;
                    }
                }

            }
        }
        if (num > 0)
        {
            Cut();
        }
    }
    public void PrimAll()
    {
        for (int i = 0; i < mapSize / 2; i++)
        {
            for (int j = 0; j < mapSize / 2; j++)
            {
                if (GetNeib(new int[] { i * 2 + 1, j * 2 + 1 }).Count >= 4)
                {
                    Prim(new int[] { i * 2 + 1, j * 2 + 1 });
                }
            }
        }
    }
    public void Prim(int[] start)
    {
        int team = teamCount;
        List<Node> nodes = new List<Node>();
        grid[start[0], start[1]] = teamCount;
        nodes.AddRange(GetNeib(new int[] { start[0], start[1] }));
        while (nodes.Count > 0)
        {
            current = nodes[Random.Range(0, nodes.Count)];
            nodes.Remove(current);
            if (CanCreat(current.pos, current.direction, team))
            {
                nodes.AddRange(GetNeib(new int[] { current.pos[0] + current.direction[0], current.pos[1] + current.direction[1] }));

            }
        }
        teamCount++;

    }
    public List<Node> GetNeib2(int[] pos)
    {
        List<Node> temp = new List<Node>();

        string info = "";
        int[] direction = new int[] { 1, 1 };
        Node node= new Node(new int[] { pos[0] + direction[0], pos[1]+ direction[1] }, direction);
        if (InMap(new int[] { pos[0] + direction[0], pos[1]  }, 0, false))
        {
            info += "*";
        }
        else
        {
            info += "_";
        }
        if (InMap(new int[] { pos[0] + direction[0], pos[1] + direction[1] },0 , false))
        {
            info += "*";
        }
        else
        {
            info += "_";
        }
        if (InMap(new int[] { pos[0] , pos[1] + direction[1] }, 0, false))
        {
            info += "*";
        }
        else
        {
            info += "_";
        }
        node.info = info;
        temp.Add(node);


         info = "";
         direction = new int[] { 1, -1 };
         node = new Node(new int[] { pos[0] + direction[0], pos[1] + direction[1] }, direction);
        if (InMap(new int[] { pos[0] + direction[0], pos[1] }, 0, false))
        {
            info += "*";
        }
        else
        {
            info += "_";
        }
        if (InMap(new int[] { pos[0] + direction[0], pos[1] + direction[1] }, 0, false))
        {
            info += "*";
        }
        else
        {
            info += "_";
        }
        if (InMap(new int[] { pos[0], pos[1] + direction[1] }, 0, false))
        {
            info += "*";
        }
        else
        {
            info += "_";
        }
        node.info = info;
        temp.Add(node);

        info = "";
        direction = new int[] { -1, 1 };
        node = new Node(new int[] { pos[0] + direction[0], pos[1] + direction[1] }, direction);
        if (InMap(new int[] { pos[0] + direction[0], pos[1] }, 0, false))
        {
            info += "*";
        }
        else
        {
            info += "_";
        }
        if (InMap(new int[] { pos[0] + direction[0], pos[1] + direction[1] }, 0, false))
        {
            info += "*";
        }
        else
        {
            info += "_";
        }
        if (InMap(new int[] { pos[0], pos[1] + direction[1] }, 0, false))
        {
            info += "*";
        }
        else
        {
            info += "_";
        }
        node.info = info;
        temp.Add(node);

        info = "";
        direction = new int[] { -1, -1 };
        node = new Node(new int[] { pos[0] + direction[0], pos[1] + direction[1] }, direction);
        if (InMap(new int[] { pos[0] + direction[0], pos[1] }, 0, false))
        {
            info += "*";
        }
        else
        {
            info += "_";
        }
        if (InMap(new int[] { pos[0] + direction[0], pos[1] + direction[1] }, 0, false))
        {
            info += "*";
        }
        else
        {
            info += "_";
        }
        if (InMap(new int[] { pos[0], pos[1] + direction[1] }, 0, false))
        {
            info += "*";
        }
        else
        {
            info += "_";
        }
        node.info = info;
        temp.Add(node);
        return temp;
    }
    

    public List<Node> GetNeib(int[] pos, int flag = 0, bool equals = true)
    {
        List<Node> temp = new List<Node>();
        if (InMap(new int[] { pos[0] + 1, pos[1] }, flag, equals))
        {
            temp.Add(new Node(new int[] { pos[0] + 1, pos[1] }, new int[] { 1, 0 }));
        }
        if (InMap(new int[] { pos[0] - 1, pos[1] }, flag, equals))
        {
            temp.Add(new Node(new int[] { pos[0] - 1, pos[1] }, new int[] { -1, 0 }));
        }
        if (InMap(new int[] { pos[0], pos[1] + 1 }, flag, equals))
        {
            temp.Add(new Node(new int[] { pos[0], pos[1] + 1 }, new int[] { 0, 1 }));
        }
        if (InMap(new int[] { pos[0], pos[1] - 1 }, flag, equals))
        {
            temp.Add(new Node(new int[] { pos[0], pos[1] - 1 }, new int[] { 0, -1 }));
        }
        return temp;
    }
    bool InMap(int[] pos, int flag = 0, bool equals = true)
    {

        if (pos[0] < mapSize && pos[0] >= 0 && pos[1] < mapSize && pos[1] >= 0)
        {

            if (equals)
            {
                if (grid[pos[0], pos[1]] == flag)
                {

                    return true;
                }
            }
            else
            {
                if (grid[pos[0], pos[1]] != flag)
                {

                    return true;
                }
            }
        }
        return false;
    }

    bool CanCreat(int[] pos, int[] direction, int team)
    {
        if (pos[0] + direction[0] < mapSize && pos[0] + direction[0] >= 0 && pos[1] + direction[1] < mapSize && pos[1] + direction[1] >= 0)
        {
            if (grid[pos[0] + direction[0], pos[1] + direction[1]] == 0)
            {



                grid[pos[0], pos[1]] = team;

                //    Instantiate(cube, new Vector3(pos[0], 0, pos[1]), Quaternion.identity);
                grid[pos[0] + direction[0], pos[1] + direction[1]] = team;
                //     Instantiate(cube, new Vector3(pos[0] + direction[0], 0, pos[1] + direction[1]), Quaternion.identity);
                return true;


            }
        }
        return false;
    }

    void ShowMap()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {

                if (grid[i, j] != 0)
                {
                    //  obj[i, j] = 
                    Instantiate(planeCube, new Vector3(i*cubeSize, 0, j * cubeSize), Quaternion.identity);
                    //  Instantiate(text, new Vector3(i, 1, j),Quaternion.LookRotation(-Vector3.up)).GetComponent<TextMesh>().text=grid[i,j].ToString();
                    CreatWall(new int[] { i, j });
                }
                else
                {
                   
                }
            }
        }
    }
    void CreatWall(int[] pos)
    {
        List<Node> nodeList = GetNeib(pos,0,true);
        if (nodeList.Count > 0)
        {
            foreach (var node in nodeList)
            {
                for (int i = -cubeSize/2+1; i < cubeSize/2; i++)
                {
                    Instantiate(wallPrefab[0], new Vector3((node.pos[0] - node.direction[0]*0.5f) * cubeSize+i* node.direction[1], 0, (node.pos[1] - node.direction[1] * 0.5f) * cubeSize + i * node.direction[0]), Quaternion.identity).transform.rotation=Quaternion.LookRotation(new Vector3(-node.direction[0],0,-node.direction[1]));

                }
                //Instantiate(wallPrefab, new Vector3((node.pos[0] + node.direction[0]) * cubeSize, 0, (node.pos[1] + node.direction[1]) * cubeSize), Quaternion.identity);
               
            }
            //foreach (var node in nodeList)
            //{
            //    foreach (var node2 in nodeList)
            //    {
            //        if(node.direction[0]!=-node2.direction[0]&& node.direction[1] != -node2.direction[1])
            //        {
            //            int[] newDirection = new int[] { node.direction[0] + node2.direction[0], node.direction[1] + node2.direction[1] };
            //            Instantiate(wallPrefab[1], new Vector3((pos[0] + newDirection[0] * 0.5f) * cubeSize, 0, (pos[1] + newDirection[1] * 0.5f) * cubeSize ), Quaternion.identity).transform.rotation = Quaternion.LookRotation(new Vector3(-newDirection[0], 0, -newDirection[1]));
            //        }
                
            //    }
            //    //Instantiate(wallPrefab, new Vector3((node.pos[0] + node.direction[0]) * cubeSize, 0, (node.pos[1] + node.direction[1]) * cubeSize), Quaternion.identity);

            //}
        }


        nodeList = GetNeib2(pos);
        foreach (var node in nodeList)
        {
            if (node.info == "*_*")
            {
                Instantiate(wallPrefab[2], new Vector3((pos[0] + node.direction[0] * 0.5f) * cubeSize, 0, (pos[1] + node.direction[1] * 0.5f) * cubeSize), Quaternion.identity).transform.rotation = Quaternion.LookRotation(new Vector3(-node.direction[0], 0, -node.direction[1]));
            }
            else if(node.info=="___")
            {
                Instantiate(wallPrefab[1], new Vector3((pos[0] + node.direction[0] * 0.5f) * cubeSize, 0, (pos[1] + node.direction[1] * 0.5f) * cubeSize), Quaternion.identity).transform.rotation = Quaternion.LookRotation(new Vector3(-node.direction[0], 0, -node.direction[1]));
            }
            else if (node.info == "__*")
            {
                Instantiate(wallPrefab[0], new Vector3((pos[0] + node.direction[0] * 0.5f) * cubeSize, 0, (pos[1] + node.direction[1] * 0.5f) * cubeSize), Quaternion.identity).transform.rotation = Quaternion.LookRotation(new Vector3(-node.direction[0], 0,0));
            }
            else if (node.info == "*__")
            {
                Instantiate(wallPrefab[0], new Vector3((pos[0] + node.direction[0] * 0.5f) * cubeSize, 0, (pos[1] + node.direction[1] * 0.5f) * cubeSize), Quaternion.identity).transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, -node.direction[1]));
            }
        }

    }
    public void FindDoor()
    {

        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                if (grid[i, j] == 0)
                {
                    List<Node> nodesTemp = GetNeib(new int[] { i, j }, 0, false);
                    if (nodesTemp.Count == 2)
                    {

                        if (grid[nodesTemp[0].pos[0], nodesTemp[0].pos[1]] != grid[nodesTemp[1].pos[0], nodesTemp[1].pos[1]])
                        {
                            doors[grid[nodesTemp[0].pos[0], nodesTemp[0].pos[1]], grid[nodesTemp[1].pos[0], nodesTemp[1].pos[1]]].Add(new Node(new int[] { i, j }));
                            doors[grid[nodesTemp[1].pos[0], nodesTemp[1].pos[1]], grid[nodesTemp[0].pos[0], nodesTemp[0].pos[1]]].Add(new Node(new int[] { i, j }));

                        }
                    }
                }
            }
        }
    }
    public void OpenDoor()
    {
        List<int> unOpen = new List<int>();
        List<int> open = new List<int>();

        open.Add(teamCount - 1);
        for (int i = 1; i < teamCount - 1; i++)
        {
            unOpen.Add(i);
        }
        int num = 0;
        while (unOpen.Count > 0)
        {

            int x = 0;
            //  int y = unOpen[Random.Range(0, unOpen.Count)];
            int max = 0;
            int y = 0;
            foreach (var tx in open)
            {

                foreach (var ty in unOpen)
                {
                    if (doors[tx, ty].Count > max)
                    {
                        y = ty;
                        x = tx;
                        max = doors[tx, ty].Count;
                    }
                }
            }
            if (y != 0 && x != 0)
            {
                int[] pos = doors[x, y][Random.Range(0, doors[x, y].Count)].pos;
                grid[pos[0], pos[1]] = 1000;
                open.Add(y);
                unOpen.Remove(y);
            }
            else
            {
                num++;

                if (num > 100)
                {
                    Debug.Log("连通失败");
                    string temp = "";
                    foreach (var item in open)
                    {
                        temp += "|" + item + "|";
                    }
                    Debug.Log("open" + temp);
                    temp = "";
                    foreach (var item in unOpen)
                    {
                        temp += "|" + item + "|";
                    }
                    Debug.Log("unOpen" + temp);
                    break;
                }
            }
        }
        foreach (var tx in open)
        {

            foreach (var ty in open)
            {
                if (doors[tx, ty].Count >0)
                {
                    int rate=Random.Range(0, 100);
                    if (rate < doorRate)
                    {
                        int[] pos= doors[tx, ty][Random.Range(0, doors[tx, ty].Count)].pos;
                        grid[pos[0], pos[1]] = 1000;
                    }
                }
            }
        }

    }
    public class Node
    {
        public int[] pos;
        public int[] direction;
        public GameObject cube;


        public string info;
        

        public Node(int[] p)
        {
            pos = p;

        }
        public Node(int[] p, int[] d)
        {
            pos = p;
            direction = d;
        }
    }
}
