using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform Player;
    public Vector2 Limit;

    private void Update()
    {
        var pos = Player.localPosition;
        var limit = Utils.MoveLimit;
        var tx = 1 - Mathf.InverseLerp(-limit.x, limit.x, pos.x);
        var ty = 1 - Mathf.InverseLerp(-limit.y, limit.y, pos.y);
        var x = Mathf.Lerp(-Limit.x, Limit.x, tx);
        var y = Mathf.Lerp(-Limit.y, Limit.y, ty);
        transform.localPosition = new Vector3(x, y, 0);
    }
}