import java.util.Vector;
import java.util.Iterator;

public class Field implements Iterable<Tile>
{
    private final int height, width, mines;
    private final Tile[][] tiles;
    private final Vector<Tile> surrounding;
    
    public Field(int height, int width, int mines)
    {
        this.height = height;
        this.width = width;
        this.mines = mines;
        tiles = new Tile[height][width];
        InitializeTiles();
        surrounding = new Vector<Tile>();
    }
    
    public Tile Get(int row, int col) { return tiles[row][col]; }
    
    private void InitializeTiles()
    {
        boolean[][] mined = new boolean[height][width];
        for (int i = 0; i < mines; i++)
        {
            boolean alreadyContained;
            do
            {
                alreadyContained = false;
                int rand = (int)(Math.random() * (height * width));
                if (mined[rand / width][rand % width]) alreadyContained = true;
                else mined[rand / width][rand % width] = true;
            } while (alreadyContained);
        }
        int accumulator;
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                accumulator = 0;
                if (row != 0 && mined[row - 1][col])
                    accumulator++;
                if (col != 0 && mined[row][col - 1])
                    accumulator++;
                if (row != 0 && col != 0 && mined[row - 1][col - 1])
                    accumulator++;
                if (col != width - 1 && mined[row][col + 1])
                    accumulator++;
                if (row != 0 && col != width - 1 && mined[row - 1][col + 1])
                    accumulator++;
                if (row != height - 1 && mined[row + 1][col])
                    accumulator++;
                if (row != height - 1 && col != 0 && mined[row + 1][col - 1])
                    accumulator++;
                if (row != height - 1 && col != width - 1 && mined[row + 1][col + 1])
                    accumulator++;
                tiles[row][col] = new Tile(mined[row][col], accumulator);
            }
        }
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
    
    public int FlagsTouching(int row, int col)
    {
        int accumulator = 0;
        for (Tile t : Surrounding(row, col))
            if (t.Flagged()) accumulator++;
        return accumulator;
    }

    public boolean AllUnminedRevealed()
    {
        for (Tile tile : this)
            if (!tile.Mined && tile.Hidden()) return false;
        return true;
    }
    
    public boolean AllHidden()
    {
        for (Tile tile : this)
            if (!tile.Hidden()) return false;
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
        for (int r = 0; r < height; r++)
        {
            for (int c = 0; c < width; c++)
            {
                if (!tiles[r][c].Mined)
                {
                    tiles[r][c] = new Tile(true, tiles[r][c].Number);
                    ReinitializeSurrounding(r, c);
                    tiles[row][col] = new Tile(false, tiles[row][col].Number);
                    ReinitializeSurrounding(row, col);
                    return;
                }
            }
        }
    }
    
    private void ReinitializeSurrounding(int row, int col)
    {
        int change = tiles[row][col].Mined ? 1 : -1;
        if(row != 0 && col != 0)
            tiles[row - 1][col - 1] = new Tile(tiles[row - 1][col - 1].Mined,
                tiles[row - 1][col - 1].Number + change);
        if(row != 0)
            tiles[row - 1][col] = new Tile(tiles[row - 1][col].Mined,
                tiles[row - 1][col].Number + change);
        if(row != 0 && col != width - 1)
            tiles[row - 1][col + 1] = new Tile(tiles[row - 1][col + 1].Mined,
                tiles[row - 1][col + 1].Number + change);
        if(col != 0)
            tiles[row][col - 1] = new Tile(tiles[row][col - 1].Mined,
                tiles[row][col - 1].Number + change);
        if(col != width - 1)
            tiles[row][col + 1] = new Tile(tiles[row][col + 1].Mined,
                tiles[row][col + 1].Number + change);
        if(row != height - 1 && col != 0)
            tiles[row + 1][col - 1] = new Tile(tiles[row + 1][col - 1].Mined,
                tiles[row + 1][col - 1].Number + change);
        if(row != height - 1)
            tiles[row + 1][col] = new Tile(tiles[row + 1][col].Mined,
                tiles[row + 1][col].Number + change);
        if(row != height - 1 && col != width - 1)
            tiles[row + 1][col + 1] = new Tile(tiles[row + 1][col + 1].Mined,
                tiles[row + 1][col + 1].Number + change);
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