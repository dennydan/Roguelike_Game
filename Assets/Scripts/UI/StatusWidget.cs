using UnityEngine.UI;
using UnityEngine;

public class StatusWidget : MonoBehaviour
{
    [SerializeField] Text m_levelText;
    [SerializeField] Image m_healthBarImg;
    [SerializeField] Image m_expBarImg;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /** <summary>
      更新UI上的顯示
      1. 等級
      2. 血量
      3. 經驗值
        </summary>*/
    public void UpdateStatus(int level, float hp, float exp)
    {
        m_levelText.text = level.ToString();
        m_healthBarImg.fillAmount = hp;
        m_expBarImg.fillAmount = exp;
    }
}
