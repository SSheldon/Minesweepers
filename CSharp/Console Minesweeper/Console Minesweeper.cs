using System;

public class Minesweeper
{
    public static void Main(string[] args)
    {
        //initializing array that shows the characters on the tiles
        char[,] fieldValues = {{' ',' ',' ',' ',' ',' ',' ',' ',' '},
                               {' ',' ',' ',' ',' ',' ',' ',' ',' '},
                               {' ',' ',' ',' ',' ',' ',' ',' ',' '},
                               {' ',' ',' ',' ',' ',' ',' ',' ',' '},
                               {' ',' ',' ',' ',' ',' ',' ',' ',' '},
                               {' ',' ',' ',' ',' ',' ',' ',' ',' '},
                               {' ',' ',' ',' ',' ',' ',' ',' ',' '},
                               {' ',' ',' ',' ',' ',' ',' ',' ',' '},
                               {' ',' ',' ',' ',' ',' ',' ',' ',' '}};
        //make bool array that says if a bomb is at the spot
        bool[,] bombHere = GenerateField();
        //calculate tile numbers
        int[,] tileNums = GenerateTileNums(bombHere);
        DisplayField(fieldValues);
        bool stillPlaying = true;
        do
        {            
            int[] coordinates = ResponseAction();
            int rowPick = coordinates[0];
            int colPick = coordinates[1];
            //carry out choice
            if (bombHere[rowPick, colPick] == true)
            {
                fieldValues[rowPick, colPick] = 'X';
                stillPlaying = false;
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        if (bombHere[row, col] == true)
                        {
                            fieldValues[row, col] = 'X';
                        }
                    }
                }
                DisplayField(fieldValues);
                Console.WriteLine("Game Over!");
            }
            else
            {
                stillPlaying = true;
                RevealTile(rowPick, colPick, tileNums, fieldValues);
                if (tileNums[rowPick, colPick] == 0)
                {
                    //code that reveals surrounding spaces on a 0
                    fieldValues = RevealTouching(rowPick, colPick, tileNums, fieldValues);
                    //end revealing code
                    bool moreZeros;
                    do
                    {
                        moreZeros = false;
                        for (int row = 0; row < 9; row++)
                        {
                            for (int col = 0; col < 9; col++)
                            {
                                if (fieldValues[row, col] == '0' & CheckHiddenTiles(row, col, fieldValues) == true)
                                {
                                    RevealTouching(row, col, tileNums, fieldValues);
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
                        if (bombHere[row, col] == false & fieldValues[row,col] == ' ')
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
                            if (bombHere[row, col] == true)
                            {
                                fieldValues[row, col] = '>';
                            }
                        }
                    }
                }
                DisplayField(fieldValues);
                if (stillPlaying == false) Console.WriteLine("Congratulations! You win!");
            }
        } while (stillPlaying == true);
        Console.WriteLine("Thanks for playing!");
        Console.ReadKey();
    }

    //displays field on console
    public static void DisplayField(char[,] fieldValues)
    {
        Console.WriteLine("");
        Console.WriteLine("  1 2 3 4 5 6 7 8 9 \n" +
                          " ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐\n" +
                          "A│" + fieldValues[0, 0] + "│" + fieldValues[0, 1] + "│" + fieldValues[0, 2] + "│" + fieldValues[0, 3] + "│" + fieldValues[0, 4] + "│" + fieldValues[0, 5] + "│" + fieldValues[0, 6] + "│" + fieldValues[0, 7] + "│" + fieldValues[0, 8] + "│\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "B│" + fieldValues[1, 0] + "│" + fieldValues[1, 1] + "│" + fieldValues[1, 2] + "│" + fieldValues[1, 3] + "│" + fieldValues[1, 4] + "│" + fieldValues[1, 5] + "│" + fieldValues[1, 6] + "│" + fieldValues[1, 7] + "│" + fieldValues[1, 8] + "│\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "C│" + fieldValues[2, 0] + "│" + fieldValues[2, 1] + "│" + fieldValues[2, 2] + "│" + fieldValues[2, 3] + "│" + fieldValues[2, 4] + "│" + fieldValues[2, 5] + "│" + fieldValues[2, 6] + "│" + fieldValues[2, 7] + "│" + fieldValues[2, 8] + "│\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "D│" + fieldValues[3, 0] + "│" + fieldValues[3, 1] + "│" + fieldValues[3, 2] + "│" + fieldValues[3, 3] + "│" + fieldValues[3, 4] + "│" + fieldValues[3, 5] + "│" + fieldValues[3, 6] + "│" + fieldValues[3, 7] + "│" + fieldValues[3, 8] + "│\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "E│" + fieldValues[4, 0] + "│" + fieldValues[4, 1] + "│" + fieldValues[4, 2] + "│" + fieldValues[4, 3] + "│" + fieldValues[4, 4] + "│" + fieldValues[4, 5] + "│" + fieldValues[4, 6] + "│" + fieldValues[4, 7] + "│" + fieldValues[4, 8] + "│\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "F│" + fieldValues[5, 0] + "│" + fieldValues[5, 1] + "│" + fieldValues[5, 2] + "│" + fieldValues[5, 3] + "│" + fieldValues[5, 4] + "│" + fieldValues[5, 5] + "│" + fieldValues[5, 6] + "│" + fieldValues[5, 7] + "│" + fieldValues[5, 8] + "│\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "G│" + fieldValues[6, 0] + "│" + fieldValues[6, 1] + "│" + fieldValues[6, 2] + "│" + fieldValues[6, 3] + "│" + fieldValues[6, 4] + "│" + fieldValues[6, 5] + "│" + fieldValues[6, 6] + "│" + fieldValues[6, 7] + "│" + fieldValues[6, 8] + "│\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "H│" + fieldValues[7, 0] + "│" + fieldValues[7, 1] + "│" + fieldValues[7, 2] + "│" + fieldValues[7, 3] + "│" + fieldValues[7, 4] + "│" + fieldValues[7, 5] + "│" + fieldValues[7, 6] + "│" + fieldValues[7, 7] + "│" + fieldValues[7, 8] + "│\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "I│" + fieldValues[8, 0] + "│" + fieldValues[8, 1] + "│" + fieldValues[8, 2] + "│" + fieldValues[8, 3] + "│" + fieldValues[8, 4] + "│" + fieldValues[8, 5] + "│" + fieldValues[8, 6] + "│" + fieldValues[8, 7] + "│" + fieldValues[8, 8] + "│\n" +
                          " └─┴─┴─┴─┴─┴─┴─┴─┴─┘");
    }

