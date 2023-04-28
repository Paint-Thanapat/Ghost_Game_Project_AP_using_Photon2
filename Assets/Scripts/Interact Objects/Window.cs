using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public WindowType windowStatus;
    [SerializeField] private bool isOpen;
    [SerializeField] private GameObject windowFull;
    [SerializeField] private GameObject windowBreak;
    [SerializeField] private GameObject windowRepair;

    public enum WindowType
    {
        Full, Break, Repair
    }

    public void SetWindow(WindowType _windowType)
    {
        windowFull.SetActive(false);
        windowBreak.SetActive(false);
        windowRepair.SetActive(false);

        windowStatus = _windowType;

        switch (_windowType)
        {
            case WindowType.Full:
                windowFull.SetActive(true);
                break;
            case WindowType.Break:
                windowBreak.SetActive(true);
                isOpen = true;
                break;
            case WindowType.Repair:
                windowRepair.SetActive(true);
                isOpen = false;
                break;
        }
    }
}
