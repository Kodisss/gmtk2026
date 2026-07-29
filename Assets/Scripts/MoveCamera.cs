using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private float offset;
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform playerTarget;
    private Transform talkableTarget;
    [SerializeField] private float yOffset;

    private bool talkingToBoss = false;

    public void SetOffset(float newOffset)
    {
        offset = newOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (!talkingToBoss) LockOnPlayer();
        else if(talkingToBoss) LockOnBossDialogue();
    }

    public void WeTalkToSomeoneNow(Transform newTalkableTarget)
    {
        talkingToBoss = true;
        talkableTarget = newTalkableTarget;
    }

    public void WeDontTalkAnymore()
    {
        talkingToBoss = false;
        talkableTarget = null;
    }

    private void LockOnPlayer()
    {
        Vector3 targetPosition = playerTarget.position + new Vector3(offset, 0f, -10f);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    private void LockOnBossDialogue()
    {
        if(talkableTarget == null)
        {
            LockOnPlayer();
            return;
        }

        float middleX = (playerTarget.position.x + talkableTarget.position.x) / 2;
        float middleY = (playerTarget.position.y + talkableTarget.position.y) / 2;

        Vector3 targetPosition = new Vector3(middleX, middleY + yOffset, -10f);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
