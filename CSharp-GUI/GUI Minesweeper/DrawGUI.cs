using System;
using System.Windows.Forms;
using System.Drawing;

public class DrawGUI : Form
{
    public int height;
    public int width;
    public int mines;
    Game x;
    Button[,] tileButton;
    GroupBox field = new GroupBox();
    
    bool stillPlaying;
    GroupBox top = new GroupBox();
    Button face = new Button();
    GroupBox minesDisplay = new GroupBox();
    GroupBox timeDisplay = new GroupBox();
    PictureBox[] minesNums = new PictureBox[3];
    PictureBox[] timeNums = new PictureBox[3];
    int flags;
    Timer t = new Timer();
    int time;
    MenuStrip menu = new MenuStrip();
    ToolStripMenuItem difficulty = new ToolStripMenuItem();
    ToolStripMenuItem beginner = new ToolStripMenuItem();
    ToolStripMenuItem intermediate = new ToolStripMenuItem();
    ToolStripMenuItem expert = new ToolStripMenuItem();
    ToolStripMenuItem custom = new ToolStripMenuItem();
    ToolStripButton newGame = new ToolStripButton();
    ToolStripButton exit = new ToolStripButton();

    public DrawGUI()
    {
        height = 9; //min 9, max 24
        width = 9; //min 9, max 30
        mines = 10; //min 10, max = (height - 1)(width - 1)
        x = new Game(height, width, mines);
        stillPlaying = true;
        tileButton = new Button[height, width];

        field.Location = new Point(7, 47 + 24);
        field.Size = new Size(4 + width * 18, 9 + height * 18); //update orig (166, 171)
        Controls.Add(field);
        Size tileButtonSize = new Size(18, 18);
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                tileButton[row, col] = new Button();
                tileButton[row, col].Location = new Point(2 + col * 18, 7 + row * 18);
                tileButton[row, col].Size = tileButtonSize;
                field.Controls.Add(tileButton[row, col]);
                tileButton[row, col].Name = row + "," + col;
                tileButton[row, col].ImageAlign = ContentAlignment.BottomRight;
                tileButton[row, col].BackColor = Color.Empty;
                tileButton[row, col].MouseUp += new MouseEventHandler(tileButton_MouseUp);
                tileButton[row, col].MouseDown += new MouseEventHandler(tileButton_MouseDown);
            }
        }

        top.Size = new Size(field.Width, 43); //update orig (166, 43)
        top.Location = new Point(7, 2 + 24);
        Controls.Add(top);
        face.Size = new Size(26, 26);
        face.Location = new Point(52 + (18 * width - 126) / 2, 11); //update orig (70, 11)
        face.Image = GUI_Minesweeper.Images.face_happy;
        top.Controls.Add(face);
        face.Click += new EventHandler(face_Click);

        flags = mines;
        minesDisplay.Size = new Size(43, 32);
        minesDisplay.Location = new Point(9, 6);
        top.Controls.Add(minesDisplay);
        minesNums[0] = new PictureBox();
        minesNums[1] = new PictureBox();
        minesNums[2] = new PictureBox();
        minesNums[0].Image = GUI_Minesweeper.Images.num_0;
        minesNums[0].Location = new Point(2, 7);
        minesNums[0].Size = new Size(13, 23);
        minesNums[0].BackColor = Color.Black;
        minesDisplay.Controls.Add(minesNums[0]);
        minesNums[1].Image = GUI_Minesweeper.Images.num_0;
        minesNums[1].Location = new Point(15, 7);
        minesNums[1].Size = new Size(13, 23);
        minesNums[1].BackColor = Color.Black;
        minesDisplay.Controls.Add(minesNums[1]);
        minesNums[2].Image = GUI_Minesweeper.Images.num_0;
        minesNums[2].Location = new Point(28, 7);
        minesNums[2].Size = new Size(13, 23);
        minesNums[2].BackColor = Color.Black;
        minesDisplay.Controls.Add(minesNums[2]);
        UpdateNums(ref minesNums, flags);

        time = 0;
        timeDisplay.Size = new Size(43, 32);
        timeDisplay.Location = new Point(18 * width - 48, 6); //update orig (114, 6)
        top.Controls.Add(timeDisplay);
        timeNums[0] = new PictureBox();
        timeNums[1] = new PictureBox();
        timeNums[2] = new PictureBox();
        timeNums[0].Image = GUI_Minesweeper.Images.num_0;
        timeNums[0].Location = new Point(2, 7);
        timeNums[0].Size = new Size(13, 23);
        timeNums[0].BackColor = Color.Black;
        timeDisplay.Controls.Add(timeNums[0]);
        timeNums[1].Image = GUI_Minesweeper.Images.num_0;
        timeNums[1].Location = new Point(15, 7);
        timeNums[1].Size = new Size(13, 23);
        timeNums[1].BackColor = Color.Black;
        timeDisplay.Controls.Add(timeNums[1]);
        timeNums[2].Image = GUI_Minesweeper.Images.num_0;
        timeNums[2].Location = new Point(28, 7);
        timeNums[2].Size = new Size(13, 23);
        timeNums[2].BackColor = Color.Black;
        timeDisplay.Controls.Add(timeNums[2]);

        t.Interval = 1000;
        t.Tick += new EventHandler(t_Tick);

        Controls.Add(menu);
        difficulty.Text = "Difficulty";
        menu.Items.Add(difficulty);
        beginner.Text = "Beginner";
        beginner.Checked = true;
        beginner.Click += new EventHandler(beginner_Click);
        difficulty.DropDownItems.Add(beginner);
        intermediate.Text = "Intermediate";
        intermediate.Click += new EventHandler(intermediate_Click);
        difficulty.DropDownItems.Add(intermediate);
        expert.Text = "Expert";
        expert.Click += new EventHandler(expert_Click);
        difficulty.DropDownItems.Add(expert);
        difficulty.DropDownItems.Add(new ToolStripSeparator());
        custom.Text = "Custom...";
        custom.Click +=new EventHandler(custom_Click);
        difficulty.DropDownItems.Add(custom);
        newGame.Text = "New";
        newGame.Click +=new EventHandler(face_Click);
        menu.Items.Add(newGame);
        exit.Text = "Exit";
        exit.Click += new EventHandler(exit_Click);
        menu.Items.Add(exit);

        Text = "Minesweeper";
        Size = new Size(field.Width + 20, field.Height + 86 + 24); //update orig (186, 257)
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
    }

    public void UpdateGUI()
    {
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                if (tileButton[row, col].Visible == false) ;
                else
                {
                    if (x.tile[row, col].Updated)
                    {
                        if (x.tile[row, col].FieldValue == "0") tileButton[row, col].Hide();
                        else
                        {
                            if (x.tile[row, col].FieldValue == " ") tileButton[row, col].Image = null;
                            else
                            {
                                if (x.tile[row, col].FieldValue == ">") tileButton[row, col].Image = GUI_Minesweeper.Images.tile_flag;
                                else
                                {
                                    if (x.tile[row, col].FieldValue == "X") tileButton[row, col].Image = GUI_Minesweeper.Images.tile_bomb;
                                    else
                                    {
                                        switch (int.Parse(x.tile[row, col].FieldValue))
                                        {
                                            case 1:
                                                tileButton[row, col].Image = GUI_Minesweeper.Images.tile_1;
                                                break;
                                            case 2:
                                                tileButton[row, col].Image = GUI_Minesweeper.Images.tile_2;
                                                break;
                                            case 3:
                                                tileButton[row, col].Image = GUI_Minesweeper.Images.tile_3;
                                                break;
                                            case 4:
                                                tileButton[row, col].Image = GUI_Minesweeper.Images.tile_4;
                                                break;
                                            case 5:
                                                tileButton[row, col].Image = GUI_Minesweeper.Images.tile_5;
                                                break;
                                            case 6:
                                                tileButton[row, col].Image = GUI_Minesweeper.Images.tile_6;
                                                break;
                                            case 7:
                                                tileButton[row, col].Image = GUI_Minesweeper.Images.tile_7;
                                                break;
                                            case 8:
                                                tileButton[row, col].Image = GUI_Minesweeper.Images.tile_8;
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    x.tile[row, col].Updated = false; //end of if-else ladder for displaying
                } //end of actions for visible tile
                if (!(stillPlaying)) tileButton[row, col].Enabled = false;
            }
        } //end of actions for each tile
        if (!stillPlaying & t.Enabled) t.Stop();
    }

    public void ResetGUI()
    {
        x = new Game(height, width, mines);
        if (t.Enabled) t.Stop();
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                tileButton[row, col].Image = null;
                tileButton[row, col].Show();
                tileButton[row, col].BackColor = Color.Empty;
                tileButton[row, col].Enabled = true;
            }
        }
        stillPlaying = true;
        face.Image = GUI_Minesweeper.Images.face_happy;
        flags = mines;
        UpdateNums(ref minesNums, flags);
        time = 0;
        UpdateNums(ref timeNums, time);
    }
    
    public void UpdateNums(ref PictureBox[] nums, int amount)
    {
        int[] amountNums = new int[3];
        if (amount >= 0)
        {
            if (amount > 999) amount = 999;
            amountNums[0] = (amount - (amount % 100)) / 100;
            amountNums[1] = ((amount - amountNums[0] * 100) - ((amount - amountNums[0] * 100) % 10)) / 10;
            amountNums[2] = amount - (amountNums[0] * 100) - (amountNums[1] * 10);
            for (int counter = 0; counter < 3; counter++)
            {
                switch (amountNums[counter])
                {
                    case 0:
                        nums[counter].Image = GUI_Minesweeper.Images.num_0;
                        break;
                    case 1:
                        nums[counter].Image = GUI_Minesweeper.Images.num_1;
                        break;
                    case 2:
                        nums[counter].Image = GUI_Minesweeper.Images.num_2;
                        break;
                    case 3:
                        nums[counter].Image = GUI_Minesweeper.Images.num_3;
                        break;
                    case 4:
                        nums[counter].Image = GUI_Minesweeper.Images.num_4;
                        break;
                    case 5:
                        nums[counter].Image = GUI_Minesweeper.Images.num_5;
                        break;
                    case 6:
                        nums[counter].Image = GUI_Minesweeper.Images.num_6;
                        break;
                    case 7:
                        nums[counter].Image = GUI_Minesweeper.Images.num_7;
                        break;
                    case 8:
                        nums[counter].Image = GUI_Minesweeper.Images.num_8;
                        break;
                    case 9:
                        nums[counter].Image = GUI_Minesweeper.Images.num_9;
                        break;
                }
            }
        }
        else
        {
            char[] amountParts = new char[10];
            amountParts = amount.ToString().ToCharArray();
            nums[0].Image = GUI_Minesweeper.Images.num_min;
            if (amount < 0 & amount > -10) nums[1].Image = GUI_Minesweeper.Images.num_0;
            else
            {
                switch (int.Parse(amountParts[amountParts.GetUpperBound(0) - 1].ToString()))
                {
                    case 0:
                        nums[1].Image = GUI_Minesweeper.Images.num_0;
                        break;
                    case 1:
                        nums[1].Image = GUI_Minesweeper.Images.num_1;
                        break;
                    case 2:
                        nums[1].Image = GUI_Minesweeper.Images.num_2;
                        break;
                    case 3:
                        nums[1].Image = GUI_Minesweeper.Images.num_3;
                        break;
                    case 4:
                        nums[1].Image = GUI_Minesweeper.Images.num_4;
                        break;
                    case 5:
                        nums[1].Image = GUI_Minesweeper.Images.num_5;
                        break;
                    case 6:
                        nums[1].Image = GUI_Minesweeper.Images.num_6;
                        break;
                    case 7:
                        nums[1].Image = GUI_Minesweeper.Images.num_7;
                        break;
                    case 8:
                        nums[1].Image = GUI_Minesweeper.Images.num_8;
                        break;
                    case 9:
                        nums[1].Image = GUI_Minesweeper.Images.num_9;
                        break;
                }
            }
            switch (int.Parse(amountParts[amountParts.GetUpperBound(0)].ToString()))
            {
                case 0:
                    nums[2].Image = GUI_Minesweeper.Images.num_0;
                    break;
                case 1:
                    nums[2].Image = GUI_Minesweeper.Images.num_1;
                    break;
                case 2:
                    nums[2].Image = GUI_Minesweeper.Images.num_2;
                    break;
                case 3:
                    nums[2].Image = GUI_Minesweeper.Images.num_3;
                    break;
                case 4:
                    nums[2].Image = GUI_Minesweeper.Images.num_4;
                    break;
                case 5:
                    nums[2].Image = GUI_Minesweeper.Images.num_5;
                    break;
                case 6:
                    nums[2].Image = GUI_Minesweeper.Images.num_6;
                    break;
                case 7:
                    nums[2].Image = GUI_Minesweeper.Images.num_7;
                    break;
                case 8:
                    nums[2].Image = GUI_Minesweeper.Images.num_8;
                    break;
                case 9:
                    nums[2].Image = GUI_Minesweeper.Images.num_9;
                    break;
            }
        }
    }

    public void GetCoordinates(string name, ref int rowPick, ref int colPick)
    {
        char[] responseParts;
        responseParts = name.ToCharArray();
        switch (responseParts.Length)
        {
            case 3:
                rowPick = int.Parse(responseParts[0].ToString());
                colPick = int.Parse(responseParts[2].ToString());
                break;
            case 4:
                if (responseParts[1] == ',')
                {
                    rowPick = int.Parse(responseParts[0].ToString());
                    colPick = int.Parse(responseParts[2].ToString() + responseParts[3].ToString());
                }
                else
                {
                    rowPick = int.Parse(responseParts[0].ToString() + responseParts[1].ToString());
                    colPick = int.Parse(responseParts[3].ToString());
                }
                break;
            case 5:
                rowPick = int.Parse(responseParts[0].ToString() + responseParts[1].ToString());
                colPick = int.Parse(responseParts[3].ToString() + responseParts[4].ToString());
                break;
        }
    }

    public void RedrawGUI()
    {
        tileButton = new Button[height, width];

        field.Location = new Point(7, 47 + 24);
        field.Size = new Size(4 + width * 18, 9 + height * 18); //update orig (166, 171)
        Size tileButtonSize = new Size(18, 18);
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                tileButton[row, col] = new Button();
                tileButton[row, col].Location = new Point(2 + col * 18, 7 + row * 18);
                tileButton[row, col].Size = tileButtonSize;
                field.Controls.Add(tileButton[row, col]);
                tileButton[row, col].Name = row + "," + col;
                tileButton[row, col].ImageAlign = ContentAlignment.BottomRight;
                tileButton[row, col].BackColor = Color.Empty;
                tileButton[row, col].MouseUp += new MouseEventHandler(tileButton_MouseUp);
                tileButton[row, col].MouseDown += new MouseEventHandler(tileButton_MouseDown);
            }
        }

        top.Size = new Size(field.Width, 43); //update orig (166, 43)
        top.Location = new Point(7, 2 + 24);
        face.Location = new Point(52 + (18 * width - 126) / 2, 11); //update orig (70, 11)

        timeDisplay.Location = new Point(18 * width - 48, 6); //update orig (114, 6)
        ResetGUI();

        Size = new Size(field.Width + 20, field.Height + 86 + 24); //update orig (186, 257)
    }
    
    public void tileButton_MouseUp(Object sender, MouseEventArgs e)
    {
        int rowPick = 0;
        int colPick = 0;
        GetCoordinates((sender as Button).Name, ref rowPick, ref colPick);

        if (!t.Enabled) t.Start();
        if (e.Button == MouseButtons.Left)
        {
            switch (x.SelectedAction(rowPick, colPick))
            {
                case 0: //Game continues
                    stillPlaying = true;
                    UpdateGUI();
                    face.Image = GUI_Minesweeper.Images.face_happy;
                    break;
                case 1: //Game won
                    stillPlaying = false;
                    for (int row = 0; row < height; row++)
                    {
                        for (int col = 0; col < width; col++)
                        {
                            if (x.tile[row, col].MineHere == true) x.tile[row, col].Flag();
                        }
                    }
                    UpdateGUI();
                    UpdateNums(ref minesNums, 0);
                    face.Image = GUI_Minesweeper.Images.face_win;
                    break;
                case 2: //Game over
                    stillPlaying = false;
                    tileButton[rowPick, colPick].BackColor = Color.Red;
                    for (int row = 0; row < height; row++)
                    {
                        for (int col = 0; col < width; col++)
                        {
                            if (x.tile[row, col].MineHere == true) x.tile[row, col].Reveal();
                        }
                    }
                    UpdateGUI();
                    for (int row = 0; row < height; row++)
                    {
                        for (int col = 0; col < width; col++)
                        {
                            if (x.tile[row, col].MineHere == false & x.tile[row, col].Flagged == true) tileButton[row, col].Image = GUI_Minesweeper.Images.tile_notbomb;
                        }
                    }
                    face.Image = GUI_Minesweeper.Images.face_dead;
                    break;
            }
        }
        

    }

    public void face_Click(Object sender, EventArgs e)
    {
        if (t.Enabled) t.Stop();
        ResetGUI();
    }

    public void t_Tick(Object sender, EventArgs e)
    {
        time++;
        UpdateNums(ref timeNums, time);
    }

    public void tileButton_MouseDown(Object sender, MouseEventArgs e)
    {        
        if (e.Button == MouseButtons.Left)
        {
            face.Image = GUI_Minesweeper.Images.face_scared;
        }
        if (e.Button == MouseButtons.Right)
        {
            int rowPick = 0;
            int colPick = 0;
            GetCoordinates((sender as Button).Name, ref rowPick, ref colPick);

            if (!(x.tile[rowPick, colPick].Flagged))
            {
                if (x.tile[rowPick, colPick].Hidden)
                {
                    x.tile[rowPick, colPick].Flag();
                    flags--;
                    UpdateNums(ref minesNums, flags);
                }
            }
            else
            {
                x.tile[rowPick, colPick].Unflag();
                flags++;
                UpdateNums(ref minesNums, flags);
            }
            UpdateGUI();
        }
    }

    public void expert_Click(object sender, EventArgs e)
    {
        expert.Checked = true;
        intermediate.Checked = false;
        beginner.Checked = false;
        custom.Checked = false;
        ResetGUI();
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                tileButton[row, col].Dispose();
            }
        }
        height = 24;
        width = 30;
        mines = 99;
        RedrawGUI();
    }

    public void intermediate_Click(object sender, EventArgs e)
    {
        expert.Checked = false;
        intermediate.Checked = true;
        beginner.Checked = false;
        custom.Checked = false;
        ResetGUI();
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                tileButton[row, col].Dispose();
            }
        }
        height = 16;
        width = 16;
        mines = 40;
        RedrawGUI();
    }

    public void beginner_Click(object sender, EventArgs e)
    {
        expert.Checked = false;
        intermediate.Checked = false;
        beginner.Checked = true;
        custom.Checked = false;
        ResetGUI();
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                tileButton[row, col].Dispose();
            }
        }
        height = 9;
        width = 9;
        mines = 10;
        RedrawGUI();
    }

    public void custom_Click(object sender, EventArgs e)
    {
        expert.Checked = false;
        intermediate.Checked = false;
        beginner.Checked = false;
        custom.Checked = true;
        ResetGUI();
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                tileButton[row, col].Dispose();
            }
        }
        CustomPopup y = new CustomPopup(this);
        RedrawGUI();
    }

    public void exit_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }
}