using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRenderer : MonoBehaviour
{
    public ActionCard actionCard;
    public CardData data;

    public SpriteRenderer[] costRenderer;
    public SpriteRenderer[] productionRenderer;
    public SpriteRenderer background;
    public TextMesh nameRenderer;
    public TextMesh chainRequirementRenderer;
    public TextMesh chainProvides1Renderer;
    public TextMesh chainProvides2Renderer;
    public TextMesh ageRenderer;

    // Start is called before the first frame update
    void Start()
    {
        Redraw();
    }
    private void OnValidate()
    {
        Redraw();
    }

    void Redraw()
    {
        data = actionCard.data;
        if (!data)
        {
            return;
        }
        nameRenderer.text = data.name;
        ageRenderer.text = data.age.ToString() ;
        background.color = CardData.CardColor[(int)data.type];
        if (data.chainRequirement != null)
        {
            chainRequirementRenderer.gameObject.SetActive(true);
            chainRequirementRenderer.text = data.chainRequirement.name;
        }
        else
        {
            chainRequirementRenderer.gameObject.SetActive(false);
        }

        if (data.chainProvides != null)
        {
            if (data.chainProvides.Length == 2)
            {
                chainProvides1Renderer.gameObject.SetActive(true);
                chainProvides2Renderer.gameObject.SetActive(true);
                chainProvides1Renderer.text = data.chainProvides[0].name;
                chainProvides2Renderer.text = data.chainProvides[1].name;

            }
            else if (data.chainProvides.Length == 1)
            {
                chainProvides1Renderer.text = data.chainProvides[0].name;
                chainProvides2Renderer.gameObject.SetActive(true);
                chainProvides2Renderer.gameObject.SetActive(false);
            }
            else
            {
                chainProvides1Renderer.gameObject.SetActive(false);
                chainProvides2Renderer.gameObject.SetActive(false);
            }
        }
        else
        {
            chainRequirementRenderer.gameObject.SetActive(false);
        }

        for (int c = 0; c < data.cost.Length; ++c)
        {
            costRenderer[c].enabled = true;
            costRenderer[c].color = Resource.resourceColor[(int)data.cost[c]];
        }
        for (int c = data.cost.Length; c < costRenderer.Length; ++c)
        {
            costRenderer[c].gameObject.SetActive(false);
        }

        int p = 0;
        foreach (var option in data.production)
        {
            if (p >= productionRenderer.Length)
            {
                break;
            }
            foreach (ResourceType production in option.content)
            {
                productionRenderer[p].enabled = true;
                productionRenderer[p].color = Resource.resourceColor[(int)production];

                ++p;
                if (p >= productionRenderer.Length)
                {
                    break;
                }
            }
        }

        for (; p < productionRenderer.Length; ++p)
        {
            productionRenderer[p].enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
