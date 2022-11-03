using UnityEngine;
using UnityEngine.UI;

public class RollButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameStages _stage;
    private GameStages NextStage => _stage + 1;

    private GameManager _gm;

    private void OnEnable()
    {
        _gm = GameManager.Instance;
        _gm.EventToSubscribe(_stage, TurnOn);
        _gm.EventToSubscribe(GameStages.GameWon, TurnOff);
        _button.onClick.AddListener(TurnOff);
    }

    private void OnDisable()
    {
        _gm.EventToUnsubscribe(_stage, TurnOn);
        _gm.EventToUnsubscribe(GameStages.GameWon, TurnOff);
    }

    private void TurnOn()
    {
        _button.interactable = true;
    }
    
    private void TurnOff()
    {
        _button.interactable = false;
        _gm.ChangeGame(NextStage);
    }
}
