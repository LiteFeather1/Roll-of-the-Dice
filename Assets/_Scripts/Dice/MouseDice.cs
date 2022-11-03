using UnityEngine;

public class MouseDice : MonoBehaviour
{
    private bool _canPutValue;
    private int _diceValue;

    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (!_canPutValue)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(mousePos, Vector3.forward);
            if (hit2D.collider == null)
                return;
            if (hit2D.collider.TryGetComponent(out Tile tile))
            {
                if(tile.SetValue(_diceValue))
                    _canPutValue = false;
            }
        }
    }

    public void Setup(int diceValue)
    {
        _canPutValue = true;
        _diceValue = diceValue;
    }
}
