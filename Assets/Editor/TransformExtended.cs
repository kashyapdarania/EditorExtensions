using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


/// <summary>
/// Class Contains Utility Methods related to Transform like animate, move etc...
/// </summary>
public static class TransformExtended
{
    static List<TransformOperation> operations = new List<TransformOperation>();

    /// <summary>
    /// Linear move object from one position to another position
    /// </summary>
    /// <param name="trans">target transform in which actual move happens</param>
    /// <param name="target">target where to move</param>
    /// <param name="time">time to reach to target</param>
    public static void DoAnimate(this Transform trans, Transform target, float time)
    {
        TransformOperation to = GetTransformOperation(trans);
        to.StartAnimation(target, time);
    }


    /// <summary>
    /// Jump from current positon to target position
    /// </summary>
    /// <param name="trans">target transform in which actual jump happens</param>
    /// <param name="target">target where to jump</param>
    /// <param name="jumpRate">jump rate</param>
    /// <param name="time">time to reach to target</param>
    public static void DoJump(this Transform trans, Transform target, float jumpRate, float time)
    {
        TransformOperation to = GetTransformOperation(trans);
        to.StartJump(target, jumpRate, time);
    }

    static TransformOperation GetTransformOperation(Transform t)
    {
        foreach (TransformOperation to in operations)
        {
            if (to.current.Equals(t))
            {
                return to;
            }
        }
        GameObject obj = new GameObject();
        TransformOperation operation = obj.AddComponent<TransformOperation>();
        operation.current = t;
        operations.Add(operation);
        return operation;
    }
}

public class TransformOperation : MonoBehaviour
{
    public Transform current;
    public void StartAnimation(Transform target, float time)
    {
        StartCoroutine(Movement(target, time));
    }

    private IEnumerator Movement(Transform target, float time)
    {
        float index = 0f;
        float rate = 1.0f / time;
        Vector3 startPos = current.position;
        do
        {
            current.position = Vector3.Lerp(startPos, target.position, index);
            index += Time.fixedDeltaTime * rate;
            yield return new WaitForEndOfFrame();
        } while (index <= 1f);
        current.position = target.position;
    }

    public void StartJump(Transform target, float jumpRate, float time)
    {
        StartCoroutine(Jump(target, jumpRate, time));
    }

    private IEnumerator Jump(Transform target, float jumpRate, float time)
    {
        float index = 0f;
        float rate = 1.0f / time;
        AnimationCurve animCurve = new AnimationCurve();
        animCurve.AddKey(0f, 0f);
        animCurve.AddKey(0.5f, 1f);
        animCurve.AddKey(1f, 0f);
        Vector3 startPos = current.position;
        do
        {
            Vector3 pos = Vector3.Lerp(startPos, target.position, index);
            pos.y += animCurve.Evaluate(index) * jumpRate;
            transform.position = pos;
            index += Time.fixedDeltaTime * rate;
            yield return new WaitForEndOfFrame();
        } while (index <= 1f);
        current.position = target.position;
    }

}

