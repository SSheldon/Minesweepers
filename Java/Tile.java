public class Tile
{
    public int Number;
    public boolean Mined;
    private boolean revealed;
    private boolean flagged;

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
        if (!Flagged()) revealed = true;
    }

    public void Hide()
    {
        revealed = false;
    }

    public void Flag()
    {
        if (Hidden()) flagged = true;
    }

    public void Unflag()
    {
        flagged = false;
    }
}