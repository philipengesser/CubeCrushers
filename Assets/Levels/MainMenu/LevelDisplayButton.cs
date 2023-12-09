using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplayButton : MonoBehaviour
{
    // The 1 based level index
    public TextMeshProUGUI Text;
    public Button Button;
    public List<Image> StarImages;



    // Start is called before the first frame update
    void Update()
    {

        int LevelIndex = GlobalData.s.CurrentLevelIndex;

        Text.text = "Level " + LevelIndex.ToString();
        if (LevelIndex > 20)
            Button.image.color = Color.red;
        else if (LevelIndex > 10)
            Button.image.color = Color.yellow;
        else
            Button.image.color = Color.green;

        foreach (var starImage in StarImages)
        {
            starImage.color = Color.black;
        }

        if (GlobalData.s.LevelsCleared.ContainsKey(LevelIndex))
        {
            for (int i = 0; i < GlobalData.s.LevelsCleared[LevelIndex]; i++)
            {
                StarImages[i].color = Color.white;
            }
        }

        Button.onClick.AddListener(() =>
        {
            MainMenuObjectReferencer.s.LevelSelectionScreen.SetActive(true);
        });
    }
}
