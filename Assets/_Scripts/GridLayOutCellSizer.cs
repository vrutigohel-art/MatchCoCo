using UnityEngine;
using UnityEngine.UI;

public class GridLayOutCellSizer : MonoBehaviour
{
    public GridLayoutGroup G;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void CellResizer(int GridRow,int GridCol)
    {
        Debug.Log("CellResizer ::"+G.GetComponent<RectTransform>().rect.width);
        float width = G.GetComponent<RectTransform>().rect.width/(GridRow+GridCol);
        Vector2 newsize = new Vector2(width, width);
        G.cellSize = newsize;
        Debug.Log("New Size is ::" + newsize);
    }
}
