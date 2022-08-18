using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState
{
    public int[,] itemsLayoutMatrix;
    public int[,] mechanicsLayoutMatrix;

    public int[] pjCell = new int[2];

    List<int[]> starsCells;
    public List<bool> starsCollected;
    public int totalStarsCollected;

    public LevelState(MatrixManager matrixManager, PlayerBehavior playerBehavior, StarBehavior[] stars)
    {
        this.itemsLayoutMatrix = (int[,]) matrixManager.GetItemsLayoutMatrix().Clone();
        this.mechanicsLayoutMatrix = (int[,]) matrixManager.GetMechanicsLayoutMatrix().Clone();
        this.pjCell = (int[]) playerBehavior.pjCell.Clone();
        ImportStarsInfo(stars);
    }

    void ImportStarsInfo(StarBehavior[] stars)
    {
        starsCells = new List<int[]>();
        starsCollected = new List<bool>();
        totalStarsCollected = 0;

        foreach(StarBehavior star in stars)
        {
            starsCells.Add(star.starCell);
            starsCollected.Add(star.starCollected);
            if(star.starCollected) totalStarsCollected++;
        }
    }
}
