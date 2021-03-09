using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorController : MonoBehaviour
{
    public RectTransform _rectTrTutor, _rectTrFade, _rectTrTxBubble;

    public List<List<string>> _conversations;

    public TextMeshProUGUI _textConversation, _textContinue;

    [SerializeField]
    Tutorial _tutorial;

    Image fadeImage;
    Color transparentWhite;

    public AnimationCurve _tutorCurve, _bounceCurve;
    int _currentSpeakIndex;
    int _speakIndex;
    Vector3 _initialPos;

    bool canContinue = false;

    private void Awake()
    {
        _conversations = new List<List<string>>();
        List<string> introConversation = new List<string>();
        introConversation.Add("CONV_0_0");
        introConversation.Add("CONV_0_1");
        _conversations.Add(introConversation);

        List<string> secondConversation = new List<string>();
        secondConversation.Add("CONV_1_0");
        _conversations.Add(secondConversation);

        List<string> thirdConversation = new List<string>();
        thirdConversation.Add("CONV_2_0");
        _conversations.Add(thirdConversation);

        List<string> fourthConversation = new List<string>();
        fourthConversation.Add("CONV_3_0");
        _conversations.Add(fourthConversation);

        List<string> fifthConversation = new List<string>();
        fifthConversation.Add("CONV_4_0");
        _conversations.Add(fifthConversation);

        List<string> sixthConversation = new List<string>();
        sixthConversation.Add("CONV_5_0");
        _conversations.Add(sixthConversation);

        List<string> seventhConversation = new List<string>();
        seventhConversation.Add("CONV_GALLERY_0");
        seventhConversation.Add("CONV_GALLERY_1");
        _conversations.Add(seventhConversation);

        List<string> eigthConversation = new List<string>();
        eigthConversation.Add("CONV_GALLERY_2");
        eigthConversation.Add("CONV_GALLERY_3");
        _conversations.Add(eigthConversation);

        fadeImage = _rectTrFade.GetComponent<Image>();
        transparentWhite = new Color(1, 1, 1, 0);
        fadeImage.color = transparentWhite;
        _initialPos = _rectTrTutor.anchoredPosition;
    }

    public void Speak(int speakIndex)
    {
        _speakIndex = speakIndex;
        _currentSpeakIndex = 0;
        StartCoroutine(CrSpeakIn());
    }

    public void Continue()
    {
        if (canContinue)
        {
            canContinue = false;
            _currentSpeakIndex++;
            if (_currentSpeakIndex == _conversations[_speakIndex].Count - 1)
            {
                _textContinue.text = LocalizationController._localizedData["CLOSE"];
            }
            if (_currentSpeakIndex < _conversations[_speakIndex].Count)
            {
                StartCoroutine(GrowTextChange());
            }
            else
            {
                StartCoroutine(CrSpeakOut());
            }
        }
    }

    IEnumerator CrSpeakIn()
    {
        _rectTrFade.gameObject.SetActive(true);
        float appearAnimationTime = 0.5f;
        float bouncedAnimationTime = 0.25f;
        _textConversation.text = LocalizationController._localizedData[_conversations[_speakIndex][_currentSpeakIndex]];

        _textContinue.text = (_currentSpeakIndex == _conversations[_speakIndex].Count - 1) ?LocalizationController._localizedData["CLOSE"] : _textContinue.text = LocalizationController._localizedData["CONTINUE"];

        for (float i = 0; i< appearAnimationTime; i+= Time.deltaTime)
        {
            _rectTrTutor.anchoredPosition = Vector2.Lerp(_initialPos, Vector2.zero, _tutorCurve.Evaluate(i / appearAnimationTime));
            fadeImage.color = Color.Lerp(transparentWhite, Color.white, i / appearAnimationTime);
            yield return null;
        }
        _rectTrTxBubble.GetComponent<Image>().color = Color.white;
        fadeImage.color = Color.white;
        _rectTrTxBubble.localScale = Vector2.zero;
        _rectTrTutor.anchoredPosition = Vector2.zero;
        for (float i = 0; i < bouncedAnimationTime; i += Time.deltaTime)
        {
            _rectTrTutor.localScale = new Vector3(1,1,1) + new Vector3(1, 1, 1)*(_bounceCurve.Evaluate(i / bouncedAnimationTime)*0.1f);
            _rectTrTxBubble.GetComponent<Image>().color = Color.Lerp(transparentWhite, Color.white, i / bouncedAnimationTime);
            _rectTrTxBubble.localScale = Vector2.Lerp(Vector2.zero, new Vector2(1.1f, 1.1f), i/bouncedAnimationTime);
            yield return null;
        }
        _rectTrTxBubble.localScale = new Vector2(1.1f, 1.1f);
        for (float i = 0; i < (bouncedAnimationTime/2); i += Time.deltaTime)
        {
            _rectTrTxBubble.localScale = Vector2.Lerp(new Vector2(1.1f, 1.1f), Vector2.one, i / (bouncedAnimationTime/2));
            yield return null;
        }
        _rectTrTxBubble.localScale = Vector2.one;
        yield return new WaitForSeconds(1f);
        canContinue = true;
    }

    IEnumerator GrowTextChange()
    {
        _textConversation.text = LocalizationController._localizedData[_conversations[_speakIndex][_currentSpeakIndex]];
        float bouncedAnimationTime = 0.15f;
        for (float i = 0; i < bouncedAnimationTime; i += Time.deltaTime)
        {
            _rectTrTxBubble.localScale = Vector2.Lerp(Vector2.one, new Vector2(1.2f, 1.2f), i / bouncedAnimationTime);
            yield return null;
        }
        _rectTrTxBubble.localScale = new Vector2(1.2f, 1.2f);
        for (float i = 0; i < (bouncedAnimationTime / 2); i += Time.deltaTime)
        {
            _rectTrTxBubble.localScale = Vector2.Lerp(new Vector2(1.2f, 1.2f), Vector2.one, i / (bouncedAnimationTime / 2));
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        canContinue = true;
    }

    IEnumerator CrSpeakOut()
    {
        float disappearAnimationTime = 0.5f;
        float bouncedAnimationTime = 0.25f;

        for (float i = 0; i < (bouncedAnimationTime / 2); i += Time.deltaTime)
        {
            _rectTrTxBubble.localScale = Vector2.Lerp(Vector2.one, new Vector2(1.1f, 1.1f), i / (bouncedAnimationTime / 2));
            yield return null;
        }
        _rectTrTxBubble.localScale = new Vector2(1.1f, 1.1f);

        for (float i = 0; i < bouncedAnimationTime; i += Time.deltaTime)
        {
            _rectTrTxBubble.GetComponent<Image>().color = Color.Lerp(Color.white, transparentWhite, i / bouncedAnimationTime);
            _rectTrTxBubble.localScale = Vector2.Lerp(new Vector2(1.1f, 1.1f), Vector2.zero, i / bouncedAnimationTime);
            yield return null;
        }
        _rectTrTxBubble.localScale = Vector2.zero;

        for (float i = 0; i < (disappearAnimationTime/4); i += Time.deltaTime)
        {
            _rectTrTutor.anchoredPosition = Vector2.Lerp(Vector2.zero, Vector2.zero - new Vector2(_initialPos.x/6, _initialPos.y/6), _tutorCurve.Evaluate(i / (disappearAnimationTime / 4)));
            yield return null;
        }
        Vector2 tutorCurrentPos = new Vector2(_rectTrTutor.anchoredPosition.x, _rectTrTutor.anchoredPosition.y);

        for (float i = 0; i < disappearAnimationTime; i += Time.deltaTime)
        {
            _rectTrTutor.anchoredPosition = Vector2.Lerp(tutorCurrentPos, _initialPos, _tutorCurve.Evaluate(i / disappearAnimationTime));
            fadeImage.color = Color.Lerp(Color.white, transparentWhite, i / disappearAnimationTime);
            yield return null;
        }
        _rectTrTxBubble.GetComponent<Image>().color = transparentWhite;
        fadeImage.color = transparentWhite;
        _rectTrTutor.anchoredPosition = _initialPos;
        _rectTrFade.gameObject.SetActive(false);
        _tutorial.EndSpeak();
    }
}
