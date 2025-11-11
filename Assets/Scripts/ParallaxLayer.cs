using UnityEngine;

[System.Serializable]
public class ParallaxLayer
{
    [SerializeField]
    private Transform background;

    [SerializeField]
    private float parallaxMultiplier;

    [SerializeField]
    private float imageWidthOffset = 10;

    private float imageFullWidth;
    private float imageHalfWidth;

    public void Move(float distanceToMove)
    {
        background.position += Vector3.right * (distanceToMove * parallaxMultiplier);
    }

    public void CalculateImageWidth()
    {
        imageFullWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
        imageHalfWidth = imageFullWidth / 2;
    }

    public void LoopBackground(float cameraLeftEdge, float cameraRightEdge)
    {
        float imageRightEdge = (background.position.x + imageHalfWidth) - imageWidthOffset;
        float imageLeftEdge = (background.position.x - imageHalfWidth) + imageWidthOffset;
        // Debug.Log("background position x " + background.position.x);
        // Debug.Log("image half width " + imageHalfWidth);

        // Debug.Log(
        //     "imageLeftEdge "
        //         + imageLeftEdge
        //         + " imageRightEdge "
        //         + imageRightEdge
        //         + " cameraLeftEdge "
        //         + cameraLeftEdge
        //         + " cameraRightEdge "
        //         + cameraRightEdge
        // );

        if (imageRightEdge < cameraLeftEdge)
        {
            Debug.Log("imageRightEdge " + imageRightEdge + " cameraLeftEdge " + cameraLeftEdge);
            background.position += Vector3.right * imageFullWidth;
            Debug.Log(
                "looping background to the right "
                    + background.position.x
                    + " imageFullWidth "
                    + imageFullWidth
            );
        }
        else if (imageLeftEdge > cameraRightEdge)
        {
            Debug.Log("imageLeftEdge " + imageLeftEdge + " cameraRightEdge " + cameraRightEdge);
            background.position += Vector3.right * -imageFullWidth;
            Debug.Log(
                "looping background to the left "
                    + background.position.x
                    + " imageFullWidth "
                    + imageFullWidth
            );
        }
    }
}
