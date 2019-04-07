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
    public int numberOfExits = 0;
    public int closedDoors = 0 ;
    public bool partOfMap;
 

    DoorScript[] doors;
    void Start()
    {
        
      
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
    public void setConnected()
    {
        doors[closedDoors].isConnected = true;
        closedDoors++;
    }

    public int getNumberofDoors()
    {
        DoorScript[] forDoorLength = this.GetComponentsInChildren<DoorScript>();
        numberOfExits = forDoorLength.Length;
        return numberOfExits;
    }
    public DoorScript[] getDoors()
    {
        return GetComponentsInChildren<DoorScript>();
    }
    public void setDirection()
    {
        
        doors = this.GetComponentsInChildren<DoorScript>();
        int index =0;
        int northSouthIndex = 0;
        int eastWestIndex = 0;
        bool firstVerticalDirection = false;
        bool firstHorizontalDirection = false;
        float checkNorthSouth = 0;
        float checkWestEast = 0;
        foreach (DoorScript door in doors)
        {
           
            if ( door.isUnset && (door.transform.rotation.eulerAngles.y % 180 == 0))
            {
                if (hasNorthExit && hasSouthExit)
                {
                    if (!firstVerticalDirection)
                    {
                        door.northDoor = true;
                        door.isUnset = false;
                        firstVerticalDirection = true;
                        checkNorthSouth = door.gameObject.transform.position.z;
                        northSouthIndex = index;

                    }
                    else
                    {
                        if (checkNorthSouth > door.gameObject.transform.position.z)
                        {
                            door.southDoor = true;
                            door.isUnset = false;
                        }
                        else
                        {
                            doors[northSouthIndex].northDoor = false;
                            doors[northSouthIndex].southDoor = true;
                            door.northDoor = true;
                            door.isUnset = false;
                        }

                    }
                }
                else if (hasNorthExit && !hasSouthExit)
                {
                    door.northDoor = true;
                    door.isUnset = false;
                }
                else if (!hasNorthExit && hasSouthExit)
                {
                    door.southDoor = true;
                    door.isUnset = false;
                }
                


            }
            else if (door.isUnset && (door.transform.rotation.eulerAngles.y % 90 == 0))
            {
                if (hasWestExit && hasEastExit)
                {
                    if (!firstHorizontalDirection)
                    {
                        door.eastDoor = true;
                        door.isUnset = false;
                        firstHorizontalDirection = true;
                        checkWestEast = door.gameObject.transform.position.x;
                        eastWestIndex = index;

                    }
                    else
                    {
                        if (checkWestEast > door.gameObject.transform.position.x)
                        {
                            door.westDoor = true;
                            door.isUnset = false;
                        }
                        else
                        {
                            doors[eastWestIndex].eastDoor = false;
                            doors[eastWestIndex].westDoor = true;
                            door.eastDoor = true;
                            door.isUnset = false;
                        }


                    }
                }
                else if (hasEastExit && !hasWestExit)
                {
                    door.eastDoor = true;
                    door.isUnset = false;
                }
                   
                else if (!hasEastExit && hasWestExit)
                {
                    door.westDoor = true;
                    door.isUnset = false;


                }

            }
             
    index++;

        }
      
    }
    public void hasDirection()
    {
        doors = this.GetComponentsInChildren<DoorScript>();
        foreach (DoorScript door in doors)
        {
            if (door.eastDoor)
                hasEastExit=true;
            if (door.northDoor)
                hasNorthExit = true;
            if (door.westDoor)
                hasWestExit = true;
            if (door.southDoor)
                hasSouthExit = true;
        }
    }
    public void checkDeadendError(Room[,] array, int row, int col)
    {
        
        int size = (int)Mathf.Sqrt(array.Length);
        if (array[row, col].isDeadend())
        {
            if (row < size - 1 && array[row + 1, col] != null)
            {
                if (array[row + 1, col].isDeadend() && array[row, col].hasSouthExit)
                {
                    Destroy(array[row + 1, col].gameObject);
                    Destroy(this.gameObject);

                }
            }
            if (row > 0 && array[row - 1, col] != null)
            {
                if (array[row -1, col].isDeadend() && array[row, col].hasNorthExit)
                {
                    Destroy(array[row - 1, col].gameObject);
                    Destroy(this.gameObject);
                }
            }
            if (col < size - 1 && array[row, col + 1] != null)
            {
                if (array[row, col+1].isDeadend() && array[row, col].hasEastExit)
                {
                    Destroy(array[row, col+1].gameObject);
                    Destroy(this.gameObject);
                }
            }
            if (col > 0 && array[row, col - 1] != null)
            {
                if (array[row, col -1].isDeadend() && array[row, col].hasWestExit)
                {
                    Destroy(array[row, col-1].gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
 
    }
    
    public void setDirectionExplicit(string direction)
    {
        foreach (DoorScript door in doors)
        {
            if (direction.Equals("north")  && door.isUnset)
            {
                door.northDoor = true;
                door.isUnset = false;
                break;
            }
            if (direction.Equals("east") && door.isUnset)
            {
                door.eastDoor = true;
                door.isUnset = false;
                break;
            }
            if (direction.Equals("west") && door.isUnset)
            {
                door.westDoor = true;
                door.isUnset = false;
                break;
            }
            if (direction.Equals("south") && door.isUnset)
            {
                door.southDoor = true;
                door.isUnset = false;
                break;
            }
        }

    }

}
