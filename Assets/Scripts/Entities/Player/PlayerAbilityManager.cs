using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityManager : EntityAbilityManager
{
    [SerializeField] private GameObject _abilityCanvas;

    void Start()
    {
        if (_abilityCanvas) {
            SetupAbilityCanvas();
        }
    }

    void Update()
    {
        if (!_entityData.entityHealthManager.isAlive) {
            //todo call canvasmanager.showEndGameUI()
        } else {
            _target = FindClosestEnemy(transform.position, rangeAttack);
            if (_canAutoAttack && _canAbilityAttack) {
                DefaultAttack();
            }
        }
    }

    public void SetupAbilityCanvas(GameObject abilityCanvas)
    {
        _abilityCanvas = abilityCanvas;
        List<Ability> abilities = _abilitiesHolder.GetOriginalAbilities();

        for (int index = 0; index < abilities.Count; index++) {
            if (abilities[index] == null) {
                _abilityCanvas.transform.GetChild(index).GetComponent<Button>().interactable = false;
                _abilityCanvas.transform.GetChild(index).GetChild(0).GetComponent<Image>().color = new Color32(200, 200, 200, 128);
                continue;
            }
            Image imgComponent = _abilityCanvas.transform.GetChild(index).GetChild(0).GetComponent<Image>();

            if (imgComponent != null) {
                imgComponent.sprite = abilities[index].thumbnail;
            }
        }
    }

    public void SetupAbilityCanvas()
    {
        List<Ability> abilities = _abilitiesHolder.GetOriginalAbilities();

        for (int index = 0; index < abilities.Count; index++) {
            Image imgComponent = _abilityCanvas.transform.GetChild(index).GetChild(0).GetComponent<Image>();

            if (imgComponent != null) {
                imgComponent.sprite = abilities[index].thumbnail;
            }
        }
    }

    private static GameObject FindClosestEnemy(Vector3 pos, float range)
    {
        float closest = 0;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            float distance = Vector3.Distance(pos, enemy.transform.localPosition);

            if (closestEnemy && (distance > closest || distance > range))
                continue;
            closestEnemy = enemy;
            closest = distance;
        }
        return closest > range ? null : closestEnemy;
    }

    IEnumerator AbilityRegenerator(float delaySeconds, int slot)
    {
        float animationTime = delaySeconds;
        Transform maskUiCanvas = _abilityCanvas.transform.GetChild(slot - 1).GetChild(0).GetChild(0);
        Image maskUiCanvasImage = maskUiCanvas.GetComponent<Image>();

        for (; animationTime > 0; animationTime -= Time.deltaTime) {
            maskUiCanvasImage.fillAmount = animationTime / delaySeconds;
            yield return new WaitForEndOfFrame();
        }
        maskUiCanvasImage.fillAmount = 0;
    }

    private void TriggerAbilityPlayer(Ability ability, bool isAutoAttack, int index = 0)
    {
        TriggerAbility(ability, isAutoAttack);
        if (index > 0) {
            StartCoroutine(AbilityRegenerator(ability.cooldownTime, index));
        }
    }

    public void LaunchAttack(int index)
    {
        try {
            if (!_canAbilityAttack || index < 1 && index > 4 || !canAttack || _abilityCanvas == null)
                return;
            if (_abilitiesHolder.abilities[index - 1].abilityType != AbilityType.ACTIVABLE
                && _abilitiesHolder.abilities[index - 1].abilityType != AbilityType.BUFF
                && !_target) {
                return;
            }
            TriggerAbilityPlayer(_abilitiesHolder.abilities[index - 1], false, index);
        } catch (System.Exception err) {
            Debug.Log(err);
        }
    }

    private void DefaultAttack()
    {
        try {
            if (!_target || !canAttack) {
                return;
            }
            TriggerAbilityPlayer(_abilitiesHolder.defaultAttack, true);
        } catch (System.Exception err) {
            Debug.Log(err);
        }
    }
}