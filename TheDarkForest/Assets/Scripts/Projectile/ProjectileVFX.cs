using System;
using Player;
using UnityEngine;

public class ProjectileVFX : MonoBehaviour
{
    [SerializeField] private GameObject _vfx1;
    [SerializeField] private GameObject _vfx2;
    [SerializeField] private GameObject _vfx3;

    public void ApplyCastVFX(int skillId)
    {
        _vfx1.SetActive(false);
        _vfx2.SetActive(false);
        _vfx3.SetActive(false);

        switch (skillId)
        {
            case 0: _vfx1.SetActive(true); break;
            case 1: _vfx2.SetActive(true); break;
            case 2: _vfx3.SetActive(true); break;
        }
    }
}
