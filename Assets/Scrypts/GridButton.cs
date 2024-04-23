using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridButton : MonoBehaviour
{
    public bool isMarkedAsBomb;
    public bool ismine;
    public int xOnGrid;
    public int yOnGrid;
    MinesweeperGrid gridmanager;
    // Start is called before the first frame update
    void Start()
    {
        // Find the GameManager GameObject
        GameObject gameManagerObject = GameObject.Find("GameManager");
        if (gameManagerObject == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }
        else Debug.Log(gameManagerObject.name);

        // Get the MinesweeperGrid component from the GameManager GameObject
        gridmanager = gameManagerObject.GetComponent<MinesweeperGrid>();
        if (gridmanager == null)
        {
            Debug.LogError("MinesweeperGrid component not found on GameManager!");
            return;
        }
        else Debug.Log(gridmanager.name);

        // Add listener to the button's onClick event
        Button button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button component not found on GridButton GameObject!");
            return;
        }
        else Debug.Log(button.name);

        // Add a listener to the onClick event and set it to call CountNearbyMines
        //button.onClick.AddListener(() => gridmanager.CountNearbyMines((int)this.transform.position.x, (int)this.transform.position.y));
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MineClicked()
    {
        if (gridmanager.mark.isOn == true)
            markAsBomb();
        else if (gridmanager.mark.isOn == false)
            CountNearbyMines(xOnGrid,yOnGrid);
    }
    public void CountNearbyMines(int x,int y)
    {
        int gridSizeX = gridmanager.gridSizeX;
        int gridSizeY = gridmanager.gridSizeY;
        int count = 0;
        // Define offsets for adjacent cells
        int[] dx = { -1, 0, 1, 1, 1, 0, -1, -1 };
        int[] dy = { -1, -1, -1, 0, 1, 1, 1, 0 };
        if (gridmanager.grid[x, y].ismine)
        {
            foreach (GridButton tile in gridmanager.grid)
            {
                if (tile.ismine == true)
                    tile.GetComponent<Image>().color = Color.red;
            }
            return;
        }
        // Iterate over adjacent cells
        for (int i = 0; i < 8; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];
            // Check if the adjacent cell is within bounds
            if (nx >= 0 && nx < gridSizeX && ny >= 0 && ny < gridSizeY)
            {
                // Check if the adjacent cell contains a mine
                if (gridmanager.grid[nx, ny].ismine)
                {
                    count++;
                }
            }
        }
        gridmanager.grid[x, y].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = count.ToString();
        if (count == 0)
        {
            for (int i = 0; i < 8; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];
                // Check if the adjacent cell is within bounds
                if (nx >= 0 && nx < gridSizeX && ny >= 0 && ny < gridSizeY && string.IsNullOrEmpty(gridmanager.grid[nx, ny].gameObject.GetComponentInChildren<TextMeshProUGUI>().text))
                {
                    CountNearbyMines(nx,ny);
                }
            }
        }
    }

    public void markAsBomb()
    {
        gridmanager.grid[xOnGrid, yOnGrid].isMarkedAsBomb = !gridmanager.grid[xOnGrid, yOnGrid].isMarkedAsBomb;
        if (gridmanager.grid[xOnGrid,yOnGrid].isMarkedAsBomb )
        {
            gridmanager.grid[xOnGrid, yOnGrid].GetComponent<Image>().color = Color.green;
            gridmanager.minesLeftCount--;
        }
        else
        {
            gridmanager.grid[xOnGrid, yOnGrid].GetComponent<Image>().color = Color.white;
            gridmanager.minesLeftCount++;
        }
        gridmanager.minesLeftCountText.text = "Mines left: " + gridmanager.minesLeftCount.ToString();
    }
}
