using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    GameManager _gameManager;
    GameObject _player;

    Rigidbody2D _rb;

    float _enemyHealth = 100;
    float _enemyMoveSpeed = 2f;

    Vector2 _moveDiraction;

    Quaternion _targetRotation;

    bool _disableEnemy = false;
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if (!_gameManager._gameOver && !_disableEnemy)
        {
            MovePlayer();
            RotateEnemy();
        }
    }

    void MovePlayer() 
    {
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _enemyMoveSpeed * Time.deltaTime); 
    }

    void RotateEnemy()
    {
        _moveDiraction = _player.transform.position - transform.position;
        _moveDiraction.Normalize();

        _targetRotation = Quaternion.LookRotation(Vector3.forward, _moveDiraction);

        if (transform.rotation != _targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 200 * Time.deltaTime);

        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            StartCoroutine(Dameged());

            _enemyHealth -= 40f;

            if (_enemyHealth <= 0)
            {
                Destroy(gameObject);
            }

            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            _gameManager._gameOver = true;
            collision.gameObject.SetActive(false);
        }
    }

    IEnumerator Dameged()
    {
        _disableEnemy = true;
        yield return new WaitForSeconds(0.5f);
        _disableEnemy = false;
    }
}
