using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionOnTimer : MonoBehaviour
{   
    private float currentTime;
    private float timeAfterActionIsCalled;
    private Action timerCallback;
    private Action timerRepeatAmountEndedCallback;
    private bool isTimerRepeating;
    private bool hasRepeatAmount;   
    private int repeatAmount;


    public float CurrentTime
    {
        get { return currentTime; }
    }

    public void AddTimerCallback(Action timerCallback)
    {
        this.timerCallback += timerCallback;
    }
    public void DeleteTimerCallback(Action timerCallback)
    {
        this.timerCallback -= timerCallback;
    }


    public void SetTimer(float timeAfterActionIsCalled, Action timerCallback, bool isTimerRepeating = false)
    {
        this.timeAfterActionIsCalled = timeAfterActionIsCalled;
        this.currentTime = timeAfterActionIsCalled;
        this.timerCallback = timerCallback;
        this.isTimerRepeating = isTimerRepeating;
        hasRepeatAmount = false;
    }

    //Overloaded Method Use this when you want the timer to do an action for a predefiend times and then get an Action when the Timer ended
    public void SetTimer(float timeAfterActionIsCalled,int repeatAmount ,Action timerCallback, Action timerRepeatAmountEndedCallback)
    {
        this.timeAfterActionIsCalled=timeAfterActionIsCalled;
        this.currentTime = timeAfterActionIsCalled;
        this.timerCallback=timerCallback;
        this.timerRepeatAmountEndedCallback = timerRepeatAmountEndedCallback;
        hasRepeatAmount = true;
        this.repeatAmount = repeatAmount;
    }


    public float GetCurrentTimeNormalized()
    {
        return currentTime / timeAfterActionIsCalled;
    }

    //Calculates the time the Action is called per second
    public float GetCalledActionsPerSeconds()
    {
        return 1 / timeAfterActionIsCalled;
    }
    public float GetCurrentTime()
    {
        return currentTime;
    }

    public void StopTimer()
    {
        currentTime = 0;
        isTimerRepeating = false;
    }


    private void Update()
    {

       if(!hasRepeatAmount)
        {

            UpdateTimerStandart();
        }
        else
        {
            UpdateTimerFixedAmount();
        }

        
    }

    //TODO Refactor this class 
    private void UpdateTimerFixedAmount()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            if (!IsTimerComplete()) return;
            timerCallback();
            repeatAmount--;
            if (repeatAmount > 0)
            {
                currentTime = timeAfterActionIsCalled;
            }
            else
            {
                timerRepeatAmountEndedCallback();
            }


        }
    }

    private void UpdateTimerStandart()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            if (!IsTimerComplete()) return;
            timerCallback();
            if (isTimerRepeating) currentTime = timeAfterActionIsCalled;


        }
    }

    private bool IsTimerComplete()
    {
        return currentTime <= 0f;
    }
    

    


}
