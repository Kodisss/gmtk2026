using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private float offset;
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;
    [SerializeField] private Transform bossTarget;
    [SerializeField] private float yOffset;

    private Transform currentTarget;

    private void Start()
    {
        currentTarget = target;
    }

    public void SetOffset(float newOffset)
    {
        offset = newOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget == target) LockOnPlayer();
        else if(currentTarget == bossTarget) LockOnBossDialogue();
    }

    public void SwitchTarget()
    {
        currentTarget = bossTarget;
    }

    private void LockOnPlayer()
    {
        Vector3 targetPosition = currentTarget.position + new Vector3(offset, 0f, -10f);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    private void LockOnBossDialogue()
    {
        Vector3 targetPosition = currentTarget.position + new Vector3(offset, yOffset, -10f);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
