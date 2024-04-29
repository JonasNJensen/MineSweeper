using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MinesweeperGrid : MonoBehaviour
{
    public TextMeshProUGUI minesLeftCountText;
    public int minesLeftCount;
    public Toggle mark;
    public GameObject gameOverText;
    public GameObject menu;
    public GameObject Game;
    public GameObject mineFieldButtonPrefab; // Reference to your MineFieldButton prefab
    public GameObject gameMap; // Reference to your GameMap object
    public TMP_InputField row;
    public TMP_InputField col;
    public TMP_InputField bomb;
    // Define the grid size
    public int gridSizeX = 10;
    public int gridSizeY = 10;
    public int mineCount = 25;
    public bool win = false;
    public bool lost = false;
    public bool bombsPlaced = false;
    // Define the grid of cells
    public GridButton[,] grid;
    private void Update()
    {
        if (bombsPlaced)
        {
            if (lost) GameOver();

            foreach (GridButton gb in grid)
            {
                if (gb.ismine == false && string.IsNullOrEmpty(gb.tileText.text))
                {
                    return;
                }
            }
            win = true;
            if (win) GameOver();

        }
    }

    public void GameOver()
    {
        if (win)
        {
            foreach (GridButton gb in grid)
                if (gb.ismine == true)
                    gb.GetComponent<Image>().color = Color.green;
            gameOverText.GetComponent<TextMeshProUGUI>().text = "YOU WON";
        }
        else if (lost)
        {
            gameOverText.GetComponent<TextMeshProUGUI>().text = "YOU LOST";
        }
        gameOverText.SetActive(true);
    }

    public void InitializeGrid()
    {
        if (!string.IsNullOrEmpty(row.text))
        {
            if (IsDigitsOnly(row.text))
                gridSizeX = int.Parse(row.text);
        }
        if (!string.IsNullOrEmpty(col.text))
        {
            if (IsDigitsOnly(col.text))
                gridSizeY = int.Parse(col.text);
        }
        if (!string.IsNullOrEmpty(bomb.text))
        {
            if (IsDigitsOnly(bomb.text))
                mineCount = int.Parse(bomb.text);
        }
        //marks win as true so the player wont imidiatly win
        win = false;
        bombsPlaced = false;
        ValidationCheck();
        if (grid == null)
        {
            SwapCanvas();
        }
        foreach (Transform t in gameMap.transform)
            Destroy(t.gameObject);
        minesLeftCount = mineCount;


        // Populate the grid with cells
        CreateGrid();

        lost = false;

        gameOverText.SetActive(false);
    }

    // Populate the grid with cells
    public void CreateGrid()
    {
        grid = new GridButton[gridSizeX, gridSizeY];

        for (int x = gridSizeX / 2 * -1; x < gridSizeX / 2 + (gridSizeX % 2); x++)
        {

            for (int y = gridSizeY / 2 * -1; y < gridSizeY / 2 + (gridSizeY % 2); y++)
            {
                // Instantiate a MineFieldButton prefab
                GridButton mineFieldButton = Instantiate(mineFieldButtonPrefab, gameMap.transform).GetComponent<GridButton>();
                // Set its position based on grid coordinates
                mineFieldButton.transform.position = new Vector2((x * 50) + gameMap.transform.position.x + 25 - (gridSizeY % 2 * 25), 
                    (y * 50) + gameMap.transform.position.y + 25 - (gridSizeY % 2 * 25));

                mineFieldButton.xOnGrid = x + gridSizeX / 2;
                mineFieldButton.yOnGrid = y + gridSizeY / 2;
                grid[x + gridSizeX / 2, y + gridSizeY / 2] = mineFieldButton;
            }
        }
        minesLeftCountText.text = "Mines left: " + minesLeftCount.ToString();
    }

    // populate the grid with bombs
    public void PlaceBombs(int clickedX, int clickedY)
    {
        for (int x = 0; x < mineCount; x++)
        {
            int randY = Random.Range(0, gridSizeY);
            int randX = Random.Range(0, gridSizeX);
            if (grid[randX, randY].ismine == true || (randX == clickedX && randY == clickedY))
            {
                x--;
                continue;
            }
            Debug.Log("placed bomob");
            grid[randX, randY].ismine = true;
        }
    }

    //validates the grid size and number of mines
    public void ValidationCheck()
    {
        if (gridSizeX <= 0) gridSizeX = 10;
        if (gridSizeY <= 0) gridSizeY = 10;
        if (gridSizeX >= 39) gridSizeX = 38;
        if (gridSizeY >= 19) gridSizeY = 18;
        if (gridSizeX * gridSizeY < mineCount) mineCount = gridSizeX * gridSizeY - 1;
        if (mineCount <= 0) mineCount = 1;
    }

    public void ExitToMenue()
    {
        foreach (Transform t in gameMap.transform)
            Destroy(t.gameObject);
        SwapCanvas();
    }

    public void SwapCanvas()
    {
        menu.SetActive(!menu.activeSelf);
        Game.SetActive(!Game.activeSelf);
    }

    bool IsDigitsOnly(string str)
    {
        foreach (char c in str)
        {
            if (c <= '0' || c >= '9')
                return false;
        }
        return true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
