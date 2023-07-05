using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class BillboardScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sprite;
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
        _sprite = GetComponent<SpriteRenderer>();
    }
    private void Rotate()
    {
        _sprite.transform.rotation = PlayerScript.PlayerInstance.transform.rotation; // test with camera rotation
    }

}
