using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

[System.Serializable]
public class GameCurrency
{
    int[] _currencyUnits;
    string[] _currencyNames = {"", "K", "M", "B", "T", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ" };
    public GameCurrency()
    {
        _currencyUnits = new int[31];
    }
    public GameCurrency(int[] currencyEntry)
    {
        _currencyUnits = new int[31];
        for (int i = 0; i<_currencyUnits.Length; i++)
        {
            if(currencyEntry.Length >= i +1)
            {
                _currencyUnits[i] = currencyEntry[i];
            }
            else
            {
                _currencyUnits[i] = 0;
            }
            
        }
    }
    public GameCurrency(int units)
    {
        _currencyUnits = new int[31];
        _currencyUnits[0] = units;
        ConvertUnits();
    }
    public int[] GetIntList()
    {
        return _currencyUnits;
    }
    public void SetCurrencyAtIndex(int index, int amount)
    {
        _currencyUnits[index] = amount;
    }
    public void AddCurrency(GameCurrency currency)
    {
        for (int i = 0; i < _currencyUnits.Length; i++)
        {
            _currencyUnits[i] += currency.GetIntList()[i];
        }
        ConvertUnits();
    }
    public bool SubstractCurrency(GameCurrency currency)
    {
        bool canSubstract = CompareCurrencies(currency);
        //Paso 1: Comprobar si el último número es negativo
        if (canSubstract)
        {
            //Paso 2: Resta normal
            for (int i = 0; i < _currencyUnits.Length; i++)
            {
                _currencyUnits[i] -= currency.GetIntList()[i];
            }

            //Paso 3 sustituir negativos por el indice mayor
            bool hasNegative;
            do
            {
                hasNegative = false;
                for (int i = _currencyUnits.Length-1; i >= 0; i--)
                {
                    if (_currencyUnits[i] < 0)
                    {
                        _currencyUnits[i] += 1000;
                        _currencyUnits[i + 1]--;
                        hasNegative = true;
                    }
                }
            } while (hasNegative);
            ConvertUnits();
        }
        return canSubstract;
    }


    public bool CompareCurrencies(GameCurrency currency)
    {
        bool canSubstract = true;
        for (int i = _currencyUnits.Length -1; i >= 0; i--)
        {
            if (_currencyUnits[i] > currency.GetIntList()[i])
            {
                canSubstract = true;
                break;
            }
            else
            {
                if (_currencyUnits[i] < currency.GetIntList()[i])
                {
                    canSubstract = false;
                    break;
                }
            }
        }
        return canSubstract;
    }


    public void ConvertUnits()
    {
        for(int i = 0; i<_currencyUnits.Length; i++)
        {
            if(_currencyUnits[i] >= 1000)
            {
                int nextCurrencyTypeAmount = 0;
                nextCurrencyTypeAmount = _currencyUnits[i] / 1000;
                if(i+1 < _currencyUnits.Length)
                {
                    _currencyUnits[i + 1] += nextCurrencyTypeAmount;
                    _currencyUnits[i] -= nextCurrencyTypeAmount * 1000;
                }
            }
        }
    }

    public string GetCurrentMoneyConvertedTo3Chars()
    {
        int currentMoney = 0;
        int charIndex = 0;
        for (int i = _currencyUnits.Length - 1; i >= 0; i--)
        {
            if (_currencyUnits[i] > 0)
            {
                if (i > 0)
                {
                    currentMoney = _currencyUnits[i - 1] + (_currencyUnits[i] * 1000);
                    charIndex = i - 1;
                    if (currentMoney > 10000)
                    {
                        currentMoney /= 1000;
                        charIndex++;
                    }
                    break;
                }
                else
                {
                    currentMoney = _currencyUnits[0];
                }
            }
        }
        return currentMoney + _currencyNames[charIndex];
    }

    public string GetCurrentMoney()
    {
        int currentMoney = 0;
        int charIndex = 0;
        for (int i = _currencyUnits.Length -1; i >= 0; i--)
        {
            if (_currencyUnits[i] > 0)
            {
                if (i > 0)
                {
                    currentMoney = _currencyUnits[i - 1] + (_currencyUnits[i] * 1000);
                    charIndex = i-1;
                    break;
                }
                else
                {
                    currentMoney = _currencyUnits[0];
                }
            }
        }
        return currentMoney + _currencyNames[charIndex];
    }

    public List<string> GetIntAndCurrency()
    {
        List<string> finalList = new List<string>();
        int currentMoney = 0;
        int charIndex = 0;
        for (int i = _currencyUnits.Length - 1; i >= 0; i--)
        {
            if (_currencyUnits[i] > 0)
            {
                if (i > 0)
                {
                    currentMoney = _currencyUnits[i - 1] + (_currencyUnits[i] * 1000);
                    charIndex = i - 1;
                    break;
                }
                else
                {
                    currentMoney = _currencyUnits[0];
                }
            }
        }
        finalList.Add(currentMoney.ToString());
        finalList.Add(charIndex.ToString());

        return finalList;
    }

    public void MultiplyCurrency(float multiplier)
    {
        for (int i = 0; i<_currencyUnits.Length; i++)
        {
            float multipliedCurrency = _currencyUnits[i] * multiplier;
            int finalMultipliedCurrency = 0;

            if(i == 0)
            {
                finalMultipliedCurrency = Mathf.RoundToInt(multipliedCurrency);
            }
            else
            {
                finalMultipliedCurrency = Mathf.FloorToInt(multipliedCurrency);
                float difference = multipliedCurrency - finalMultipliedCurrency;
                _currencyUnits[i - 1] += Mathf.RoundToInt(difference * 1000);
            }
            _currencyUnits[i] = finalMultipliedCurrency;
        }
        ConvertUnits();
    }

    public void PowCurrency(float multiplier)
    {
        for (int i = 0; i < _currencyUnits.Length; i++)
        {
            _currencyUnits[i] = (int)(_currencyUnits[i] * multiplier);
        }
        ConvertUnits();
    }

    public void Print()
    {
        string s = "";
        for (int i = 0; i<_currencyUnits.Length; i++)
        {
            if (i != 0) {
                s += ",";
            }
            s += _currencyUnits[i];
            s += _currencyNames[i];
        }
        Debug.Log(s);
    }
}
