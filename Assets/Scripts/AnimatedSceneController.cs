using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimatedSceneController : MonoBehaviour
{
    Animator animator;
    ParticleSystem showerParticles;
    [SerializeField] TextMeshProUGUI counter;
    [SerializeField] Transform animationsParent;
    [SerializeField] Image pleasureFill;
    [SerializeField] Image fadeImage;
    [SerializeField] Color transparent;
    [SerializeField] AnimationCurve animCurve;
    [SerializeField] AudioSource finalSound;
    AnimationInstance animationInstance;
    float lerpSpeed = 5f;
    Coroutine resetCr;
    List<float> clickTimers;
    bool canShower, transitionActive;
    float clicksAvgSum;
    int clicksAvg;
    float helperNumber;

    private void Start()
    {
        clickTimers = new List<float>();
        canShower = true;
        helperNumber = 6f;
        StartCoroutine(AutoIncrementNumber());
        InitAnimation();
    }
    void Update()
    {
        clicksAvg = clickTimers.Count;
        if (Input.GetMouseButtonDown(0))
        {
            clickTimers.Add(0);
            if(resetCr != null)
            {
                StopCoroutine(resetCr);
                resetCr = null;
            }
        }
        for(int i = 0; i< clickTimers.Count; i++)
        {
            clickTimers[i] +=  Time.deltaTime;
            if (clickTimers[i] >= 2)
            {
                clickTimers.RemoveAt(i);
            }
        }
        if(pleasureFill.fillAmount >= 0.95f && canShower)
        {
            showerParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            showerParticles.Play();
            canShower = false;
            StartCoroutine(FadeOutTransition());
            //StartCoroutine(ResetShower(Random.Range(3f, 10f))); 
        }
        clicksAvgSum = Mathf.Clamp(clicksAvg / 6.5f, 0f, 3f);
        if (!transitionActive)
        {
            animator.SetFloat("TouchSpeed", 1 + clicksAvgSum);
            counter.text = clicksAvg.ToString();
            float lastFill = pleasureFill.fillAmount;
            pleasureFill.fillAmount = Mathf.Lerp(lastFill, (float)clicksAvgSum / helperNumber, lerpSpeed * Time.deltaTime);
        }
    }

    IEnumerator ResetShower(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canShower = true;
    }

    public void InitAnimation()
    {
        GameObject animPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/AnimatedScenes/" + PlayerPrefs.GetInt("AnimatedCharIndex")), animationsParent);
        animator = animPrefab.GetComponent<Animator>();
        animationInstance = animPrefab.GetComponent<AnimationInstance>();
        showerParticles = animationInstance.GetParticles();
    }

    public void GoTavern()
    {
        PlayerPrefs.SetInt("ComeFromAnimation", 1);
        GameEvents.LoadScene.Invoke("MainGame");
    }

    public IEnumerator FadeOutTransition()
    {
        float duration = 5f;
        int clicksAm = clicksAvg;
        transitionActive = true;
        animator.SetFloat("TouchSpeed", 1.5f);
        counter.text = clicksAm.ToString();
        animationInstance.StopSounds();
        finalSound.Play();
        for (float i = 0; i<duration; i += Time.deltaTime)
        {
            fadeImage.color = Color.Lerp(transparent, Color.black, i/duration);
            animator.speed = Mathf.Lerp(1f, 0.1f, animCurve.Evaluate(i/duration));
            yield return null;
        }
        fadeImage.color = Color.black;
        GoTavern();
    }

    public IEnumerator AutoIncrementNumber()
    {
        yield return new WaitForSeconds(1f);
        helperNumber -= 0.2f;
        if(helperNumber > 0)
        {
            StartCoroutine(AutoIncrementNumber());
        }
    }
}
