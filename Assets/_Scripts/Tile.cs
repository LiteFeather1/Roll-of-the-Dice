using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private BoxCollider2D _c;
    private int _xGridLocation, _yGridLocation;
    private int _value;

    private GameStages _myStage;
    private GameStages NextGameStateToSet
    {
        get
        {
            GameStages gs = _myStage;
            gs++;
            if (gs == GameStages.GameWon)
                gs = 0;
            return gs;
        }
    }

    private GridManager _myManager;
    private GameManager _gm;

    private void Awake()
    {
        _gm = GameManager.Instance;
    }

    private void OnDisable()
    {
        _gm.EventToUnsubscribe(_myStage, SpriteWhite);
        _gm.EventToUnsubscribe(NextGameStateToSet, SpriteFade);
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.Label(transform.position, $"{_value}");
    }

    public void SetGridLocation(GridManager myManager, int xGridLocation, int yGridLocation, int value, GameStages stage)
    {
        _myManager = myManager;
        _xGridLocation = xGridLocation;
        _yGridLocation = yGridLocation;
        _value = value;
        _myStage = stage;

        _gm.EventToSubscribe(_myStage, SpriteWhite);
        _gm.EventToSubscribe(NextGameStateToSet,SpriteFade);
    }

    public bool SetValue(int valueToSet)
    {
        if (_value != 0)
            return false;
        _value = valueToSet;
        _myManager.SetGridValue(_xGridLocation, _yGridLocation, _value);
        _myManager.CheckDestroy(_xGridLocation, _value);
        _gm.ChangeGame(NextGameStateToSet);
        return true;
    }

    public void SetValueRaw(int valueToSet)
    {
        _value = valueToSet;
    }

    public void SpriteFade()
    {
        _sr.color = new(1, 1, 1, 0.5f);
        _c.enabled = false;
    }

    private void SpriteWhite()
    {
        if (_value != 0)
            return;
        _sr.color = Color.white;
        _c.enabled = true;
    }
}
