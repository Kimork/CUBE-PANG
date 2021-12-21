using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumToImageString : MonoBehaviour
{
    public Sprite[] NumImages;
    public Image[] ImageObjects;

    private List<Vector2Int> m_FontsSize;

    private void Awake()
    {
        m_FontsSize = new List<Vector2Int>();

        m_FontsSize.Add(new Vector2Int(76, 86));
        m_FontsSize.Add(new Vector2Int(45, 86));
        m_FontsSize.Add(new Vector2Int(64, 86));
        m_FontsSize.Add(new Vector2Int(64, 86));
        m_FontsSize.Add(new Vector2Int(78, 86));
        m_FontsSize.Add(new Vector2Int(65, 86));
        m_FontsSize.Add(new Vector2Int(69, 86));
        m_FontsSize.Add(new Vector2Int(63, 86));
        m_FontsSize.Add(new Vector2Int(71, 86));
        m_FontsSize.Add(new Vector2Int(69, 86));
    }

    public void SetString(int num)
    {
        if (num <= 999999)
        {
            var _string = num.ToString();

            int _objectIndex = 0;
            int _prevX = 0;

            foreach (var _char in _string)
            {
                var _target = ImageObjects[_objectIndex++];
                var _num = (int)char.GetNumericValue(_char);

                _target.sprite = NumImages[_num];
                _target.rectTransform.sizeDelta = m_FontsSize[_num];
                _target.rectTransform.anchoredPosition = new Vector2(_prevX, 0);

                _prevX += m_FontsSize[_num].x;
                _target.gameObject.SetActive(true);
            }
        }
    }

    private int testa = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (testa == 0)
            {
                SetString(10);
                testa++;
            }
            else if (testa == 1)
            {
                SetString(213);
                testa++;
            }
            else if (testa == 2)
            {
                SetString(3494);
                testa++;
            }
            else if (testa == 3)
            {
                SetString(9746);
                testa++;
            }
            else if (testa == 4)
            {
                SetString(39271);
                testa++;
            }
        }
    }

}
