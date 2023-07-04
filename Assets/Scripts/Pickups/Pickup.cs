using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickUpType
    {
        Default
    }

    [Header("PickUp Variables")]
    [SerializeField] private int _healthRestored;
    [SerializeField] private int _ammoRestored;
    [SerializeField] private PickUpType _pickUpType;

    [Header("Debug")]
    private AudioManagerScript _audioManager;
    [SerializeField] LinkUIScript _UILinker;

    private void Start()
    {
        _audioManager = AudioManagerScript.AMInstance;
        _UILinker = UIManagerScript.UIMInstance.GetComponent<LinkUIScript>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerScript>())
        {
            switch (_pickUpType)
            {
                
                
            }

            Destroy(gameObject);
        }
    }
}
