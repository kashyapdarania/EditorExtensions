﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Contains Monobehaviours extended methods
/// </summary>
public static class MonobehaviourExtended
{
    /// <summary>
    /// Wait and Call method
    /// </summary>
    /// <param name="monoBehaviour">Monobehaviour reference for coroutine</param>
    /// <param name="delay">How much time wait before call method</param>
    /// <param name="action">Callback</param>
    public static void WaitAndCall(this MonoBehaviour monoBehaviour, float delay, Action action)
    {
        monoBehaviour.StartCoroutine(Wait(delay, action));
    }

    private static IEnumerator Wait(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

}
