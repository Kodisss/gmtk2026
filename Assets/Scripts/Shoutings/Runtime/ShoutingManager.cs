using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShoutingNodeHolder))]
[RequireComponent(typeof(Collider2D))]
public class ShoutingManager : MonoBehaviour
{
    private ShoutingNode shoutingNode;

    [SerializeField] private ShoutingSlot[] listOfNPCs;

    [Header("Typing Settings")]
    [SerializeField] private float charactersPerSecond = 40f;

    [Header("Punctuation")]
    [SerializeField] private float commaPause = 0.15f;
    [SerializeField] private float periodPause = 0.35f;
    [SerializeField] private float ellipsisPause = 0.8f;

    [Header("Sound")]
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;
    [SerializeField] private int charactersPerBlip = 2;

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
            slot.SetupTypewriter(charactersPerSecond, commaPause, periodPause, ellipsisPause, minPitch, maxPitch, charactersPerBlip);

            npcSlots.Add(npc.npc, slot);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartTheShouting();
            GetComponent<Collider2D>().enabled = false;
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