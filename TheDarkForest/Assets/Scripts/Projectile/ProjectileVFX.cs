using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class ProjectileVFX : MonoBehaviour
{
    [SerializeField] private List<GameObject> _vfxPrefabs;

    public void ApplyCastVFX(int skillId)
    {
        foreach (var vfx in _vfxPrefabs)
        {
            vfx.SetActive(false);
        }
        if (skillId != -1)
            _vfxPrefabs[skillId].SetActive(true);
    }
}
