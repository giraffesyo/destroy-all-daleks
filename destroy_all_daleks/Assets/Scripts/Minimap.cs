using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform minimap;
    private void LateUpdate()
    {
        minimap.rotation = Quaternion.Euler(new Vector3(-90, 0, 180));
    }
}
