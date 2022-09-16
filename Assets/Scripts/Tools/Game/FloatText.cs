using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
//使用方式: FloatArrow.FloatText().
//飘字的效果(可根据这个改编成飘其他东西，其他效果, 会自动销毁)
public class FloatText : MonoBehaviour 
{
    public void StartFloat(string str,Color color, Transform parent, float x = 0f, float y = 0f, int FontSize = 200)
    {
        Text tempText = GetComponent<Text>();
        tempText.text = str;
        tempText.fontSize = FontSize;
        tempText.color = color;
        transform.SetParent(parent);
        transform.localPosition = new Vector3(x, y, 0);
        transform.localRotation = Quaternion.identity;      


        Vector3 oldPos = transform.localPosition;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(tempText.DOFade(1.0f, 1.0f));
        sequence.Join(tempText.transform.DOLocalMoveY(oldPos.y + 5, 1.0f));

        sequence.Append(tempText.DOFade(0.0f, 1.0f));
        sequence.Join(tempText.transform.DOLocalMoveY(oldPos.y + 10, 1.0f));
        sequence.OnComplete(() => { Destroy(gameObject); });
    }

    public static void ShowMessage(string msg, Color color, string parentName, float x = 0f, float y = 0f, int FontSize = 200)
    {
        Instantiate(Resources.Load<FloatText>("TempFloatText"))
            .StartFloat(msg, color, GameObject.Find(parentName).transform, x, y, FontSize);
    }

}
