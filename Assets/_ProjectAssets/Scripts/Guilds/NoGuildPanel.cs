using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGuildPanel : GuildPanelBase
{
    public override void Setup()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
