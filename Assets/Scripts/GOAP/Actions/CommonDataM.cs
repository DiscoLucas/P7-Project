using UnityEngine;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Interfaces;

public class CommonDataM : IActionData
{
    public ITarget Target { get; set; }
    public float timer { get; set; }
}
