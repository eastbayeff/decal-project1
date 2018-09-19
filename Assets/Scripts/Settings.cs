using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider columnsSlider;
    public Slider rowsSlider;

    public TextMeshProUGUI columnsValueText;
    public TextMeshProUGUI rowsValueText;

    private int columns;
    private int rows;

    public int GetColumns
    {
        get { return columns; }
        protected set { }
    }

    public int GetRows
    {
        get { return rows; }
        protected set { }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        columnsSlider.onValueChanged.AddListener(delegate { UpdateColumnsValue(); });
        rowsSlider.onValueChanged.AddListener(delegate { UpdateRowsValue(); });
    }

    void UpdateColumnsValue()
    {
        var value = (int)columnsSlider.value;
        columns = value;
        columnsValueText.text = value.ToString();
    }

    void UpdateRowsValue()
    {
        var value = (int)rowsSlider.value;
        rows = value;
        rowsValueText.text = value.ToString();
    }

}
