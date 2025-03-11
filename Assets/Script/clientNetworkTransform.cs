using Unity.Netcode.Components;
using UnityEngine;

public class clientNetworkTransform : NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
