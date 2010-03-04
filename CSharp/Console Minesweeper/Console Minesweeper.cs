using System;

public class Minesweeper
{
    public static void Main(string[] args)
    {
        StopWatch s = new StopWatch();
        do
        {
            Field x = new Field(9, 9, 10);
            s.Start();
            bool stillPlaying = true;
            DisplayField(x);
            do
            {
                int[] coordinates = ResponseAction();
                int rowPick = coordinates[0];
                int colPick = coordinates[1];
                if (coordinates[2] == 0) //player is not flagging a tile
                {
                    if (x.Click(rowPick, colPick))
                    {
                        for (int row = 0; row < 9; row++)
                        {
                            for (int col = 0; col < 9; col++)
                            {
                                if (x[row, col].Mined == true)
                                {
                                    x.Click(row, col);
                                }
                            }
                        }
                        DisplayField(x);
                        Console.WriteLine("Game Over!");
                        stillPlaying = false;
                    }
                    else
                    {
                        if (x.AllUnminedRevealed)
                        {
                            s.Stop();
                            for (int row = 0; row < 9; row++)
                            {
                                for (int col = 0; col < 9; col++)
                                {
                                    if (x[row, col].Mined == true)
                                    {
                                        x.Click(row, col);
                                    }
                                }
                            }
                            DisplayField(x);
                            Console.WriteLine("Congratulations! You win!");
                            Console.WriteLine("You found all the mines in " + Convert.ToInt32(s.GetElapsedTimeSecs()) + " seconds!");
                            stillPlaying = false;
                        }
                        else
                        {
                            stillPlaying = true;
                            DisplayField(x);
                        }
                    }
                }
                else //player is flagging a tile
                {
                    if (!(x[rowPick, colPick].Flagged)) x.Flag(rowPick, colPick);
                    else x.Unflag(rowPick, colPick);
                    DisplayField(x);
                    stillPlaying = true;
                }
            } while (stillPlaying == true);
            Console.WriteLine("Thanks for playing!");
            Console.Write("Play again? (Press any key to continue) ");
            Console.ReadKey();
        } while (true);
    }

