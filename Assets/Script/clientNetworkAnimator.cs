using Unity.Netcode.Components;
using UnityEngine;

public class clientNetworkAnimator : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
