using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    public GameObject fearPanel;
    public GameObject questionPanel;
    public void onButtonClick(Button _button)
    {
        switch (_button.tag)
        {
            case "SelectOceanButton":
                GameManager.Instance.fear = GameManager.Fear.Ocean;
                break;
            case "SelectSpaceButton":
                GameManager.Instance.fear = GameManager.Fear.Space;
                break;
        }
        fearPanel.SetActive(false);
        questionPanel.SetActive(true);
    }
}
