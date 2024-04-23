using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MinesweeperGrid : MonoBehaviour
{
    public TextMeshProUGUI minesLeftCountText;
    public int minesLeftCount;
    public Toggle mark;
    public GameObject menu;
    public GameObject Game;
    public GameObject mineFieldButtonPrefab; // Reference to your MineFieldButton prefab
    public GameObject gameMap; // Reference to your GameMap object
    // Define the grid size
    public int gridSizeX = 10;
    public int gridSizeY = 10;
    public int mineCount = 25;

    // Define the grid of cells
    public GridButton[,] grid;

    public void InitializeGrid()
    {
        if(gridSizeX*gridSizeY < mineCount) mineCount = gridSizeX*gridSizeY-1;
        menu.SetActive(false);
        Game.SetActive(true);
        minesLeftCount = mineCount;
        foreach (Transform t in gameMap.transform)
        {
            Destroy(t.gameObject);
        }

        grid = new GridButton[gridSizeX, gridSizeY];
        // Populate the grid with cells
        for (int x = gridSizeX / 2 * -1; x < gridSizeX / 2; x++)
        {
            for (int y = gridSizeY / 2 * -1; y < gridSizeY / 2; y++)
            {
                // Instantiate a MineFieldButton prefab
                GridButton mineFieldButton = Instantiate(mineFieldButtonPrefab, gameMap.transform).GetComponent<GridButton>();
                // Set its position based on grid coordinates
                mineFieldButton.transform.position = new Vector2((x * 50) + gameMap.transform.position.x, (y * 50) + gameMap.transform.position.y);

                mineFieldButton.xOnGrid = x + gridSizeX / 2;
                mineFieldButton.yOnGrid = y + gridSizeY / 2;
                grid[x + gridSizeX / 2, y + gridSizeY / 2] = mineFieldButton;
            }
        }
        for (int x = 0; x < mineCount; x++)
        {
            int randY = Random.Range(0, gridSizeY);
            int randX = Random.Range(0, gridSizeX);
            if (grid[randX, randY].ismine == true)
            {
                x--;
                continue;
            }
            grid[randX, randY].ismine = true;
        }
    }
   
}
