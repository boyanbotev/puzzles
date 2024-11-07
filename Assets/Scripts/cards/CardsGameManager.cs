using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class CardsGameManager : MonoBehaviour
{
    [SerializeField] private string[] letters;
    UIDocument uIDoc;
    VisualElement root;
    VisualElement cardsContainer;
    private int letterIndex = 0;
    private int rowCount = 2;
    private int cardPerRow = 4;
    private List<Card> cards = new List<Card>();
    private int cardsShown;

    private void Awake()
    {
        uIDoc = FindObjectOfType<UIDocument>();
        root = uIDoc.rootVisualElement;
        cardsContainer = root.Q<VisualElement>(className: "cards");
    }

    private void OnEnable()
    {
        Card.onFlip += OnCardFlip;
    }

    private void OnDisable()
    {
        Card.onFlip -= OnCardFlip;
    }

    private void Start()
    {
        BuildCards();
    }

    void BuildCards()
    {
        for (int i = 0; i < rowCount; i++)
        {
            BuildRow();
        }
    }

    void OnCardFlip(string letter)
    {
        var isWinning = false;
        foreach (var card in cards)
        {
            if (card.letter.ToLower() == letter.ToLower() && card.isShown && card.letter != letter)
            {
               StartCoroutine(CorrectCardsRoutine(letter));
                isWinning = true;
               cardsShown = 0;
            }
        }

        if (!isWinning)
        {
            cardsShown++;

            if (cardsShown == 2)
            {
                foreach (var card in cards)
                {
                    if (card.isShown)
                    {
                        StartCoroutine(FalseCardRoutine(card));
                    }
                }
                cardsShown = 0;
            }
        }

    }

    private void RemoveWinningCards(string letter)
    {
        foreach (var card in cards)
        {
            if (card.letter.ToLower() == letter.ToLower())
            {
                card.Remove();
            }
        }
    }

    VisualElement BuildRow()
    {
        var row = new VisualElement();
        row.AddToClassList("cards-row");
        cardsContainer.Add(row);

        for (int j = 0; j < cardPerRow; j++)
        {
            var card = new Card(letters[letterIndex]);
            letterIndex++;
            row.Add(card);
            cards.Add(card);
        }
        return row;
    }

    IEnumerator FalseCardRoutine(Card card)
    {
        yield return new WaitForSeconds(0.4f);
        card.Flip();
    }

    IEnumerator CorrectCardsRoutine(string letter)
    {
        yield return new WaitForSeconds(0.4f);
        RemoveWinningCards(letter);
    }
}
