using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public Shot ShotPrefab;
    public float ShotSpeed;
    public float ShotAngleRange;
    public float ShotTimer;
    public int ShotCount;
    public float ShotInterval;
    public int HpMax;
    public int Hp;
    public static Player Instance;
    public float MagnetDistance;
    public int NextExpBase;
    public int NextExpInterval;
    public int Level;
    public int Exp;
    public int PrevNeedExp;
    public int NeedExp;
    public AudioClip LevelUpClip;
    public AudioClip DamageClip;
    public int LevelMax;
    public int ShotCountFrom;
    public int ShotCountTo;
    public float ShotIntervalFrom;
    public float ShotIntervalTo;
    public float MagnetDistanceFrom;
    public float MagnetDistanceTo;

    void Awake()
    {
        Instance = this;
        Hp = HpMax;
        Level = 1;
        NeedExp = GetNeedExp(1);
        ShotCount = ShotCountFrom;
        ShotInterval = ShotIntervalFrom;
        MagnetDistance = MagnetDistanceFrom;
    }

    void Update()
    {
        var enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager.Congraturations)
        {
            return;
        }
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        var velocity = new Vector3(h, v) * Speed;
        transform.localPosition += velocity * Time.deltaTime;
        transform.localPosition = Utils.ClampPosition(transform.localPosition);

        var screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        var direction = Input.mousePosition - screenPosition;
        var angle = Utils.GetAngle(Vector3.zero, direction);
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        ShotTimer += Time.deltaTime;
        if (ShotTimer < ShotInterval) return;
        ShotTimer = 0;
        ShootNWay(angle, ShotAngleRange, ShotSpeed, ShotCount);
    }
    private void ShootNWay(
    float angleBase, float angleRange, float speed, int count)
    {
        var pos = transform.localPosition;
        var rot = transform.localRotation;
        if (1 < count)
        {
            for (int i = 0; i < count; ++i)
            {
                var angle = angleBase +
                    angleRange * ((float)i / (count - 1) - 0.5f);
                var shot = Instantiate(ShotPrefab, pos, rot);
                shot.Init(angle, speed);
            }
        }
        // 弾を 1 つだけ発射する場合
        else if (count == 1)
        {
            var shot = Instantiate(ShotPrefab, pos, rot);
            shot.Init(angleBase, speed);
        }
    }

    public void Damage(int damage)
    {
        Hp -= damage;
        var audioSource = FindObjectOfType<AudioSource>();
        audioSource.PlayOneShot(DamageClip);
        if (0 < Hp) return;
        gameObject.SetActive(false);
    }

    public void AddExp(int exp)
    {
        Exp += exp;
        if (Exp < NeedExp) return;
        Level++;
        PrevNeedExp = NeedExp;
        NeedExp = GetNeedExp(Level);
        var angleBase = 0;
        var angleRange = 360;
        var count = 28;
        ShootNWay(angleBase, angleRange, 15f, count);
        ShootNWay(angleBase, angleRange, 20f, count);
        ShootNWay(angleBase, angleRange, 25f, count);
        var audioSource = FindObjectOfType<AudioSource>();
        audioSource.PlayOneShot(LevelUpClip);
        var t = (float)(Level - 1) / (LevelMax - 1);
        ShotCount = Mathf.RoundToInt(Mathf.Lerp(ShotCountFrom, ShotCountTo, t));
        ShotInterval = Mathf.Lerp(ShotIntervalFrom, ShotIntervalTo, t);
        MagnetDistance = Mathf.Lerp(MagnetDistanceFrom, MagnetDistanceTo, t);
    }

    public int GetNeedExp(int level)
    {
        return NextExpBase + NextExpInterval * ((Level - 1) * (Level - 1));
    }
}
