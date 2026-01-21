using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public enum UpgradeType { Weapon, Passive }

public enum PassiveType { Damage, FireRate, Health, Speed }

public class UpgradeOption
{
    public UpgradeType type;
    public WeaponData weapon;
    public PassiveType passive;

    public override string ToString()
    {
        if (type == UpgradeType.Weapon)
        {
            return $"{weapon.type} Weapon";
        }
        else
        {
            return $"{passive} Passive";
        }
    }
}

public class LevelUpUI : MonoBehaviour
{
    public GameObject levelUpPanel;
    public Button[] optionButtons = new Button[3];
    public TMPro.TextMeshProUGUI[] optionTexts = new TMPro.TextMeshProUGUI[3];

    private System.Action<int> onOptionSelected;
    public List<UpgradeOption> currentOptions;

    public void ShowLevelUpOptions(List<UpgradeOption> options, System.Action<int> callback)
    {
        currentOptions = options;
        onOptionSelected = callback;

        for (int i = 0; i < 3; i++)
        {
            optionTexts[i].text = options[i].ToString();
            int index = i; // Capture for lambda
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => SelectOption(index));
        }

        levelUpPanel.SetActive(true);
        Time.timeScale = 0f; // Pause game
    }

    private void SelectOption(int index)
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f; // Resume game
        onOptionSelected?.Invoke(index);
    }
}