using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

[CreateAssetMenu]
public class FMODParameter : ScriptableObject
{
    public FMOD.Studio.PARAMETER_ID id;
    public string name;

}
