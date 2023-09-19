using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RandomSlot : MonoBehaviour
{
    [SerializeField] Character DefaultCharacter;
    [SerializeField] GameObject DefaultThumbnail;
    [SerializeField] GameObject RandomThumbnail;
    [SerializeField] GameObject FocusFrame;

    public Slider Health;
    public Slider HealthMax;
    public Slider Power;
    public Slider PowerMax;
    public Slider Speed;
    public Slider SpeedMax;

    void Start()
    {
        ShowSlot();
    }

    void ShowSlot()
    {
        if (GameManager.Instance.Stage <= 0)
        {
            DefaultThumbnail.SetActive(true);
            RandomThumbnail.SetActive(false);

            Health.value = this.DefaultCharacter.Health / 100.0f;
            HealthMax.value = 0;
            Power.value = this.DefaultCharacter.Power / 10.0f;
            PowerMax.value = 0;
            Speed.value = this.DefaultCharacter.Speed / 10.0f;
            SpeedMax.value = 0;
        }
        else
        {
            DefaultThumbnail.SetActive(false);
            RandomThumbnail.SetActive(true);

            float[] min = GetMinValue();
            float[] max = GetMaxValue();
            Health.value = min[0] / 100.0f;
            HealthMax.value = max[0] / 100.0f;
            Power.value = min[1] / 10.0f;
            PowerMax.value = max[1] / 10.0f;
            Speed.value = min[2] / 10.0f;
            SpeedMax.value = max[2] / 10.0f;
        }  
    }

    float[] GetMinValue()
    {
        List<Character> list = GameManager.Instance.DefeatedMonsters;

        List<float> healthValues = new List<float>();
        List<float> powerValues = new List<float>();
        List<float> speedValues = new List<float>();
        for (int i =0; i< list.Count; i++)
        {
            healthValues.Add(list[i].Health);
            powerValues.Add(list[i].Power);
            speedValues.Add(list[i].Speed);
        }

        return new float[] { healthValues.Min(), powerValues.Min(), speedValues.Min() };
    }

    float[] GetMaxValue()
    {
        List<Character> list = GameManager.Instance.DefeatedMonsters;

        List<float> healthValues = new List<float>();
        List<float> powerValues = new List<float>();
        List<float> speedValues = new List<float>();
        for (int i = 0; i < list.Count; i++)
        {
            healthValues.Add(list[i].Health);
            powerValues.Add(list[i].Power);
            speedValues.Add(list[i].Speed);
        }

        return new float[] { healthValues.Max(), powerValues.Max(), speedValues.Max() };
    }

    public void OnFocus(bool focus)
    {
        FocusFrame.SetActive(focus);
    }

    public void OnSelect()
    {
        GameManager gm = GameManager.Instance;

        if ( gm.Stage <= 0)
        {
            GameManager.Instance.PlayerCharacter = this.DefaultCharacter;
        }    
        else
        {
            int random = Random.Range(0, gm.DefeatedMonsters.Count);
            GameManager.Instance.PlayerCharacter = gm.DefeatedMonsters[random];
        }
        
        SceneManager.LoadScene(GameManager.Instance.Scenes[2].name);
    }
}
