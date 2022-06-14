using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public Image HpGauge;
    public Image ExpGauge;
    public TextMeshProUGUI LevelText;
    public GameObject GameOverText;
    public GameObject CongraturationsText;
    public TextMeshProUGUI DifficultyText;
    public GameObject DescriptionText;
    public bool Congraturations = false;

    enum DIFFICULTY { EASY, NORMAL, HARD, HARDEST, INFERNO }

    void Awake()
    {
        CongraturationsText.SetActive(false);
    }

    void Update()
    {
        var player = Player.Instance;
        var hp = player.Hp;
        var hpMax = player.HpMax;
        HpGauge.fillAmount = (float)hp / hpMax;
        var exp = player.Exp;
        var prevNeedExp = player.PrevNeedExp;
        var needExp = player.NeedExp;
        ExpGauge.fillAmount = (float)(exp - prevNeedExp) / (needExp - prevNeedExp);
        LevelText.text = player.Level.ToString();

        var enemyManager = EnemyManager.Instance;
        var difficultyIndex = Mathf.RoundToInt(Mathf.Lerp(0, 4, enemyManager.ElapsedTime / enemyManager.ElapsedTimeMax));
        DescriptionText.SetActive(difficultyIndex == 0);
        DifficultyText.text = ((DIFFICULTY)difficultyIndex).ToString();
        if (enemyManager.ElapsedTime >= enemyManager.ElapsedTimeMax && player.gameObject.activeSelf || Congraturations)
        {
            Congraturations = true;
            CongraturationsText.SetActive(true);
            return;
        }
        GameOverText.SetActive(!player.gameObject.activeSelf);
    }
}
