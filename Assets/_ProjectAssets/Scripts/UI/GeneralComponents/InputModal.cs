using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputModal : MonoBehaviour
{
    [SerializeField]
    private GameObject wrapper;
    [SerializeField]
    private TMPro.TextMeshProUGUI description;
    [SerializeField]
    private TMPro.TMP_InputField input;
    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private Button cancelButton;

    private Action<string> confirmCallback;

    public void Show(string description, string placeholder, bool isCancellable, Action<string> OnStringSet)
    {
        wrapper.SetActive(true);
        this.description.text = description;
        this.input.placeholder.GetComponent<TMPro.TextMeshProUGUI>().text = placeholder;

        this.cancelButton.gameObject.SetActive(isCancellable);

        this.cancelButton.onClick.AddListener(Hide);
        this.confirmButton.onClick.AddListener(Hide);
        this.confirmButton.onClick.AddListener(Apply);

        input.text = "";
        OnValueChanged(input.text);
        input.onValueChanged.AddListener(OnValueChanged);
        this.confirmCallback = OnStringSet;
    }

    public void Apply()
    {
        confirmCallback?.Invoke(input.text);
    }

    public void Hide()
    {
        wrapper.SetActive(false);

        this.cancelButton.onClick.RemoveAllListeners();
        this.confirmButton.onClick.RemoveAllListeners();
    }

    private void OnValueChanged(string value)
    {
        confirmButton.interactable = !string.IsNullOrEmpty(value);
    }


}
