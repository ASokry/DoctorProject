using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalAndChart : MonoBehaviour
{
    public static JournalAndChart Instance { get; private set; }

    //Chart
    [SerializeField] private Image chartObject;
    [SerializeField] private TextMeshProUGUI chartObjectText;
    [SerializeField] private bool holdingChart = false;
    private bool viewingChart = false;
    public bool GetHoldingChart() { return holdingChart; }
    public void SetHoldingChart(bool b) { holdingChart = b; }

    [SerializeField] private GameObject chartButtonUI;
    private Image chartButtonImageUI;
    private Color chartButtonUIColor;
    private Color greyedOutColor;
    [SerializeField,Range(0,1)] private float greyedOutColorAlpha = 0.5f;

    //Journal
    [SerializeField] Image journalObject;
    private bool viewingJournal = false;

    [SerializeField] private GameObject journalButtonUI;

    //Both Chart and Journal
    public bool IsViewing() { return viewingJournal || viewingChart; }
    private bool canView = true;
    public void SetViewableState(bool b) { canView = b; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        chartObject.gameObject.SetActive(false);
        journalObject.gameObject.SetActive(false);

        chartButtonImageUI = chartButtonUI.GetComponentInChildren<Image>();
        chartButtonUIColor = chartButtonImageUI.color;
        greyedOutColor = chartButtonImageUI.color;
        greyedOutColor.a = greyedOutColorAlpha;
        chartButtonImageUI.color = greyedOutColor;
    }

    public void SetChartData(string str)
    {
        chartObjectText.text = str;
    }

    private void Update()
    {
        UpdateChartButton();

        KeyPressViewChart();
        KeyPressViewJournal();
    }

    private void UpdateChartButton()
    {
        chartButtonImageUI.color = holdingChart ? chartButtonUIColor : greyedOutColor;
    }

    private void KeyPressViewChart()
    {
        if (Input.GetKeyDown(KeyCode.E)) ViewChart();
    }

     public void ViewChart()
    {
        if (holdingChart && canView && !CheckCurrentlyViewingScreen())
        {
            viewingChart = !viewingChart;
            chartObject.gameObject.SetActive(viewingChart);
            PlayerController.Instance.SetPlayerMovement(!viewingChart && !viewingJournal);
        }
    }

    private void KeyPressViewJournal()
    {
        if (Input.GetKeyDown(KeyCode.Q)) ViewJournal();
    }

    public void ViewJournal()
    {
        if (canView && !CheckCurrentlyViewingScreen())
        {
            viewingJournal = !viewingJournal;
            journalObject.gameObject.SetActive(viewingJournal);
            PlayerController.Instance.SetPlayerMovement(!viewingChart && !viewingJournal);
        }
    }

    public void ToggleButtons()
    {
        chartButtonUI.SetActive(!chartButtonUI.activeSelf);
        journalButtonUI.SetActive(!journalButtonUI.activeSelf);
    }

    private bool CheckCurrentlyViewingScreen() { return ScreenManager.Instance.IsViewingScreen(); }
}
