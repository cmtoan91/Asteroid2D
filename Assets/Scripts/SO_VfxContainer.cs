using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_VFX_", menuName = "SOs/New Vfx Container")]
public class SO_VfxContainer : ScriptableObject
{
    public string VfxID;
    public int DefaultPoolCount = 1;
    public ParticleSystem VfxPrefab;
}
