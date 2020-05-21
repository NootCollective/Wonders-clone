using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRenderer : CustomRenderer
{
    public ActionCard actionCard;
    public CardData data;

    public GameObject front;
        public ResourceRenderer[] costRenderer;
        public ResourceRenderer[] productionRenderer;
        public SpriteRenderer background;
        public TextMesh nameRenderer;
        public TextMesh chainRequirementRenderer;
        public TextMesh chainProvides1Renderer;
        public TextMesh chainProvides2Renderer;
        public TextMesh ageRenderer;
    public GameObject back;

    // Start is called before the first frame update
    void Start()
    {
        Redraw();
    }
    private void OnValidate()
    {
        Redraw();
    }

    public void Redraw()
    {
        if(actionCard == null)
        {
            actionCard = GetComponent<ActionCard>();
        }
        front.SetActive(actionCard.Visible);
        back.SetActive(!actionCard.Visible);

        if (actionCard.Visible)
        {
            data = actionCard.data;
            if (!data)
            {
                return;
            }
            nameRenderer.text = data.name;
            ageRenderer.text = data.age.ToString();
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
                costRenderer[c].gameObject.SetActive(true);
                costRenderer[c].Set(data.cost[c]);
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
                    productionRenderer[p].gameObject.SetActive(true);
                    productionRenderer[p].Set(production);

                    ++p;
                    if (p >= productionRenderer.Length)
                    {
                        break;
                    }
                }
            }

            for (; p < productionRenderer.Length; ++p)
            {
                productionRenderer[p].gameObject.SetActive(false);
            }
        }
    }
}
