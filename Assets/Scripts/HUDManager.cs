using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HUDManager : MonoBehaviour
{
    public Image life1, life2;
    public TextMeshProUGUI damagePercentileText;

    public void SetLives(int currentLives)
    {
        life1.enabled = currentLives >= 1;
        life2.enabled = currentLives >= 2;
    }
}
