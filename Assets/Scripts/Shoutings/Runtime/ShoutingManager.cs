using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShoutingNodeHolder))]
public class ShoutingManager : MonoBehaviour
{
    private ShoutingNode shoutingNode;

    [SerializeField] private ShoutingSlot[] listOfNPCs;

    private Dictionary<ShoutingNPC, ShoutingSlot> npcSlots = new();


    private void Start()
    {
        shoutingNode = GetComponent<ShoutingNodeHolder>().MyNode;

        DrawTheSprite();
    }


    private void DrawTheSprite()
    {
        foreach (ShoutingNPCInstance npc in shoutingNode.npcs)
        {
            ShoutingSlot slot = listOfNPCs[(int)npc.position];

            slot.SetNPC(npc.npc);

            npcSlots.Add(npc.npc, slot);
        }
    }


    [ContextMenu("Test Shouting")]
    private void StartTheShouting()
    {
        StartCoroutine(ShoutingRoutine());
    }


    private IEnumerator ShoutingRoutine()
    {
        foreach (ShoutingLine line in shoutingNode.lines)
        {
            ShoutingSlot slot = npcSlots[line.speaker];

            slot.Say(line.text, line.typingSpeed);


            while (slot.IsTyping())
            {
                yield return null;
            }


            yield return new WaitForSeconds(line.waitAfter);

            slot.ClearText();
        }
    }
}