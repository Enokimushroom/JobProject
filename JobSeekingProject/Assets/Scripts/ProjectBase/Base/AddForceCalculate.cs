using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceCalculate
{
    public static Vector2 CalculateFroce(Vector3 startPos,Vector3 endPos,float highestPosY)
    {
        float jumpGravity = Mathf.Abs(Physics2D.gravity.y);

        float height1 = highestPosY;
        float height2 = (highestPosY + startPos.y) - endPos.y;

        float time1 = Mathf.Sqrt(2f * height1 / jumpGravity);
        float time = time1 + Mathf.Sqrt(2f * height2 / jumpGravity);

        Vector2 dis = endPos - startPos;
        Vector2 vel = dis / time;

        Vector2 velvalue = vel + time1 * jumpGravity * Vector2.up;
        return velvalue;
    }
}
