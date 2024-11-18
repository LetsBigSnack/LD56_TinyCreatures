using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ToggleButton : MonoBehaviour
{
    private Image _image;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite deactiveSprite;

    private void OnEnable()
    {
        _image = GetComponent<Image>();
        SetToggleState(BattleManager.Instance.AutoBattle);
    }
    public void SetToggleState(bool state)
    {
        Debug.Log("SetToggleState called");
        Debug.Log("_renderer " + _image);
        if (_image == null) return;
        
        if(state) {
            _image.sprite = activeSprite;
        } else {
            _image.sprite = deactiveSprite;
        }
    }
}
