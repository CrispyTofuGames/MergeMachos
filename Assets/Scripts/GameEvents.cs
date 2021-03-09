using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Clase estática que contiene los distintos eventos que pueden lanzarse en el juego
public static class GameEvents
{

    /**** Ejemplo de evento sin parámetros
     *
     * 
     * public static UnityEvent [[EventName]] = new UnityEvent();
     * 
     */

    /**** Ejemplo de evento con parámetros específicos
     * 
     * 
     * 1 --> Creo la clase a la que pertenecerá el evento
     * 
     * public class [[MyEventType]] : UnityEvent<[[param_type]]>
     * 
     * 2 --> Creo el evento
     * 
     * public static [[MyEventType]] [[EventName]] = new [[MyEventType]]();
     * 
     */

    /**** Ejemplo de evento con parámetros especificados por una clase
     * 
     * 
     * 1 --> Creo la clase que contendrá los parámetros
     * 
     * public class [[MyEventType]]Data 
     * {
     * 
     *     int _param1;
     *     float _param2;
     *     
     *     public [[MyEventType]]Data(int param1, float param2)
     *     {
     *         _param1 = param1;
     *         _param2 = param2;
     *     }
     * }
     * 
     * 2 --> Creo la clase a la que pertenecerá el evento
     * 
     * public class [[MyEventType]] : UnityEvent<[[MyEventType]]Data>{};
     * 
     * 
     * 3 --> Creo el evento
     * 
     * public static [[MyEventType]] [[EventName]] = new [[MyEventType]]();
     * 
     */
    public static IntEvent Purchase = new IntEvent();
    public static IntEvent LevelUp = new IntEvent();
    public static IntEvent DinoUp = new IntEvent();
    public static UnlockSkinEvent GetSkin = new UnlockSkinEvent();
    public static IntEvent TouristWatchDino = new IntEvent();
    public static IntEvent MergeDino = new IntEvent();
    public static UnityEvent WorkDino = new UnityEvent();
    public static UnityEvent OpenBox = new UnityEvent();
    public static UnityEvent TakeBack = new UnityEvent();
    public static UnityEvent CloseDinoUpPanel = new UnityEvent();
    public static UnityEvent CloseLevelUpPanel = new UnityEvent();
    public static AdviceEvent ShowAdvice = new AdviceEvent();

    public static MoneyEvent EarnMoney = new MoneyEvent();
    public static StringEvent LoadScene = new StringEvent();
    public static StringEvent PlaySFX = new StringEvent();
    public static IntEvent RewardMergeUp = new IntEvent();
    public static UnityEvent BuySkinFragments = new UnityEvent();
    public static UnityEvent BuyCharacterFragments = new UnityEvent();
    public static UnityEvent BuyCustomizeSkinFragments = new UnityEvent();
    public static UnityEvent ObtainSkin = new UnityEvent();
    public class StringEvent : UnityEvent<string> { };
    public class AdviceEvent : UnityEvent<AdviceEventData> { };
    public class IntEvent : UnityEvent<int> { };
    public class MoneyEvent : UnityEvent<MoneyEventData> { };
    
    public class UnlockSkinEvent : UnityEvent<UnlockSkinEventData> { };

    public class UnlockSkinEventData
    {
        public int _skinIndex;
        public int _purchaseType;
        public bool _extraSkin;
        public UnlockSkinEventData(int skinIndex, int purchaseType)
        {
            _skinIndex = skinIndex;
            _purchaseType = purchaseType;
            _extraSkin = false;
        }
        public UnlockSkinEventData(int skinIndex, int purchaseType, bool extraSkin)
        {
            _skinIndex = skinIndex;
            _purchaseType = purchaseType;
            _extraSkin = extraSkin;
        }
    }

    public class MoneyEventData
    {

        public Vector3 _position;
        public GameCurrency _money;

        public MoneyEventData(Vector3 pos, GameCurrency money)
        {
            _position = pos;
            _money = money;
        }
    }
    public class AdviceEventData
    {
        public string _mainString;
        public string _parameter;

        public AdviceEventData(string main, string param)
        {
            _mainString = main;
            _parameter = param;
        }
        public AdviceEventData(string main)
        {
            _mainString = main;
            _parameter = "";
        }
    }
}
