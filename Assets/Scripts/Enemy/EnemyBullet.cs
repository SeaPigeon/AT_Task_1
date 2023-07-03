using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] int _rDamage;
    [SerializeField] float _bulletLifeSpan;
    [SerializeField] float _bulletSpeed;
    [SerializeField] Rigidbody _bulletRB;
    [SerializeField] Vector3 _moveDirection;

    public int RDamage { get { return _rDamage; } set { _rDamage = value; } }
    public float BulletSpeed { get { return _bulletSpeed; } set { _bulletSpeed = value; } }
    public float BulletLifeSpan { get { return _bulletLifeSpan; } set { _bulletLifeSpan = value; } }
    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }

    private void Awake()
    {
        _bulletRB = gameObject.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(DestroyBullet());
        MoveBullet();
    }

    private void MoveBullet()
    {
        _bulletRB.velocity = _moveDirection * _bulletSpeed; 
    }
    
    IEnumerator DestroyBullet()
    {
        yield return new WaitForSecondsRealtime(_bulletLifeSpan);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerScript>() ||
            other.CompareTag("Wall") || 
            (other.CompareTag("Door") && 
            other.GetComponent<DoorScript>().CurrentDoorState == DoorScript.DoorState.Closed))
        {
            Destroy(gameObject);
        }
    }
}
