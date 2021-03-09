using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetManager : MonoBehaviour
{
    [SerializeField]
    float _streetCellDistance;
    [SerializeField]
    GameObject _streetPointPrefab;
    [SerializeField]
    GameObject _touristPrefab;
    List<List<Vector3>> _streetPointsPositons;
    List<List<int>> _expositorsMatrix;
    List<Vector2> _expoMatrixCoordinates;
    float _visitorSpawnTime = 3f;
    float _currentVisitorTime = 0f;
    UpgradesManager _upgradesManager;
    SpeedUpManager _speedUpManager;
    public enum Directions { Up, Down, Left, Right };

    List<List<List<List<Directions>>>> _directionsManager; 

    private void Start()
    {
        _upgradesManager = FindObjectOfType<UpgradesManager>();
        _speedUpManager = FindObjectOfType<SpeedUpManager>();
        _directionsManager = new List<List<List<List<Directions>>>>();
        _expositorsMatrix = new List<List<int>>();
        _expositorsMatrix.Add(new List<int>() { 6, 4, 7 });
        _expositorsMatrix.Add(new List<int>() { 0, -1, 1 });
        _expositorsMatrix.Add(new List<int>() { 2, -1, 3 });
        _expositorsMatrix.Add(new List<int>() { 8, 5, 9 });

        _expoMatrixCoordinates = new List<Vector2>();
        _expoMatrixCoordinates.Add(new Vector2(1,0));
        _expoMatrixCoordinates.Add(new Vector2(1,2));
        _expoMatrixCoordinates.Add(new Vector2(2,0));
        _expoMatrixCoordinates.Add(new Vector2(2,2));
        _expoMatrixCoordinates.Add(new Vector2(0,1));
        _expoMatrixCoordinates.Add(new Vector2(3,1));
        _expoMatrixCoordinates.Add(new Vector2(0,0));
        _expoMatrixCoordinates.Add(new Vector2(0,2));
        _expoMatrixCoordinates.Add(new Vector2(3,0));
        _expoMatrixCoordinates.Add(new Vector2(3, 2));

        // ZERO  / / / / / / /

        List<List<List<Directions>>> _directionsListManagerZero = new List<List<List<Directions>>>();
        for (int i = 0; i < 10; i++)
        {
            _directionsListManagerZero.Add(new List<List<Directions>>());
        }
        _directionsListManagerZero[0].Add(new List<Directions>());
        _directionsListManagerZero[1].Add(new List<Directions>() { Directions.Up, Directions.Right, Directions.Right, Directions.Down });
        _directionsListManagerZero[2].Add(new List<Directions>() { Directions.Down });
        _directionsListManagerZero[3].Add(new List<Directions>() { Directions.Up, Directions.Right, Directions.Right, Directions.Down, Directions.Down });
        _directionsListManagerZero[3].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Right, Directions.Right, Directions.Up });
        _directionsListManagerZero[4].Add(new List<Directions>() { Directions.Up, Directions.Right });
        _directionsListManagerZero[5].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Right });
        _directionsListManagerZero[6].Add(new List<Directions>() { Directions.Up });
        _directionsListManagerZero[7].Add(new List<Directions>() { Directions.Up, Directions.Right, Directions.Right });
        _directionsListManagerZero[8].Add(new List<Directions>() { Directions.Down, Directions.Down });
        _directionsListManagerZero[9].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Right, Directions.Right });
        _directionsManager.Add(_directionsListManagerZero);

        // ONE  / / / / / / /

        List<List<List<Directions>>> _directionsListManagerOne = new List<List<List<Directions>>>();
        for (int i = 0; i < 10; i++)
        {
            _directionsListManagerOne.Add(new List<List<Directions>>());
        }

        _directionsListManagerOne[0].Add(new List<Directions>() { Directions.Up, Directions.Left, Directions.Left, Directions.Down });
        _directionsListManagerOne[1].Add(new List<Directions>());
        _directionsListManagerOne[2].Add(new List<Directions>() { Directions.Up, Directions.Left, Directions.Left, Directions.Down, Directions.Down });
        _directionsListManagerOne[2].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Left, Directions.Left, Directions.Up });
        _directionsListManagerOne[3].Add(new List<Directions>() { Directions.Down });
        _directionsListManagerOne[4].Add(new List<Directions>() { Directions.Up, Directions.Left });
        _directionsListManagerOne[5].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Left });
        _directionsListManagerOne[6].Add(new List<Directions>() { Directions.Up, Directions.Left, Directions.Left });
        _directionsListManagerOne[7].Add(new List<Directions>() { Directions.Up });
        _directionsListManagerOne[8].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Left, Directions.Left });
        _directionsListManagerOne[9].Add(new List<Directions>() { Directions.Down, Directions.Down });
        _directionsManager.Add(_directionsListManagerOne);

        // TWO  / / / / / / /

        List<List<List<Directions>>> _directionsListManagerTwo = new List<List<List<Directions>>>();
        for (int i = 0; i < 10; i++)
        {
            _directionsListManagerTwo.Add(new List<List<Directions>>());
        }

        _directionsListManagerTwo[0].Add(new List<Directions>() { Directions.Up });
        _directionsListManagerTwo[1].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Right, Directions.Right, Directions.Down });
        _directionsListManagerTwo[1].Add(new List<Directions>() { Directions.Down, Directions.Right, Directions.Right, Directions.Up, Directions.Up });
        _directionsListManagerTwo[2].Add(new List<Directions>());
        _directionsListManagerTwo[3].Add(new List<Directions>() { Directions.Down, Directions.Right, Directions.Right, Directions.Up });
        _directionsListManagerTwo[4].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Right });
        _directionsListManagerTwo[5].Add(new List<Directions>() { Directions.Down, Directions.Right });
        _directionsListManagerTwo[6].Add(new List<Directions>() { Directions.Up, Directions.Up });
        _directionsListManagerTwo[7].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Right, Directions.Right });
        _directionsListManagerTwo[8].Add(new List<Directions>() { Directions.Down });
        _directionsListManagerTwo[9].Add(new List<Directions>() { Directions.Down, Directions.Right, Directions.Right });
        _directionsManager.Add(_directionsListManagerTwo);
        // THREE  / / / / / / /

        List<List<List<Directions>>> _directionsListManagerThree = new List<List<List<Directions>>>();
        for (int i = 0; i < 10; i++)
        {
            _directionsListManagerThree.Add(new List<List<Directions>>());
        }

        _directionsListManagerThree[0].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Left, Directions.Left, Directions.Down });
        _directionsListManagerThree[0].Add(new List<Directions>() { Directions.Down, Directions.Left, Directions.Left, Directions.Up, Directions.Up });
        _directionsListManagerThree[1].Add(new List<Directions>() { Directions.Up});
        _directionsListManagerThree[2].Add(new List<Directions>() { Directions.Down, Directions.Left, Directions.Left, Directions.Up});
        _directionsListManagerThree[3].Add(new List<Directions>());
        _directionsListManagerThree[4].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Left });
        _directionsListManagerThree[5].Add(new List<Directions>() { Directions.Down, Directions.Left});
        _directionsListManagerThree[6].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Left, Directions.Left});
        _directionsListManagerThree[7].Add(new List<Directions>() { Directions.Up, Directions.Up });
        _directionsListManagerThree[8].Add(new List<Directions>() { Directions.Down, Directions.Left, Directions.Left });
        _directionsListManagerThree[9].Add(new List<Directions>() { Directions.Down });
        _directionsManager.Add(_directionsListManagerThree);

        // FOUR  / / / / / / /

        List<List<List<Directions>>> _directionsListManagerFour = new List<List<List<Directions>>>();
        for (int i = 0; i < 10; i++)
        {
            _directionsListManagerFour.Add(new List<List<Directions>>());
        }

        _directionsListManagerFour[0].Add(new List<Directions>() { Directions.Left, Directions.Down });
        _directionsListManagerFour[1].Add(new List<Directions>() { Directions.Right, Directions.Down });
        _directionsListManagerFour[2].Add(new List<Directions>() { Directions.Left, Directions.Down, Directions.Down });
        _directionsListManagerFour[3].Add(new List<Directions>() { Directions.Right, Directions.Down, Directions.Down });
        _directionsListManagerFour[4].Add(new List<Directions>());
        _directionsListManagerFour[5].Add(new List<Directions>() { Directions.Right, Directions.Down, Directions.Down, Directions.Down, Directions.Left });
        _directionsListManagerFour[5].Add(new List<Directions>() { Directions.Left, Directions.Down, Directions.Down, Directions.Down, Directions.Right });
        _directionsListManagerFour[6].Add(new List<Directions>() { Directions.Left });
        _directionsListManagerFour[7].Add(new List<Directions>() { Directions.Right });
        _directionsListManagerFour[8].Add(new List<Directions>() { Directions.Left, Directions.Down, Directions.Down, Directions.Down });
        _directionsListManagerFour[9].Add(new List<Directions>() { Directions.Right, Directions.Down, Directions.Down, Directions.Down });
        _directionsManager.Add(_directionsListManagerFour );

        // FIVE  / / / / / / /

        List<List<List<Directions>>> _directionsListManagerFive = new List<List<List<Directions>>>();
        for (int i = 0; i < 10; i++)
        {
            _directionsListManagerFive.Add(new List<List<Directions>>());
        }

        _directionsListManagerFive[0].Add(new List<Directions>() { Directions.Left, Directions.Up, Directions.Up });
        _directionsListManagerFive[1].Add(new List<Directions>() { Directions.Right, Directions.Up, Directions.Up });
        _directionsListManagerFive[2].Add(new List<Directions>() { Directions.Left, Directions.Up });
        _directionsListManagerFive[3].Add(new List<Directions>() { Directions.Right, Directions.Up });
        _directionsListManagerFive[4].Add(new List<Directions>() { Directions.Right, Directions.Up, Directions.Up, Directions.Up, Directions.Left });
        _directionsListManagerFive[4].Add(new List<Directions>() { Directions.Left, Directions.Up, Directions.Up, Directions.Up, Directions.Right });
        _directionsListManagerFive[5].Add(new List<Directions>());
        _directionsListManagerFive[6].Add(new List<Directions>() { Directions.Left, Directions.Up, Directions.Up, Directions.Up });
        _directionsListManagerFive[7].Add(new List<Directions>() { Directions.Right, Directions.Up, Directions.Up, Directions.Up });
        _directionsListManagerFive[8].Add(new List<Directions>() { Directions.Left });
        _directionsListManagerFive[9].Add(new List<Directions>() { Directions.Right });
        _directionsManager.Add(_directionsListManagerFive);

        // SIX  / / / / / / /

        List<List<List<Directions>>> _directionsListManagerSix = new List<List<List<Directions>>>();
        for (int i = 0; i < 10; i++)
        {
            _directionsListManagerSix.Add(new List<List<Directions>>());
        }

        _directionsListManagerSix[0].Add(new List<Directions>() { Directions.Down });
        _directionsListManagerSix[1].Add(new List<Directions>() { Directions.Right, Directions.Right, Directions.Down });
        _directionsListManagerSix[2].Add(new List<Directions>() { Directions.Down, Directions.Down });
        _directionsListManagerSix[3].Add(new List<Directions>() { Directions.Right, Directions.Right, Directions.Down, Directions.Down });
        _directionsListManagerSix[4].Add(new List<Directions>() { Directions.Right });
        _directionsListManagerSix[5].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Down, Directions.Right });
        _directionsListManagerSix[6].Add(new List<Directions>());
        _directionsListManagerSix[7].Add(new List<Directions>() { Directions.Right, Directions.Right });
        _directionsListManagerSix[8].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Down });
        _directionsListManagerSix[9].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Down, Directions.Right, Directions.Right });
        _directionsListManagerSix[9].Add(new List<Directions>() { Directions.Right, Directions.Right, Directions.Down, Directions.Down, Directions.Down });
        _directionsManager.Add(_directionsListManagerSix);

        // SEVEN  / / / / / / /

        List<List<List<Directions>>> _directionsListManagerSeven = new List<List<List<Directions>>>();
        for (int i = 0; i < 10; i++)
        {
            _directionsListManagerSeven.Add(new List<List<Directions>>());
        }

        _directionsListManagerSeven[0].Add(new List<Directions>() { Directions.Left, Directions.Left, Directions.Down });
        _directionsListManagerSeven[1].Add(new List<Directions>() { Directions.Down });
        _directionsListManagerSeven[2].Add(new List<Directions>() { Directions.Left, Directions.Left, Directions.Down, Directions.Down });
        _directionsListManagerSeven[3].Add(new List<Directions>() { Directions.Down, Directions.Down });
        _directionsListManagerSeven[4].Add(new List<Directions>() { Directions.Left });
        _directionsListManagerSeven[5].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Down, Directions.Left });
        _directionsListManagerSeven[6].Add(new List<Directions>() { Directions.Left, Directions.Left });
        _directionsListManagerSeven[7].Add(new List<Directions>());
        _directionsListManagerSeven[8].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Down, Directions.Left, Directions.Left });
        _directionsListManagerSeven[8].Add(new List<Directions>() { Directions.Left, Directions.Left, Directions.Down, Directions.Down, Directions.Down });
        _directionsListManagerSeven[9].Add(new List<Directions>() { Directions.Down, Directions.Down, Directions.Down });
        _directionsManager.Add(_directionsListManagerSeven);

        // EIGHT  / / / / / / /

        List<List<List<Directions>>> _directionsListManagerEight = new List<List<List<Directions>>>();
        for (int i = 0; i < 10; i++)
        {
            _directionsListManagerEight.Add(new List<List<Directions>>());
        }

        _directionsListManagerEight[0].Add(new List<Directions>() { Directions.Up, Directions.Up });
        _directionsListManagerEight[1].Add(new List<Directions>() { Directions.Right, Directions.Right, Directions.Up, Directions.Up });
        _directionsListManagerEight[2].Add(new List<Directions>() { Directions.Up });
        _directionsListManagerEight[3].Add(new List<Directions>() { Directions.Right, Directions.Right, Directions.Up });
        _directionsListManagerEight[4].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Up, Directions.Right });
        _directionsListManagerEight[5].Add(new List<Directions>() { Directions.Right });
        _directionsListManagerEight[6].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Up });
        _directionsListManagerEight[7].Add(new List<Directions>() { Directions.Right, Directions.Right, Directions.Up, Directions.Up, Directions.Up });
        _directionsListManagerEight[7].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Up, Directions.Right, Directions.Right });
        _directionsListManagerEight[8].Add(new List<Directions>());
        _directionsListManagerEight[9].Add(new List<Directions>() { Directions.Right, Directions.Right });
        _directionsManager.Add(_directionsListManagerEight);

        // NINE  / / / / / / /

        List<List<List<Directions>>> _directionsListManagerNine = new List<List<List<Directions>>>();
        for (int i = 0; i < 10; i++)
        {
            _directionsListManagerNine.Add(new List<List<Directions>>());
        }

        _directionsListManagerNine[0].Add(new List<Directions>() { Directions.Left, Directions.Left, Directions.Up, Directions.Up });
        _directionsListManagerNine[1].Add(new List<Directions>() { Directions.Up, Directions.Up });
        _directionsListManagerNine[2].Add(new List<Directions>() { Directions.Left, Directions.Left, Directions.Up });
        _directionsListManagerNine[3].Add(new List<Directions>() { Directions.Up });
        _directionsListManagerNine[4].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Up, Directions.Left });
        _directionsListManagerNine[5].Add(new List<Directions>() { Directions.Left });
        _directionsListManagerNine[6].Add(new List<Directions>() { Directions.Left, Directions.Left, Directions.Up, Directions.Up, Directions.Up });
        _directionsListManagerNine[6].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Up, Directions.Left, Directions.Left });
        _directionsListManagerNine[7].Add(new List<Directions>() { Directions.Up, Directions.Up, Directions.Up });
        _directionsListManagerNine[8].Add(new List<Directions>() { Directions.Left, Directions.Left });
        _directionsListManagerNine[9].Add(new List<Directions>());
        _directionsManager.Add(_directionsListManagerNine);
    }

    public void Init(List<ExpositorInstance> expositors)
    {
        _streetPointsPositons = new List<List<Vector3>>();
        for (int i = 0; i<expositors.Count; i++)
        {
            List<Vector3> aux = new List<Vector3>();
            GameObject botMidPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.down * _streetCellDistance), _streetPointPrefab.transform.rotation);
            GameObject botLeftPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.down * _streetCellDistance) + (Vector3.left * _streetCellDistance), _streetPointPrefab.transform.rotation);
            GameObject botRightPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.down * _streetCellDistance) + (Vector3.right * _streetCellDistance), _streetPointPrefab.transform.rotation);
            GameObject topLeftPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.up * _streetCellDistance) + (Vector3.left * _streetCellDistance), _streetPointPrefab.transform.rotation);
            GameObject topRightPoint = Instantiate(_streetPointPrefab, expositors[i].transform.position + (Vector3.up * _streetCellDistance) + (Vector3.right * _streetCellDistance), _streetPointPrefab.transform.rotation);
            
            aux.Add(botMidPoint.transform.position);
            aux.Add(topLeftPoint.transform.position);
            aux.Add(topRightPoint.transform.position);
            aux.Add(botLeftPoint.transform.position);
            aux.Add(botRightPoint.transform.position);
            
            _streetPointsPositons.Add(aux);
        }
    }

    public List<int> CreateRoute()
    {
        List<int> streetPointsIndex = new List<int>();

        for (int i = 0; i < UserDataController._currentUserData._unlockedExpositors; i++)
        {
            streetPointsIndex.Add(i);
        }

        for(int i = 0; i<15; i++)
        {
            int firstIndex = Random.Range(0,streetPointsIndex.Count);
            int secondIndex = Random.Range(0, streetPointsIndex.Count);
            int aux = streetPointsIndex[firstIndex];
            streetPointsIndex[firstIndex] = streetPointsIndex[secondIndex];
            streetPointsIndex[secondIndex] = aux;
        }
        return streetPointsIndex;
    }

    private void Update()
    {
        _currentVisitorTime += Time.deltaTime * (1 + _upgradesManager.GetExtraTouristSpeed() / 100f);
        if(_currentVisitorTime >= _visitorSpawnTime)
        {
            SpawnTourist();
            _currentVisitorTime = 0;
            _visitorSpawnTime = Random.Range(1f,3f);
        }
    }
    public void  SpawnTourist()
    {
        List<int> route = CreateRoute();
        GameObject nTourist = Instantiate(_touristPrefab, Vector3.zero, Quaternion.identity);
        nTourist.GetComponent<TouristInstance>().SetRoute(route, this, _upgradesManager, _speedUpManager);  
    }

    public Vector3 GetExpositorTransformPosByCoords(int expoIndex, int pointIndex)
    {
        return _streetPointsPositons[expoIndex][pointIndex];
    }

    public Vector2 GetExpoCoordsByIndex(int expositorIndex)
    {
        return _expoMatrixCoordinates[expositorIndex];
    }

    public int GetExpoIndexByCoords(Vector2 coords)
    {
        return _expositorsMatrix[(int)coords.x][(int)coords.y];
    }

    public List<Directions> GetDirectionsList(int origin, int dest)
    {
        int dirIndex = Random.Range(0, _directionsManager[origin][dest].Count);
        return _directionsManager[origin][dest][dirIndex];
    }
}
