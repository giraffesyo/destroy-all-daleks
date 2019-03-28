using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLoad : MonoBehaviour
{
    public GameObject room;
    // Start is called before the first frame update
    void Start()
    {
        GameObject level = Instantiate(room);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
