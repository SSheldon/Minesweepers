using System;

public class Game
{
    public Tile[,] tile = new Tile[9,9];
    
    //Game's constructor
    public Game()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                tile[row, col] = new Tile();
            }
        }
        GenerateBombs();
        GenerateTileNums();
    }

    //randomly generates bombs and puts them into an array
    public void GenerateBombs()
    {
        int[,] bomb = new int[10, 3];
        Random RandomClass = new Random();
        bomb[0, 0] = RandomClass.Next(1, 82);
        do
        {
            bomb[1, 0] = RandomClass.Next(1, 82);
        } while (bomb[1, 0] == bomb[0, 0]);
        do
        {
            bomb[2, 0] = RandomClass.Next(1, 82);
        } while (bomb[2, 0] == bomb[0, 0] | bomb[2, 0] == bomb[1, 0]);
        do
        {
            bomb[3, 0] = RandomClass.Next(1, 82);
        } while (bomb[3, 0] == bomb[0, 0] | bomb[3, 0] == bomb[1, 0] | bomb[3, 0] == bomb[2, 0]);
        do
        {
            bomb[4, 0] = RandomClass.Next(1, 82);
        } while (bomb[4, 0] == bomb[0, 0] | bomb[4, 0] == bomb[1, 0] | bomb[4, 0] == bomb[2, 0] | bomb[4, 0] == bomb[3, 0]);
        do
        {
            bomb[5, 0] = RandomClass.Next(1, 82);
        } while (bomb[5, 0] == bomb[0, 0] | bomb[5, 0] == bomb[1, 0] | bomb[5, 0] == bomb[2, 0] | bomb[5, 0] == bomb[3, 0] | bomb[5, 0] == bomb[4, 0]);
        do
        {
            bomb[6, 0] = RandomClass.Next(1, 82);
        } while (bomb[6, 0] == bomb[0, 0] | bomb[6, 0] == bomb[1, 0] | bomb[6, 0] == bomb[2, 0] | bomb[6, 0] == bomb[3, 0] | bomb[6, 0] == bomb[4, 0] | bomb[6, 0] == bomb[5, 0]);
        do
        {
            bomb[7, 0] = RandomClass.Next(1, 82);
        } while (bomb[7, 0] == bomb[0, 0] | bomb[7, 0] == bomb[1, 0] | bomb[7, 0] == bomb[2, 0] | bomb[7, 0] == bomb[3, 0] | bomb[7, 0] == bomb[4, 0] | bomb[7, 0] == bomb[5, 0] | bomb[7, 0] == bomb[6, 0]);
        do
        {
            bomb[8, 0] = RandomClass.Next(1, 82);
        } while (bomb[8, 0] == bomb[0, 0] | bomb[8, 0] == bomb[1, 0] | bomb[8, 0] == bomb[2, 0] | bomb[8, 0] == bomb[3, 0] | bomb[8, 0] == bomb[4, 0] | bomb[8, 0] == bomb[5, 0] | bomb[8, 0] == bomb[6, 0] | bomb[8, 0] == bomb[7, 0]);
        do
        {
            bomb[9, 0] = RandomClass.Next(1, 82);
        } while (bomb[9, 0] == bomb[0, 0] | bomb[9, 0] == bomb[1, 0] | bomb[9, 0] == bomb[2, 0] | bomb[9, 0] == bomb[3, 0] | bomb[9, 0] == bomb[4, 0] | bomb[9, 0] == bomb[5, 0] | bomb[9, 0] == bomb[6, 0] | bomb[9, 0] == bomb[7, 0] | bomb[9, 0] == bomb[8, 0]);

        for (int counter = 0; counter < 10; counter++)
        {
            if ((bomb[counter, 0] % 9) == 0)
            {
                bomb[counter, 1] = ((bomb[counter, 0] - (bomb[counter, 0] % 9)) / 9) - 1;
                bomb[counter, 2] = 8;
            }
            else
            {
                bomb[counter, 1] = (bomb[counter, 0] - (bomb[counter, 0] % 9)) / 9;
                bomb[counter, 2] = (bomb[counter, 0] % 9) - 1;
            }
        }

        for (int counter = 0; counter < 10; counter++)
        {
            tile[bomb[counter, 1], bomb[counter, 2]].BombHere = true;
        }
    }

    //calculates tile numbers
    public void GenerateTileNums()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                int accumulator = 0;
                if (tile[row,col].BombHere == true)
                {
                    accumulator = 9;
                }
                else
                {
                    if (!(row == 0))
                    {
                        if (tile[(row - 1), col].BombHere == true)
                        {
                            accumulator++;
                        }
                    }
                    if (!(col == 0))
                    {
                        if (tile[row, (col - 1)].BombHere == true)
                        {
                            accumulator++;
                        }
                    }
                    if (!(row == 0) & !(col == 0))
                    {
                        if (tile[(row - 1), (col - 1)].BombHere == true)
                        {
                            accumulator++;
                        }
                    }
                    if (!(col == 8))
                    {
                        if (tile[row, (col + 1)].BombHere == true)
                        {
                            accumulator++;
                        }
                    }
                    if (!(row == 0) & !(col == 8))
                    {
                        if (tile[(row - 1), (col + 1)].BombHere == true)
                        {
                            accumulator++;
                        }
                    }
                    if (!(row == 8))
                    {
                        if (tile[(row + 1), col].BombHere == true)
                        {
                            accumulator++;
                        }
                    }
                    if (!(row == 8) & !(col == 0))
                    {
                        if (tile[(row + 1), (col - 1)].BombHere == true)
                        {
                            accumulator++;
                        }
                    }
                    if (!(row == 8) & !(col == 8))
                    {
                        if (tile[(row + 1), (col + 1)].BombHere == true)
                        {
                            accumulator++;
                        }
                    }
                }
                tile[row, col].TileNum = accumulator;
            }
        }
    }

    //carries out choice and returns 0 if game continues, 1 if it is won, and 2 if it is over
    public int SelectedAction(int rowPick, int colPick)
    {
        if (tile[rowPick, colPick].Flagged == true) return 0;
        if (tile[rowPick, colPick].BombHere == true)
        {
            //Game Over
            return 2;
        }
        else
        {
            tile[rowPick, colPick].Reveal();
            if (tile[rowPick, colPick].TileNum == 0)
            {
                RevealTouching(rowPick, colPick);
                bool moreZeros;
                do
                {
                    moreZeros = false;
                    for (int row = 0; row < 9; row++)
                    {
                        for (int col = 0; col < 9; col++)
                        {
                            if (tile[row, col].FieldValue == "0" & CheckHiddenTiles(row, col) == true)
                            {
                                RevealTouching(row, col);
                                moreZeros = true;
                            }
                        }
                    }
                } while (moreZeros == true);
            } //end reveal more on zeroes
        } //end tile revealing
        //check to see if the player has won
        bool moreTilesToReveal = false;
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (tile[row, col].BombHere == false & tile[row, col].Hidden == true)
                {
                    moreTilesToReveal = true;
                }
            }
        }
        if (moreTilesToReveal == false)
        {
            //Game won
            return 1;
        }
        else return 0;
    }

    //triggers the reveal method on all tiles touching the specified tile
    public void RevealTouching(int row, int col)
    {
        if (!(row == 0)) tile[(row - 1), col].Reveal();
        if (!(col == 0)) tile[row, (col - 1)].Reveal();
        if (!(row == 0) & !(col == 0)) tile[(row - 1), (col - 1)].Reveal();
        if (!(col == 8)) tile[row, (col + 1)].Reveal();
        if (!(row == 0) & !(col == 8)) tile[(row - 1), (col + 1)].Reveal();
        if (!(row == 8)) tile[(row + 1), col].Reveal();
        if (!(row == 8) & !(col == 0)) tile[(row + 1), (col - 1)].Reveal();
        if (!(row == 8) & !(col == 8)) tile[(row + 1), (col + 1)].Reveal();
    }

    //checkes to see if the specified tile is touching a hidden tile
    public bool CheckHiddenTiles(int row, int col)
    {
        bool anyHidden = false;
        if (!(row == 0)) 
            if (tile[(row - 1), col].Hidden == true & tile[(row - 1), col].Flagged == false) anyHidden = true;
        if (!(col == 0)) 
            if (tile[row, (col - 1)].Hidden == true & tile[row, (col - 1)].Flagged == false) anyHidden = true;
        if (!(row == 0) & !(col == 0)) 
            if (tile[(row - 1), (col - 1)].Hidden == true & tile[(row - 1), (col - 1)].Flagged == false) anyHidden = true;
        if (!(col == 8)) 
            if (tile[row, (col + 1)].Hidden == true & tile[row, (col + 1)].Flagged == false) anyHidden = true;
        if (!(row == 0) & !(col == 8)) 
            if (tile[(row - 1), (col + 1)].Hidden == true & tile[(row - 1), (col + 1)].Flagged == false) anyHidden = true;
        if (!(row == 8)) 
            if (tile[(row + 1), col].Hidden == true & tile[(row + 1), col].Flagged == false) anyHidden = true;
        if (!(row == 8) & !(col == 0)) 
            if (tile[(row + 1), (col - 1)].Hidden == true & tile[(row + 1), (col - 1)].Flagged == false) anyHidden = true;
        if (!(row == 8) & !(col == 8)) 
            if (tile[(row + 1), (col + 1)].Hidden == true & tile[(row + 1), (col + 1)].Flagged == false) anyHidden = true;
        return anyHidden;
    }
}