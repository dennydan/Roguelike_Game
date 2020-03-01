using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    bool isFacingRight = false;

    public void LookAt_Player(Transform player)
    {
        if (transform.position.x < player.position.x && !isFacingRight)
        {
            Filp();
            Debug.Log(isFacingRight);
        }
        else if (transform.position.x > player.position.x && isFacingRight)
        {
            Filp();
            Debug.Log(isFacingRight);
        }
    }

    void Filp()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
