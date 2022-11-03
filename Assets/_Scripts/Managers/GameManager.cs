using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [SerializeField] private int _gridSize;
    [SerializeField] private GridManager _playerOneGrid;
    [SerializeField] private GridManager _playerTwoGrid;
    [SerializeField] private UiManager _uiManager;

    [SerializeField] private Tile _tilePrefab;
    #region Events
    private Action _playerOneRoll;
    private Action _playerOnePlace;
    private Action _playerTwoRoll;
    private Action _playerTwoPlace;
    private Action _gameWon;

    private Dictionary<GameStages, Action> _stageToEvent;
    #endregion
    private float TileSize => _tilePrefab.transform.localScale.x;
    private float GapBetWeenTiles => TileSize / 4;

    private Camera _cam;

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private void Awake()
    {
        _instance = this;
        InitStageToEvent();
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        _playerOneGrid.Init(_gridSize, TileSize, GapBetWeenTiles);
        _playerTwoGrid.Init(_gridSize, TileSize, GapBetWeenTiles);
        HandleSecondGridPos();
        HandleCamPos();
        ChangeGame(GameStages.PlayerOneRoll);
    }

    private void HandleCamPos()
    {
        _cam = Camera.main;
        _cam.orthographicSize = _gridSize * 2;
        float yMove = _gridSize * (TileSize + GapBetWeenTiles);
        float xMove = yMove / 16 * 9 / 2;
        _cam.transform.position = new Vector3(_cam.transform.position.x + xMove, _cam.transform.position.y + yMove, _cam.transform.position.z);
    }

    private void HandleSecondGridPos()
    {
        float gapBetweenGrids = (TileSize + GapBetWeenTiles) * _gridSize + 1;
        _playerTwoGrid.transform.position = new Vector2(transform.position.x, transform.position.y + gapBetweenGrids);
    }

    private void InitStageToEvent()
    {
        _stageToEvent = new()
        {
            { GameStages.PlayerOneRoll, _playerOneRoll },
            { GameStages.PlayerOnePlace, _playerOnePlace },
            { GameStages.PlayerTwoRoll, _playerTwoRoll },
            { GameStages.PlayerTwoPlace, _playerTwoPlace },
            { GameStages.GameWon, _gameWon}
        };
    }

    public void ChangeGame(GameStages stage)
    {
        _stageToEvent[stage]?.Invoke();
    }

    public void EventToSubscribe(GameStages stage, Action method)
    {
        _stageToEvent[stage] += method; 
    }

    public void EventToUnsubscribe(GameStages stage, Action method)
    {
        switch (stage)
        {
            case GameStages.PlayerOneRoll:
                _playerOneRoll -= method;
                break;
            case GameStages.PlayerOnePlace:
                _playerOnePlace -= method;
                break;
            case GameStages.PlayerTwoRoll:
                _playerTwoRoll -= method;
                break;
            case GameStages.PlayerTwoPlace:
                _playerTwoPlace -= method;
                break;
            case GameStages.GameWon:
                _gameWon -= method;
                break;
        }
    }

    public void GameWon(string who)
    {
        _uiManager.Win(who + " Won!!!");   
    }
}
