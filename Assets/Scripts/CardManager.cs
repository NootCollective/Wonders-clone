using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public DeckData deck;
    public LocationDeck[] decks;
    public LocationDiscard[] discards;

    public ActionCard cardPrefab;

    public LocationWorld world;

    int turn = 0;
    int age = 1;
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "turn: " + turn);
    }

    void Generate()
    {
        MakeCards(age: 1);
        MakeCards(age: 2);
        MakeCards(age: 3);

    }
    // Start is called before the first frame update
    void Start()
    {
        Generate();
        StartAge();
    }

    void StartAge()
    {
        Distribute(decks[age - 1]);
        turn = 0;
    }
    void StartTurn()
    {
        //Do startof turn event
    }
    public void EndTurn()
    {
        AIPlay();
        ++turn;
        if (turn == 5)
        {
            DiscardLastCard();
            DoWar();
            if (age == 1 || age == 2)
            {
                ++age;
                StartAge();
            }
            else
            {
                EndGame();
            }
        }
        else
        {
            NextTurn();
        }
    }
    void DiscardLastCard()
    {
        for (int c = 0; c < world.cities.Length; ++c)
        {
            var player = world.cities[c].player;
            world.cities[c].hand.MoveTo(0, discards[age-1]);
        }
    }
    void DoWar()
    {
        int players = world.cities.Length;
        int[] shields = new int[players];
        for (int c = 0; c < players; ++c)
        {
            var player = world.cities[c].player;
            shields[c] = world.cities[c].MilitaryStrength;
        }
        for (int c = 0; c < players; ++c)
        {
            {
                int enemy = (c - 1) % players;
                if (c == 0)
                {
                    enemy = 6;
                }
               
                int comparison = shields[enemy].CompareTo(shields[c]);
                Debug.Log("WAR:" + c +"("+ shields[c] + ")"+ " vs " + enemy + "(" + shields[enemy] + ")" + "=" + comparison);
            }
            {
                int enemy = (c +1) % players;
                int comparison = shields[enemy].CompareTo(shields[c]);
                Debug.Log("WAR:" + c + "(" + shields[c] + ")" + " vs " + enemy + "(" + shields[enemy] + ")" + "=" + comparison);
            }
        }
    }
    void EndGame()
    {
        Debug.LogError("Game ended");
    }
    void NextTurn()
    {
        PassCards();
        StartTurn();
    }
    void AIPlay()
    {
        for (int c = 0; c < world.cities.Length; ++c)
        {
            var player = world.cities[c].player;
            if (player.AI)
            {
                player.DoPlay();
            }
        }
    }
    void PassCards(int direction = 1)
    {
        int players = world.cities.Length;
        var handContents = new ActionCard[players][];
        for (int p = 0; p < players; ++p)
        {
            handContents[p] = world.cities[p].hand.ExtractAll();
        }
        for (int p = 0; p < players; ++p)
        {
            world.cities[(p + direction) % players].hand.AddAll(handContents[p]);
        }
    }
    void MakeCards(int age)
    {
        var cardsData = age == 1 ? deck.age1 : age == 2 ? deck.age2 : deck.age3;
        List<ActionCard> guilds = new List<ActionCard>();
        foreach (var carddata in cardsData)
        {
            for (int c = 0; c < carddata.copies; ++c)
            {
                var card = Instantiate<ActionCard>(cardPrefab);
                card.data = carddata;
                card.name = card.data.name + "#" + c;
                card.Initialize();
                //cards.Add(card);
                if (card.data.type == CardType.Guild)
                {
                    guilds.Add(card);
                }
                else
                {
                    decks[age - 1].Add(card);
                }
            }
        }
        if (guilds.Count > 0)
        {
            int players = 7;
            for (int g = 0; g < players + 2; ++g)
            {
                int idx = Random.Range(0, guilds.Count);
                decks[3 - 1].Add(guilds[idx]);
                guilds.RemoveAt(idx);
            }
            foreach (var unused in guilds)
            {
                Destroy(unused.gameObject);
            }
        }
        decks[age - 1].Shuffle();
    }
    void Distribute(LocationDeck deck)
    {
        int p = 0;
        for (int idx = deck.cards.Count - 1; idx >= 0; --idx)
        {
            deck.MoveTo(idx, world.cities[p].hand);
            p = (p + 1) % world.cities.Length;
        }
    }

    void Hide(List<ActionCard> cards)
    {
        foreach (var card in cards)
        {
            card.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDiscard(ActionCard card)
    {
        discards[card.data.age].Add(card);
    }
    public void OnPlayerChangeStatus(int player, int prevStatus, int newStatus)
    {
    }
}
