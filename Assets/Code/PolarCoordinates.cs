using System;
using UnityEngine;

[Serializable]
public struct PolarCoordinates
{

    /// <summary>
    /// 极坐标角度
    /// </summary>
    [SerializeField] private float angle;
    public float Angle
    {
        get
        {
            return angle;
        }
        set
        {

            if (value < -180)
            {
                angle = value + 360;
            }
            else if (value > 180)
            {
                angle = value - 360;
            }
            else
            {
                angle = value;
            }

        }
    }
    /// <summary>
    /// 极坐标半径
    /// </summary>
    public float Radius;

    public Vector2 GetPosition2()
    {

        float angleValue = angle * Mathf.PI / 180;

        return new Vector2
        {

            x = Radius * Mathf.Cos(angleValue),
            y = Radius * Mathf.Sin(angleValue),

        };

    }

    public Vector3 GetPosition3(float Height)
    {

        float angleValue = angle * Mathf.PI / 180;

        return new Vector3
        {

            x = Radius * Mathf.Cos(angleValue),
            y = Height,
            z = Radius * Mathf.Sin(angleValue)

        };

    }

    public void Vector2To(float x,float y)
    {
        Radius = Mathf.Sqrt(x * x + y * y);
        Angle = Mathf.Atan2(y, x) / Mathf.PI * 180;
    }

    public static PolarCoordinates Lerp(PolarCoordinates start, PolarCoordinates end, float time)
    {

        return new PolarCoordinates
        {
            
            angle = Mathf.LerpAngle(start.angle, end.angle, time),
            Radius = Mathf.Lerp(start.Radius, end.Radius, time)

        };

    }

}

public static class PolarCoordinatesExtension
{

    public static PolarCoordinates ToPolarCoordinates(this Vector2 pos)
    {

        return new PolarCoordinates
        {
            Radius = Mathf.Sqrt(pos.x * pos.x + pos.y * pos.y),

            Angle = Mathf.Atan2(pos.y, pos.x) / Mathf.PI * 180
        };

    }

    public static PolarCoordinates ToPolarCoordinates(this Vector3 pos)
    {

        return new PolarCoordinates
        {
            Radius = Mathf.Sqrt(pos.x * pos.x + pos.z * pos.z),

            Angle = Mathf.Atan2(pos.z, pos.x) / Mathf.PI * 180
        };

    }

}