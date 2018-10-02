using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates list of 20 planets, which are then activated and 
/// deactivated rather than instantiated and destroyed at runtime.
/// </summary>

public class PlanetPooler : MonoBehaviour
{
    public static PlanetPooler SharedInstance;
    public List<GameObject> pooledPlanets;
    public GameObject planet;
    public int amountInPool;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledPlanets = new List<GameObject>();

        for (int i = 0; i < amountInPool; i++)
        {
            GameObject p = Instantiate(planet);
            p.SetActive(false);
            pooledPlanets.Add(p);
        }
    }

    public GameObject GetPooledPlanet()
    {
        for (int i = 0; i < pooledPlanets.Count; i++)
        {
            if (!pooledPlanets[i].activeInHierarchy)
                return pooledPlanets[i];
        }

        return null;
    }

    public void Clear()
    {
        for (int i = 0; i < amountInPool; i++)
        {
            pooledPlanets[i].SetActive(false);
        }
    }
}
