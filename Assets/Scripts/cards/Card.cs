using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Card : VisualElement
{
    public static event Action<string> onFlip;
    public string letter;
    public bool isShown = false;
    public bool isRemoved = false;

    public Card(string letter)
    {
        AddToClassList("card");
        RegisterCallback<MouseDownEvent>(evt => Flip());
        this.letter = letter;

        var label = new Label(letter);
        Add(label);
    }

    public void Flip()
    {
        if (isRemoved) return;

        if (isShown)
        {
            isShown = false;
            RemoveFromClassList("shown");
        }
        else
        {
            isShown = true;
            AddToClassList("shown");
            onFlip?.Invoke(letter);
        }
    }

    public void Remove()
    {
        AddToClassList("removed");
        isRemoved = true;
    }

}
