using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationWorld : MonoBehaviour
{
    public City[] cities;

    private void OnValidate()
    {
        cities = GetComponentsInChildren<City>();
    }
}
