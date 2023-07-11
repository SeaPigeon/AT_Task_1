using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class BillboardScript : MonoBehaviour
{
    private RectTransform _UIElement;

    void Start()
    {
        SetUpReferences();
    }

    void LateUpdate()
    {
        Rotate();
    }
    private void SetUpReferences()
    {
        _UIElement = GetComponent<RectTransform>();
    }
    private void Rotate()
    {
        _UIElement.transform.rotation = PlayerScript.PlayerInstance.InGameCamera.transform.rotation;
    }

}
