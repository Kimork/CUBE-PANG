using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumToImageString : MonoBehaviour
{
    public Sprite[] NumImages;
    public Image[] ImageObjects;

    private List<Vector2Int> m_FontsSize;
    private RectTransform m_ThisRectTransform;
    private Vector2 m_BasePosition;

    private void Awake()
    {
        if (m_FontsSize == null)
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

            m_ThisRectTransform = (RectTransform)GetComponent("RectTransform");
            m_BasePosition = m_ThisRectTransform.anchoredPosition;
        }
    }

    public void SetString(int num, bool rePosition = false)
    {
        if (num <= 999999)
        {
            var _string = num.ToString();

            int _objectIndex = 0;
            int _prevX = 0;
            int _calPos = 0;
            foreach (var _char in _string)
            {
                var _target = ImageObjects[_objectIndex++];
                var _num = (int)char.GetNumericValue(_char);

                _target.sprite = NumImages[_num];
                _target.rectTransform.sizeDelta = m_FontsSize[_num];
                _target.rectTransform.anchoredPosition = new Vector2(_prevX, 0);

                if (rePosition)
                    _calPos += m_FontsSize[_num].x;

                _prevX += m_FontsSize[_num].x;
                _target.gameObject.SetActive(true);
            }

            if (rePosition)
                m_ThisRectTransform.anchoredPosition = new Vector2((468 + _calPos) / 2 * -1f, m_BasePosition.y);
        }
    }

}
