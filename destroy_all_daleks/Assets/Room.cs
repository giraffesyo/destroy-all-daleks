using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Start is called before the first frame update
    public bool hasEastExit;
    public bool hasWestExit;
    public bool hasNorthExit;
    public bool hasSouthExit;
    public int numberOfDoors;
    public bool partOfMap;
    public int row;
    public int col;
    public float rotation;
    public bool hasDalek;
    public GameObject[] Daleks;

    public DoorScript[] doors;
    void Start()
    {
        if (hasDalek)
        {
            Daleks[Random.Range(0, Daleks.Length)].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool isDeadend()
    {
        if (getNumberofDoors() == 1)
            return true;
        else
            return false;
    }
    public void setNumberofDoors()
    {
        numberOfDoors = GetComponentsInChildren<DoorScript>().Length;
    }
    public int getNumberofDoors()
    {
        return this.numberOfDoors;
    }
    public void setCol(int _col)
    {
        col = _col;
    }
    public int getCol()
    {
        return col;
    }
    public void setRow(int _row)
    {
        row = _row;
    }
    public int getRow()
    {
        return row;
    }
    public DoorScript[] getDoors()
    {
        return doors;
    }
    public void setDoors()
    {
        rotation = this.gameObject.transform.rotation.eulerAngles.y / 90f;

        //rotation on N is 0
        // E = 90
        // S = 180
        // W = 270


        doors = this.GetComponentsInChildren<DoorScript>();
        setNumberofDoors();
        foreach (DoorScript door in doors)
        {
            door.setRoom(this);
            door.northDoor = false;
            door.southDoor = false;
            door.eastDoor = false;
            door.westDoor = false;
            int trueRotation = (int)door.gameObject.transform.rotation.eulerAngles.y;
            if (trueRotation == 0)
            {
                door.northDoor = true;
            }
            else if (trueRotation == 180)
            {
                door.southDoor = true;
            }
            else if (trueRotation == 90)
            {
                door.eastDoor = true;
            }
            else
            {
                door.westDoor = true;
            }
        }
    }
    public void hasDirection()
    {
        doors = this.GetComponentsInChildren<DoorScript>();
        foreach (DoorScript door in doors)
        {
            if (door.eastDoor)
                hasEastExit = true;
            if (door.northDoor)
                hasNorthExit = true;
            if (door.westDoor)
                hasWestExit = true;
            if (door.southDoor)
                hasSouthExit = true;
        }
    }


}
