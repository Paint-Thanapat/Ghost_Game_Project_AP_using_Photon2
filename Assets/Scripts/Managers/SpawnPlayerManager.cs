using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform[] spawnPoints;

    private Vector3 GetSpawnPosition()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].position;
    }

    private void Start()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, GetSpawnPosition(), Quaternion.identity);
    }
}
