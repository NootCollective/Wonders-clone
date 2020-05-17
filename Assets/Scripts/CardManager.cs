using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public DeckData deck;
    public ActionCard cardPrefab;
    public List<ActionCard> cards;
    // Start is called before the first frame update
    void Start()
    {
        int x = 0;
        int y = 0;
        foreach(var carddata in deck.cards)
        {
            var card = Instantiate<ActionCard>(cardPrefab);
            card.data = carddata;
            card.transform.position = new Vector3(x*3*2, y*5*2);
            ++x;
            if( x > 10)
            {
                x = 0;
                ++y;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
