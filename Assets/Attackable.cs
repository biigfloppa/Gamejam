using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attackable : MonoBehaviour
{
    [SerializeField] int maxHP = 100;
    [SerializeField] int hp = 100;
    bool isStopped = false;
    int stopTime = 0;

    Coroutine stopTimeCoroutine = null;
    public void GetDamage(int damage)
    {
        if (hp <= damage) { Kill(); }
        hp = Mathf.Max(hp - damage, 0);
        Debug.Log(hp);
    }
    public virtual void Kill()
    {
        if (null != stopTimeCoroutine)
        {
            stopTimeCoroutine = null;
            StopCoroutine(StopTime());
        }
        gameObject.SetActive(false);
    }
    public void SetStopTime(int time)
    {
        stopTime = time;
        stopTimeCoroutine = StartCoroutine(StopTime());
    }
    public bool GetStopped()
    {
        return isStopped;
    }
    IEnumerator StopTime()
    {
        isStopped = true;
        while (stopTime > 0)
        {
            yield return new WaitForSeconds(1);
            stopTime--;
        }
        isStopped = false;
        stopTimeCoroutine = null;
    }
}
