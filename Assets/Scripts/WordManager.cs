using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    [SerializeField]
    private string Word = "hola";
    [SerializeField]
    private WordLetter letterPrefab;
    [SerializeField]
    private Transform LettersContainer;

    private List<WordLetter> Letters;
    

    private void Start()
    {
        Letters = new List<WordLetter>();
        foreach (Transform child in LettersContainer)
        {
            Destroy(child.gameObject);
        }

        CreateLetters(Word);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            bool result = CheckLetters();
            Debug.Log("result:" + result);
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
            WordLetter newLetter = Instantiate(letterPrefab, LettersContainer);
            newLetter.Letter = ""+list[i];

            Letters.Add(newLetter);
        }

    }

    public bool CheckLetters()
    {
        string currentWord = "";
        
        foreach (var wordLetter in Letters)
        {
            currentWord += wordLetter.Letter;
        }
        Debug.Log("current word:" + currentWord);

        return currentWord == Word;
    }
}
