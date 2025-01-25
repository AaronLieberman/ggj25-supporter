using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    public GameObject[] BaseTilePrefabs;
    public float[] BaseTileWeights;

    void Start()
    {
        CreateGround();
    }

    void CreateGround()
    {
        if (BaseTilePrefabs.Length == 0 && BaseTilePrefabs[0] != null)
            return;

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 2; x++)
            {
                Instantiate(BaseTilePrefabs[0], new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }
}
