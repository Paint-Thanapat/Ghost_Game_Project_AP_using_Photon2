using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPointManager : MonoBehaviour
{
    public Transform[] spawnPoints;

    public Vector3 GetRandomSpawnPosition()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].position;
    }
}
