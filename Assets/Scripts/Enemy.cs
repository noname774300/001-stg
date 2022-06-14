using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RESPAWN_TYPE
{
    UP,
    RIGHT,
    DOWN,
    LEFT,
    SIZEOF
}

public class Enemy : MonoBehaviour
{
    public Vector2 RespawnPositionInside;
    public Vector2 RespawnPositionOutside;
    public float Speed;
    public int HpMax;
    public int Exp;
    public int Damage;
    public Explosion ExplosionPrefab;
    private int _hp;
    private Vector3 _direction;
    public bool IsFollow;
    public Gem[] GemPrefabs;
    public float GemSpeedMin;
    public float GemSpeedMax;
    public AudioClip DeathClip;

    void Start()
    {
        _hp = HpMax;
        if (!IsFollow)
        {
            Destroy(gameObject, 30);
        }
    }

    void Update()
    {
        var enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager.Congraturations)
        {
            return;
        }
        if (IsFollow)
        {
            var angle = Utils.GetAngle(transform.localPosition, Player.Instance.transform.localPosition);
            var direction = Utils.GetDirection(angle);
            transform.localPosition += direction * Speed * Time.deltaTime;
            var angles = transform.localEulerAngles;
            angles.z = angle - 90;
            transform.localEulerAngles = angles;
            return;
        }
        transform.localPosition += _direction * Speed * Time.deltaTime;
    }

    public void Init(RESPAWN_TYPE respawnType)
    {
        var position = Vector3.zero;
        switch (respawnType)
        {
            case RESPAWN_TYPE.UP:
                position.x = Random.Range(-RespawnPositionInside.x, RespawnPositionInside.x);
                position.y = RespawnPositionOutside.y;
                _direction = Vector2.down;
                break;
            case RESPAWN_TYPE.RIGHT:
                position.x = RespawnPositionOutside.x;
                position.y = Random.Range(-RespawnPositionInside.y, RespawnPositionInside.y);
                _direction = Vector2.left;
                break;
            case RESPAWN_TYPE.DOWN:
                position.x = Random.Range(-RespawnPositionInside.x, RespawnPositionInside.x);
                position.y = -RespawnPositionOutside.y;
                _direction = Vector2.up;
                break;
            case RESPAWN_TYPE.LEFT:
                position.x = -RespawnPositionOutside.x;
                position.y = Random.Range(
                    -RespawnPositionInside.y, RespawnPositionInside.y);
                _direction = Vector2.right;
                break;
        }
        transform.localPosition = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Player"))
        {
            var player = collision.GetComponent<Player>();
            player.Damage(Damage);
            Destroy(gameObject);
            return;
        }
        if (collision.name.Contains("Shot"))
        {
            Instantiate(ExplosionPrefab, collision.transform.localPosition, Quaternion.identity);
            Destroy(collision.gameObject);
            _hp--;
            if (0 < _hp) return;
            var audioSource = FindObjectOfType<AudioSource>();
            audioSource.PlayOneShot(DeathClip);
            Destroy(gameObject);

            var exp = Exp;
            while (0 < exp)
            {
                var gemPrefabs = GemPrefabs.Where(gemPrefab => gemPrefab.Exp <= exp).ToArray();
                var gemPrefab = gemPrefabs[Random.Range(0, gemPrefabs.Length)];
                var gem = Instantiate(gemPrefab, transform.localPosition, Quaternion.identity);
                gem.Init(exp, GemSpeedMin, GemSpeedMax);
                exp -= gem.Exp;
            }
        }
    }
}
