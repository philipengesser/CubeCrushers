using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    // The 1 based level index
    public int LevelIndex;
    public TextMeshProUGUI Text;
    public Button Button;
    public List<Image> StarImages;



    // Start is called before the first frame update
    void Start()
    {
        Text.text = LevelIndex.ToString();
        if (LevelIndex > 20)
            Button.image.color = Color.red;
        else if (LevelIndex > 10)
            Button.image.color = Color.yellow;
        else
            Button.image.color = Color.green;

        if (GlobalData.s.LevelsCleared.ContainsKey(LevelIndex))
        {
            for (int i = 0; i < GlobalData.s.LevelsCleared[LevelIndex]; i++)
            {
                StarImages[i].color = Color.white;
            }
        }

        Button.onClick.AddListener(() =>
        {
            GlobalData.s.CurrentLevelIndex = LevelIndex;
            MainMenuObjectReferencer.s.LevelSelectionScreen.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
