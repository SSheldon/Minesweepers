using System;

public class Minesweeper
{
    public static void Main(string[] args)
    {
        Tile[,] tile = {{new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile()},
                        {new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile()},
                        {new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile()},
                        {new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile()},
                        {new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile()},
                        {new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile()},
                        {new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile()},
                        {new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile()},
                        {new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile(),new Tile()}};
        StopWatch s = new StopWatch();
        do
        {
            tile = GenerateField(tile);
            tile = GenerateTileNums(tile);
            DisplayField(tile);
            s.Start();
            bool stillPlaying = true;
            do
            {
                int[] coordinates = ResponseAction();
                int rowPick = coordinates[0];
                int colPick = coordinates[1];
                //carry out choice
                if (tile[rowPick, colPick].BombHere == true)
                {
                    tile[rowPick, colPick].Reveal();
                    stillPlaying = false;
                    for (int row = 0; row < 9; row++)
                    {
                        for (int col = 0; col < 9; col++)
                        {
                            if (tile[row, col].BombHere == true)
                            {
                                tile[row, col].Reveal();
                            }
                        }
                    }
                    DisplayField(tile);
                    Console.WriteLine("Game Over!");
                    s.Stop();
                }
                else
                {
                    stillPlaying = true;
                    tile[rowPick, colPick].Reveal();
                    if (tile[rowPick, colPick].TileNum == 0)
                    {
                        //code that reveals surrounding spaces on a 0
                        tile = RevealTouching(rowPick, colPick, tile);
                        //end revealing code
                        bool moreZeros;
                        do
                        {
                            moreZeros = false;
                            for (int row = 0; row < 9; row++)
                            {
                                for (int col = 0; col < 9; col++)
                                {
                                    if (tile[row, col].FieldValue == '0' & CheckHiddenTiles(row, col, tile) == true)
                                    {
                                        RevealTouching(row, col, tile);
                                        moreZeros = true;
                                    }
                                }
                            }
                        } while (moreZeros == true);
                    }
                    //check to see if the player has won
                    stillPlaying = false;
                    for (int row = 0; row < 9; row++)
                    {
                        for (int col = 0; col < 9; col++)
                        {
                            if (tile[row, col].BombHere == false & tile[row, col].Hidden == true)
                            {
                                stillPlaying = true;
                            }
                        }
                    }
                    if (stillPlaying == false)
                    {
                        for (int row = 0; row < 9; row++)
                        {
                            for (int col = 0; col < 9; col++)
                            {
                                if (tile[row, col].BombHere == true)
                                {
                                    tile[row, col].Reveal();
                                }
                            }
                        }
                    }
                    DisplayField(tile);
                    if (stillPlaying == false)
                    {
                        s.Stop();
                        Console.WriteLine("Congratulations! You win!");
                        Console.WriteLine("You found all the mines in " + Convert.ToInt32(s.GetElapsedTimeSecs()) + " seconds!");
                    }
                }
            } while (stillPlaying == true);
            Console.WriteLine("Thanks for playing!");
            Console.Write("Play again? (Press any key to continue) ");
            Console.ReadKey();
            Console.WriteLine("");
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    tile[row, col].Reset();
                }
            }
        } while (true);
    }

    //displays field on console
    public static void DisplayField(Tile[,] tile)
    {
        Console.WriteLine("");
        Console.WriteLine("  1 2 3 4 5 6 7 8 9 \n" +
                          " ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐\n" +
                          "A│" + tile[0, 0].FieldValue + "│" + tile[0, 1].FieldValue + "│" + tile[0, 2].FieldValue + "│" + tile[0, 3].FieldValue + "│" + tile[0, 4].FieldValue + "│" + tile[0, 5].FieldValue + "│" + tile[0, 6].FieldValue + "│" + tile[0, 7].FieldValue + "│" + tile[0, 8].FieldValue + "│A\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "B│" + tile[1, 0].FieldValue + "│" + tile[1, 1].FieldValue + "│" + tile[1, 2].FieldValue + "│" + tile[1, 3].FieldValue + "│" + tile[1, 4].FieldValue + "│" + tile[1, 5].FieldValue + "│" + tile[1, 6].FieldValue + "│" + tile[1, 7].FieldValue + "│" + tile[1, 8].FieldValue + "│B\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "C│" + tile[2, 0].FieldValue + "│" + tile[2, 1].FieldValue + "│" + tile[2, 2].FieldValue + "│" + tile[2, 3].FieldValue + "│" + tile[2, 4].FieldValue + "│" + tile[2, 5].FieldValue + "│" + tile[2, 6].FieldValue + "│" + tile[2, 7].FieldValue + "│" + tile[2, 8].FieldValue + "│C\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "D│" + tile[3, 0].FieldValue + "│" + tile[3, 1].FieldValue + "│" + tile[3, 2].FieldValue + "│" + tile[3, 3].FieldValue + "│" + tile[3, 4].FieldValue + "│" + tile[3, 5].FieldValue + "│" + tile[3, 6].FieldValue + "│" + tile[3, 7].FieldValue + "│" + tile[3, 8].FieldValue + "│D\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "E│" + tile[4, 0].FieldValue + "│" + tile[4, 1].FieldValue + "│" + tile[4, 2].FieldValue + "│" + tile[4, 3].FieldValue + "│" + tile[4, 4].FieldValue + "│" + tile[4, 5].FieldValue + "│" + tile[4, 6].FieldValue + "│" + tile[4, 7].FieldValue + "│" + tile[4, 8].FieldValue + "│E\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "F│" + tile[5, 0].FieldValue + "│" + tile[5, 1].FieldValue + "│" + tile[5, 2].FieldValue + "│" + tile[5, 3].FieldValue + "│" + tile[5, 4].FieldValue + "│" + tile[5, 5].FieldValue + "│" + tile[5, 6].FieldValue + "│" + tile[5, 7].FieldValue + "│" + tile[5, 8].FieldValue + "│F\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "G│" + tile[6, 0].FieldValue + "│" + tile[6, 1].FieldValue + "│" + tile[6, 2].FieldValue + "│" + tile[6, 3].FieldValue + "│" + tile[6, 4].FieldValue + "│" + tile[6, 5].FieldValue + "│" + tile[6, 6].FieldValue + "│" + tile[6, 7].FieldValue + "│" + tile[6, 8].FieldValue + "│G\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "H│" + tile[7, 0].FieldValue + "│" + tile[7, 1].FieldValue + "│" + tile[7, 2].FieldValue + "│" + tile[7, 3].FieldValue + "│" + tile[7, 4].FieldValue + "│" + tile[7, 5].FieldValue + "│" + tile[7, 6].FieldValue + "│" + tile[7, 7].FieldValue + "│" + tile[7, 8].FieldValue + "│H\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "I│" + tile[8, 0].FieldValue + "│" + tile[8, 1].FieldValue + "│" + tile[8, 2].FieldValue + "│" + tile[8, 3].FieldValue + "│" + tile[8, 4].FieldValue + "│" + tile[8, 5].FieldValue + "│" + tile[8, 6].FieldValue + "│" + tile[8, 7].FieldValue + "│" + tile[8, 8].FieldValue + "│I\n" +
                          " └─┴─┴─┴─┴─┴─┴─┴─┴─┘\n" +
                          "  1 2 3 4 5 6 7 8 9 \n");
    }

    //randomly generates bombs and puts them into an array
    public static Tile[,] GenerateField(Tile[,] tile)
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

        return tile;
    }

    //calculates tile numbers
    public static Tile[,] GenerateTileNums(Tile[,] tile)
    {
        int accumulator = 0;

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                accumulator = 0;
                if (tile[row, col].BombHere == true)
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

        return tile;
    }

    //coordinates[0] = rowpick, coordinates[1] = colpick
    public static int[] ResponseAction()
    {
        bool goodResponse = true;
        string response;
        char[] responseParts;
        do
        {
            goodResponse = true;
            Console.Write("Tile to reveal: ");
            response = Console.ReadLine();
            response = response.ToUpper();
            responseParts = response.ToCharArray();
            if (!(responseParts.Length == 2)) goodResponse = false;
            else
                if (!(responseParts[0] == 'A' | responseParts[0] == 'B' | responseParts[0] == 'C' | responseParts[0] == 'D' | responseParts[0] == 'E' | responseParts[0] == 'F' | responseParts[0] == 'G' | responseParts[0] == 'H' | responseParts[0] == 'I')) goodResponse = false;
                else
                    if (!(responseParts[1] == '1' | responseParts[1] == '2' | responseParts[1] == '3' | responseParts[1] == '4' | responseParts[1] == '5' | responseParts[1] == '6' | responseParts[1] == '7' | responseParts[1] == '8' | responseParts[1] == '9')) goodResponse = false;
        } while (goodResponse == false);
        //Turn response into coordinates
        int[] coordinates = new int[2];
        if (responseParts[0] == 'A')
            coordinates[0] = 0;
        else
            if (responseParts[0] == 'B')
                coordinates[0] = 1;
            else
                if (responseParts[0] == 'C')
                    coordinates[0] = 2;
                else
                    if (responseParts[0] == 'D')
                        coordinates[0] = 3;
                    else
                        if (responseParts[0] == 'E')
                            coordinates[0] = 4;
                        else
                            if (responseParts[0] == 'F')
                                coordinates[0] = 5;
                            else
                                if (responseParts[0] == 'G')
                                    coordinates[0] = 6;
                                else
                                    if (responseParts[0] == 'H')
                                        coordinates[0] = 7;
                                    else
                                        coordinates[0] = 8;
        coordinates[1] = int.Parse(responseParts[1].ToString()) - 1;
        return coordinates;
    }

    //triggers the reveal method on all tiles touching the specified tile
    public static Tile[,] RevealTouching(int rowPick, int colPick, Tile[,] tile)
    {
        if (!(rowPick == 0)) tile[(rowPick - 1), colPick].Reveal();
        if (!(colPick == 0)) tile[rowPick, (colPick - 1)].Reveal();
        if (!(rowPick == 0) & !(colPick == 0)) tile[(rowPick - 1), (colPick - 1)].Reveal();
        if (!(colPick == 8)) tile[rowPick, (colPick + 1)].Reveal();
        if (!(rowPick == 0) & !(colPick == 8)) tile[(rowPick - 1), (colPick + 1)].Reveal();
        if (!(rowPick == 8)) tile[(rowPick + 1), colPick].Reveal();
        if (!(rowPick == 8) & !(colPick == 0)) tile[(rowPick + 1), (colPick - 1)].Reveal();
        if (!(rowPick == 8) & !(colPick == 8)) tile[(rowPick + 1), (colPick + 1)].Reveal();
        return tile;
    }

    //checkes to see if the specified tile is touching a hidden tile
    public static bool CheckHiddenTiles(int rowPick, int colPick, Tile[,] tile)
    {
        bool anyHidden = false;
        if (!(rowPick == 0)) if (tile[(rowPick - 1), colPick].Hidden == true) anyHidden = true;
        if (!(colPick == 0)) if (tile[rowPick, (colPick - 1)].Hidden == true) anyHidden = true;
        if (!(rowPick == 0) & !(colPick == 0)) if (tile[(rowPick - 1), (colPick - 1)].Hidden == true) anyHidden = true;
        if (!(colPick == 8)) if (tile[rowPick, (colPick + 1)].Hidden == true) anyHidden = true;
        if (!(rowPick == 0) & !(colPick == 8)) if (tile[(rowPick - 1), (colPick + 1)].Hidden == true) anyHidden = true;
        if (!(rowPick == 8)) if (tile[(rowPick + 1), colPick].Hidden == true) anyHidden = true;
        if (!(rowPick == 8) & !(colPick == 0)) if (tile[(rowPick + 1), (colPick - 1)].Hidden == true) anyHidden = true;
        if (!(rowPick == 8) & !(colPick == 8)) if (tile[(rowPick + 1), (colPick + 1)].Hidden == true) anyHidden = true;
        return anyHidden;
    }
}