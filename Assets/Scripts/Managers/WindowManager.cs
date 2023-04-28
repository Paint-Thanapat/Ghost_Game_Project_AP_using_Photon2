using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private Window[] windows;
    [SerializeField] private Vector2 minMaxWindowBreakTime;
    [SerializeField] private Vector2 minMaxSpawnGhost;

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

        int randomWindow = Random.Range(0, windows.Length);
        windows[randomWindow].SetWindow(Window.WindowType.Break);
    }
}
