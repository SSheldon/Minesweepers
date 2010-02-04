import java.util.Vector;
import java.util.Iterator;

public class Field implements Iterable<Tile>
{
    private int height, width, mines;
    public Tile[][] tiles;
    private Vector<Tile> surrounding;
    
    public Field(int height, int width, int mines)
    {
        this.height = height;
        this.width = width;
        this.mines = mines;
        tiles = new Tile[height][width];
        for (int row = 0; row < height; row++)
            for (int col = 0; col < width; col++)
                tiles[row][col] = new Tile();
        surrounding = new Vector<Tile>(8);
        GenerateMines();
        AssignTileNumbers();
    }

    private void GenerateMines()
    {
        int[] randomNumbers = new int[mines];
        for (int i = 0; i < mines; i++)
        {
            boolean alreadyContained = false;
            do
            {
                alreadyContained = false;
                int randomNumber = (int)(Math.random() * (height * width));
                for (int j = 0; j < i; j++)
                {
                    if (randomNumbers[j] == randomNumber)
                    {
                        alreadyContained = true;
                        break;
                    }
                }
                if (!alreadyContained) randomNumbers[i] = randomNumber;
            } while (alreadyContained);
        }
        for (int i : randomNumbers)
        {
            tiles[i / width][i % width].Mined = true;
        }
    }

    private void AssignTileNumbers()
    {
        for (int row = 0; row < height; row++)
            for (int col = 0; col < width; col++)
                tiles[row][col].Number = TileNumber(row, col);
    }

    private int TileNumber(int row, int col)
    {
        int accumulator = 0;
        for (Tile t : Surrounding(row, col))
            if (t.Mined) accumulator++;
        return accumulator;
    }

    public void RevealTouching(int row, int col)
    {
        for (Tile t : Surrounding(row, col))
            t.Reveal();
    }

    public boolean TouchingHiddenTile(int row, int col)
    {
        for (Tile t : Surrounding(row, col))
            if (t.Hidden() && !t.Flagged()) return true;
        return false;
    }

    public boolean AllUnminedRevealed()
    {
        for (Tile tile : this)
            if (!tile.Mined && tile.Hidden()) return false;
        return true;
    }

    public boolean Click(int row, int col)
    {
        tiles[row][col].Reveal();
        boolean checkAgain;
        //check to see if there are zero tiles touching hidden tiles
        do
        {
            checkAgain = false;
            for (int r = 0; r < height; r++)
            {
                for (int c = 0; c < width; c++)
                {
                    if (!tiles[r][c].Hidden() && !tiles[r][c].Mined &&
                        tiles[r][c].Number == 0 && TouchingHiddenTile(r, c))
                    {
                        RevealTouching(r, c);
                        checkAgain = true;
                    }
                }
            }
        } while (checkAgain);
        return tiles[row][col].Mined && !tiles[row][col].Flagged();
    }

    public void MoveMine(int row, int col)
    {
        if (!tiles[row][col].Mined) return;
        for (Tile tile : this)
        {
            if (!tile.Mined)
            {
                tile.Mined = true;
                tiles[row][col].Mined = false;
                break;
            }
        }
        AssignTileNumbers();
    }
    
    private Vector<Tile> Surrounding(int row, int col)
    {
        surrounding.clear();
        if (row != 0 && col != 0)
            surrounding.add(tiles[row - 1][col - 1]);
        if (row != 0)
            surrounding.add(tiles[row - 1][col]);
        if (row != 0 && col != width - 1)
            surrounding.add(tiles[row - 1][col + 1]);
        if (col != 0)
            surrounding.add(tiles[row][col - 1]);
        if (col != width - 1)
            surrounding.add(tiles[row][col + 1]);
        if (row != height - 1 && col != 0)
            surrounding.add(tiles[row + 1][col - 1]);
        if (row != height - 1)
            surrounding.add(tiles[row + 1][col]);
        if (row != height - 1 && col != width - 1)
            surrounding.add(tiles[row + 1][col + 1]);
        return surrounding;
    }
    
    public Iterator<Tile> iterator()
    {
        return new FieldIterator(this);
    }
    
    public class FieldIterator implements Iterator<Tile>
    {
        int position = -1;
        Field field;
        
        public FieldIterator(Field field)
        {
            this.field = field;
        }
        
        public boolean hasNext()
        {
            return position + 1 < field.height * field.width;
        }
        
        public Tile next()
        {
            position++;
            return field.tiles[position / field.width][position % field.width];
        }
        
        public void remove() { }
    }
}