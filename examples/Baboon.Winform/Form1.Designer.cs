namespace Baboon.Winform;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.menuStrip1 = new MenuStrip();
        this.SuspendLayout();
        // 
        // menuStrip1
        // 
        this.menuStrip1.ImageScalingSize = new Size(32, 32);
        this.menuStrip1.Location = new Point(0, 0);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Size = new Size(1600, 24);
        this.menuStrip1.TabIndex = 0;
        this.menuStrip1.Text = "menuStrip1";
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new SizeF(14F, 31F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(1600, 821);
        this.Controls.Add(this.menuStrip1);
        this.MainMenuStrip = this.menuStrip1;
        this.Margin = new Padding(6, 5, 6, 5);
        this.Name = "Form1";
        this.Text = "Form1";
        this.Load += this.Form1_Load;
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private MenuStrip menuStrip1;
}