    public static void DisplayField(Field field)
    {
        Console.WriteLine("");
        Console.WriteLine("  1 2 3 4 5 6 7 8 9 \n" +
                          " ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐\n" +
                          "A│" + FieldValue(field[0, 0]) + "│" + FieldValue(field[0, 1]) + "│" + FieldValue(field[0, 2]) + "│" + FieldValue(field[0, 3]) + "│" + FieldValue(field[0, 4]) + "│" + FieldValue(field[0, 5]) + "│" + FieldValue(field[0, 6]) + "│" + FieldValue(field[0, 7]) + "│" + FieldValue(field[0, 8]) + "│A\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "B│" + FieldValue(field[1, 0]) + "│" + FieldValue(field[1, 1]) + "│" + FieldValue(field[1, 2]) + "│" + FieldValue(field[1, 3]) + "│" + FieldValue(field[1, 4]) + "│" + FieldValue(field[1, 5]) + "│" + FieldValue(field[1, 6]) + "│" + FieldValue(field[1, 7]) + "│" + FieldValue(field[1, 8]) + "│B\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "C│" + FieldValue(field[2, 0]) + "│" + FieldValue(field[2, 1]) + "│" + FieldValue(field[2, 2]) + "│" + FieldValue(field[2, 3]) + "│" + FieldValue(field[2, 4]) + "│" + FieldValue(field[2, 5]) + "│" + FieldValue(field[2, 6]) + "│" + FieldValue(field[2, 7]) + "│" + FieldValue(field[2, 8]) + "│C\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "D│" + FieldValue(field[3, 0]) + "│" + FieldValue(field[3, 1]) + "│" + FieldValue(field[3, 2]) + "│" + FieldValue(field[3, 3]) + "│" + FieldValue(field[3, 4]) + "│" + FieldValue(field[3, 5]) + "│" + FieldValue(field[3, 6]) + "│" + FieldValue(field[3, 7]) + "│" + FieldValue(field[3, 8]) + "│D\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "E│" + FieldValue(field[4, 0]) + "│" + FieldValue(field[4, 1]) + "│" + FieldValue(field[4, 2]) + "│" + FieldValue(field[4, 3]) + "│" + FieldValue(field[4, 4]) + "│" + FieldValue(field[4, 5]) + "│" + FieldValue(field[4, 6]) + "│" + FieldValue(field[4, 7]) + "│" + FieldValue(field[4, 8]) + "│E\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "F│" + FieldValue(field[5, 0]) + "│" + FieldValue(field[5, 1]) + "│" + FieldValue(field[5, 2]) + "│" + FieldValue(field[5, 3]) + "│" + FieldValue(field[5, 4]) + "│" + FieldValue(field[5, 5]) + "│" + FieldValue(field[5, 6]) + "│" + FieldValue(field[5, 7]) + "│" + FieldValue(field[5, 8]) + "│F\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "G│" + FieldValue(field[6, 0]) + "│" + FieldValue(field[6, 1]) + "│" + FieldValue(field[6, 2]) + "│" + FieldValue(field[6, 3]) + "│" + FieldValue(field[6, 4]) + "│" + FieldValue(field[6, 5]) + "│" + FieldValue(field[6, 6]) + "│" + FieldValue(field[6, 7]) + "│" + FieldValue(field[6, 8]) + "│G\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "H│" + FieldValue(field[7, 0]) + "│" + FieldValue(field[7, 1]) + "│" + FieldValue(field[7, 2]) + "│" + FieldValue(field[7, 3]) + "│" + FieldValue(field[7, 4]) + "│" + FieldValue(field[7, 5]) + "│" + FieldValue(field[7, 6]) + "│" + FieldValue(field[7, 7]) + "│" + FieldValue(field[7, 8]) + "│H\n" +
                          " ├─┼─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                          "I│" + FieldValue(field[8, 0]) + "│" + FieldValue(field[8, 1]) + "│" + FieldValue(field[8, 2]) + "│" + FieldValue(field[8, 3]) + "│" + FieldValue(field[8, 4]) + "│" + FieldValue(field[8, 5]) + "│" + FieldValue(field[8, 6]) + "│" + FieldValue(field[8, 7]) + "│" + FieldValue(field[8, 8]) + "│I\n" +
                          " └─┴─┴─┴─┴─┴─┴─┴─┴─┘\n" +
                          "  1 2 3 4 5 6 7 8 9 \n");
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
            if (responseParts.Length == 3)
            {
                if (!(responseParts[0] == '>')) goodResponse = false;
                else
                    if (!(responseParts[1] == 'A' | responseParts[1] == 'B' | responseParts[1] == 'C' | responseParts[1] == 'D' | responseParts[1] == 'E' | responseParts[1] == 'F' | responseParts[1] == 'G' | responseParts[1] == 'H' | responseParts[1] == 'I')) goodResponse = false;
                    else
                        if (!(responseParts[2] == '1' | responseParts[2] == '2' | responseParts[2] == '3' | responseParts[2] == '4' | responseParts[2] == '5' | responseParts[2] == '6' | responseParts[2] == '7' | responseParts[2] == '8' | responseParts[2] == '9')) goodResponse = false;
            }
            else
            {
                if (!(responseParts.Length == 2)) goodResponse = false;
                else
                    if (!(responseParts[0] == 'A' | responseParts[0] == 'B' | responseParts[0] == 'C' | responseParts[0] == 'D' | responseParts[0] == 'E' | responseParts[0] == 'F' | responseParts[0] == 'G' | responseParts[0] == 'H' | responseParts[0] == 'I')) goodResponse = false;
                    else
                        if (!(responseParts[1] == '1' | responseParts[1] == '2' | responseParts[1] == '3' | responseParts[1] == '4' | responseParts[1] == '5' | responseParts[1] == '6' | responseParts[1] == '7' | responseParts[1] == '8' | responseParts[1] == '9')) goodResponse = false;
            }
        } while (goodResponse == false);
        //Turn response into coordinates
        int[] coordinates = new int[3];
        switch (responseParts.Length)
        {
            case 2:
                switch (responseParts[0])
                {
                    case 'A':
                        coordinates[0] = 0;
                        break;
                    case 'B':
                        coordinates[0] = 1;
                        break;
                    case 'C':
                        coordinates[0] = 2;
                        break;
                    case 'D':
                        coordinates[0] = 3;
                        break;
                    case 'E':
                        coordinates[0] = 4;
                        break;
                    case 'F':
                        coordinates[0] = 5;
                        break;
                    case 'G':
                        coordinates[0] = 6;
                        break;
                    case 'H':
                        coordinates[0] = 7;
                        break;
                    case 'I':
                        coordinates[0] = 8;
                        break;
                }
                coordinates[1] = int.Parse(responseParts[1].ToString()) - 1;
                coordinates[2] = 0;
                break;
            case 3:
                switch (responseParts[1])
                {
                    case 'A':
                        coordinates[0] = 0;
                        break;
                    case 'B':
                        coordinates[0] = 1;
                        break;
                    case 'C':
                        coordinates[0] = 2;
                        break;
                    case 'D':
                        coordinates[0] = 3;
                        break;
                    case 'E':
                        coordinates[0] = 4;
                        break;
                    case 'F':
                        coordinates[0] = 5;
                        break;
                    case 'G':
                        coordinates[0] = 6;
                        break;
                    case 'H':
                        coordinates[0] = 7;
                        break;
                    case 'I':
                        coordinates[0] = 8;
                        break;
                }
                coordinates[1] = int.Parse(responseParts[2].ToString()) - 1;
                coordinates[2] = 1;
                break;
        }
        return coordinates;
    }

    public static string FieldValue(Tile tile)
    {
        if (tile.Flagged) return ">";
        else
        {
            if (tile.Hidden) return " ";
            else
            {
                if (tile.Mined == true) return "X";
                else return tile.Number.ToString();
            }
        }
    }
}