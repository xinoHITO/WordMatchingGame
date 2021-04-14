using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class WordLetter : MonoBehaviour
{
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

    void Awake()
    {
        LetterLabel = GetComponentInChildren<TMP_Text>();
    }

}
