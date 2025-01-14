using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_Combo : MonoBehaviour
{
    [SerializeField] private TMP_Text m_scoreText = null;

    [SerializeField] private Color m_colorTransparent = Color.white;
    [SerializeField] private Color m_colorDefault = Color.white;
    [SerializeField] private Color m_comboColor2 = Color.white;
    [SerializeField] private Color m_comboColor3 = Color.white;
    [SerializeField] private Color m_comboColor4 = Color.white;
    [SerializeField] private Color m_comboColor5 = Color.white;


    public void Initialize(float combo)
    {
        if (combo <= 1)
            Destroy(gameObject);
        else
            Destroy(gameObject, 3f);

        Color selectedColor = m_colorDefault;

        if (combo == 2)
            selectedColor = m_comboColor2;
        else if (combo == 3)
            selectedColor = m_comboColor3;
        else if (combo == 4)
            selectedColor = m_comboColor4;
        else
            selectedColor = m_comboColor5;


        m_scoreText.color = selectedColor;

        m_scoreText.transform.localScale = Vector3.zero;

        m_scoreText.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack)
            .OnComplete(() => m_scoreText.transform.localScale = Vector3.one)
            .OnComplete(() => m_scoreText.DOColor(m_colorTransparent, 2.5f));

        m_scoreText.text = "Combo\n" + "x" + combo;
    }

}