using System.Collections.Generic;
using UnityEngine;

public class UnitManager : BehaviourSingleton<UnitManager>
{
    private List<UnitStat> _units;
    public List<UnitStat> Units => _units;


    [Header("유닛 프리팹")]
    [SerializeField] private List<GameObject> _unitPrefabs;
    [SerializeField] private List<Feature> _features;


    //랜덤 min/max 값
    private int _minValueMaxHealth;
    private int _maxValueMaxHealth;

    private int _minValueDamage;
    private int _maxValueDamage;

    private int _minValueAttackSpeed;
    private int _maxValueAttackSpeed;

    private int _minValueMoveSpeed;
    private int _maxValueMoveSpeed;

    private int _minValueWoodSpeed;
    private int _maxValueWoodSpeed;

    private int _minValueRockSpeed;
    private int _maxValueRockSpeed;

    private void Start()
    {
        // TODO: 랜덤 최소 최대값, 특성 데이터 받아오기
    }
    public void GenerateRandomUnit()
    {
        GameObject unit = Instantiate(_unitPrefabs[Random.Range(0, _unitPrefabs.Count)]);
        UnitStat unitStat = unit.GetComponent<UnitStat>();

        int randomMaxHealth = Random.Range(_minValueMaxHealth, _maxValueMaxHealth + 1);
        int randomDamage = Random.Range(_minValueDamage, _maxValueDamage + 1);
        int randomAttackSpeed = Random.Range(_minValueAttackSpeed, _maxValueAttackSpeed + 1);
        int randomMoveSpeed = Random.Range(_minValueMoveSpeed, _maxValueMoveSpeed + 1);
        int randomWoodSpeed = Random.Range(_minValueWoodSpeed, _maxValueWoodSpeed + 1);
        int randomRockSpeed = Random.Range(_minValueRockSpeed, _maxValueRockSpeed + 1);


        float getFeatureProbabilty = 0.9f;
        for(int i = 0; i<3; i++)
        {
            if(Random.value < getFeatureProbabilty)
            {
                int randomIndex = Random.Range(0, 6);
                unitStat.GoodFeatures.Add(_features[randomIndex]);
                getFeatureProbabilty -= 0.4f;
            }
        }

        getFeatureProbabilty = 0.9f;
        for (int i = 0; i < 3; i++)
        {
            if (Random.value < getFeatureProbabilty)
            {
                int randomIndex = Random.Range(6, _features.Count);
                unitStat.BadFeatures.Add(_features[randomIndex]);
                getFeatureProbabilty -= 0.4f;
            }
        }
    }
}
