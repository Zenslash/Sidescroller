using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmartCreatureView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI    actionLbl;
    [SerializeField] private Image              healthBar;

    public void UpdateAction(GoapAction action)
    {
        actionLbl.text = action.GetType().ToString();
    }
}
