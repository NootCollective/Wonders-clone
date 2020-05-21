using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CardManager manager;
    public City city;
    new public Camera camera;
    public ActionCard hovered;

    public bool AI = false;
    public int option = 0;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    public void SetOption(int option)
    {
        this.option = option;
    }

    void OnGUI()
    {

        GUI.Label(new Rect(10, 50, 100, 20), "Gold=" + city.Money.ToString());
        int y = 0;
        city.ComputeOwnResources();
        foreach (var resource in city.resources.Keys)
        {
            GUI.Label(new Rect(10, 70 + y * 20, 100, 20), resource.ToString() + "=" + city.resources[resource]);
            ++y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            hovered = objectHit.gameObject.GetComponent<ActionCard>();

            if (hovered != null)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    if (!city.hand.Contains(hovered))
                    {
                        Debug.LogWarning("This is not your hand");
                    }
                    else
                    {
                        if (option == 0)
                        {
                            if (TryPlay(hovered))
                            {
                                Debug.Log("you can play this card");
                                manager.EndTurn();
                            }
                        }
                        else if (option == 1)
                        {
                            Debug.Log("Marvel");
                            if (city.BuildMarvel(hovered))
                            {
                                manager.EndTurn();
                            }
                        }
                        if (option == 2)
                        {
                            Debug.Log("Discarding");
                            city.Discard(hovered);
                            manager.EndTurn();
                        }
                    }
                }
            }
        }
        else
        {
            hovered = null;
        }
    }

    bool TryPlay(ActionCard card)
    {
        if (city.CanPlay(card))
        {
            city.Play(card);
            city.hand.cards.Remove(card);
            return true;
        }
        else
        {
            return false;
        }
    }


    public void DoPlay()
    {
        foreach (var card in city.hand.cards)
        {
            if (TryPlay(card))
            {
                return;
            }
        }
        city.Discard(city.hand.ExtractAt(0));
    }
}
