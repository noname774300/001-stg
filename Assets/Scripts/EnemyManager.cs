using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public Enemy[] EnemyPrefabs;
    private float _timer;
    public float IntervalFrom;
    public float IntervalTo;
    public float ElapsedTimeMax;
    public float ElapsedTime;
    public bool Congraturations = false;

    void Awake()
    {
        Instance = this;
    }


    void Update()
    {
        if (Congraturations)
        {
            return;
        }
        if (ElapsedTime >= ElapsedTimeMax)
        {
            Congraturations = true;
            return;
        }
        ElapsedTime += Time.deltaTime;
        _timer += Time.deltaTime;
        var time = ElapsedTime / ElapsedTimeMax;
        var interval = Mathf.Lerp(IntervalFrom, IntervalTo, time);
        if (_timer < interval) return;
        _timer = 0;
        var enemyIndex = Random.Range(0, EnemyPrefabs.Length);
        var enemyPrefab = EnemyPrefabs[enemyIndex];
        var enemy = Instantiate(enemyPrefab);
        var respawnType = (RESPAWN_TYPE)Random.Range(0, (int)RESPAWN_TYPE.SIZEOF);
        enemy.Init(respawnType);
    }
}
