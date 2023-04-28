using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SendDataTest : MonoBehaviour
{
    [SerializeField] private PhotonView myView;
    [SerializeField] private GameObject dataPrefab;
    [SerializeField] private Transform[] spawnPoints;
    DataList dataList;

    void Start()
    {
        StartCoroutine(TestSpawnObject());
    }

    IEnumerator TestSpawnObject()
    {
        yield return new WaitForSeconds(5f);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("IsMasterClient");
            dataList = new DataList();
            dataList.list = new List<Vector3>();

            for (int i = 0; i < 3; i++)
            {
                Vector3 randomPos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

                GameObject interactItemClone = Instantiate(dataPrefab, transform);
                interactItemClone.transform.position = randomPos;
                dataList.list.Add(interactItemClone.transform.position);
            }

            string dataString = JsonUtility.ToJson(dataList);
            myView.RPC("RPC_SyncInteracts", RpcTarget.OthersBuffered, dataString);
            Debug.Log(dataString);
        }
    }

    [PunRPC]
    public void RPC_SyncInteracts(string datas)
    {
        Debug.Log("RPC_SyncInteracts Working...");
        dataList = JsonUtility.FromJson<DataList>(datas);
        Debug.Log(dataList.list.Count);
        for (int i = 0; i < dataList.list.Count; i++)
        {
            Debug.Log(dataList.list[i]);
            GameObject interact = Instantiate(dataPrefab, transform);
            interact.transform.position = dataList.list[i];
        }
    }
}

class DataList
{
    public List<Vector3> list;
}
