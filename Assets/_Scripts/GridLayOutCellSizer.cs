using UnityEngine;
using UnityEngine.UI;

public class GridLayOutCellSizer : MonoBehaviour
{
    public GridLayoutGroup CardContainer;
    private bool IsLandscape;
    // Update is called once per frame
    public void CellResizer(int GridRow,int GridCol)
    {
        float ResizerFactor;

        IsLandscape = Screen.width > Screen.height;

        if (IsLandscape)
        {
            Debug.Log("Ladscape Display");
            ResizerFactor = CardContainer.GetComponent<RectTransform>().rect.width / (GridCol + GridRow);
        }
        else
        {
            Debug.Log("Portrait Display");
            ResizerFactor = CardContainer.GetComponent<RectTransform>().rect.height / (GridCol + GridRow +1);//GridRow + GridCol-1
        }

        Vector2 newsize = new Vector2(ResizerFactor, ResizerFactor);
        CardContainer.cellSize = newsize;
        Debug.Log("New Size is ::" + newsize);
    }
}
