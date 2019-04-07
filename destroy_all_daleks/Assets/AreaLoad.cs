using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLoad : MonoBehaviour
{
    public GameObject startingRoom;
    public GameObject empty;
    public GameObject roomPrefabs;
    public GameObject[] roomArray;
    private Room[,] array;
    public Room[] deadendRooms;

    public GameObject deadEnds;
    private Room[] roomsGlobal;
    public int size = 5;
    private int openDoors;

    private List<Room> EastExitRooms = new List<Room>();
    private List<Room> WestExitRooms = new List<Room>();
    private List<Room> NorthExitRooms = new List<Room>();
    private List<Room> SouthExitRooms= new List<Room>();

    public GameObject gObj;
    public bool snakedown = true;
    public int count = 0;

    private int numberofConnections =  0;

    public static Random rnd = new Random();
    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        deadendRooms = deadEnds.GetComponentsInChildren<Room>();
        roomsGlobal = roomPrefabs.GetComponentsInChildren<Room>();
        roomArray = new GameObject[roomsGlobal.Length];
        Room startingRoomScript = startingRoom.GetComponent<Room>();
        startingRoomScript.setDirection();
        foreach(Room room in roomsGlobal)
        {
           
            room.setDirection();
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
       
        BuildLevel(startingRoomScript, openDoors, size/2, size/2);
       // isConnectedMap();
       
        trimMap();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void trimMap()
    {
        foreach(Room room in array)
        {
            room.partOfMap = false;
        }
        array[2, 2].partOfMap = true;
        for (int i = 2; i < size - 1; i++)
        {
            for (int j = 2; j < size - 1; j++)
            {
                if(array[i, j].partOfMap)
                {
                    setAdjacent(i, j);
                }
            }
        }
        for (int i = 2; i >= 0; i--)
        {
            for (int j = 2; j >= 0; j--)
            {
                if (array[i, j].partOfMap)
                {
                    setAdjacent(i, j);
                }
            }
        }
    }

    public void setAdjacent(int row, int col)
    {

        if (row < size - 1 && array[row + 1, col] != null)
        {
            if(array[row + 1, col].hasSouthExit && array[row,col].hasNorthExit && array[row, col].partOfMap)
                array[row + 1, col].partOfMap = true;
        }
        if (row > 0 && array[row - 1, col] != null)
        {
            if (array[row - 1, col].hasNorthExit && array[row, col].hasSouthExit && array[row, col].partOfMap)
                array[row - 1, col].partOfMap = true;
        }
        if (col < size - 1 && array[row, col + 1] != null)
        {
            if (array[row, col+1].hasWestExit && array[row, col].hasEastExit && array[row, col].partOfMap)
                array[row , col + 1].partOfMap = true;
        }
        if (col > 0 && array[row, col - 1] != null)
        {
            if (array[row, col -1].hasEastExit && array[row, col].hasWestExit && array[row, col].partOfMap)
                array[row, col - 1].partOfMap = true;
        }
    }

    public void BuildLevel(Room current, int remainingDoors, int row, int col)
    {
 
        DoorScript[] adjacentDoors;
        List<Room> roomCanidates = new List<Room>();
        List<Room> lstToRemove = new List<Room>();
        foreach(Room gRoom in roomsGlobal)
        {
            roomCanidates.Add(gRoom);
        }
        if (array[row, col] != null || count == 0)
        {
            //todo:Delete this
        }
        else
        {
            if (row != 0 && array[row - 1, col] != null)
            {
                if (array[row - 1, col].hasNorthExit)
                {
                    foreach (Room room in SouthExitRooms)
                    {
                        if (!roomCanidates.Contains(room))
                            roomCanidates.Add(room);
                    }
                    adjacentDoors = array[row - 1, col].getDoors();
                    foreach (DoorScript door in adjacentDoors)
                    {
                        if (door.northDoor)
                            door.isConnected = true;
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
            if (row < size - 1 && array[row + 1, col] != null)
            {

                if (array[row + 1, col].hasSouthExit)
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
                    adjacentDoors = array[row + 1, col].getDoors();
                    foreach (DoorScript door in adjacentDoors)
                    {
                        if (door.southDoor)
                            door.isConnected = true;
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
            if (col < size - 1 && array[row, col + 1] != null)
            {
                if (array[row, col + 1].hasWestExit)
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
                    adjacentDoors = array[row, col + 1].getDoors();
                    foreach (DoorScript door in adjacentDoors)
                    {
                        if (door.eastDoor)
                            door.isConnected = true;
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

            if (col != 0 && array[row, col - 1] != null)
            {
                if (array[row, col - 1].hasEastExit)
                {
                    foreach (Room room in WestExitRooms)
                    {
                        if (!roomCanidates.Contains(room))
                            roomCanidates.Add(room);
                    }
                    adjacentDoors = array[row, col - 1].getDoors();
                    foreach (Room room in roomCanidates)
                    {
                        if (!WestExitRooms.Contains(room) && roomCanidates.Contains(room) && !lstToRemove.Contains(room))
                            lstToRemove.Add(room);
                    }
                    foreach (DoorScript door in adjacentDoors)
                    {
                        if (door.westDoor)
                            door.isConnected = true;
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


            if (row == size - 1)
            {
                foreach (Room room in NorthExitRooms)
                {
                    if (roomCanidates.Contains(room) && !lstToRemove.Contains(room))
                        lstToRemove.Add(room);
                }
            }
            if (row == 0)
            {
                foreach (Room room in SouthExitRooms)
                {
                    if (roomCanidates.Contains(room) && !lstToRemove.Contains(room))
                        lstToRemove.Add(room);
                }
            }
            if (col == 0)
            {
                foreach (Room room in WestExitRooms)
                {
                    if (roomCanidates.Contains(room) && !lstToRemove.Contains(room))
                        lstToRemove.Add(room);
                }
            }
            if (col == size - 1)
            {
                foreach (Room room in EastExitRooms)
                {
                    if (roomCanidates.Contains(room) && !lstToRemove.Contains(room))
                        lstToRemove.Add(room);
                }
            }
            if (roomCanidates.Count>1)
            {
                foreach (Room room in deadendRooms)
                {
                    if (!lstToRemove.Contains(room))
                        lstToRemove.Add(room);
                }
            }
            foreach (Room room in lstToRemove)
            {
                if(roomCanidates.Contains(room))
                {
                    roomCanidates.Remove(room);
                }
            }
            if (roomCanidates.Count != 0)
                current = roomCanidates[Random.Range(0, roomCanidates.Count)];
           
                
          

            

            
           

        }
        array[row, col] = current;
        
        gObj = Instantiate(current.gameObject);
        gObj.gameObject.transform.position = new Vector3(11f * col, 0, 11f * row);
        if (col == 0 && row == 0)
        {
            snakedown = false;
            col = 3;
            row = 2;
        }
        else if (snakedown)
        {
            if (col != 0 && array[row, col - 1] == null)
                col--;
            else if (col < size - 1 && array[row, col + 1] == null)
                col++;
            else if (row != 0 && array[row - 1, col] == null)
                row--;
  
        }
        else
        {
            if (col < size - 1 && array[row, col + 1] == null)
                col++;
            else if (col != 0 && array[row, col - 1] == null)
                col--;
            else if (row < size - 1 && array[row + 1, col] == null)
                row++;
            
            
        }
        count++;
        

        if (remainingDoors>100)
            return;
        if (count < 25 )
        {
            BuildLevel(array[row, col], remainingDoors, row, col);
        }
        else return;
    }
}
