using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int Exp;
    public float Brake = 0.5f;
    private Vector3 _direction;
    private float _speed;
    public float FollowAccel = 0.003f;
    private bool _isFollow;
    private float _followSpeed;
    public AudioClip GemClip;

    void Update()
    {
        var enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager.Congraturations)
        {
            return;
        }
        var playerPosition = Player.Instance.transform.localPosition;
        var distance = Vector3.Distance(playerPosition, transform.localPosition);
        if (distance < Player.Instance.MagnetDistance)
        {
            _isFollow = true;
        }
        if (_isFollow && Player.Instance.gameObject.activeSelf)
        {
            var direction = playerPosition - transform.localPosition;
            direction.Normalize();
            transform.localPosition += direction * _followSpeed * Time.deltaTime;
            _followSpeed += FollowAccel;
            return;
        }

        var velocity = _direction * _speed * Time.deltaTime;
        transform.localPosition += velocity;
        _speed *= Brake;
        transform.localPosition = Utils.ClampPosition(transform.localPosition);
    }

    public void Init(int score, float speedMin, float speedMax)
    {
        var angle = Random.Range(0, 360);
        var radian = angle * Mathf.Deg2Rad;
        _direction = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0);
        _speed = Mathf.Lerp(speedMin, speedMax, Random.value);
        Destroy(gameObject, 20);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.name.Contains("Player")) return;
        Destroy(gameObject);
        var player = collision.GetComponent<Player>();
        player.AddExp(Exp);
        var audioSource = FindObjectOfType<AudioSource>();
        audioSource.PlayOneShot(GemClip);
    }
}
