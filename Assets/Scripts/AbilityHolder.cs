using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public ActiveAbility SelectedActiveAbility;
    public PassiveAbility SelectedPassiveAbility;

    public GameObject ImageQ_Active;
    public GameObject ImageQ_NoActive;

    float coolDownTime;
    float activeTime;
    float tickTime;
    AbilityState state;

    public enum AbilityState
    {
        ready,
        active,
        cooldown
    }

    public AbilityState State { get { return state;} }

    public void ProcessPassive() => SelectedPassiveAbility.Activate(gameObject);

    public void SwitchToInactiveIcon()
    {
        ImageQ_Active.SetActive(false);
        ImageQ_NoActive.SetActive(true);
    }

    public void SwitchToActiveIcon()
    {
        ImageQ_Active.SetActive(true);
        ImageQ_NoActive.SetActive(false);
    }

    public void ProcessActive(bool input)
    {
        if (SelectedActiveAbility is null)
        {
            SwitchToInactiveIcon();
            return;
        }

       switch (state) 
       {
           case AbilityState.ready:
                if(input)
                {
                    Debug.Log("player used active ability");
                    SelectedActiveAbility.Activate(gameObject);
                    state = AbilityState.active;
                    activeTime = SelectedActiveAbility.ActiveTime;
                    SwitchToInactiveIcon();
                }              
                break;
           case AbilityState.active:
                if(activeTime > 0)
                { 
                    activeTime -= Time.deltaTime;
                    tickTime += Time.deltaTime;
                    if(tickTime >= SelectedActiveAbility.TickTime) 
                    { 
                        Debug.Log("ability used due to tick");
                        SelectedActiveAbility.Activate(gameObject);
                        tickTime = 0;
                    }
                }
                else 
                {
                    SelectedActiveAbility.Reset(gameObject);
                    state = AbilityState.cooldown;
                    coolDownTime = SelectedActiveAbility.coolDownTime;
                }
                break;
           case AbilityState.cooldown:
                if (coolDownTime > 0)
                {
                    coolDownTime -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.ready;
                    SwitchToActiveIcon();
                }
                break;
       }
    }
}
