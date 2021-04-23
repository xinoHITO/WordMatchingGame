using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowTransparentLetterPosition : MonoBehaviour
{
    public RectTransform TransparentLetter;
    private TMP_Text Label;

    // Start is called before the first frame update
    void Start()
    {
        Label = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Label.text = ""+TransparentLetter.anchoredPosition.x;
    }
}
