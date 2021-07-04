
namespace GameOfLife
{
    partial class Game2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Game2));
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_New = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_File_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_File_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View_HUD = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View_NeighborCount = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View_Grid = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_View_Torodial = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_View_Finite = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Control = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Control_Start = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Control_Pause = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Control_Next = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Randomize = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Randomize_Regenerate = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Randomize_FromSeed = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Randomize_FromTime = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Settings = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Settings_BackColor = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Settings_CellColor = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Settings_GridColor = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Settings_GridX10Color = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_Settings_Reset = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Settings_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu_Settings_Advanced = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Help_About = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.ToolStrip_New = new System.Windows.Forms.ToolStripButton();
            this.ToolStrip_Open = new System.Windows.Forms.ToolStripButton();
            this.ToolStrip_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStrip_Start = new System.Windows.Forms.ToolStripButton();
            this.ToolStrip_Next = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStrip_SeedBox = new System.Windows.Forms.ToolStripTextBox();
            this.ToolStrip_Generate = new System.Windows.Forms.ToolStripButton();
            this.ToolStrip_RandomSeed = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel_Generations = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_CellCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_Boundary = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_UniverseSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.runToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.nextToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.colorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backColorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cellColorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.gridColorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.gridX10ColorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.hUDToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.neighborCountToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.gridToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.torodialToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.finiteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.GraphicsPanel = new GameOfLife.GraphicsPanel2();
            this.MenuStrip.SuspendLayout();
            this.ToolStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(500, 24);
            this.MenuStrip.TabIndex = 0;
            this.MenuStrip.Click += new System.EventHandler(this.SeedBox_ParseSeed);
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File_New,
            this.Menu_File_Open,
            this.toolStripSeparator3,
            this.Menu_File_Save,
            this.Menu_File_SaveAs,
            this.toolStripSeparator1,
            this.Menu_File_Exit});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(37, 20);
            this.Menu_File.Text = "&File";
            // 
            // Menu_File_New
            // 
            this.Menu_File_New.Image = ((System.Drawing.Image)(resources.GetObject("Menu_File_New.Image")));
            this.Menu_File_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Menu_File_New.Name = "Menu_File_New";
            this.Menu_File_New.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.Menu_File_New.Size = new System.Drawing.Size(146, 22);
            this.Menu_File_New.Text = "&New";
            this.Menu_File_New.Click += new System.EventHandler(this.File_New);
            // 
            // Menu_File_Open
            // 
            this.Menu_File_Open.Image = ((System.Drawing.Image)(resources.GetObject("Menu_File_Open.Image")));
            this.Menu_File_Open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Menu_File_Open.Name = "Menu_File_Open";
            this.Menu_File_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.Menu_File_Open.Size = new System.Drawing.Size(146, 22);
            this.Menu_File_Open.Text = "&Open";
            this.Menu_File_Open.Click += new System.EventHandler(this.File_Open);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(143, 6);
            // 
            // Menu_File_Save
            // 
            this.Menu_File_Save.Image = ((System.Drawing.Image)(resources.GetObject("Menu_File_Save.Image")));
            this.Menu_File_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Menu_File_Save.Name = "Menu_File_Save";
            this.Menu_File_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.Menu_File_Save.Size = new System.Drawing.Size(146, 22);
            this.Menu_File_Save.Text = "&Save";
            this.Menu_File_Save.Click += new System.EventHandler(this.File_Save);
            // 
            // Menu_File_SaveAs
            // 
            this.Menu_File_SaveAs.Name = "Menu_File_SaveAs";
            this.Menu_File_SaveAs.Size = new System.Drawing.Size(146, 22);
            this.Menu_File_SaveAs.Text = "Save &As";
            this.Menu_File_SaveAs.Click += new System.EventHandler(this.File_SaveAs);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // Menu_File_Exit
            // 
            this.Menu_File_Exit.Name = "Menu_File_Exit";
            this.Menu_File_Exit.Size = new System.Drawing.Size(146, 22);
            this.Menu_File_Exit.Text = "E&xit";
            this.Menu_File_Exit.Click += new System.EventHandler(this.File_Exit);
            // 
            // Menu_View
            // 
            this.Menu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_View_HUD,
            this.Menu_View_NeighborCount,
            this.Menu_View_Grid,
            this.toolStripSeparator7,
            this.Menu_View_Torodial,
            this.Menu_View_Finite});
            this.Menu_View.Name = "Menu_View";
            this.Menu_View.Size = new System.Drawing.Size(44, 20);
            this.Menu_View.Text = "View";
            // 
            // Menu_View_HUD
            // 
            this.Menu_View_HUD.Checked = true;
            this.Menu_View_HUD.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Menu_View_HUD.Name = "Menu_View_HUD";
            this.Menu_View_HUD.Size = new System.Drawing.Size(160, 22);
            this.Menu_View_HUD.Text = "HUD";
            this.Menu_View_HUD.Click += new System.EventHandler(this.View_HUD);
            // 
            // Menu_View_NeighborCount
            // 
            this.Menu_View_NeighborCount.Checked = true;
            this.Menu_View_NeighborCount.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Menu_View_NeighborCount.Name = "Menu_View_NeighborCount";
            this.Menu_View_NeighborCount.Size = new System.Drawing.Size(160, 22);
            this.Menu_View_NeighborCount.Text = "Neighbor Count";
            this.Menu_View_NeighborCount.Click += new System.EventHandler(this.View_NeighborCount);
            // 
            // Menu_View_Grid
            // 
            this.Menu_View_Grid.Checked = true;
            this.Menu_View_Grid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Menu_View_Grid.Name = "Menu_View_Grid";
            this.Menu_View_Grid.Size = new System.Drawing.Size(160, 22);
            this.Menu_View_Grid.Text = "Grid";
            this.Menu_View_Grid.Click += new System.EventHandler(this.View_Grid);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(157, 6);
            // 
            // Menu_View_Torodial
            // 
            this.Menu_View_Torodial.Checked = true;
            this.Menu_View_Torodial.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Menu_View_Torodial.Name = "Menu_View_Torodial";
            this.Menu_View_Torodial.Size = new System.Drawing.Size(160, 22);
            this.Menu_View_Torodial.Text = "Torodial";
            this.Menu_View_Torodial.Click += new System.EventHandler(this.View_Torodial);
            // 
            // Menu_View_Finite
            // 
            this.Menu_View_Finite.Name = "Menu_View_Finite";
            this.Menu_View_Finite.Size = new System.Drawing.Size(160, 22);
            this.Menu_View_Finite.Text = "Finite";
            this.Menu_View_Finite.Click += new System.EventHandler(this.View_Finite);
            // 
            // Menu_Control
            // 
            this.Menu_Control.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Control_Start,
            this.Menu_Control_Pause,
            this.Menu_Control_Next});
            this.Menu_Control.Name = "Menu_Control";
            this.Menu_Control.Size = new System.Drawing.Size(59, 20);
            this.Menu_Control.Text = "Control";
            // 
            // Menu_Control_Start
            // 
            this.Menu_Control_Start.Image = global::GameOfLife.Properties.Resources.startIcon;
            this.Menu_Control_Start.Name = "Menu_Control_Start";
            this.Menu_Control_Start.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.Menu_Control_Start.Size = new System.Drawing.Size(124, 22);
            this.Menu_Control_Start.Text = "Start";
            this.Menu_Control_Start.Click += new System.EventHandler(this.Control_Start);
            // 
            // Menu_Control_Pause
            // 
            this.Menu_Control_Pause.Enabled = false;
            this.Menu_Control_Pause.Image = global::GameOfLife.Properties.Resources.pauseIcon;
            this.Menu_Control_Pause.Name = "Menu_Control_Pause";
            this.Menu_Control_Pause.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.Menu_Control_Pause.Size = new System.Drawing.Size(124, 22);
            this.Menu_Control_Pause.Text = "Pause";
            this.Menu_Control_Pause.Click += new System.EventHandler(this.Control_Pause);
            // 
            // Menu_Control_Next
            // 
            this.Menu_Control_Next.Enabled = false;
            this.Menu_Control_Next.Image = global::GameOfLife.Properties.Resources.nextIcon;
            this.Menu_Control_Next.Name = "Menu_Control_Next";
            this.Menu_Control_Next.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.Menu_Control_Next.Size = new System.Drawing.Size(124, 22);
            this.Menu_Control_Next.Text = "Next";
            this.Menu_Control_Next.Click += new System.EventHandler(this.Control_Next);
            // 
            // Menu_Randomize
            // 
            this.Menu_Randomize.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Randomize_Regenerate,
            this.Menu_Randomize_FromSeed,
            this.Menu_Randomize_FromTime});
            this.Menu_Randomize.Name = "Menu_Randomize";
            this.Menu_Randomize.Size = new System.Drawing.Size(78, 20);
            this.Menu_Randomize.Text = "Randomize";
            // 
            // Menu_Randomize_Regenerate
            // 
            this.Menu_Randomize_Regenerate.Name = "Menu_Randomize_Regenerate";
            this.Menu_Randomize_Regenerate.Size = new System.Drawing.Size(139, 22);
            this.Menu_Randomize_Regenerate.Text = "Regenerate";
            this.Menu_Randomize_Regenerate.Click += new System.EventHandler(this.Randomize_GenerateSeed);
            // 
            // Menu_Randomize_FromSeed
            // 
            this.Menu_Randomize_FromSeed.Name = "Menu_Randomize_FromSeed";
            this.Menu_Randomize_FromSeed.Size = new System.Drawing.Size(139, 22);
            this.Menu_Randomize_FromSeed.Text = "From Seed...";
            // 
            // Menu_Randomize_FromTime
            // 
            this.Menu_Randomize_FromTime.Name = "Menu_Randomize_FromTime";
            this.Menu_Randomize_FromTime.Size = new System.Drawing.Size(139, 22);
            this.Menu_Randomize_FromTime.Text = "From Time";
            this.Menu_Randomize_FromTime.Click += new System.EventHandler(this.Randomize_RandomSeed);
            // 
            // Menu_Settings
            // 
            this.Menu_Settings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Settings_BackColor,
            this.Menu_Settings_CellColor,
            this.Menu_Settings_GridColor,
            this.Menu_Settings_GridX10Color,
            this.toolStripSeparator4,
            this.Menu_Settings_Reset,
            this.Menu_Settings_Reload,
            this.toolStripSeparator2,
            this.Menu_Settings_Advanced});
            this.Menu_Settings.Name = "Menu_Settings";
            this.Menu_Settings.Size = new System.Drawing.Size(61, 20);
            this.Menu_Settings.Text = "Settings";
            // 
            // Menu_Settings_BackColor
            // 
            this.Menu_Settings_BackColor.Name = "Menu_Settings_BackColor";
            this.Menu_Settings_BackColor.Size = new System.Drawing.Size(149, 22);
            this.Menu_Settings_BackColor.Text = "Back Color";
            this.Menu_Settings_BackColor.Click += new System.EventHandler(this.Settings_BackColor);
            // 
            // Menu_Settings_CellColor
            // 
            this.Menu_Settings_CellColor.Name = "Menu_Settings_CellColor";
            this.Menu_Settings_CellColor.Size = new System.Drawing.Size(149, 22);
            this.Menu_Settings_CellColor.Text = "Cell Color";
            this.Menu_Settings_CellColor.Click += new System.EventHandler(this.Settings_CellColor);
            // 
            // Menu_Settings_GridColor
            // 
            this.Menu_Settings_GridColor.Name = "Menu_Settings_GridColor";
            this.Menu_Settings_GridColor.Size = new System.Drawing.Size(149, 22);
            this.Menu_Settings_GridColor.Text = "Grid Color";
            this.Menu_Settings_GridColor.Click += new System.EventHandler(this.Settings_GridColor);
            // 
            // Menu_Settings_GridX10Color
            // 
            this.Menu_Settings_GridX10Color.Name = "Menu_Settings_GridX10Color";
            this.Menu_Settings_GridX10Color.Size = new System.Drawing.Size(149, 22);
            this.Menu_Settings_GridX10Color.Text = "Grid x10 Color";
            this.Menu_Settings_GridX10Color.Click += new System.EventHandler(this.Settings_GridX10Color);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(146, 6);
            // 
            // Menu_Settings_Reset
            // 
            this.Menu_Settings_Reset.Name = "Menu_Settings_Reset";
            this.Menu_Settings_Reset.Size = new System.Drawing.Size(149, 22);
            this.Menu_Settings_Reset.Text = "Reset";
            this.Menu_Settings_Reset.Click += new System.EventHandler(this.Settings_Reset);
            // 
            // Menu_Settings_Reload
            // 
            this.Menu_Settings_Reload.Image = ((System.Drawing.Image)(resources.GetObject("Menu_Settings_Reload.Image")));
            this.Menu_Settings_Reload.Name = "Menu_Settings_Reload";
            this.Menu_Settings_Reload.Size = new System.Drawing.Size(149, 22);
            this.Menu_Settings_Reload.Text = "Reload";
            this.Menu_Settings_Reload.Click += new System.EventHandler(this.Settings_Reload);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(146, 6);
            // 
            // Menu_Settings_Advanced
            // 
            this.Menu_Settings_Advanced.Name = "Menu_Settings_Advanced";
            this.Menu_Settings_Advanced.Size = new System.Drawing.Size(149, 22);
            this.Menu_Settings_Advanced.Text = "Advanced";
            // 
            // Menu_Help
            // 
            this.Menu_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Help_About});
            this.Menu_Help.Name = "Menu_Help";
            this.Menu_Help.Size = new System.Drawing.Size(44, 20);
            this.Menu_Help.Text = "&Help";
            // 
            // Menu_Help_About
            // 
            this.Menu_Help_About.Name = "Menu_Help_About";
            this.Menu_Help_About.Size = new System.Drawing.Size(116, 22);
            this.Menu_Help_About.Text = "&About...";
            // 
            // ToolStrip
            // 
            this.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip.Location = new System.Drawing.Point(0, 24);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.Size = new System.Drawing.Size(500, 25);
            this.ToolStrip.TabIndex = 1;
            this.ToolStrip.Text = "ToolStrip";
            this.ToolStrip.Click += new System.EventHandler(this.SeedBox_ParseSeed);
            // 
            // ToolStrip_New
            // 
            this.ToolStrip_New.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStrip_New.Image = ((System.Drawing.Image)(resources.GetObject("ToolStrip_New.Image")));
            this.ToolStrip_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStrip_New.Name = "ToolStrip_New";
            this.ToolStrip_New.Size = new System.Drawing.Size(23, 22);
            this.ToolStrip_New.Text = "&New";
            this.ToolStrip_New.Click += new System.EventHandler(this.File_New);
            // 
            // ToolStrip_Open
            // 
            this.ToolStrip_Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStrip_Open.Image = ((System.Drawing.Image)(resources.GetObject("ToolStrip_Open.Image")));
            this.ToolStrip_Open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStrip_Open.Name = "ToolStrip_Open";
            this.ToolStrip_Open.Size = new System.Drawing.Size(23, 22);
            this.ToolStrip_Open.Text = "&Open";
            this.ToolStrip_Open.Click += new System.EventHandler(this.File_Open);
            // 
            // ToolStrip_Save
            // 
            this.ToolStrip_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStrip_Save.Image = ((System.Drawing.Image)(resources.GetObject("ToolStrip_Save.Image")));
            this.ToolStrip_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStrip_Save.Name = "ToolStrip_Save";
            this.ToolStrip_Save.Size = new System.Drawing.Size(23, 22);
            this.ToolStrip_Save.Text = "&Save";
            this.ToolStrip_Save.Click += new System.EventHandler(this.File_Save);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStrip_Start
            // 
            this.ToolStrip_Start.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStrip_Start.Image = global::GameOfLife.Properties.Resources.startIcon;
            this.ToolStrip_Start.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStrip_Start.Name = "ToolStrip_Start";
            this.ToolStrip_Start.Size = new System.Drawing.Size(23, 22);
            this.ToolStrip_Start.Text = "Start";
            this.ToolStrip_Start.Click += new System.EventHandler(this.Control_Start);
            // 
            // ToolStrip_Next
            // 
            this.ToolStrip_Next.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStrip_Next.Enabled = false;
            this.ToolStrip_Next.Image = global::GameOfLife.Properties.Resources.nextIcon;
            this.ToolStrip_Next.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStrip_Next.Name = "ToolStrip_Next";
            this.ToolStrip_Next.Size = new System.Drawing.Size(23, 22);
            this.ToolStrip_Next.Text = "Next";
            this.ToolStrip_Next.ToolTipText = "Next";
            this.ToolStrip_Next.Click += new System.EventHandler(this.Control_Next);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStrip_SeedBox
            // 
            this.ToolStrip_SeedBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.ToolStrip_SeedBox.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ToolStrip_SeedBox.Name = "ToolStrip_SeedBox";
            this.ToolStrip_SeedBox.Size = new System.Drawing.Size(100, 25);
            this.ToolStrip_SeedBox.Text = "Enter a seed...";
            this.ToolStrip_SeedBox.Leave += new System.EventHandler(this.SeedBox_ParseSeed);
            this.ToolStrip_SeedBox.Click += new System.EventHandler(this.SeedBox_Click);
            // 
            // ToolStrip_Generate
            // 
            this.ToolStrip_Generate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStrip_Generate.Image = global::GameOfLife.Properties.Resources.goIcon;
            this.ToolStrip_Generate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStrip_Generate.Name = "ToolStrip_Generate";
            this.ToolStrip_Generate.Size = new System.Drawing.Size(23, 22);
            this.ToolStrip_Generate.ToolTipText = "Generate";
            this.ToolStrip_Generate.Click += new System.EventHandler(this.Randomize_GenerateSeed);
            // 
            // ToolStrip_RandomSeed
            // 
            this.ToolStrip_RandomSeed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStrip_RandomSeed.Image = global::GameOfLife.Properties.Resources.randomizeIcon;
            this.ToolStrip_RandomSeed.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStrip_RandomSeed.Name = "ToolStrip_RandomSeed";
            this.ToolStrip_RandomSeed.Size = new System.Drawing.Size(23, 22);
            this.ToolStrip_RandomSeed.ToolTipText = "Random Seed";
            this.ToolStrip_RandomSeed.Click += new System.EventHandler(this.Randomize_RandomSeed);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 549);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(500, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "StatusStrip";
            // 
            // StatusLabel_Generations
            // 
            this.StatusLabel_Generations.Name = "StatusLabel_Generations";
            this.StatusLabel_Generations.Size = new System.Drawing.Size(90, 17);
            this.StatusLabel_Generations.Text = "Generations = 0";
            // 
            // StatusLabel_CellCount
            // 
            this.StatusLabel_CellCount.Name = "StatusLabel_CellCount";
            this.StatusLabel_CellCount.Padding = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.StatusLabel_CellCount.Size = new System.Drawing.Size(99, 17);
            this.StatusLabel_CellCount.Text = "Cell Count = 0";
            // 
            // StatusLabel_Boundary
            // 
            this.StatusLabel_Boundary.Name = "StatusLabel_Boundary";
            this.StatusLabel_Boundary.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.StatusLabel_Boundary.Size = new System.Drawing.Size(122, 17);
            this.StatusLabel_Boundary.Text = "Boundary = Torodial";
            // 
            // StatusLabel_UniverseSize
            // 
            this.StatusLabel_UniverseSize.Name = "StatusLabel_UniverseSize";
            this.StatusLabel_UniverseSize.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.StatusLabel_UniverseSize.Size = new System.Drawing.Size(133, 17);
            this.StatusLabel_UniverseSize.Text = "Universe Size = 10 x 10";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem1,
            this.pauseToolStripMenuItem1,
            this.nextToolStripMenuItem1,
            this.toolStripSeparator5,
            this.colorToolStripMenuItem,
            this.viewToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(106, 120);
            // 
            // runToolStripMenuItem1
            // 
            this.runToolStripMenuItem1.Image = global::GameOfLife.Properties.Resources.startIcon;
            this.runToolStripMenuItem1.Name = "runToolStripMenuItem1";
            this.runToolStripMenuItem1.Size = new System.Drawing.Size(105, 22);
            this.runToolStripMenuItem1.Text = "Start";
            this.runToolStripMenuItem1.Click += new System.EventHandler(this.Control_Start);
            // 
            // pauseToolStripMenuItem1
            // 
            this.pauseToolStripMenuItem1.Enabled = false;
            this.pauseToolStripMenuItem1.Image = global::GameOfLife.Properties.Resources.pauseIcon;
            this.pauseToolStripMenuItem1.Name = "pauseToolStripMenuItem1";
            this.pauseToolStripMenuItem1.Size = new System.Drawing.Size(105, 22);
            this.pauseToolStripMenuItem1.Text = "Pause";
            this.pauseToolStripMenuItem1.Click += new System.EventHandler(this.Control_Pause);
            // 
            // nextToolStripMenuItem1
            // 
            this.nextToolStripMenuItem1.Enabled = false;
            this.nextToolStripMenuItem1.Image = global::GameOfLife.Properties.Resources.nextIcon;
            this.nextToolStripMenuItem1.Name = "nextToolStripMenuItem1";
            this.nextToolStripMenuItem1.Size = new System.Drawing.Size(105, 22);
            this.nextToolStripMenuItem1.Text = "Next";
            this.nextToolStripMenuItem1.Click += new System.EventHandler(this.Control_Next);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(102, 6);
            // 
            // colorToolStripMenuItem
            // 
            this.colorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backColorToolStripMenuItem1,
            this.cellColorToolStripMenuItem1,
            this.gridColorToolStripMenuItem1,
            this.gridX10ColorToolStripMenuItem1});
            this.colorToolStripMenuItem.Name = "colorToolStripMenuItem";
            this.colorToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.colorToolStripMenuItem.Text = "Color";
            // 
            // backColorToolStripMenuItem1
            // 
            this.backColorToolStripMenuItem1.Name = "backColorToolStripMenuItem1";
            this.backColorToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.backColorToolStripMenuItem1.Text = "Back Color";
            this.backColorToolStripMenuItem1.Click += new System.EventHandler(this.Settings_BackColor);
            // 
            // cellColorToolStripMenuItem1
            // 
            this.cellColorToolStripMenuItem1.Name = "cellColorToolStripMenuItem1";
            this.cellColorToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.cellColorToolStripMenuItem1.Text = "Cell Color";
            this.cellColorToolStripMenuItem1.Click += new System.EventHandler(this.Settings_CellColor);
            // 
            // gridColorToolStripMenuItem1
            // 
            this.gridColorToolStripMenuItem1.Name = "gridColorToolStripMenuItem1";
            this.gridColorToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.gridColorToolStripMenuItem1.Text = "Grid Color";
            this.gridColorToolStripMenuItem1.Click += new System.EventHandler(this.Settings_GridColor);
            // 
            // gridX10ColorToolStripMenuItem1
            // 
            this.gridX10ColorToolStripMenuItem1.Name = "gridX10ColorToolStripMenuItem1";
            this.gridX10ColorToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.gridX10ColorToolStripMenuItem1.Text = "Grid x10 Color";
            this.gridX10ColorToolStripMenuItem1.Click += new System.EventHandler(this.Settings_GridX10Color);
            // 
            // viewToolStripMenuItem1
            // 
            this.viewToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hUDToolStripMenuItem1,
            this.neighborCountToolStripMenuItem1,
            this.gridToolStripMenuItem1,
            this.toolStripSeparator8,
            this.torodialToolStripMenuItem1,
            this.finiteToolStripMenuItem1});
            this.viewToolStripMenuItem1.Name = "viewToolStripMenuItem1";
            this.viewToolStripMenuItem1.Size = new System.Drawing.Size(105, 22);
            this.viewToolStripMenuItem1.Text = "View";
            // 
            // hUDToolStripMenuItem1
            // 
            this.hUDToolStripMenuItem1.Checked = true;
            this.hUDToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hUDToolStripMenuItem1.Name = "hUDToolStripMenuItem1";
            this.hUDToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.hUDToolStripMenuItem1.Text = "HUD";
            this.hUDToolStripMenuItem1.Click += new System.EventHandler(this.View_HUD);
            // 
            // neighborCountToolStripMenuItem1
            // 
            this.neighborCountToolStripMenuItem1.Checked = true;
            this.neighborCountToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.neighborCountToolStripMenuItem1.Name = "neighborCountToolStripMenuItem1";
            this.neighborCountToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.neighborCountToolStripMenuItem1.Text = "Neighbor Count";
            this.neighborCountToolStripMenuItem1.Click += new System.EventHandler(this.View_NeighborCount);
            // 
            // gridToolStripMenuItem1
            // 
            this.gridToolStripMenuItem1.Checked = true;
            this.gridToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gridToolStripMenuItem1.Name = "gridToolStripMenuItem1";
            this.gridToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.gridToolStripMenuItem1.Text = "Grid";
            this.gridToolStripMenuItem1.Click += new System.EventHandler(this.View_Grid);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(157, 6);
            // 
            // torodialToolStripMenuItem1
            // 
            this.torodialToolStripMenuItem1.Checked = true;
            this.torodialToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.torodialToolStripMenuItem1.Name = "torodialToolStripMenuItem1";
            this.torodialToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.torodialToolStripMenuItem1.Text = "Torodial";
            this.torodialToolStripMenuItem1.Click += new System.EventHandler(this.View_Torodial);
            // 
            // finiteToolStripMenuItem1
            // 
            this.finiteToolStripMenuItem1.Name = "finiteToolStripMenuItem1";
            this.finiteToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.finiteToolStripMenuItem1.Text = "Finite";
            this.finiteToolStripMenuItem1.Click += new System.EventHandler(this.View_Finite);
            // 
            // GraphicsPanel
            // 
            this.GraphicsPanel.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.GraphicsPanel.BackColor = System.Drawing.SystemColors.Window;
            this.GraphicsPanel.ContextMenuStrip = this.contextMenuStrip1;
            this.GraphicsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GraphicsPanel.Location = new System.Drawing.Point(0, 49);
            this.GraphicsPanel.Name = "GraphicsPanel";
            this.GraphicsPanel.Size = new System.Drawing.Size(500, 500);
            this.GraphicsPanel.TabIndex = 3;
            this.GraphicsPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.Process_GraphicsPanel_Paint);
            this.GraphicsPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Process_GraphicsPanel_MouseClick);
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 571);
            this.Controls.Add(this.GraphicsPanel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ToolStrip);
            this.Controls.Add(this.MenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.MenuStrip;
            this.MinimumSize = new System.Drawing.Size(356, 450);
            this.Name = "Game";
            this.Text = "Game of Life";
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStrip ToolStrip;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private GraphicsPanel2 GraphicsPanel;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_New;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Open;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Save;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_SaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Exit;
        private System.Windows.Forms.ToolStripMenuItem Menu_Help;
        private System.Windows.Forms.ToolStripMenuItem Menu_Help_About;
        private System.Windows.Forms.ToolStripButton ToolStrip_New;
        private System.Windows.Forms.ToolStripButton ToolStrip_Open;
        private System.Windows.Forms.ToolStripButton ToolStrip_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_Generations;
        private System.Windows.Forms.ToolStripButton ToolStrip_Start;
        private System.Windows.Forms.ToolStripButton ToolStrip_Next;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_CellCount;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_Boundary;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_UniverseSize;
        private System.Windows.Forms.ToolStripMenuItem Menu_Settings;
        private System.Windows.Forms.ToolStripMenuItem Menu_Settings_BackColor;
        private System.Windows.Forms.ToolStripMenuItem Menu_Settings_CellColor;
        private System.Windows.Forms.ToolStripMenuItem Menu_Settings_GridColor;
        private System.Windows.Forms.ToolStripMenuItem Menu_Settings_GridX10Color;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem Menu_Settings_Reset;
        private System.Windows.Forms.ToolStripMenuItem Menu_Settings_Reload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem Menu_Settings_Advanced;
        private System.Windows.Forms.ToolStripMenuItem Menu_Control;
        private System.Windows.Forms.ToolStripMenuItem Menu_Control_Start;
        private System.Windows.Forms.ToolStripMenuItem Menu_Control_Pause;
        private System.Windows.Forms.ToolStripMenuItem Menu_Control_Next;
        private System.Windows.Forms.ToolStripMenuItem Menu_View;
        private System.Windows.Forms.ToolStripMenuItem Menu_View_HUD;
        private System.Windows.Forms.ToolStripMenuItem Menu_View_NeighborCount;
        private System.Windows.Forms.ToolStripMenuItem Menu_View_Grid;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem Menu_View_Torodial;
        private System.Windows.Forms.ToolStripMenuItem Menu_View_Finite;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem pauseToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem nextToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem colorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backColorToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cellColorToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem gridColorToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem gridX10ColorToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem hUDToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem neighborCountToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem gridToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem torodialToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem finiteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton ToolStrip_RandomSeed;
        private System.Windows.Forms.ToolStripTextBox ToolStrip_SeedBox;
        private System.Windows.Forms.ToolStripButton ToolStrip_Generate;
        private System.Windows.Forms.ToolStripMenuItem Menu_Randomize;
        private System.Windows.Forms.ToolStripMenuItem Menu_Randomize_Regenerate;
        private System.Windows.Forms.ToolStripMenuItem Menu_Randomize_FromSeed;
        private System.Windows.Forms.ToolStripMenuItem Menu_Randomize_FromTime;
    }
}

