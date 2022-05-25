using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
    private VariableJoystick _joystick;
    private GameObject _abilityCanvas;
    private StatesIconHandler _playerStatesIconHandler;
    private Slider _healtBar;

    void Awake()
    {
        GameObject joystick = GameObject.Find("Variable Joystick");
        GameObject healtBar = GameObject.Find("HealthBar");
        Transform _statesBars = gameObject.transform.Find("StatesBar");
        _abilityCanvas = GameObject.Find("Abilities");
        if (joystick)
            _joystick = joystick.GetComponent<VariableJoystick>();
        if (healtBar)
            _healtBar = healtBar.GetComponent<Slider>();
        if (_statesBars) {
            Transform statesPanel = _statesBars.Find("StatesPanel");
            _playerStatesIconHandler = statesPanel.GetComponent<StatesIconHandler>();
        }
    }

    private void SetJoystick(GameObject player)
    {
        PlayerMovementManager mvtManager = player.GetComponent<PlayerMovementManager>();
        if (mvtManager != null) {
            mvtManager.SetJoystick(_joystick);
        }
    }

    private void SetAbilities(GameObject player)
    {
        PlayerAbilityManager abilityManager = player.GetComponent<PlayerAbilityManager>();
        if (abilityManager != null) {
            foreach (Transform child in _abilityCanvas.transform) {
                Button btn = child.gameObject.GetComponent<Button>();
                btn.onClick.AddListener(()  => abilityManager.LaunchAttack(child.name[child.name.Length - 1] - '0'));
            }
            abilityManager.SetupAbilityCanvas(_abilityCanvas);
        }
    }

    private void SetHealtBar(EntityData playerEntityData)
    {
        if (playerEntityData != null) {
            playerEntityData.entityHealthManager.SetHealthBar(_healtBar);
        }
    }

    private void SetStatesBar(EntityData playerEntityData)
    {
        if (playerEntityData != null) {
            playerEntityData.entityStateManager.SetStateIconHandler(_playerStatesIconHandler);
        }
    }

    public void LinkToPlayer(GameObject player)
    {
        EntityData playerEntityData = player.GetComponent<EntityData>();
        if (_joystick != null) {
            SetJoystick(player);
        }
        if (_abilityCanvas != null) {
            SetAbilities(player);
        }
        if (_healtBar != null) {
            SetHealtBar(playerEntityData);
        }
        if (_playerStatesIconHandler != null) {
            SetStatesBar(playerEntityData);
        }
    }
}
