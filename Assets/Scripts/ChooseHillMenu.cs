using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChooseHillMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject attachedHill;

    public void OnPointerEnter(PointerEventData eventData) {
        GetComponentInChildren<Text>().color = Color.white;
    }
    public void OnPointerExit(PointerEventData eventData) {
        GetComponentInChildren<Text>().color = Color.black;
    }

    public void LoadHill() {
        GameManager.hillPrefab = attachedHill;
        SceneManager.LoadScene("Competition");
    }
}
