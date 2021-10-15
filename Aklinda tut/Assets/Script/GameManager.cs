using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Sprite bgImage; // Arkaplan resmi

    [SerializeField]
    public GameObject panel;

    public Sprite[] puzzles;
    public List<Sprite> gamePuzzles = new List<Sprite>();

    public List<Button> btns = new List<Button>(); // Kartlar icin butonlar

    private bool firstGuess, secondGuess;
    private int countGuesses;
    private int correctGuesses;
    private int gameGuesses;

    private int firstGuessIndex, secondGuessIndex;
    private string firstGuessPuzzle, secondGuessPuzzle;

    private void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("Picture/Resimler");

    }

    private void Start()
    {
        panel.SetActive(false);
        // Butonlari taglarına göre listeye ekle
        GetButtons();

        // Her butona listener ekle
        AddListeners();

        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
    }

    // Butonları taglarına göre çekip arkaplanları ekleyen fonksiyon
    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

        for (var i = 0; i < objects.Length; i ++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }
    }

    // Bu fonksiyonda kart sayısının yarısı kadar gamePuzzle seçiliyor ve ekleniyor
    void AddGamePuzzles()
    {
        int looper = btns.Count;
        int index = 0;

        for (int i = 0; i < looper; i ++)
        {
            if (index == looper / 2)
            {
                index = 0;
            }

            gamePuzzles.Add(puzzles[index]);
            index++; 
        }
    }

    void AddListeners()
    {
        foreach(Button btn in btns)
        {
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }

    public void PickAPuzzle()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(name);
            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;
            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
        }
        else if (!secondGuess) 
        {
            secondGuess = true;
            secondGuessIndex = int.Parse(name);
            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;
            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];
            countGuesses++;

            StartCoroutine(CheckIfThePuzzlesMatch());
        }
    }

    // Eşleşme gerçekleşti
    IEnumerator CheckIfThePuzzlesMatch()
    {
        yield return new WaitForSeconds(1f);

        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(.5f);
            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);

            PuanSistemi.GetInstance().PuanArttir();
            CheckIfTheGameIsFinished();
        }
        else
        {
            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
        }
        yield return new WaitForSeconds(.5f);

        firstGuess = secondGuess = false;
    }

    // Oyunun bitip bitmediğini kontrol eden fonksiyon
    void CheckIfTheGameIsFinished()
    {
        correctGuesses++;

        if (correctGuesses == gameGuesses)
        {
            Debug.Log("Oyun bitti!");
            Debug.Log("Deneme sayisi: " + countGuesses);
            panel.SetActive(true);
            panel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "OYUN BİTTİ! DENEME SAYISI: " + countGuesses;
            PlayerPrefs.SetInt("levelAt", PlayerPrefs.GetInt("levelAt") + 1);
        }
    }

    void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i ++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void MenuyeDon()
    {
        SceneManager.LoadScene("BaslangicScene");
    }
}
