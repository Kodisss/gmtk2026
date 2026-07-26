using UnityEngine;

public class PropsStarter : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach(SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sortingLayerName = "Props";
        }
    }
}
