using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    private int _points;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private GameStages _stage;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GridManager _opposite;

    private float _tileSize;
    private float _gapBetweenTiles;

    //First is rows // Second is collums
    private int[,] _grid;
    private Dictionary<Tuple<int,int>, Tile> _gridToValue;

    public void Init(int gridSize, float tileSize, float gapBetweenTiles)
    {
        _grid = new int[gridSize, gridSize];
        _tileSize = tileSize;
        _gapBetweenTiles = gapBetweenTiles;
        HandleGridLayout(gridSize);
    }

    private void HandleGridLayout(int gridSize)
    {
        _gridToValue = new();
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                _grid[x, y] = 0;
                Tile tile = Instantiate(_tilePrefab);
                tile.SetGridLocation(this, x, y, _grid[x, y], _stage);
                SetTilePostion(tile, x, y);
                _gridToValue.Add(Tuple.Create(x,y), tile);
            }
        }
    }

    public void SetGridValue(int xGridLocation, int yGridLocation, int value)
    {
        _grid[xGridLocation, yGridLocation] =  value;
        CheckPoints();
    }

    private void SetTilePostion(Tile currentTile, int collumn, int row)
    {
        float xPos = (_tileSize + _gapBetweenTiles) * collumn;
        float yPos = (_tileSize + _gapBetweenTiles) * row;

        currentTile.transform.position = new Vector2(xPos, yPos);
        currentTile.transform.SetParent(transform);
    }

    private void CheckPoints()
    {
        int point = 0;
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            point += CheckCollumMultiplier(x);
        }
        _points = point;
        _text.text = _points.ToString();

        foreach (var val in _grid)
        {
            if (val == 0)
                return;
        }
        GameManager.Instance.GameWon(gameObject.name);
    }

    private int CheckCollumMultiplier(int collumn)
    {
        int biggestValue = 0;
        List<int> valuesOnCollumn = new();
        int point = 0;
        for (int i = 0; i < _grid.GetLength(1); i++)
        {
            int interateValue = _grid[collumn, i];
            valuesOnCollumn.Add(interateValue);
            if (interateValue > biggestValue)
                biggestValue = interateValue;
        }
        int multiplier = 0;
        int biggestValueAmount = 0;
        foreach (var value in valuesOnCollumn)
        {
            if (value == biggestValue)
            {
                multiplier++;
                biggestValueAmount++;
            }
            else
                point += value;
        }
        biggestValue *= biggestValueAmount;
        point += biggestValue * multiplier;
        return point;
    }

    public void CheckDestroy(int collum, int value)
    {
        for (int i = 0; i < _grid.GetLength(1); i++)
        {
            int intereateValue = _opposite._grid[collum, i];
            if(intereateValue == value)
            {
                _opposite._grid[collum, i] = 0;
                _opposite._gridToValue[Tuple.Create(collum, i)].SetValueRaw(0);
            }
        }
    }
}
