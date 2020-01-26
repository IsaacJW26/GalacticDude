using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character3DMovement : Movement
{
    public const float CAM_SIZE = 4.787879f;
    public const float X_PIXELS = 240;
    public const float PIX_RATIO = 3.01f*CAM_SIZE / X_PIXELS;

    Vector2 realPosition;
    Vector2 cameraPos;

    void OnEnable()
    {
        realPosition = transform.position;
    }

    protected override void Move(Vector2 velocity, float deltaTime)
    {
        realPosition = (realPosition + (velocity * deltaTime));

        rb.MovePosition(SnapToPixel(realPosition));
    }

    /// <summary>
    /// Determines camera position based on realworld position
    /// </summary>
    /// <param name="position">Current actual world position</param>
    /// <returns></returns>
    public static Vector2 SnapToPixel(Vector2 position)
    {
        float xPixelNumber = position.x / PIX_RATIO;
        float yPixelNumber = position.y / PIX_RATIO;

        xPixelNumber = Mathf.Round(xPixelNumber);
        yPixelNumber = Mathf.Round(yPixelNumber);

        return new Vector2(xPixelNumber, yPixelNumber) * PIX_RATIO;
    }
}

