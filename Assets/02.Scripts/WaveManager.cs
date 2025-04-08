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

    private int _currentEnemyCount;
    private Dictionary<int, WaveData> WaveDataDic;
    private List<Vector3> _placedEnemyPositionList;

    public void Start()
    {
        WaveDataDic = new Dictionary<int, WaveData>();
        Global.Instance.OnDataLoaded += _LoadWaveData;
    }

    public IEnumerator WaveSystemCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(WaveInterval);

            _placedEnemyPositionList = new List<Vector3>();

            yield return StartWave();
            CurrentWaveNum++;

            while (_currentEnemyCount > 0) yield return null;

            if (WaveDataDic.ContainsKey(CurrentWaveNum) == false) break;
        }
    }

    public IEnumerator StartWave()
    {
        int enemyHealth = WaveDataDic[CurrentWaveNum].EnemyHealth;
        int enemyDamage = WaveDataDic[CurrentWaveNum].EnemyDamage;
        int enemyCount = WaveDataDic[CurrentWaveNum].EnemyCount;

        int termCount = Random.Range(1, 3);
        float termTimer = Random.Range(2f, 5f);
        int spawned = 0;

        while (spawned < enemyCount)
        {
            int spawnCount = Random.Range(enemyCount / 3, enemyCount / 2);
            spawnCount = Mathf.Min(spawnCount, enemyCount - spawnCount);

            spawned = SpawnEnemyCluster(enemyHealth, enemyDamage, spawnCount);

            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }

    // 스폰 개수 반환
    public int SpawnEnemyCluster(int health, int damage, int spawnCount)
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
            enemyStat.Initialize(health, damage);
            _currentEnemyCount++;
            spawned++;
        }

        return spawned;
    }

    public void OnEnemyDeath()
    {
        _currentEnemyCount--;
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
