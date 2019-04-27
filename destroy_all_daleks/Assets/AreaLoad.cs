using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AreaLoad : MonoBehaviour
{
    public NavMeshSurface[] floors;
    public GameObject startingRoom;
    public GameObject empty;
    public GameObject roomPrefabs;
    public GameObject[] roomArray;
    public GameObject parentObject;
    public bool tester = false;

    private Room[,] array;
    public Room[] deadendRooms;
    Room startingRoomScript;

    public GameObject deadEnds;
    private Room[] roomsGlobal;
    public int size;
    private int openDoors;

    private List<Room> EastExitRooms = new List<Room>();
    private List<Room> WestExitRooms = new List<Room>();
    private List<Room> NorthExitRooms = new List<Room>();
    private List<Room> SouthExitRooms= new List<Room>();

    private GameObject gObj;
    public int count;
    public bool firstIteration;


    public static Random rnd = new Random();
    // Start is called before the first frame update
    void Start()
    {
        firstIteration = true;
        int i = 0;
        size = 5;
        count = 0;
        deadendRooms = deadEnds.GetComponentsInChildren<Room>();
        roomsGlobal = roomPrefabs.GetComponentsInChildren<Room>();
        roomArray = new GameObject[roomsGlobal.Length];
        startingRoomScript = startingRoom.GetComponent<Room>();
        startingRoomScript.setDoors();
        startingRoomScript.setRow(size / 2);
        startingRoomScript.setCol(size / 2);
        foreach(Room room in roomsGlobal)
        {
            room.setDoors();
            roomArray[i] = room.gameObject;
            i++;
        }
        array = new Room[size, size];
       
        foreach(GameObject roomPrefab in roomArray)
        {
            
            Room room = roomPrefab.GetComponent<Room>();
            if (room.hasEastExit) 
                EastExitRooms.Add(room);
            if (room.hasNorthExit)
                NorthExitRooms.Add(room);
            if (room.hasSouthExit)
                SouthExitRooms.Add(room);
            if (room.hasWestExit)
                WestExitRooms.Add(room);
        }
       
        BuildLevel(startingRoomScript);
        GenerateLevel();
    }
    public void reGenerate()
    {
        tester = false;
        firstIteration = true;
        Room[] roomsToDestroy = parentObject.GetComponentsInChildren<Room>();
        foreach(Room room in roomsToDestroy)
        {
            Destroy(room.gameObject);
        }
        array = new Room[size, size];
        BuildLevel(startingRoomScript);
        GenerateLevel();

    }
    // Update is called once per frame
    void Update()
    {
        if (tester)
        {
            reGenerate();
        }
    
    }
    public void GenerateLevel()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (array[i, j] != null)
                {
                    gObj = Instantiate(array[i, j].gameObject);
                    gObj.gameObject.transform.position = new Vector3(10.6f * i, 0, 10.6f * j);
                    gObj.transform.parent = parentObject.transform;
                }
               
            }
        }
        floors = parentObject.GetComponentsInChildren<NavMeshSurface>();
        foreach(NavMeshSurface floor in floors)
        {
            floor.BuildNavMesh();
        }

    }

    public List<Room> checkSurroundings(int row, int col)
    {

        List<Room> roomCanidates = new List<Room>();
        List<Room> lstToRemove = new List<Room>();
        foreach (Room gRoom in roomsGlobal)
        {
            roomCanidates.Add(gRoom);
        }
            if (row != 0 && array[row - 1, col] != null)
            {
                if (array[row - 1, col].hasEastExit)
                {
                    foreach (Room room in WestExitRooms)
                    {
                        if (!roomCanidates.Contains(room))
                            roomCanidates.Add(room);
                    }

                    foreach (Room room in roomCanidates)
                    {
                        if (!WestExitRooms.Contains(room) && roomCanidates.Contains(room) && !lstToRemove.Contains(room))
                            lstToRemove.Add(room);
                    }

                }
                else
                {
                    foreach (Room room in WestExitRooms)
                    {
                        if (roomCanidates.Contains(room) && !lstToRemove.Contains(room))
                            lstToRemove.Add(room);
                    }
                }
            }
            if (row < size - 1 && array[row + 1, col] != null)
            {

                if (array[row + 1, col].hasWestExit)
                {
                    foreach (Room room in EastExitRooms)
                    {
                        if (!roomCanidates.Contains(room))
                            roomCanidates.Add(room);
                    }
                    foreach (Room room in roomCanidates)
                    {
                        if (!EastExitRooms.Contains(room) && roomCanidates.Contains(room) && !lstToRemove.Contains(room))
                            lstToRemove.Add(room);
                    }


                }
                else
                {
                    foreach (Room room in EastExitRooms)
                    {
                        if (roomCanidates.Contains(room) && !lstToRemove.Contains(room))
                            lstToRemove.Add(room);
                    }
                }
            }
            if (col < size - 1 && array[row, col + 1] != null)
            {
                if (array[row, col + 1].hasSouthExit)
                {
                    foreach (Room room in NorthExitRooms)
                    {

                        if (!roomCanidates.Contains(room))
                            roomCanidates.Add(room);
                    }
                    foreach (Room room in roomCanidates)
                    {
                        if (!NorthExitRooms.Contains(room) && roomCanidates.Contains(room) && !lstToRemove.Contains(room))
                            lstToRemove.Add(room);
                    }

                }
                else
                {
                    foreach (Room room in NorthExitRooms)
                    {
                        if (roomCanidates.Contains(room) && !lstToRemove.Contains(room))
                            lstToRemove.Add(room);
                    }
                }
            }
            if (col != 0 && array[row, col - 1] != null)
            {
                if (array[row, col - 1].hasNorthExit)
                {
                    foreach (Room room in SouthExitRooms)
                    {
                        if (!roomCanidates.Contains(room))
                            roomCanidates.Add(room);
                    }

                    foreach (Room room in roomCanidates)
                    {
                        if (!SouthExitRooms.Contains(room) && roomCanidates.Contains(room) && !lstToRemove.Contains(room))
                            lstToRemove.Add(room);
                    }

                }
                else
                {
                    foreach (Room room in SouthExitRooms)
                    {
                        if (roomCanidates.Contains(room) && !lstToRemove.Contains(room))
                            lstToRemove.Add(room);
                    }
                }
            }

            if (row == size - 1)
            {
                foreach (Room room in EastExitRooms)
                {
                    if (roomCanidates.Contains(room) && !lstToRemove.Contains(room))
                        lstToRemove.Add(room);
                }
            }
            if (row == 0)
            {
                foreach (Room room in WestExitRooms)
                {
                    if (roomCanidates.Contains(room) && !lstToRemove.Contains(room))
                        lstToRemove.Add(room);
                }
            }
            if (col == 0)
            {
                foreach (Room room in SouthExitRooms)
                {
                    if (roomCanidates.Contains(room) && !lstToRemove.Contains(room))
                        lstToRemove.Add(room);
                }
            }
            if (col == size - 1)
            {
                foreach (Room room in NorthExitRooms)
                {
                    if (roomCanidates.Contains(room) && !lstToRemove.Contains(room))
                        lstToRemove.Add(room);
                }
            }
            if(lstToRemove.Count == 0)
            {
                foreach(Room room in deadendRooms)
                {
                    lstToRemove.Add(room);
                }
                
            }
            foreach (Room room in lstToRemove)
            {
                if (roomCanidates.Contains(room))
                {
                    roomCanidates.Remove(room);
                }
            }
            return roomCanidates;
        
    }

   

    public void BuildLevel(Room current)
    {

        
        List<Room> finalRoomCanidates = new List<Room>();
        foreach(DoorScript door in current.getDoors())
        {
            bool island = true;
            int row = door.parentRoom.getRow();
            int col = door.parentRoom.getCol();
            if (firstIteration)
            {
                array[size / 2, size / 2] = current;
                firstIteration = false;
            }
            else
            {

                if (col < size - 1 && door.northDoor && array[row, col+1] == null)
                {
                    col = col +1;
                    finalRoomCanidates = checkSurroundings(row, col);
                    island = false;
                }
                else if (row!=0 && door.westDoor && array[row-1, col] == null)
                {
                    row = row - 1;
                    finalRoomCanidates = checkSurroundings(row, col);
                    island = false;

                }
                else if (col !=0 && door.southDoor && array[row, col-1] == null)
                {
                    col = col - 1;
                    finalRoomCanidates = checkSurroundings(row, col);
                    island = false;

                }
                else if (row<size-1 && door.eastDoor && array[row+1, col] == null)
                {
                    row = row + 1;
                    finalRoomCanidates = checkSurroundings(row, col);
                    island = false;


                }
                if (finalRoomCanidates.Count != 0 && !island)
                {
                    
                    current = finalRoomCanidates[Random.Range(0, finalRoomCanidates.Count-1)];           
                    if (array[row, col] == null)
                    {
                        array[row, col] = current;
                    }
                    

                }     
                else
                {
                    current = empty.GetComponent<Room>();
                    if (array[row, col] == null)
                        array[row, col] = current;
                }


            }


            if (!current.isDeadend())
            {
                current.setCol(col);
                current.setRow(row);
                current.setDoors();
                BuildLevel(current);
            }
            else
            {

                current.setCol(col);
                current.setRow(row);
                current.setDoors();

            }
                

        }








        
        
        
            
        
        

        
    }
}
