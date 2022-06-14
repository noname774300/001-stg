using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private Vector3 _velocity;

    void Update()
    {
        var enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager.Congraturations)
        {
            return;
        }
        transform.localPosition += _velocity * Time.deltaTime;
    }

    public void Init(float angle, float speed)
    {
        var direction = Utils.GetDirection(angle);
        _velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;
        Destroy(gameObject, 2);
    }
}
