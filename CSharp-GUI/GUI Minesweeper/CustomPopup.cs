using System;
using System.Windows.Forms;
using System.Drawing;

class CustomPopup : Form
{
    TextBox txtHeight = new TextBox();
    TextBox txtWidth = new TextBox();
    TextBox txtMines = new TextBox();
    Button ok = new Button();
    Button cancel = new Button();
    Label lblHeight = new Label();
    Label lblWidth = new Label();
    Label lblMines = new Label();
    int height, width, bombs;
    DrawGUI x;

    public CustomPopup(DrawGUI x)
    {
        this.x = x;

        txtHeight.Location = new Point(59, 32);
        txtHeight.Size = new Size(38, 20);
        txtHeight.Text = x.height.ToString();
        Controls.Add(txtHeight);
        txtWidth.Location = new Point(59, 56);
        txtWidth.Size = new Size(38, 20);
        txtWidth.Text = x.width.ToString();
        Controls.Add(txtWidth);
        txtMines.Location = new Point(59, 80);
        txtMines.Size = new Size(38, 20);
        txtMines.Text = x.mines.ToString();
        Controls.Add(txtMines);

        ok.Location = new Point(120, 33);
        ok.Size = new Size(58, 24);
        ok.Text = "OK";
        ok.Click += new EventHandler(ok_Click);
        Controls.Add(ok);
        cancel.Location = new Point(120, 75);
        cancel.Size = new Size(58, 24);
        cancel.Text = "Cancel";
        cancel.Click += new EventHandler(cancel_Click);
        Controls.Add(cancel);

        lblHeight.Location = new Point(12, 32);
        lblHeight.Size = new Size(42, 20);
        lblHeight.TextAlign = ContentAlignment.MiddleLeft;
        lblHeight.Text = "Height:";
        Controls.Add(lblHeight);
        lblWidth.Location = new Point(12, 56);
        lblWidth.Size = new Size(42, 20);
        lblWidth.TextAlign = ContentAlignment.MiddleLeft;
        lblWidth.Text = "Width:";
        Controls.Add(lblWidth);
        lblMines.Location = new Point(12, 56 + 25);
        lblMines.Size = new Size(42, 20);
        lblMines.TextAlign = ContentAlignment.MiddleLeft;
        lblMines.Text = "Mines:";
        Controls.Add(lblMines); 

        Text = "Custom Field";
        Size = new Size(201, 170);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;

        ShowDialog();
    }

    public void cancel_Click(object sender, EventArgs e)
    {
        Close();
    }

    public void ok_Click(object sender, EventArgs e)
    {
        int.TryParse(txtHeight.Text, out height);
        if (height < 9) height = 9;
        if (height > 24) height = 24;
        x.height = height;
        int.TryParse(txtWidth.Text, out width);
        if (width < 9) width = 9;
        if (width > 30) width = 30;
        x.width = width;
        int.TryParse(txtMines.Text, out bombs);
        if (bombs < 10) bombs = 10;
        if (bombs > (height - 1) * (width - 1)) bombs = (height - 1) * (width - 1);
        x.mines = bombs;
        Close();
    }
}