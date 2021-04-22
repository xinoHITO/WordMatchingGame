using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class WordLetter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public delegate void OnLetterDelegate(WordLetter wordLetter);
    public OnLetterDelegate OnLetterPressed;
    public OnLetterDelegate OnLetterReleased;

    private Image Background;
    private TMP_Text LetterLabel;
    public string Letter
    {
        get
        {
            return LetterLabel.text;
        }
        set
        {
            LetterLabel.text = value;
        }
    }

    private Color DefaultBackgroundColor;
    private Color DefaultLetterColor;

    void Awake()
    {
        Background = GetComponent<Image>();
        LetterLabel = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        DefaultBackgroundColor = Background.color;
        DefaultLetterColor = LetterLabel.color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnLetterPressed?.Invoke(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnLetterReleased?.Invoke(this);
    }

    public void HideLetter()
    {
        Color newColor = Background.color;
        newColor.a = 0;
        Background.color = newColor;

        Color newLetterColor = LetterLabel.color;
        newLetterColor.a = 0;
        LetterLabel.color = newLetterColor;
    }

    public void ShowLetter()
    {
        Background.color = DefaultBackgroundColor;
        LetterLabel.color = DefaultLetterColor;
    }
}
