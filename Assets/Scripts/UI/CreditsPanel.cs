using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPanel : UIPanel
{
    public override void Setup()
    {
        base.Setup();
    }

    public override void Activate()
    {
        base.Activate();

    }

    public override void Disable()
    {
        base.Disable();
    }

    public void Close()
    {
        UIManager.Instance.OpenMainMenu();
    }
}
