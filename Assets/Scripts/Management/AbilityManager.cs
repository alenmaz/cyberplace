using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityManager : MonoBehaviour
{
    [System.Serializable]
    public class ButtonHolder
    {
        [SerializeField] private Button button;
        [SerializeField] private Ability ability;

        public ButtonHolder(Button button, Ability ability)
        {
            this.button = button;
            this.ability = ability;
        }

        public Button Button { get => button; }
        public Ability Ability { get => ability; set { if (value != null) ability = value; } }
    }

    [System.Serializable]
    public class AbilitySettings
    {
        [SerializeField] private Ability[] abilityPool;

        public Ability[] ABilityPool { get => abilityPool; }
    }

    public AbilityHolder PlayerAbilityHolder;
    [SerializeField] private List<ButtonHolder> buttonHolders;

    [SerializeField] private AbilitySettings[] abilitySettings;
    private Stack<Ability> abilityStack;

    public int Index;

    void Start() 
    {
        buttonHolders = new List<ButtonHolder>();
        abilityStack = new Stack<Ability>();
    }

    private void FillStack(int waveNumber)
    {
        if (waveNumber < 0 || waveNumber > abilitySettings.Length)
        {
            Debug.Log($"no ability drop settings for wave with index {waveNumber}");
            return;
        }
        foreach(var ability in abilitySettings[waveNumber].ABilityPool)
            abilityStack.Push(ability);
    }

    public void FillButtons(Button[] buttons, int waveNumber)
    {
        ButtonHolder temp;
        Ability ability;
        FillStack(waveNumber);
        foreach(var button in buttons)
        {
            ability = abilityStack.Pop();
            temp = new ButtonHolder(button, ability);
            temp.Button.transform.Find("AbilityName").GetComponent<TextMeshProUGUI>().text = ability.Name;
            temp.Button.transform.Find("AbilityIcon").GetComponent<Image>().sprite = ability.Icon;
            temp.Button.transform.Find("AbilityDesc").GetComponent<TextMeshProUGUI>().text = ability.Description;
            buttonHolders.Add(temp);
        }
    }

    public void SetAbility(int index)
    {
        if (index >= 0 && index <= buttonHolders.Count) Index = index;
        else
            Debug.Log($"No ability with such index {index}, your ability array length is {buttonHolders.Count}");
    }

    public void ApplyAbility()
    {
        if (buttonHolders[Index].Ability is ActiveAbility)
        {
            PlayerAbilityHolder.SelectedActiveAbility = buttonHolders[Index].Ability as ActiveAbility;
            PlayerAbilityHolder.SwitchToActiveIcon();
        }
        if (buttonHolders[Index].Ability is PassiveAbility)
        {
            PlayerAbilityHolder.SelectedPassiveAbility = buttonHolders[Index].Ability as PassiveAbility;
            PlayerAbilityHolder.ProcessPassive();
        }
    }

    public void ClearButtons()
    {
        buttonHolders.Clear();
    }
}
