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
        cards.Clear();
        cardsContainer.Clear();

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

        CheckIfShouldReset();
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

    void CheckIfShouldReset()
    {
        foreach (var card in cards)
        {
            if (!card.isRemoved)
            {
                return;
            }
        }
        StartCoroutine(NextChallengeRoutine());
    }

    private void Reset()
    {
        // shuffle letters
        for (int i = 0; i < letters.Length; i++)
        {
            string temp = letters[i];
            int randomIndex = UnityEngine.Random.Range(i, letters.Length);
            letters[i] = letters[randomIndex];
            letters[randomIndex] = temp;
        }

        letterIndex = 0;

        BuildCards();
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

    IEnumerator NextChallengeRoutine()
    {
        yield return new WaitForSeconds(0.6f);
        Reset();
    }
}
