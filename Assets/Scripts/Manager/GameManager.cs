using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SimpleSingleton<GameManager> 
{
    public int CurrentLife = 3;
    Player _player;

    [SerializeField]
    int _defaultAsteroidLevel = 4;

    [SerializeField]
    int _defaultAsteroidCount = 5;

    public Asteroid _asteroidPrefab;
    public int CurrentScore;
    [SerializeField]
    List<Transform> _spawnPositions;

    [SerializeField]
    int _curentAsteroidCount;
    public Bounds GameBound;
    [SerializeField]
    BoxCollider2D _boxCollider;
    protected override void Awake()
    {
        base.Awake();
        GlobalPubSub.SubcribeEvent<PlayerSpawnMessage>(OnPlayerSpawn);
        GlobalPubSub.SubcribeEvent<PlayerDieMessage>(OnPlayerDie);
        GlobalPubSub.SubcribeEvent<AsteroidSpawnMessage>(OnAsteroidSpawn);
        GlobalPubSub.SubcribeEvent<AsteroidDieMessage>(OnAsteroidDie);
        PoolManager.PoolGameObject(_asteroidPrefab, "", 100);
        GetBound();
    }

    private void Start()
    {
        OnLevelStart();
    }

    void GetBound()
    {
        Vector3 downleft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 upRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        GameBound = new Bounds(upRight.y, downleft.y, downleft.x, upRight.x);
        _boxCollider.size = (upRight - downleft);
    }

    void OnLevelStart()
    {
        int currentSpawnIdx = 0;
        for(int i = 0; i < _defaultAsteroidCount; i++)
        {
            Vector2 dir = Random.insideUnitCircle;
            if (dir == Vector2.zero) dir = Vector2.up;
            else dir = dir.normalized;
            SpawnAsteroid(_spawnPositions[currentSpawnIdx].position, _defaultAsteroidLevel, 1, dir);
            currentSpawnIdx++;
            if(currentSpawnIdx >= _spawnPositions.Count) 
            {
                currentSpawnIdx = 0;
            }
        }
        GUI_Canvas.Instance.InitLifeDisplay(CurrentLife);
    }

    void GameClear(bool isWin)
    {
        _player.gameObject.SetActive(false);
        GUI_Canvas.Instance.FinishGame(isWin);
    }

    void OnPlayerDie(PlayerDieMessage msg)
    {
        CurrentLife--;
        if (CurrentLife >= 0)
        {
            _player?.OnReinit();
            GUI_Canvas.Instance.UpdateLife(CurrentLife);
        }
        else
        {
            CurrentLife = 0;
            GameClear(false);
        }
    }

    void OnPlayerSpawn(PlayerSpawnMessage msg)
    {
        _player = msg.Player;
    }

    void OnAsteroidSpawn(AsteroidSpawnMessage msg)
    {
        _curentAsteroidCount++;
    }
    public void SpawnAsteroid(Vector3 position, int level, float speed, Vector3 dir)
    {
        Asteroid ast = PoolManager.GetInstance<Asteroid>(_asteroidPrefab);
        ast.transform.position = position;
        ast.Init(level, speed, dir);
    }

    void OnAsteroidDie(AsteroidDieMessage msg)
    {
        _curentAsteroidCount--;
        CurrentScore += msg.AsteroidLevel * 100;
        GUI_Canvas.Instance.UpdateScore(CurrentScore);
        if (_curentAsteroidCount <= 0)
        {
            GameClear(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Bullet") return;
        Vector3 newposition = collision.transform.position;
        if (collision.transform.position.y > GameBound.Up) newposition.y = GameBound.Down;
        if (collision.transform.position.y < GameBound.Down) newposition.y = GameBound.Up;
        if (collision.transform.position.x < GameBound.Left) newposition.x = GameBound.Right;
        if (collision.transform.position.x > GameBound.Right) newposition.x = GameBound.Left;
        collision.transform.position = newposition;
    }

    private void OnDestroy()
    {
        GlobalPubSub.UnsubcribeEvent<PlayerSpawnMessage>(OnPlayerSpawn);
        GlobalPubSub.UnsubcribeEvent<PlayerDieMessage>(OnPlayerDie);
        GlobalPubSub.UnsubcribeEvent<AsteroidSpawnMessage>(OnAsteroidSpawn);
        GlobalPubSub.UnsubcribeEvent<AsteroidDieMessage>(OnAsteroidDie);
    }
}

public struct Bounds
{
    public float Up;
    public float Down;
    public float Left;
    public float Right;
    public Bounds(float up, float down, float left, float right)
    {
        Up = up;
        Down = down;
        Left = left;
        Right = right;
    }
}