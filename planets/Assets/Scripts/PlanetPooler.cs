using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Clear(){
        for (int i = 0; i < amountInPool; i++)
        {
            pooledPlanets[i].SetActive(false);
        }
    }

}
