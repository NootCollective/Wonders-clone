using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceRenderer : CustomRenderer
{
    [SerializeField] ResourceType resource;
    [SerializeField] int amount = 1;
    [Header("Links")]
    [SerializeField] SpriteRenderer icon;
    [SerializeField] SpriteRenderer border;
    [SerializeField] SpriteRenderer background;
    [SerializeField] TextMesh amountDisplay;

    public Sprite[] icons;
    // Start is called before the first frame update
   
    public void Set(ResourceType type, int amount = 1)
    {
        this.resource = type;
        this.amount = amount;
        Redraw();
    }

    private void OnValidate()
    {
        Redraw();
    }

    public override void Redraw()
    {
        icon.sprite = icons[(int)resource];
        background.color = Resource.resourceColor[(int)resource];
        amountDisplay.gameObject.SetActive(amount > 1);
        amountDisplay.text = amount.ToString();
    }
}
