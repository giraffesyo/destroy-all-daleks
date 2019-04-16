using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Room parentRoom;
    public bool northDoor;
    public bool southDoor;
    public bool westDoor;
    public bool eastDoor;
    public int direction;
    public DoorScript connectedDoor;
    void Start()
    {
        direction = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setRoom(Room parentRoom_)
    {
        parentRoom = parentRoom_;
    }
    public void setConnectedDoor(DoorScript door_)
    {
        connectedDoor = door_;
    }
    public DoorScript getConnectedDoor()
    {
        return connectedDoor;
    }
    public void setOrientation()
    {
        direction = direction + (int)parentRoom.rotation;
        switch (direction)
        {
            case 0:
                this.northDoor = true;
                break;
            case 1:
                this.eastDoor = true;
                break;
            case 2:
                this.southDoor = true;
                break;
            case 3:
                this.westDoor = true;
                break;
            default: return;
        }
      
    }
    public void setDirection(int _direction)
    {
        direction = _direction;
        setOrientation();
        
    }
}
