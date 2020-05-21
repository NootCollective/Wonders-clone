using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLocation : MonoBehaviour
{
    public List<ActionCard> cards = new List<ActionCard>();

    public int Count
    {
        get
        {
            return cards.Count;
        }
    }
    virtual public void Add(ActionCard card)
    {
        cards.Add(card);
        card.location = this;
    }
    virtual public void AddAll(IEnumerable<ActionCard> newCards)
    {
        foreach(var card in newCards)
        {
            Add(card);
        }
    }

    public bool ContainsInstance(CardData other)
    {
        foreach (var card in cards)
        {
            if (card.data == other)
            {
                return true;
            }
        }
        return false;
    }
    public ActionCard GetInstance(CardData other)
    {
        foreach (var card in cards)
        {
            if (card.data == other)
            {
                return card;
            }
        }
        return null;
    }

    virtual public void Extract(ActionCard card)
    {
        cards.Remove(card);
    }
    virtual public ActionCard ExtractAt(int idx)
    {
        var card = cards[idx];
        cards.RemoveAt(idx);
        return card;
    }
    virtual public ActionCard[] ExtractAll()
    {
        var result = cards.ToArray();
        cards.Clear();
        return result;
    }
    virtual public void MoveTo(ActionCard card,CardLocation target)
    {
        Extract(card);
        target.Add(card);
    }
    virtual public void MoveTo(int idx, CardLocation target)
    {
        var card = cards[idx];
        ExtractAt(idx);
        target.Add(card);
    }


    virtual public void Shuffle()
    {
        Shuffle(cards);
    }

    virtual public bool Contains(ActionCard card)
    {
        return cards.Contains(card);
    }


    public static void Shuffle<T>(List<T> elements)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            T temp = elements[i];
            int randomIndex = Random.Range(i, elements.Count);
            elements[i] = elements[randomIndex];
            elements[randomIndex] = temp;
        }
    }

}
