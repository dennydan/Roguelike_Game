using UnityEngine;
using UnityEngine.UI;

public class ActionBar : MonoBehaviour
{
    [SerializeField] GameObject[] m_skillBtns;
    float[] m_skillsColdTime;
    Image[] m_coldDownMask;

    private void Awake()
    {
        m_skillsColdTime = new float[m_skillBtns.Length];   
        m_coldDownMask = new Image[m_skillBtns.Length];
    }

    // Start is called before the first frame update
    void Start()
    {
        SetColdDownMask();
    }
    void SetColdDownMask()
    {
        for(int i = 0; i < m_skillBtns.Length; i++)
        {
            m_coldDownMask[i] = m_skillBtns[i].GetComponent<Image>();
        }
    }
    public void UpdateSkillState(int index, float time)
    {
        m_coldDownMask[index].fillAmount = time / m_skillsColdTime[index];
    }

    public void SetSkillsColdTime(int index, float time)
    {
        m_skillsColdTime[index] = time;
    }
}
