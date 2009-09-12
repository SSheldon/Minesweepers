using System;

public class Tile
{
    protected char fieldValue = ' ';
    protected int tileNum = 0;
    protected bool bombHere = false;
    protected bool hidden = true;
    protected bool flagged = false;

    public char FieldValue
    {
        get
        {
            if (Hidden == true) fieldValue = ' ';
            if (Hidden == false)
                if (BombHere == true) fieldValue = 'X';
                else fieldValue = char.Parse(TileNum.ToString());
            return fieldValue;
        }
    }

    public int TileNum
    {
        get
        {
            return tileNum;
        }
        set
        {
            tileNum = value;
        }
    }

    public bool BombHere
    {
        get
        {
            return bombHere;
        }
        set
        {
            bombHere = value;
        }
    }

    public bool Hidden
    {
        get
        {
            return hidden;
        }
    }

    public bool Flagged
    {
        get
        {
            return flagged;
        }
    }

    public void Reveal()
    {
        hidden = false;
    }

    public void Hide()
    {
        hidden = true;
    }

    public void Reset()
    {
        TileNum = 0;
        BombHere = false;
        Hide();
    }

    public void Flag()
    {
           
    }
}