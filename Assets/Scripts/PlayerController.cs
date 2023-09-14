using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;

    Rigidbody2D _rb;
    Camera _mainCamera;

    float _moveVertical;
    float _moveHorizontal;  
    float _moveSpeed = 10f;

    Vector2 _moveVelocity;

    Vector2 _mousePosition;
    Vector2 _offset;

    [SerializeField] GameObject _bullet;
    [SerializeField] GameObject _bulletSpawn;

    bool _isShooting = false;
    float _bulletSpeed = 15f;

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
    }

    void Update()
    {
        _moveHorizontal = Input.GetAxisRaw("Horizontal");
        _moveVertical = Input.GetAxisRaw("Vertical");

        _moveVelocity = new Vector2(_moveHorizontal, _moveVertical) * _moveSpeed;

        if (Input.GetMouseButtonDown(0))
        {
            _isShooting = true;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();

        if (_isShooting)
        {
            StartCoroutine(Fire());
        }
    }

    void MovePlayer()
    {
        if (_moveHorizontal != 0 || _moveVertical != 0)
        {
            _rb.velocity = _moveVelocity.normalized;
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }

    void RotatePlayer()
    {
        _mousePosition = Input.mousePosition;
        Vector3 screenPoint = _mainCamera.WorldToScreenPoint(transform.localPosition);
        _offset = new Vector2(_mousePosition.x - screenPoint.x , _mousePosition.y - screenPoint.y).normalized;

        float angle = Mathf.Atan2(_offset.y, _offset.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    IEnumerator Fire()
    {
        _isShooting = false;
        GameObject bullet = Instantiate(_bullet, _bulletSpawn.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = _offset * _bulletSpeed;
        yield return new WaitForSeconds(3);
        Destroy(bullet);
    }
}
