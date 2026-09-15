using System;
using UnityEngine;
using UnityEngine.Rendering;

public class UIComponents : MonoBehaviour
{
    [SerializeField] private GameObject _uiSkills;
    private bool bSkillsActive = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SkillsShow();
        }
    }

    private void SkillsShow()
    {
        if (!bSkillsActive)
        {
            _uiSkills.SetActive(true);
            bSkillsActive = true;
            return;
        }
        _uiSkills.SetActive(false);
        bSkillsActive = false;
    }
}
