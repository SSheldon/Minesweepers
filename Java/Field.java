public class Field
{
    private int height, width, mines;
    public Tile[][] tiles;
    private SurroundingTileIterator sti;
    
    public Field(int height, int width, int mines)
    {
        this.height = height;
        this.width = width;
        this.mines = mines;
        tiles = new Tile[height][width];
        for (int row = 0; row < height; row++)
            for (int col = 0; col < width; col++)
                tiles[row][col] = new Tile();
        sti = new SurroundingTileIterator();
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
        for (Tile t = sti.First(row, col); sti.HasCurrent(); t = sti.Next())
            if (t.Mined) accumulator++;
        return accumulator;
    }

    public void RevealTouching(int row, int col)
    {
        for (Tile t = sti.First(row, col); sti.HasCurrent(); t = sti.Next())
            t.Reveal();
    }

    public boolean TouchingHiddenTile(int row, int col)
    {
        for (Tile t = sti.First(row, col); sti.HasCurrent(); t = sti.Next())
            if (t.Hidden() && !t.Flagged()) return true;
        return false;
    }

    public boolean AllUnminedRevealed()
    {
        for (int row = 0; row < height; row++)
            for (int col = 0; col < width; col++)
                if (!tiles[row][col].Mined && tiles[row][col].Hidden()) return false;
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
            for (int rowCounter = 0; rowCounter < height; rowCounter++)
            {
                for (int colCounter = 0; colCounter < width; colCounter++)
                {
                    if (!tiles[rowCounter][colCounter].Hidden() && !tiles[rowCounter][colCounter].Mined &&
                        tiles[rowCounter][colCounter].Number == 0 && TouchingHiddenTile(rowCounter, colCounter))
                    {
                        RevealTouching(rowCounter, colCounter);
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
        for (int rowCounter = 0; rowCounter < height; rowCounter++)
        {
            for (int colCounter = 0; colCounter < width; colCounter++)
            {
                if (!tiles[rowCounter][colCounter].Mined)
                {
                    tiles[rowCounter][colCounter].Mined = true;
                    tiles[row][col].Mined = false;
                    break;
                }
            }
            if (!tiles[row][col].Mined) break;
        }
        AssignTileNumbers();
    }
    
    private class SurroundingTileIterator
    {
        private int row, col;
        private int position;
        
        public void Set(int row, int col)
        {
            this.row = row;
            this.col = col;
            position = -1;
            Advance();
        }
        
        public Tile First(int row, int col)
        {
            Set(row, col);
            return Current();
        }
        
        public Tile Next()
        {
            Advance();
            return Current();
        }
        
        public boolean HasCurrent()
        {
            return PositionExists(position);
        }
        
        public void Advance()
        {
            for (position++; !PositionExists(position) && position < 8; position++);
        }
        
        public Tile Current()
        {
            switch (position)
            {
                case 0:
                    return tiles[row - 1][col - 1];
                case 1:
                    return tiles[row - 1][col];
                case 2:
                    return tiles[row - 1][col + 1];
                case 3:
                    return tiles[row][col - 1];
                case 4:
                    return tiles[row][col + 1];
                case 5:
                    return tiles[row + 1][col - 1];
                case 6:
                    return tiles[row + 1][col];
                case 7:
                    return tiles[row + 1][col + 1];
                default:
                    return null;
            }
        }
        
        private boolean PositionExists(int position)
        {
            switch (position)
            {
                case 0:
                    return row != 0 && col != 0;
                case 1:
                    return row != 0;
                case 2:
                    return row != 0 && col != width - 1;
                case 3:
                    return col != 0;
                case 4:
                    return col != width - 1;
                case 5:
                    return row != height - 1 && col != 0;
                case 6:
                    return row != height - 1;
                case 7:
                    return row != height - 1 && col != width - 1;
                default:
                    return false;
            }
        }
    }
}