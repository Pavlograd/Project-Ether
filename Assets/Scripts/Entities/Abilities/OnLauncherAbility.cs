using UnityEngine;

[CreateAssetMenu(fileName = "Abitlity", menuName = "Ability/OnLauncherAbility")]
public class OnLauncherAbility : Ability
{
    [Header("Effects")]
    [SerializeField] private States _buff;
    [SerializeField] private float _duration;
    [SerializeField] private float _increasedAmount;
    [SerializeField] private bool _isPourcentage;

    public override void Activate(Transform parent)
    {
        GameObject gameObject = Instantiate(this._particles, parent.position, new Quaternion(0, 0, 0, 1));
        EntityData entityData = parent.gameObject.GetComponent<EntityData>();

        DataBuff buff = new DataBuff {
            type = _buff,
            duration = _duration,
            increasedAmount = _increasedAmount + tierUpgradesValue[lvl - 1],
            isPourcentage =  _isPourcentage,
            name = name,
            gameObject = gameObject
        };
        if (entityData) {
            entityData.entityStateManager.ActiveBuffState(buff);
        }
        gameObject.transform.parent = parent;
        Destroy(gameObject, this._duration);
    }

    public override float GetValueEffect() => _increasedAmount;
}
