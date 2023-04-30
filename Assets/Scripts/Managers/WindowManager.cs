using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager Instance;
    [SerializeField] private Window[] windows;
    [SerializeField] private Vector2 minMaxWindowBreakTime;
    [SerializeField] private Vector2 minMaxSpawnGhost;
    PhotonView PV;

    private void Awake()
    {
        Instance = this;

        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].SetWindow(windows[i].windowStatus);
        }

        StartCoroutine(RandomWindowBreak());
    }

    IEnumerator RandomWindowBreak()
    {
        yield return new WaitForSeconds(Random.Range(minMaxWindowBreakTime.x, minMaxWindowBreakTime.y));
        int randomWindowIndex = Random.Range(0, windows.Length);
        SetWindowBreak(randomWindowIndex);
    }

    public void CheckToSetWindow(Window _window)
    {
        int windowIndex = FindWindowIndex(_window);

        if (windows[windowIndex].windowStatus == Window.WindowType.Full)
        {
            SetWindowFullClose(windowIndex);
        }
        else if (windows[windowIndex].windowStatus == Window.WindowType.Break)
        {
            SetWindowRepair(windowIndex);
        }
    }

    public int FindWindowIndex(Window _window)
    {
        for (int i = 0; i < windows.Length; i++)
        {
            if (_window == windows[i])
            {
                return i;
            }
        }

        return -1;
    }

    public void SetWindowBreak(int _windowIndex)
    {
        if (_windowIndex == -1)
            return;

        PV.RPC("RPC_SyncWindowBreak", RpcTarget.AllBuffered, _windowIndex);
    }

    public void SetWindowFullClose(int _windowIndex)
    {
        if (_windowIndex == -1)
            return;

        PV.RPC("RPC_SyncWindowFullClose", RpcTarget.AllBuffered, _windowIndex);
    }

    public void SetWindowRepair(int _windowIndex)
    {
        if (_windowIndex == -1)
            return;

        PV.RPC("RPC_SyncWindowRepair", RpcTarget.AllBuffered, _windowIndex);
    }

    [PunRPC]
    public void RPC_SyncWindowBreak(int _windowIndex)
    {
        windows[_windowIndex].SetWindow(Window.WindowType.Break);
    }

    [PunRPC]
    public void RPC_SyncWindowFullClose(int _windowIndex)
    {
        windows[_windowIndex].SetWindow(Window.WindowType.Full);
        windows[_windowIndex].CloseWindowFull();
    }

    [PunRPC]
    public void RPC_SyncWindowRepair(int _windowIndex)
    {
        windows[_windowIndex].SetWindow(Window.WindowType.Repair);
    }
}
