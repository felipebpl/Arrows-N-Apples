using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartView : MonoBehaviour
{
    [SerializeField] private GameObject _darkPanel;

    public void SetHeartActive(bool state)
    {
        _darkPanel.SetActive(!state);
    }
}
