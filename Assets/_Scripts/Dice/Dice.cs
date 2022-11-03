    using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Dice : MonoBehaviour
{
    [SerializeField] private int _diceSize;
    private int _rollValue;

    [SerializeField] private TMP_Text _text1;
    [SerializeField] private TMP_Text _text2;
    private TMP_Text _currentText;

    [SerializeField] private MouseDice _mouseDice;
    private bool _diceRolling;

    private GameManager _gm;

    private void OnEnable()
    {
        _gm = GameManager.Instance;
        _gm.EventToSubscribe(GameStages.PlayerOnePlace, SetCurrentText1);
        _gm.EventToSubscribe(GameStages.PlayerTwoPlace, SetCurrentText2);
        _currentText = _text1;
    }

    private void OnDisable()
    {
        _gm.EventToUnsubscribe(GameStages.PlayerOnePlace, SetCurrentText1);
        _gm.EventToUnsubscribe(GameStages.PlayerTwoPlace, SetCurrentText2);
    }

    //OnClick Button Event
    public void RollDice()
    {
        if (!_diceRolling)
        {
            _diceRolling = true;
            StartCoroutine(RollDice_Co());
        }
    }

    private IEnumerator RollDice_Co()
    {
        int rolls = Random.Range(_diceSize, _diceSize * 3);

        for (int i = 0; i < rolls; i++)
        {
            RollingDice();
            yield return new WaitForSeconds(0.125f/2);
        }
        _mouseDice.Setup(_rollValue);
        _diceRolling = false;
    }

    private void RollingDice()
    {
        _rollValue = Random.Range(1, _diceSize);
        _currentText.text = $"{_rollValue}";
    }

    private void SetCurrentText1()
    {
        _currentText = _text1;
    }

    private void SetCurrentText2()
    {
        _currentText = _text2;
    }
}
