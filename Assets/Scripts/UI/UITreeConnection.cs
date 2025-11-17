using Unity.Mathematics;
using UnityEngine;

public class UITreeConnection : MonoBehaviour
{
    [SerializeField]
    private RectTransform rotationPoint;

    [SerializeField]
    private RectTransform connectionLength;

    [SerializeField]
    private RectTransform childNodeConnectionPoint;

    private void Awake()
    {
        if (rotationPoint != null)
        {
            rotationPoint.localRotation = Quaternion.identity; // Quaternion.identity is (0, 0, 0, 1) rotation
        }
    }

    public Vector2 GetConnectionPoint(RectTransform rect)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect.parent as RectTransform,
            childNodeConnectionPoint.position,
            null,
            out var localPosition
        );
        return localPosition;
    }

    public void DirectConnection(NodeDirectionType direction, float length)
    {
        bool shouldBeActive = direction != NodeDirectionType.None;
        float finalLength = shouldBeActive ? length : 0;
        float angle = GetDirectionAngle(direction);
        Debug.Log($"euler {angle}");
        rotationPoint.localRotation = Quaternion.Euler(0, 0, angle);
        Debug.Log($"Final Local Z Rotation: {rotationPoint.localRotation.eulerAngles.z}");
        connectionLength.sizeDelta = new Vector2(finalLength, connectionLength.sizeDelta.y);
    }

    private float GetDirectionAngle(NodeDirectionType type)
    {
        switch (type)
        {
            case NodeDirectionType.Left:
                Debug.Log("returning 180 for length");
                return 180f;
            case NodeDirectionType.Right:
                return 0f;
            case NodeDirectionType.Up:
                return 90f;
            case NodeDirectionType.Down:
                return -90f;
            case NodeDirectionType.UpRight:
                return 45f;
            case NodeDirectionType.DownRight:
                return -45f;
            case NodeDirectionType.UpLeft:
                return 135f;
            case NodeDirectionType.DownLeft:
                return -135f;
            default:
                return 0f;
        }
    }
}

public enum NodeDirectionType
{
    None,
    UpLeft,
    Up,
    UpRight,
    Left,
    Right,
    DownLeft,
    Down,
    DownRight,
}
