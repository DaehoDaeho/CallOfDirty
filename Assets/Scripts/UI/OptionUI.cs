using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField]
    private GameObject optionPanel;

    [SerializeField]
    private Toggle toggleInverMouseY;

    [SerializeField]
    private Slider mouseAngleLimit;

    private void Start()
    {
        optionPanel.SetActive(false);

        OptionData.invertMouseY = toggleInverMouseY.isOn;
        OptionData.mouseAngleLimit = mouseAngleLimit.value;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) == true)
        {
            optionPanel.SetActive(!optionPanel.activeSelf);
        }

        if(optionPanel.activeSelf == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OnClickInvertMouseY(bool value)
    {
        OptionData.invertMouseY = value;
    }    

    public void OnChangeMouseAngleLimit(float value)
    {
        OptionData.mouseAngleLimit = value;
    }
}
