using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouristInstance : MonoBehaviour
{
    List<int> _touristRoute;
    StreetManager _streetManager;
    float moveDuration = 3f;
    [SerializeField]
    GameObject[] _characters;
    Animator _animator;
    float _touristSpeed;
    float globalSpeed;
    float _randomFactor;
    UpgradesManager _upgradesManager;
    SpeedUpManager _speedUpManager;
    public void SetRoute(List<int> route, StreetManager sManager, UpgradesManager upgradesManager, SpeedUpManager speedUpManager)
    {
        _upgradesManager = upgradesManager;
        _speedUpManager = speedUpManager;
        int random = Random.Range(0, _characters.Length);
        _randomFactor = Random.Range(1f, 2f);
        _touristSpeed = GetSpeed();
        _characters[random].SetActive(true);
        _animator = _characters[random].GetComponent<Animator>();
        _touristRoute = route;
        _streetManager = sManager;
        StartCoroutine(Move());
    }

    public float GetSpeed()
    {
        float upgradeExtraSpeed = 1 + (_upgradesManager.GetExtraTouristSpeed() / 100f);
        float happyHourSpeed = _speedUpManager.GetHappyHourSpeed();
        return _randomFactor * upgradeExtraSpeed * happyHourSpeed;
    }

    IEnumerator Move()
    {
        globalSpeed = GetSpeed();
        string finalP = "";
        for (int i = 0; i<_touristRoute.Count; i++)
        {
            finalP += _touristRoute[i];
        }

        Vector3 initialPos = Vector3.zero;
        _touristSpeed= GetSpeed();

        _animator.SetFloat("Speed", _touristSpeed);
        switch (_touristRoute[0])
        {
            case 6:
            case 0:
            case 2:
            case 8:
                initialPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[0], 0) + new Vector3(-2,0,0);
                break;
            case 7:
            case 1:
            case 3:
            case 9:
                initialPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[0], 0) + new Vector3(2, 0, 0);
                break;
            case 4:
                if (Random.value < 0.5f)
                {
                    initialPos = _streetManager.GetExpositorTransformPosByCoords(7, 0) + new Vector3(2, 0, 0);
                }
                else
                {
                    initialPos = _streetManager.GetExpositorTransformPosByCoords(6, 0) + new Vector3(-2, 0, 0);
                }
                break;
            case 5:
                if (Random.value < 0.5f)
                {
                    initialPos = _streetManager.GetExpositorTransformPosByCoords(9, 0) + new Vector3(2, 0, 0);
                }
                else
                {
                    initialPos = _streetManager.GetExpositorTransformPosByCoords(8, 0) + new Vector3(-2, 0, 0);
                }
                break;
        }

        Vector3 startLerpPos = initialPos;
        Vector3 targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[0], 0);
        targetLerpPos += Vector3.right * Random.Range(-0.75f, 0.75f);
        float lerpDistance = Mathf.Abs((targetLerpPos - startLerpPos).magnitude);
        float lerpTime = lerpDistance /  _touristSpeed;

        if(startLerpPos.x == targetLerpPos.x)
        {
            if(startLerpPos.y < targetLerpPos.y)
            {
                _animator.SetTrigger("Up");
            }
            else
            {
                if(startLerpPos.y > targetLerpPos.y)
                {
                    _animator.SetTrigger("Down");
                }
            }
        }
        else
        {
            if (startLerpPos.x < targetLerpPos.x)
            {
                _animator.SetTrigger("Right");
            }
            else
            {
                _animator.SetTrigger("Left");
            }
        }
        for (float j = 0; j < lerpTime; j += Time.deltaTime)
        {

            if (_touristSpeed != globalSpeed)
            {

                float previousLerpTime = lerpTime;
                float previousJ = j;
                float currentLerp = j / lerpTime;
                lerpTime = lerpDistance / globalSpeed;
                j *= lerpTime/ previousLerpTime ;
                
                float newCurrentLerp = j/ lerpTime;

                _touristSpeed = globalSpeed;

            }
            transform.position = Vector3.Lerp(startLerpPos, targetLerpPos, j / lerpTime);
            yield return null;
        }
        transform.position = targetLerpPos;
        _animator.SetTrigger("Idle");
        float waitingTime = Random.Range(0.5f, 1.5f);
        yield return new WaitForSeconds(waitingTime/2f);
        GameEvents.TouristWatchDino.Invoke(_touristRoute[0]);
        yield return new WaitForSeconds(waitingTime/2f);


        for (int i = 0; i<_touristRoute.Count -1; i++)
        {
            int startingPoint = _touristRoute[i];
            int targetPoint = _touristRoute[i + 1];
            int initialSide = Random.Range(3,5);

            startLerpPos = transform.position;
            targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[i],initialSide);
            lerpDistance = Mathf.Abs((targetLerpPos - startLerpPos).magnitude);
            lerpTime = lerpDistance / _touristSpeed;
            if (startLerpPos.x == targetLerpPos.x)
            {
                if (startLerpPos.y < targetLerpPos.y)
                {
                    _animator.SetTrigger("Up");
                }
                else
                {
                    if (startLerpPos.y > targetLerpPos.y)
                    {
                        _animator.SetTrigger("Down");
                    }
                }
            }
            else
            {
                if (startLerpPos.x < targetLerpPos.x)
                {
                    _animator.SetTrigger("Right");
                }
                else
                {
                    _animator.SetTrigger("Left");
                }
            }
            for (float j = 0; j<lerpTime; j += Time.deltaTime)
            {
                if (_touristSpeed != globalSpeed)
                {
                    float previousLerpTime = lerpTime;
                    float previousJ = j;
                    float currentLerp = j / lerpTime;
                    lerpTime = lerpDistance / globalSpeed;
                    j *=lerpTime/ previousLerpTime;

                    float newCurrentLerp = j / lerpTime;
                    _touristSpeed = globalSpeed;
                }
                transform.position = Vector3.Lerp(startLerpPos, targetLerpPos, (j / lerpTime));
                yield return null;
            }
            transform.position = targetLerpPos;
            //Obtengo las direcciones para ir desde startPoint hasta targetPoint
            List <StreetManager.Directions> directionsList = _streetManager.GetDirectionsList(startingPoint, targetPoint);

            for (int j = 0; j<directionsList.Count; j++)
            {
                Vector2 initialMatrixCoord = _streetManager.GetExpoCoordsByIndex(startingPoint);
                Vector2 targetMatrixCoord = initialMatrixCoord;
                switch (directionsList[j])
                {
                    case StreetManager.Directions.Up:
                        targetMatrixCoord += new Vector2(-1,0);
                        break;
                    case StreetManager.Directions.Down:
                        targetMatrixCoord += new Vector2(1, 0);
                        break;
                    case StreetManager.Directions.Left:
                        targetMatrixCoord += new Vector2(0, -1);
                        break;
                    case StreetManager.Directions.Right:
                        targetMatrixCoord += new Vector2(0, 1);
                        break;
                }
                targetPoint = _streetManager.GetExpoIndexByCoords(targetMatrixCoord);
                startLerpPos = _streetManager.GetExpositorTransformPosByCoords(startingPoint, initialSide);
                targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(targetPoint, initialSide);
                lerpDistance = Mathf.Abs((targetLerpPos - startLerpPos).magnitude);
                lerpTime = lerpDistance /  _touristSpeed;
                if (startLerpPos.x == targetLerpPos.x)
                {
                    if (startLerpPos.y < targetLerpPos.y)
                    {
                        _animator.SetTrigger("Up");

                    }
                    else
                    {
                        if (startLerpPos.y > targetLerpPos.y)
                        {
                            _animator.SetTrigger("Down");

                        }
                    }
                }
                else
                {
                    if (startLerpPos.x < targetLerpPos.x)
                    {
                        _animator.SetTrigger("Right");
                    }
                    else
                    {
                        _animator.SetTrigger("Left");
                    }
                }
                for (float l = 0; l< lerpTime; l += Time.deltaTime)
                {
                    if (_touristSpeed != globalSpeed)
                    {

                        float previousLerpTime = lerpTime;
                        float previousJ = l;
                        float currentLerp = l / lerpTime;
                        lerpTime = lerpDistance / globalSpeed;
                        l *= lerpTime / previousLerpTime ;

                        float newCurrentLerp = l / lerpTime;

                    }
                    transform.position = Vector3.Lerp(startLerpPos, targetLerpPos, l/ lerpTime);
                    yield return null;
                }
                transform.position = targetLerpPos;
                startingPoint = targetPoint; 
            }

            startLerpPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[i + 1], initialSide);
            targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[i + 1], 0);
            targetLerpPos += Vector3.right * Random.Range(-0.75f, 0.75f);

            lerpDistance = Mathf.Abs((targetLerpPos - startLerpPos).magnitude);
            lerpTime = lerpDistance /  _touristSpeed;
            if (startLerpPos.x == targetLerpPos.x)
            {
                if (startLerpPos.y < targetLerpPos.y)
                {
                    _animator.SetTrigger("Up");

                }
                else
                {
                    if (startLerpPos.y > targetLerpPos.y)
                    {
                        _animator.SetTrigger("Down");

                    }
                }
            }
            else
            {
                if (startLerpPos.x < targetLerpPos.x)
                {
                    _animator.SetTrigger("Right");
                }
                else
                {
                    _animator.SetTrigger("Left");
                }
            }
            for (float k = 0; k < lerpTime; k += Time.deltaTime)
            {
                if (_touristSpeed != globalSpeed)
                {
                    float previousLerpTime = lerpTime;
                    float previousJ = k;
                    float currentLerp = k / lerpTime;
                    lerpTime = lerpDistance / globalSpeed;
                    k*= lerpTime / previousLerpTime ;

                    float newCurrentLerp = k / lerpTime;
                }
                transform.position = Vector3.Lerp(startLerpPos, targetLerpPos, (k / lerpTime));
                yield return null;
            }
            transform.position = targetLerpPos;
            _animator.SetTrigger("Idle");
            waitingTime = Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(waitingTime / 2f);

            //LANZAR MONEDA
            GameEvents.TouristWatchDino.Invoke(_touristRoute[i+1]);
            yield return new WaitForSeconds(waitingTime / 2f);
            yield return new WaitForSeconds(0.2f);
        }

        switch (_touristRoute[_touristRoute.Count -1])
        {
            case 0:
            case 2:
            case 6:
            case 8:
                targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[_touristRoute.Count - 1], 0) + new Vector3(-2, 0, 0);
                break;
            case 7:
            case 1:
            case 3:
            case 9:
                targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(_touristRoute[_touristRoute.Count - 1], 0) + new Vector3(2, 0, 0);
                break;
            case 4:
                if (Random.value < 0.5f)
                {
                    targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(7, 0) + new Vector3(2, 0, 0);
                }
                else
                {
                    targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(6, 0) + new Vector3(-2, 0, 0);
                }
                break;
            case 5:
                if (Random.value < 0.5f)
                {
                    targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(9, 0) + new Vector3(2, 0, 0);
                }
                else
                {
                    targetLerpPos = _streetManager.GetExpositorTransformPosByCoords(8, 0) + new Vector3(-2, 0, 0);
                }
                break;
        }
        startLerpPos = transform.position;
        lerpDistance = Mathf.Abs((targetLerpPos - startLerpPos).magnitude);
        lerpTime = lerpDistance / _touristSpeed;
        if (startLerpPos.x == targetLerpPos.x)
        {
            if (startLerpPos.y < targetLerpPos.y)
            {
                _animator.SetTrigger("Up");

            }
            else
            {
                if (startLerpPos.y > targetLerpPos.y)
                {
                    _animator.SetTrigger("Down");
                }
            }
        }
        else
        {
            if (startLerpPos.x < targetLerpPos.x)
            {
                _animator.SetTrigger("Right");
            }
            else
            {
                _animator.SetTrigger("Left");
            }
        }
        for (float j = 0; j < lerpTime; j += Time.deltaTime)
        {
            if (_touristSpeed != globalSpeed)
            {
                float previousLerpTime = lerpTime;
                float previousJ = j;
                float currentLerp = j / lerpTime;
                lerpTime = lerpDistance / globalSpeed;
                j /= (currentLerp / lerpTime);
                float newCurrentLerp = j / lerpTime;
            }
            transform.position = Vector3.Lerp(startLerpPos, targetLerpPos, j / lerpTime);
            yield return null;
        }
        transform.position = targetLerpPos;
        Destroy(gameObject,1f);
    }

    private void Update()
    {
        _animator.speed = _touristSpeed;
        globalSpeed = GetSpeed();

    }
}
