using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class WaveManager : BehaviourSingleton<WaveManager>
{
    public float WaveInterval;
    public float LastWaveEndTime;
    public int CurrentWaveNum = 1;

    public List<GameObject> EnemyPrefab;

    public int _currentEnemyCount;
    public int WaveMaxEnemyCount;

    public int KillCount = 0;
    private Dictionary<int, WaveData> WaveDataDic;
    private List<Vector3> _placedEnemyPositionList;

    public System.Action OnEnemyDeadAction;
    public System.Action<float> OnCoolTimer;
    public System.Action OnStateChange;

    public void Start()
    {
        WaveDataDic = new Dictionary<int, WaveData>();
        Global.Instance.OnDataLoaded += _LoadWaveData;
    }

    public IEnumerator WaveSystemCoroutine()
    {
        while (true)
        {
            float timer = 0f;
            while(timer < WaveInterval)
            {
                timer += Time.deltaTime;
                OnCoolTimer?.Invoke(timer / WaveInterval);
                yield return null;
            }

            OnStateChange?.Invoke();
            _placedEnemyPositionList = new List<Vector3>();

            yield return StartCoroutine(StartWave());
            while (_currentEnemyCount > 0) yield return null;

            OnStateChange?.Invoke();
            CurrentWaveNum++;
            KillCount = 0;

            if (WaveDataDic.ContainsKey(CurrentWaveNum) == false) break;
        }
    }

    public IEnumerator StartWave()
    {
        int enemyHealth = WaveDataDic[CurrentWaveNum].EnemyHealth;
        int enemyDamage = WaveDataDic[CurrentWaveNum].EnemyDamage;
        WaveMaxEnemyCount = WaveDataDic[CurrentWaveNum].EnemyCount;
        int enemyDropAshCount = WaveDataDic[CurrentWaveNum].EnemyAsh;

        int termCount = Random.Range(1, 3);
        float termTimer = Random.Range(2f, 5f);
        int spawned = 0;

        while (spawned < WaveMaxEnemyCount)
        {
            int spawnCount = Random.Range(WaveMaxEnemyCount / 3, WaveMaxEnemyCount / 2);
            if (spawned + spawnCount > WaveMaxEnemyCount)
            {
                spawnCount = WaveMaxEnemyCount - spawned;
            }
            spawned += SpawnEnemyCluster(enemyHealth, enemyDamage, spawnCount, enemyDropAshCount);

            yield return new WaitForSeconds(Random.Range(4f, 6f));
        }
    }

    // 스폰 개수 반환
    public int SpawnEnemyCluster(int health, int damage, int spawnCount, int dropAshCount)
    {
        int tries = 0;
        int spawned = 0;

        float angle = Random.Range(0f, Mathf.PI * 2);
        float spawnRadius = FireManager.Instance.GetRange() + 5f;

        float centerX = Mathf.Cos(angle) * spawnRadius;
        float centerY = Mathf.Sin(angle) * spawnRadius;

        while (spawned < spawnCount && tries < spawnCount * 10)
        {
            tries++;

            angle = Random.Range(0f, Mathf.PI * 2);
            spawnRadius = Random.Range(2, 5);

            float radius = Mathf.Sqrt(Random.Range(0f, 1f));

            float x = Mathf.Cos(angle) * radius * spawnRadius;
            float y = Mathf.Sin(angle) * radius * spawnRadius;

            Vector3 spawnPoint = new Vector3(centerX, centerY) + new Vector3(x, y, 0);

            bool tooClose = _placedEnemyPositionList.Any(pos => Vector3.Distance(pos, spawnPoint) < 0.1f);
            if (tooClose) continue;

            EnemyStat enemyStat = Instantiate(EnemyPrefab[0], spawnPoint, Quaternion.identity).GetComponent<EnemyStat>();
            enemyStat.Initialize(health, damage, dropAshCount);
            _currentEnemyCount++;
            spawned++;
        }

        return spawned;
    }

    public void OnEnemyDeath()
    {
        _currentEnemyCount--;
        KillCount++;
        OnEnemyDeadAction?.Invoke();
    }
    private void _LoadWaveData()
    {
        for (int i = 0; i < DataTable.Instance.GetWaveDataList().Count; i++)
        {
            WaveDataDic.Add(i + 1, DataTable.Instance.GetWaveDataList()[i]);
        }

        StartCoroutine(WaveSystemCoroutine());
    }
}
