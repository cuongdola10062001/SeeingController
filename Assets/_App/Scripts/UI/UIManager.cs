using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI desPoseFullbodyText;
    public TextMeshProUGUI DesPoseFullbodyText=> desPoseFullbodyText;

    [SerializeField] private UISelection uiSelection;
    public UISelection UISelection => uiSelection;
}
