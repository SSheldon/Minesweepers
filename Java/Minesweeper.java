import java.util.Scanner;

public class Minesweeper
{
    public static void main(String[] args)
    {
        Minesweeper x = new Minesweeper();
    }
    
    private Field field;
    //StopWatch s;
    private Scanner scanner;
    private final int HEIGHT = 9, WIDTH = 9, MINES = 10;

    public Minesweeper()
    {
        //s = new StopWatch();
        scanner = new Scanner(System.in);
        
        do
        {
            field = new Field(HEIGHT, WIDTH, MINES);
            //s.Start();
            DisplayField();
            while (HandleInput());
            System.out.println("Thanks for playing!");
            System.out.print("Play again? (Press enter to continue) ");
            scanner.nextLine();
        } while (true);
    }

    private void DisplayField()
    {
        System.out.println();
        System.out.print("  ");
        for (int col = 0; col < WIDTH; col++)
            System.out.print((col + 1) / 10 + " ");
        System.out.println();
        System.out.print("  ");
        for (int col = 0; col < WIDTH; col++)
            System.out.print((col + 1) % 10 + " ");
        System.out.println();
        char c = 'A';
        for (int row = 0; row < HEIGHT; row++)
        {
            System.out.print(c + " ");
            c++;
            for (int col = 0; col < WIDTH; col++)
                System.out.print(FieldValue(row, col) + " ");
            System.out.println();
        }
    }
    
    private boolean HandleInput()
    {
        Input input = GetInput();
        if (!input.flagging) //player is not flagging a tile
        {
            if (field.Click(input.row, input.col))
            {
                for (Tile tile : field)
                    if (tile.Mined) tile.Reveal();
                DisplayField();
                System.out.println("Game Over!");
                return false;
            }
            else
            {
                if (field.AllUnminedRevealed())
                {
                    //s.Stop();
                    for (Tile tile : field)
                        if (tile.Mined) tile.Reveal();
                    DisplayField();
                    System.out.println("Congratulations! You win!");
                    //Console.WriteLine("You found all the mines in " + Convert.ToInt32(s.GetElapsedTimeSecs()) + " seconds!");
                    return false;
                }
                else
                {
                    DisplayField();
                    return true;
                }
            }
        }
        else //player is flagging a tile
        {
            if (!field.Get(input.row, input.col).Flagged())
                field.Get(input.row, input.col).Flag();
            else field.Get(input.row, input.col).Unflag();
            DisplayField();
            return true;
        }
    }
    
    private class Input
    {
        public boolean flagging;
        public int row, col;
    }

    private Input GetInput()
    {
        boolean goodResponse = true;
        String response;
        do
        {
            goodResponse = true;
            System.out.print("Tile to reveal: ");
            response = scanner.nextLine().trim().toUpperCase();
            if (response.charAt(0) == '>')
            {
                if (!IsValidRow(response.charAt(1))) goodResponse = false;
                else if (!IsValidCol(response.substring(2))) goodResponse = false;
            }
            else
            {
                if (!IsValidRow(response.charAt(0))) goodResponse = false;                
                else if (!IsValidCol(response.substring(1))) goodResponse = false;
            }
        } while (goodResponse == false);
        
        Input input = new Input();
        if (response.charAt(0) == '>')
        {
            input.row = LetterPosition(response.charAt(1));
            input.col = Integer.parseInt(response.substring(2)) - 1;
            input.flagging = true;
        }
        else
        {
            input.row = LetterPosition(response.charAt(0));
            input.col = Integer.parseInt(response.substring(1)) - 1;
            input.flagging = false;
        }
        return input;
    }
    
    private boolean IsValidRow(char letter)
    {
        //check if it's an english letter?
        return (Character.getNumericValue(letter) >= 10) && 
            (LetterPosition(letter) < HEIGHT);
    }
    
    private boolean IsValidCol(String number)
    {
        int num;
        try
        {
            num = Integer.parseInt(number);
        }
        catch (NumberFormatException e)
        {
            return false;
        }
        num--;
        return num >= 0 && num < WIDTH;
    }
    
    private int LetterPosition(char letter)
    {
        return Character.getNumericValue(letter) - 10;
    }

    private String FieldValue(int row, int col)
    {
        return FieldValue(field.Get(row, col));
    }
    
    private static String FieldValue(Tile tile)
    {
        if (tile.Flagged()) return ">";
        else if (tile.Hidden()) return " ";
        else if (tile.Mined) return "X";
        else return Integer.toString(tile.Number);
    }
}