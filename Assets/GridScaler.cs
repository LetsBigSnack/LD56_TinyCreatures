using UnityEngine;
using UnityEngine.UI;

public class GridScaler : MonoBehaviour
{
    public GridLayoutGroup gridLayout;
    public Vector2 baseResolution = new Vector2(1920, 1080);
    public Vector2 baseCellSize = new Vector2(100, 100); // Set this to your desired cell size

    void Start()
    {
        AdjustGridLayout();
    }

    void AdjustGridLayout()
    {
        float scaleWidth = Screen.width / baseResolution.x;
        float scaleHeight = Screen.height / baseResolution.y;
        float scaleFactor = Mathf.Min(scaleWidth, scaleHeight);

        gridLayout.cellSize = baseCellSize * scaleFactor;
    }
}