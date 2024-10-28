using UnityEngine;
using UnityEngine.UI;

public class UI_BattleBackground_Manager : MonoBehaviour
{

    public Image battleBackground;
    public Sprite[] backgroundImages;
    
    // Start is called before the first frame update
    void Start()
    {
        ChangeBackground();
    }

    public void ChangeBackground()
    {
        int randomIndex = Random.Range(0, backgroundImages.Length);
        battleBackground.sprite = backgroundImages[randomIndex];
    }
}
