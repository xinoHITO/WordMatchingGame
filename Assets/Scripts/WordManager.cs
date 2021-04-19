using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    [SerializeField]
    private string Word = "hola";
    [SerializeField]
    private WordLetter LetterPrefab;
    [SerializeField]
    private TransparentLetter TransparentLetter;
    [SerializeField]
    private Transform LettersContainer;

    private List<WordLetter> Letters;
    private List<float> LetterPositions;
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

    private void ReorderLetters()
    {
        if (SelectedLetter != null)
        {
            Vector3 pos = TransparentLetter.GetComponent<RectTransform>().anchoredPosition;

            for (int i = 0; i < Letters.Count; i++)
            {
                RectTransform letterTransform = Letters[i].GetComponent<RectTransform>();
                var letterMin = LetterPositions[i] - (letterTransform.sizeDelta.x * 0.5f);
                var letterMax = LetterPositions[i] + (letterTransform.sizeDelta.x * 0.5f);
                Debug.Log(string.Format("letter X:{0} | min:{1} | max:{2}", letterTransform.anchoredPosition.x, letterMin, letterMax));

                if (pos.x > letterMin && pos.x < letterMax)
                {
                    Debug.DrawLine(letterTransform.position, letterTransform.position + (Vector3.up * 100), Color.green);
                    SelectedLetter.GetComponent<RectTransform>().SetSiblingIndex(i);
                    break;
                }
            }
            Debug.Log("-------------------");
        }
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
            Letters.Add(newLetter);
        }

        StartCoroutine(UnparentLetterCoroutine());

        IEnumerator UnparentLetterCoroutine()
        {
            yield return null;
            LetterPositions = new List<float>();
            var originalParent = Letters[0].GetComponent<RectTransform>().parent;
            foreach (WordLetter letter in Letters)
            {
                var letterTransform = letter.GetComponent<RectTransform>();
                letterTransform.SetParent(transform.parent);
            }
            yield return null;
            foreach (WordLetter letter in Letters)
            {
                var letterTransform = letter.GetComponent<RectTransform>();
                LetterPositions.Add(letterTransform.anchoredPosition.x);
                letterTransform.SetParent(originalParent);
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