    //randomly generates bombs and puts them into an array
    public static bool[,] GenerateField()
    {
        int[,] bomb = new int[10,3];
        Random RandomClass = new Random();
        bomb[0,0] = RandomClass.Next(1, 82);
        do
        {
            bomb[1,0] = RandomClass.Next(1, 82);
        } while (bomb[1,0] == bomb[0,0]);
        do
        {
            bomb[2,0] = RandomClass.Next(1, 82);
        } while (bomb[2,0] == bomb[0,0] | bomb[2,0] == bomb[1,0]);
        do
        {
            bomb[3,0] = RandomClass.Next(1, 82);
        } while (bomb[3,0] == bomb[0,0] | bomb[3,0] == bomb[1,0] | bomb[3,0] == bomb[2,0]);
        do
        {
            bomb[4,0] = RandomClass.Next(1, 82);
        } while (bomb[4,0] == bomb[0,0] | bomb[4,0] == bomb[1,0] | bomb[4,0] == bomb[2,0] | bomb[4,0] == bomb[3,0]);
        do
        {
            bomb[5,0] = RandomClass.Next(1, 82);
        } while (bomb[5,0] == bomb[0,0] | bomb[5,0] == bomb[1,0] | bomb[5,0] == bomb[2,0] | bomb[5,0] == bomb[3,0] | bomb[5,0] == bomb[4,0]);
        do
        {
            bomb[6,0] = RandomClass.Next(1, 82);
        } while (bomb[6,0] == bomb[0,0] | bomb[6,0] == bomb[1,0] | bomb[6,0] == bomb[2,0] | bomb[6,0] == bomb[3,0] | bomb[6,0] == bomb[4,0] | bomb[6,0] == bomb[5,0]);
        do
        {
            bomb[7,0] = RandomClass.Next(1, 82);
        } while (bomb[7,0] == bomb[0,0] | bomb[7,0] == bomb[1,0] | bomb[7,0] == bomb[2,0] | bomb[7,0] == bomb[3,0] | bomb[7,0] == bomb[4,0] | bomb[7,0] == bomb[5,0] | bomb[7,0] == bomb[6,0]);
        do
        {
            bomb[8,0] = RandomClass.Next(1, 82);
        } while (bomb[8,0] == bomb[0,0] | bomb[8,0] == bomb[1,0] | bomb[8,0] == bomb[2,0] | bomb[8,0] == bomb[3,0] | bomb[8,0] == bomb[4,0] | bomb[8,0] == bomb[5,0] | bomb[8,0] == bomb[6,0] | bomb[8,0] == bomb[7,0]);
        do
        {
            bomb[9,0] = RandomClass.Next(1, 82);
        } while (bomb[9,0] == bomb[0,0] | bomb[9,0] == bomb[1,0] | bomb[9,0] == bomb[2,0] | bomb[9,0] == bomb[3,0] | bomb[9,0] == bomb[4,0] | bomb[9,0] == bomb[5,0] | bomb[9,0] == bomb[6,0] | bomb[9,0] == bomb[7,0] | bomb[9,0] == bomb[8,0]);

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

        bool[,] bombHere = {{false,false,false,false,false,false,false,false,false},
                            {false,false,false,false,false,false,false,false,false},
                            {false,false,false,false,false,false,false,false,false},
                            {false,false,false,false,false,false,false,false,false},
                            {false,false,false,false,false,false,false,false,false},
                            {false,false,false,false,false,false,false,false,false},
                            {false,false,false,false,false,false,false,false,false},
                            {false,false,false,false,false,false,false,false,false},
                            {false,false,false,false,false,false,false,false,false}};

        for (int counter = 0; counter < 10; counter++)
        {
            bombHere[bomb[counter, 1], bomb[counter, 2]] = true;
        }

        return bombHere;
    }

