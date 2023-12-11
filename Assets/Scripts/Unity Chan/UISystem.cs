using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    public GameObject model;

    enum PanelStates{
        // all the panels are not showing
        ALLOFF,
        // only the character related panel is showing
        CharacterSettingOn,
        // only the TCP related panel is showing
        TCPSettingOn
    }

    // Control the states of the UI panels
    private PanelStates panelStates = PanelStates.ALLOFF;

    // references of both character related panel and the TCP setting panel
    public Canvas character_setting_canvas;
    public Canvas tcp_setting_canvas;

    // UI components on the character related panel
    public Slider max_rotation_slider;
    public Slider ear_max_slider;
    public Slider ear_min_slider;
    public Toggle enable_auto_blink_toggle;
    public Slider mar_max_slider;
    public Slider mar_min_slider;

    public Text log_text;

    // Start is called before the first frame update
    void Start()
    {
        // Automatically not showing the panels when the game launches.
        CanvasUpdates();
    }

    // Update is called once per frame
    void Update()
    {
        // change the corresponding value in the controller script when the UI is popped up
        if(panelStates == PanelStates.CharacterSettingOn)
        {
            model.GetComponent<UnityChanController>().max_rotation_angle = max_rotation_slider.value;
            model.GetComponent<UnityChanController>().ear_max_threshold = ear_max_slider.value;
            model.GetComponent<UnityChanController>().ear_min_threshold = ear_min_slider.value;
            model.GetComponent<UnityChanController>().mar_max_threshold = mar_max_slider.value;
            model.GetComponent<UnityChanController>().mar_min_threshold = mar_min_slider.value;
        }
    }

    // Initialize the UI components with the init values
    public void InitUI()
    {
        max_rotation_slider.value = model.GetComponent<UnityChanController>().max_rotation_angle;
        ear_max_slider.value = model.GetComponent<UnityChanController>().ear_max_threshold;
        ear_min_slider.value = model.GetComponent<UnityChanController>().ear_min_threshold;
        enable_auto_blink_toggle.isOn = model.GetComponent<UnityChanController>().isAutoBlinkActive;
        mar_max_slider.value = model.GetComponent<UnityChanController>().mar_max_threshold;
        mar_min_slider.value = model.GetComponent<UnityChanController>().mar_min_threshold;

        log_text.text = "";
    }

    // button listener that attaches to the people-like button
    // control whether the character panel is showing or not.
    public void SetCharacterSettingCanvas()
    {
        // it means there maybe other panels on screen or no panel is on screen
        if (panelStates != PanelStates.CharacterSettingOn)
            panelStates = PanelStates.CharacterSettingOn;
        else
            panelStates = PanelStates.ALLOFF;

        CanvasUpdates();
    }

    // button listener that attaches to the setting button
    // control whether the TCP panel is showing or not.
    public void SetTCPSettingCanvas() {
        if (panelStates != PanelStates.TCPSettingOn)
            panelStates = PanelStates.TCPSettingOn;
        else
            panelStates = PanelStates.ALLOFF;
            
        CanvasUpdates();
    }

    // internal shared method to enable/ disable panel showing
    private void CanvasUpdates() {
        switch (panelStates) {
            case PanelStates.ALLOFF:
                character_setting_canvas.enabled = false;
                tcp_setting_canvas.enabled = false;
                break;
            case PanelStates.CharacterSettingOn:
                character_setting_canvas.enabled = true;
                tcp_setting_canvas.enabled = false;
                break;
            case PanelStates.TCPSettingOn:
                character_setting_canvas.enabled = false;
                tcp_setting_canvas.enabled = true;
                break;
        }
    }

    // On Value Changer Listener on the Toggle
    public void SetAutoBlinkActive(bool enabled)
    {
        model.GetComponent<UnityChanController>().EnableAutoBlink(enabled);
    }

    public void SaveData()
    {
        List<ISaveable> saveables = new List<ISaveable> {
            model.GetComponent<UnityChanController>()
        };

        bool isSuccess = SaveDataManager.SaveJsonData(saveables);

        if (isSuccess)
        {
            log_text.text = "Successfully Save Data";
        }
        else
        {
            log_text.text = "Failed to save data";
        }
    }

    public void LoadData()
    {
        List<ISaveable> saveables = new List<ISaveable> {
            model.GetComponent<UnityChanController>()
        };

        bool isSuccess = SaveDataManager.LoadJsonData(saveables);

        if (isSuccess)
        {
            log_text.text = "Successfully Load Data";
        }
        else
        {
            log_text.text = "Failed to load data";
        }
    }

}
