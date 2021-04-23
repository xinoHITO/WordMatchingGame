using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WordManager : MonoBehaviour
{
    [SerializeField]
    private string Word = "hola";

    [SerializeField]
    private float LetterSpacing = 200;

    [SerializeField]
    private WordLetter LetterPrefab;
    [SerializeField]
    private TransparentLetter TransparentLetter;
    [SerializeField]
    private Transform LettersContainer;

    private List<WordLetter> Letters;
    private WordLetter SelectedLetter;

    private void Start()
    {
        Letters = new List<WordLetter>();
        foreach (Transform child in LettersContainer)
        {
            Destroy(child.gameObject);
        }
        TransparentLetter.HideLetter();
        CreateLetters(Word);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            bool result = CheckLetters();
            Debug.Log("result:" + result);
        }

        if (Input.GetMouseButton(0))
        {
            ReorderLetters();
        }

        if (Input.GetMouseButtonUp(0))
        {
            TransparentLetter.HideLetter();
            SelectedLetter = null;
        }
    }

    private RectTransform LetterToMove;

    private void ReorderLetters()
    {
        RectTransform TransparentLetterTransform = TransparentLetter.GetComponent<RectTransform>();
        if (SelectedLetter != null && LetterToMove == null)
        {
            Vector3 pos = TransparentLetterTransform.anchoredPosition;

            for (int i = 0; i < Letters.Count; i++)
            {
                RectTransform letterTransform = Letters[i].GetComponent<RectTransform>();
                var letterMin = letterTransform.anchoredPosition.x - (letterTransform.sizeDelta.x * 0.5f);
                var letterMax = letterTransform.anchoredPosition.x + (letterTransform.sizeDelta.x * 0.5f);

                if (Letters[i] != SelectedLetter &&
                    pos.x > letterMin && pos.x < letterMax)
                {
                    Debug.DrawLine(letterTransform.position, letterTransform.position + (Vector3.up * 100), Color.green, 0.5f);
                    LetterToMove = letterTransform;
                    //Start tween to swap letters around
                    //TODO: Detect when tween finishes and reset LetterToMove to null
                    //TODO: Find a way to handle multiple animations moving letters at the same time.
                    RectTransform selectedLetterTransform = SelectedLetter.GetComponent<RectTransform>();
                    LetterToMove.DOMoveX(selectedLetterTransform.position.x, 0.5f).OnComplete(OnLetterSwapFinished);
                    selectedLetterTransform.DOMoveX(letterTransform.position.x, 0.5f);
                    Debug.Log(string.Format("<color=yellow>posX: {0} | letter[{1}] X:{2} min:{3} max:{4}</color>", pos.x, i, letterTransform.anchoredPosition.x, letterMin, letterMax));
                    Debug.Log("TWEEN STARTED");

                    break;
                }
                else
                {
                    Debug.Log(string.Format("<color=red>posX: {0} | letter[{1}] X:{2} min:{3} max:{4}</color>", pos.x, i, letterTransform.anchoredPosition.x, letterMin, letterMax));
                }
            }
            Debug.Log("-------------------");
        }

    }

    private void OnLetterSwapFinished()
    {
        Debug.Log("TWEEN FINISHED");
        LetterToMove = null;
    }

    private void CreateLetters(string word)
    {
        List<char> list = new List<char>();

        for (int i = 0; i < word.Length; i++)
        {
            list.Add(word[i]);
        }

        for (int i = 0; i < word.Length; i++)
        {
            int randomIndex = Random.Range(0, word.Length);
            var temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

        for (int i = 0; i < word.Length; i++)
        {
            WordLetter newLetter = Instantiate(LetterPrefab, LettersContainer);
            newLetter.Letter = "" + list[i];
            newLetter.OnLetterPressed += OnLetterPressed;

            float posX = 0;
            int halfLetters = word.Length / 2;
            //odd
            posX = (i - halfLetters) * (LetterSpacing);
            //even
            if (word.Length % 2 == 0)
            {
                posX += LetterSpacing * 0.5f;
            }

            newLetter.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
            Letters.Add(newLetter);
        }

        StartCoroutine(UnparentLetterCoroutine());

        IEnumerator UnparentLetterCoroutine()
        {
            yield return null;
            var originalParent = Letters[0].GetComponent<RectTransform>().parent;
            foreach (WordLetter letter in Letters)
            {
                var letterTransform = letter.GetComponent<RectTransform>();
                letterTransform.SetParent(transform.parent);
            }
        }

    }

    private void OnLetterPressed(WordLetter wordLetter)
    {
        SelectedLetter = wordLetter;
        TransparentLetter.Letter = wordLetter.Letter;
        TransparentLetter.ShowLetter();
    }

    public bool CheckLetters()
    {
        string currentWord = "";

        foreach (Transform child in LettersContainer)
        {
            WordLetter wordLetter = child.GetComponent<WordLetter>();
            currentWord += wordLetter.Letter;
        }
        Debug.Log("current word:" + currentWord);

        return currentWord == Word;
    }
}
