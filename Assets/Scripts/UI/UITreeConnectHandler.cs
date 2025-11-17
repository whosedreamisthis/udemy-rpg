using System;
using Unity.Cinemachine;
using UnityEngine;

[Serializable]
public class UITreeConnectDetails
{
    public UITreeConnectHandler childNode;
    public NodeDirectionType direction;

    [Range(100f, 350f)]
    public float length;
}

public class UITreeConnectHandler : MonoBehaviour
{
    private RectTransform rect;

    [SerializeField]
    private UITreeConnectDetails[] connectionDetails;

    [SerializeField]
    private UITreeConnection[] connections;

    private void OnValidate()
    {
        if (rect == null)
        {
            rect = GetComponent<RectTransform>();
        }
        if (connectionDetails.Length != connections.Length)
        {
            Debug.Log($"The number should be the same as number of connections: {gameObject.name}");
        }
        UpdateConnections();
    }

    private void UpdateConnections()
    {
        for (int i = 0; i < connectionDetails.Length; i++)
        {
            var detail = connectionDetails[i];
            var connection = connections[i];
            Vector2 targetPosition = connection.GetConnectionPoint(rect);

            connection.DirectConnection(detail.direction, detail.length);
            detail.childNode.SetPosition(targetPosition);
        }
    }

    public void SetPosition(Vector2 position) => rect.anchoredPosition = position;
}
