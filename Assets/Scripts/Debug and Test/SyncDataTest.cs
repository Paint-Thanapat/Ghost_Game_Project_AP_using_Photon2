using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class SyncDataTest : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    [SerializeField] private GameObject[] ObjectArray;
    [SerializeField] private int currentObject;

    private void Start()
    {
        PV = GetComponent<PhotonView>();

        SetData(currentObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentObject++;
            if (currentObject >= ObjectArray.Length)
            {
                currentObject = 0;
            }
            SetData(currentObject);

            if (PV.IsMine)
            {
                Hashtable _hash = new Hashtable();
                _hash.Add("ItemIndex", currentObject);
                PhotonNetwork.LocalPlayer.SetCustomProperties(_hash);
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    {
        if (!PV.IsMine && targetPlayer == PV.Owner)
        {
            SetData((int)changedProps["ItemIndex"]);
        }
    }

    private void SetData(int _currentObj)
    {
        for (int i = 0; i < ObjectArray.Length; i++)
        {
            ObjectArray[i].SetActive(false);
        }

        ObjectArray[_currentObj].SetActive(true);

        currentObject = _currentObj;
    }
}
