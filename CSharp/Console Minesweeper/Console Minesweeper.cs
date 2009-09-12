using System;

public class Minesweeper
{
    public static void Main(string[] args)
    {
        do
        {
            Game x = new Game();
            bool stillPlaying = true;
            DisplayField(x.tile);
            do
            {
                int[] coordinates = ResponseAction();
                int rowPick = coordinates[0];
                int colPick = coordinates[1];
                stillPlaying = x.SelectedAction(rowPick, colPick);
                DisplayField(x.tile);
            } while (stillPlaying == true);
            Console.WriteLine("Thanks for playing!");
            Console.Write("Play again? (Press any key to continue) ");
            Console.ReadKey();
        } while (true);
    }

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
}