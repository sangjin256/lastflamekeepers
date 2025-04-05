using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : BehaviourSingleton<UnitManager>
{
    private List<Unit> _unitList = new List<Unit>();
    public List<Unit> UnitList;


    [Header("유닛 프리팹")]
    [SerializeField] private List<GameObject> _unitPrefabList;

    [Header("이름")]
    [SerializeField] private List<string> _nameList = new List<string>();
    //랜덤 min/max 값
    private RandomStatData _randomMaxHealthData;
    private RandomStatData _randomDamageData;
    private RandomStatData _randomAttackSpeedData;
    private RandomStatData _randomMoveSpeedData;
    private RandomStatData _randomWoodSpeedData;
    private RandomStatData _randomRockSpeedData;

    [Header("특성")]
    private List<Feature> _positiveFeatureList = new List<Feature>();
    private List<Feature> _negativeFeatureList = new List<Feature>();

    private float[] _getFeatureProbabilty = { 0.9f, 0.4f, 0.1f };
    private void Start()
    {
        Global.Instance.OnDataLoaded += LoadData;
    }


    public List<Unit> GenerateRandomUnit()
    {
        List<Unit> randomUnitList = new List<Unit>();

        for (int generateCount = 0; generateCount < 3; generateCount++)
        {
            GameObject unitGameObject = Instantiate(_unitPrefabList[Random.Range(0, _unitPrefabList.Count)]);

            UnitStat unitStat = unitGameObject.GetComponent<UnitStat>();
            Unit unit = unitGameObject.GetComponent<Unit>();

            unitStat.Name = _nameList[Random.Range(0, _nameList.Count)];
            
            int randomMaxHealth = Random.Range(_randomMaxHealthData.MinValue, _randomMaxHealthData.MaxValue + 1);
            int randomDamage = Random.Range(_randomDamageData.MinValue, _randomDamageData.MaxValue + 1);
            int randomAttackSpeed = Random.Range(_randomAttackSpeedData.MinValue, _randomAttackSpeedData.MaxValue + 1);
            int randomMoveSpeed = Random.Range(_randomMoveSpeedData.MinValue, _randomMoveSpeedData.MaxValue + 1);
            int randomWoodSpeed = Random.Range(_randomWoodSpeedData.MinValue, _randomWoodSpeedData.MaxValue + 1);
            int randomRockSpeed = Random.Range(_randomRockSpeedData.MinValue, _randomRockSpeedData.MaxValue + 1);

            unitStat = new UnitStat(randomMaxHealth, randomDamage, randomAttackSpeed, randomMoveSpeed, randomWoodSpeed, randomRockSpeed);

            unitStat.AllocateFeatureList();
            for (int featureCount = 0; featureCount < 3; featureCount++)
            {
                if (Random.value < _getFeatureProbabilty[featureCount])
                {
                    int randomIndex = Random.Range(0, _positiveFeatureList.Count);
                    Feature randomFeature = _positiveFeatureList[randomIndex];

                    //중복검사
                    bool isDuplicate = false;
                    foreach (Feature feature in unitStat.PositiveFeatureList)
                    {
                        if (feature.FeatureType == randomFeature.FeatureType)
                        {
                            featureCount--;
                            isDuplicate = true;
                            break;
                        }
                    }

                    if (!isDuplicate)
                    {
                        unitStat.AddPositiveFeature(randomFeature);
                    }

                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (Random.value < _getFeatureProbabilty[i])
                {
                    int randomIndex = Random.Range(0, _negativeFeatureList.Count);
                    Feature randomFeature = _negativeFeatureList[randomIndex];

                    //중복검사
                    bool isDuplicate = false;
                    foreach (Feature feature in unitStat.NegativeFeatureList)
                    {
                        if (feature.FeatureType == randomFeature.FeatureType)
                        {
                            i--;
                            isDuplicate = true;
                            break;
                        }
                    }

                    //긍정적 특성과의 충돌 검사
                    bool isConflict = false;
                    foreach (Feature feature in unitStat.PositiveFeatureList)
                    {
                        if (feature.GetConflictingFeature() == randomFeature.FeatureType)
                        {
                            i--;
                            isConflict = true;
                            break;
                        }
                    }
                    if (!isConflict && !isDuplicate)
                    {
                        unitStat.AddNegativeFeature(randomFeature);
                    }
                }
            }

            unitStat.DebugStat();
            unitGameObject.SetActive(false);
        }


        return randomUnitList;
    }

    public void SpawnUnit(Unit unit, Vector3 position)
    {
        unit.gameObject.SetActive(true);
        unit.transform.position = position;
        _unitList.Add(unit);
    }

    public void DestroyUnit(Unit unit)
    {
        UnitList.Remove(unit);
    }

    public void LoadData()
    {
        //이름 리스트
        ReadOnlyList<UnitNameData> unitNameDataList = DataTable.Instance.GetUnitNameDataList();
        foreach(UnitNameData unitNameData in unitNameDataList)
        {
            _nameList.Add(unitNameData.Name);
        }

        //랜덤 스탯 세팅값
        ReadOnlyList<RandomStatData> randomStatDataList = DataTable.Instance.GetRandomStatDataList();
        _randomMaxHealthData = randomStatDataList[0];
        _randomDamageData = randomStatDataList[1];
        _randomAttackSpeedData = randomStatDataList[2];
        _randomMoveSpeedData = randomStatDataList[3];
        _randomWoodSpeedData = randomStatDataList[4];
        _randomRockSpeedData = randomStatDataList[5];

        //특성 리스트
        ReadOnlyList<FeatureData> FeatureDataList = DataTable.Instance.GetFeatureDataList();
        int index = 0;
        for (; index < 6; index++)
        {
            Feature feature = new Feature(FeatureDataList[index]);
            _positiveFeatureList.Add(feature);
        }
        for (; index < FeatureDataList.Count; index++)
        {
            Feature feature = new Feature(FeatureDataList[index]);
            _negativeFeatureList.Add(feature);
        }

        //테스트용
        //GenerateRandomUnit();
    }


}
