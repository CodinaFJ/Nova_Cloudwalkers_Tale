using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClass
{
    protected bool canMove;
    protected bool canBeStepped;
    protected TileType tileType;
    protected CrystalTileBehavior crystalTileBehavior;

    public bool CANMOVE {get => canMove;}
    public bool CANBESTEPPED {get => canBeStepped;}

    public TileClass(TileType tileType){
        this.tileType = tileType;
        SetCrystalBehavior();
        SetCanMove();
        SetCanBeStepped();
    }

    private void SetCanMove(){
        if(this.tileType == TileType.WhiteCloud || 
           this.tileType == TileType.CrystalCloud || 
           this.tileType == TileType.ThunderCloud)
            canMove = true;
        else canMove = false;
    }
    private void SetCanMove(bool canMove) => this.canMove = canMove;

    private void SetCanBeStepped(){
        if(this.tileType == TileType.ThunderCloud || 
           this.tileType == TileType.SpikedFloor)
           canBeStepped = false;
        else canBeStepped = true;
    }

    private void SetCrystalBehavior(){
        if(this.tileType == TileType.CrystalCloud || 
           this.tileType == TileType.CrystalFloor) 
            crystalTileBehavior = new CrackOnStepBehavior();

        else crystalTileBehavior = new NothingOnStepBehavior();
    }

    public enum TileType {
        WhiteCloud,
        GreyCloud,
        CrystalCloud,
        ThunderCloud,
        Floor,
        CrystalFloor,
        SpikedFloor
    }
}
