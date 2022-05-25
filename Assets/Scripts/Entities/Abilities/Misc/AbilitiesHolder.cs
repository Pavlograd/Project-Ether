using System.Collections.Generic;
using UnityEngine;

public class AbilitiesHolder : MonoBehaviour
{
    [SerializeField] private Ability _defaultAttack;
    [SerializeField] private List<Ability> _abilities = new List<Ability>();
    [HideInInspector] public Ability defaultAttack;
    [HideInInspector] public List<Ability> abilities = new List<Ability>();

    void Awake()
    {
        if (_defaultAttack != null) {
            this.defaultAttack = Instantiate(_defaultAttack) as Ability;
        }
        if (_abilities.Count > 0) {
            if (this.abilities.Count > 0) {
                this.abilities.Clear();
            }
            for (int i = 0; i < _abilities.Count; i++) {
                if (_abilities[i] == null) {
                    continue;
                }
                this.abilities.Add(Instantiate(_abilities[i]) as Ability);
            }
        }
    }

    public List<Ability> GetOriginalAbilities()
    {
        return this._abilities;
    }

    public Ability GetOriginalDefaultAttack()
    {
        return this._defaultAttack;
    }

    private void SetDefaultAttack(Ability newDefaultAttack)
    {
        if (newDefaultAttack != null) {
            this.defaultAttack = Instantiate(newDefaultAttack) as Ability;
            this._defaultAttack = newDefaultAttack;
        }
    }

    private void SetAbilities(List<Ability> newAbilitiesArr)
    {
        if (newAbilitiesArr != null && newAbilitiesArr.Count > 0) {
            if (this.abilities.Count > 0) {
                this.abilities.Clear();
                this._abilities.Clear();
            }
            for (int i = 0; i < newAbilitiesArr.Count; i++) {
                if (newAbilitiesArr[i] == null) {
                    this.abilities.Add(null);
                } else {
                    this.abilities.Add(Instantiate(newAbilitiesArr[i]) as Ability);
                }
                this._abilities.Add(newAbilitiesArr[i]);
            }
        }
    }

    public void SetNewAbilities(Ability newDefaultAttack, List<Ability> newAbilitiesArr)
    {
        SetDefaultAttack(newDefaultAttack);
        SetAbilities(newAbilitiesArr);
    }

    public void SetNewAbilities(Ability newDefaultAttack)
    {
        SetDefaultAttack(newDefaultAttack);
    }

    public void SetNewAbilities(List<Ability> newAbilitiesArr)
    {
        SetAbilities(newAbilitiesArr);
    }
}
