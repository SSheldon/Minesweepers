public class Tile
{
    public final int Number;
    public final boolean Mined;
    private boolean revealed;
    private boolean flagged;
    
    public Tile(boolean mined, int number)
    {
        Mined = mined;
        Number = number;
    }

    public boolean Hidden()
    {
        return !revealed;
    }

    public boolean Flagged()
    {
        return flagged;
    }

    public void Reveal()
    {
        if (!flagged) revealed = true;
    }

    public void Hide()
    {
        revealed = false;
    }

    public void Flag()
    {
        if (!revealed) flagged = true;
    }

    public void Unflag()
    {
        flagged = false;
    }
}