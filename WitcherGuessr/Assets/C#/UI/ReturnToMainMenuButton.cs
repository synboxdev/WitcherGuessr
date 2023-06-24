using UnityEngine;

public class ReturnToMainMenuButton : MonoBehaviour
{
    void Update()
    {
        var localScale = Mathf.Lerp(1f, 1.1f, Mathf.PingPong(Time.time, .5f));
        GetComponent<RectTransform>().localScale = new Vector3(localScale, localScale, localScale);
    }
}