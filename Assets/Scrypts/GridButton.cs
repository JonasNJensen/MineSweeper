using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridButton : MonoBehaviour
{
    public Image imageColor;
    public EventTrigger trigger;
    public TextMeshProUGUI tileText;
    public bool isMarkedAsBomb;
    public bool ismine;
    public int xOnGrid;
    public int yOnGrid;
    MinesweeperGrid gridmanager;
    // Start is called before the first frame update
    void Start()
    {
        // Get the MinesweeperGrid component from the GameManager GameObject
        gridmanager = GameObject.Find("GameManager").GetComponent<MinesweeperGrid>();
        if (gridmanager == null)
        {
            Debug.LogError("MinesweeperGrid component not found on GameManager!");
            return;
        }
        EventTrigger.Entry entry = new()
        {
            eventID = EventTriggerType.PointerDown
        };
        entry.callback.AddListener((data) => { OnPointerClick((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }
    public void OnPointerClick(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Right)
            markAsBomb();
        else if (data.button == PointerEventData.InputButton.Left)
            CountNearbyMines(xOnGrid, yOnGrid);

    }
    public void CountNearbyMines(int x, int y)
    {
        if (!Valiation(x, y)) return;


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
                    tile.imageColor.color = Color.red;
                gridmanager.lost = true;
            }
            return;
        }

        // Iterate over adjacent cells
        for (int i = 0; i < 8; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];
            // Check if the adjacent cell is within bounds
            if (nx < 0) 
                nx = gridSizeX - 1;
            
            if (nx >= gridSizeX) 
                nx = 0;

            if (ny < 0) 
                ny = gridSizeY - 1;

            if (ny >= gridSizeY) 
                ny = 0;

            // Check if the adjacent cell contains a mine
            if (gridmanager.grid[nx, ny].ismine)
            {
                count++;
            }
        }
        gridmanager.grid[x, y].tileText.text = count.ToString();
        if (count == 0)
        {
            for (int i = 0; i < 8; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];
                // Check if the adjacent cell is within bounds
                if (nx >= 0 && nx < gridSizeX && ny >= 0 && ny < gridSizeY
                    && string.IsNullOrEmpty(gridmanager.grid[nx, ny].tileText.text))
                {
                    CountNearbyMines(nx, ny);
                }
            }
        }
    }

    public void markAsBomb()
    {
        if (gridmanager.win || gridmanager.lost) return;
        gridmanager.grid[xOnGrid, yOnGrid].isMarkedAsBomb = !gridmanager.grid[xOnGrid, yOnGrid].isMarkedAsBomb;
        if (gridmanager.grid[xOnGrid, yOnGrid].isMarkedAsBomb)
        {
            gridmanager.grid[xOnGrid, yOnGrid].imageColor.color = Color.green;
            gridmanager.minesLeftCount--;
        }
        else
        {
            gridmanager.grid[xOnGrid, yOnGrid].imageColor.color = Color.white;
            gridmanager.minesLeftCount++;
        }
        gridmanager.minesLeftCountText.text = "Mines left: " + gridmanager.minesLeftCount.ToString();
    }

    public bool Valiation(int x, int y)
    {
        if (!gridmanager.bombsPlaced)
        {
            gridmanager.bombsPlaced = true;
            gridmanager.PlaceBombs(x, y);
            gridmanager.win = false;
        }
        if (gridmanager.grid[x, y].imageColor.color == Color.green) return false;
        if (gridmanager.lost) return false;
        if (gridmanager.win) return false;
        return true;
    }
}
