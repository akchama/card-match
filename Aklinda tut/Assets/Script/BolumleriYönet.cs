using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BolumleriYönet : MonoBehaviour
{
    public static int olusacakKare = 0;
    public GameObject panel;
    public GameObject button;
    public int LevelSayisi = 5;
    public int levelAt;

    private void Start()
    {
        levelAt = PlayerPrefs.GetInt("levelAt", 0);

        for (var i = 0; i < LevelSayisi; i ++)
        {
            GameObject level = Instantiate(button) as GameObject;
            level.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Level " + (i + 1).ToString();
            level.transform.SetParent(panel.transform, false);
            level.name = "" + i;
            Button tempButton = level.GetComponent<Button>();
            int tempInt = i;

            tempButton.onClick.AddListener(() => ButtonClicked(tempInt));

            if (int.Parse(level.name) > levelAt)
            {
                Debug.Log(int.Parse(level.name));
                tempButton.interactable = false;
                tempButton.GetComponent<Image>().color = Color.red;
            }
            else
            {
                tempButton.interactable = true;
            }
        }
    }

    void ButtonClicked(int level)
    {
        olusacakKare = (level + 1) * 2;
        SceneManager.LoadScene("Oyunekranı");
    }

    public void MenuyeDon()
    {
        SceneManager.LoadScene("BaslangicScene");
    }
}
