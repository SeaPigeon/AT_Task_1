using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Base,
    W2,
    W3
}
public class BulletScript : MonoBehaviour
{
    [Header("Bullet Variables")]
    [SerializeField] BulletType _bulletType;
    [SerializeField] float _bulletLifeSpan;
    [SerializeField] float _bulletSpeed;
    [SerializeField] int _bulletDamage;
    [SerializeField] int _currentAmmo;
    [SerializeField] float _fireDelay;
    [SerializeField] Sprite _weaponSprite;
    [SerializeField] AudioClip _fireSFX;

    [Header("Debug")]
    [SerializeField] Rigidbody _bulletRB;
    [SerializeField] AudioManagerScript _audioManager;
  

    public int BulletDamage { get { return _bulletDamage; } set { _bulletDamage = value; } }
    public int Ammo { get { return _currentAmmo; } set {_currentAmmo = value; } }
    public float FireDelay { get { return _fireDelay; } set { _fireDelay = value; } }
    public Sprite WeaponSprite { get { return _weaponSprite; } }

    private void Awake()
    {
        _bulletRB = gameObject.GetComponent<Rigidbody>();
        _audioManager = AudioManagerScript.AMInstance;
        PlaySound();
    }

    private void Start()
    {
        StartCoroutine(DestroyBullet());
        MoveBullet();
    }

    private void MoveBullet()
    {
        _bulletRB.velocity = transform.forward * _bulletSpeed;
    }

    public void Reload(int ammo)
    {
        _currentAmmo += ammo;
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSecondsRealtime(_bulletLifeSpan);
        Destroy(gameObject);
    }

    private void PlaySound()
    {
        if (_bulletType == BulletType.Base)
        {
            _audioManager.PlayWeaponBSFX();
        }
        else if (_bulletType == BulletType.W2)
        {
            _audioManager.PlayWeapon2SFX();
        }
        else if (_bulletType == BulletType.W3)
        {
            _audioManager.PlayWeapon3SFX();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") || other.GetComponent<EnemyScript>())
        {
            Destroy(gameObject);
        }
    }
   
}
