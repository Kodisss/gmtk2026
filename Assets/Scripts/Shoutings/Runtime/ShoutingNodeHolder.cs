using UnityEngine;

public class ShoutingNodeHolder : MonoBehaviour
{
    [SerializeField] private ShoutingNode myNode;

    public ShoutingNode MyNode
    {
        get => myNode;
    }
}