    //calculates tile numbers
    public static int[,] GenerateTileNums(bool[,] bombHere)
    {
        int[,] tileNums = {{0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0},
                           {0,0,0,0,0,0,0,0,0}};
        int accumulator = 0;

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                accumulator = 0;
                if (bombHere[row, col] == true)
                {
                    accumulator = 9;
                }
                else
                {
                    if (!(row == 0))
                    {
                        if (bombHere[(row - 1), col] == true)
                        {
                            accumulator++;
                        }
                    }
                    if (!(col == 0))
                    {
                        if (bombHere[row, (col - 1)] == true)
                        {
                            accumulator++;
                        }
                    }
                    if (!(row == 0) & !(col == 0))
                    {
                        if (bombHere[(row - 1), (col - 1)] == true)
                        {
                            accumulator++;
                        }
                    }
                    if (!(col == 8))
                    {
                        if (bombHere[row, (col + 1)] == true)
                        {
                            accumulator++;
                        }
                    }
                    if (!(row == 0) & !(col == 8))
                    {
                        if (bombHere[(row - 1), (col + 1)] == true)
                        {
                            accumulator++;
                        }
                    }
                    if (!(row == 8))
                    {
                        if (bombHere[(row + 1), col] == true)
                        {
                            accumulator++;
                        }
                    }
                    if (!(row == 8) & !(col == 0))
                    {
                        if (bombHere[(row + 1), (col - 1)] == true)
                        {
                            accumulator++;
                        }
                    }
                    if (!(row == 8) & !(col == 8))
                    {
                        if (bombHere[(row + 1), (col + 1)] == true)
                        {
                            accumulator++;
                        }
                    }
                }
                tileNums[row, col] = accumulator;
            }
        }

        return tileNums;
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

    //edits the field values to reveal a tile's number
    public static char[,] RevealTile(int rowPick, int colPick, int[,] tileNums, char[,] fieldValues)
    {
        fieldValues[rowPick, colPick] = char.Parse(tileNums[rowPick, colPick].ToString());
        return fieldValues;
    }

    //edits the field values to reveal the numbers of all tiles touching the specified tile
    public static char[,] RevealTouching(int rowPick, int colPick, int[,] tileNums, char[,] fieldValues)
    {
        if (!(rowPick == 0))
        {
            RevealTile((rowPick - 1), colPick, tileNums,fieldValues);
        }
        if (!(colPick == 0))
        {
            RevealTile(rowPick, (colPick - 1), tileNums,fieldValues);
        }
        if (!(rowPick == 0) & !(colPick == 0))
        {
            RevealTile((rowPick - 1), (colPick - 1), tileNums,fieldValues);
        }
        if (!(colPick == 8))
        {
            RevealTile(rowPick, (colPick + 1), tileNums,fieldValues);
        }
        if (!(rowPick == 0) & !(colPick == 8))
        {
            RevealTile((rowPick - 1), (colPick + 1), tileNums,fieldValues);
        }
        if (!(rowPick == 8))
        {
            RevealTile((rowPick + 1), colPick, tileNums,fieldValues);
        }
        if (!(rowPick == 8) & !(colPick == 0))
        {
            RevealTile((rowPick + 1), (colPick - 1), tileNums,fieldValues);
        }
        if (!(rowPick == 8) & !(colPick == 8))
        {
            RevealTile((rowPick + 1), (colPick + 1), tileNums, fieldValues);
        }
        return fieldValues;
    }

    //checkes to see if the specified tile is touching a hidden tile
    public static bool CheckHiddenTiles(int rowPick, int colPick, char[,] fieldValues)
    {
        bool covered = false;
        if (!(rowPick == 0))
        {
            if (fieldValues[(rowPick - 1), colPick] == ' ') covered = true;
        }
        if (!(colPick == 0))
        {
            if (fieldValues[rowPick, (colPick - 1)] == ' ') covered = true;
        }
        if (!(rowPick == 0) & !(colPick == 0))
        {
            if (fieldValues[(rowPick - 1), (colPick - 1)] == ' ') covered = true;
        }
        if (!(colPick == 8))
        {
            if (fieldValues[rowPick, (colPick + 1)] == ' ') covered = true;
        }
        if (!(rowPick == 0) & !(colPick == 8))
        {
            if (fieldValues[(rowPick - 1), (colPick + 1)] == ' ') covered = true;
        }
        if (!(rowPick == 8))
        {
            if (fieldValues[(rowPick + 1), colPick] == ' ') covered = true;
        }
        if (!(rowPick == 8) & !(colPick == 0))
        {
            if (fieldValues[(rowPick + 1), (colPick - 1)] == ' ') covered = true;
        }
        if (!(rowPick == 8) & !(colPick == 8))
        {
            if (fieldValues[(rowPick + 1), (colPick + 1)] == ' ') covered = true;
        }
        return covered;
    }
}