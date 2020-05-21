using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityRenderer : CustomRenderer
{
    public City city;
    CityData data;

    public ResourceRenderer productionRenderer;
    public SpriteRenderer background;
    public TextMesh nameRenderer;
    public CitySlotRenderer[] slots;

    public bool update = false;
    // Start is called before the first frame update
    void Start()
    {
        Redraw();
    }
    private void OnValidate()
    {
        //if (update)
        //{
       //     update = false;
            Redraw();
       // }
    }

    void Redraw()
    {
        data = city.data;
        if (!data)
        {
            return;
        }
        try
        {
            nameRenderer.text = data.name;
            background.color = data.color;

            productionRenderer.enabled = true;
            productionRenderer.Set(data.production);
        }catch(System.Exception e)
        {
            Debug.LogWarning(this.name + e.ToString());
        }
    }
}
