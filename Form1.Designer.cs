namespace DSR_BossRush
{
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
            btnLaunchDSR = new Button();
            btnOpenMenu = new Button();
            btnDSRDebug = new Button();
            btnCloseMenu = new Button();
            btnWarp = new Button();
            btnArena = new Button();
            cmbBoss = new ComboBox();
            SuspendLayout();
            // 
            // btnLaunchDSR
            // 
            btnLaunchDSR.Location = new Point(12, 12);
            btnLaunchDSR.Name = "btnLaunchDSR";
            btnLaunchDSR.Size = new Size(101, 23);
            btnLaunchDSR.TabIndex = 0;
            btnLaunchDSR.Text = "Launch BR";
            btnLaunchDSR.UseVisualStyleBackColor = true;
            btnLaunchDSR.Click += btnLaunchDSR_Click;
            // 
            // btnOpenMenu
            // 
            btnOpenMenu.Location = new Point(12, 125);
            btnOpenMenu.Name = "btnOpenMenu";
            btnOpenMenu.Size = new Size(101, 23);
            btnOpenMenu.TabIndex = 1;
            btnOpenMenu.Text = "Open Menu";
            btnOpenMenu.UseVisualStyleBackColor = true;
            btnOpenMenu.Click += btnLoadTpf_Click;
            // 
            // btnDSRDebug
            // 
            btnDSRDebug.Location = new Point(119, 12);
            btnDSRDebug.Name = "btnDSRDebug";
            btnDSRDebug.Size = new Size(101, 23);
            btnDSRDebug.TabIndex = 2;
            btnDSRDebug.Text = "Launch Dbg";
            btnDSRDebug.UseVisualStyleBackColor = true;
            btnDSRDebug.Click += btnDSRDebug_Click;
            // 
            // btnCloseMenu
            // 
            btnCloseMenu.Location = new Point(119, 125);
            btnCloseMenu.Name = "btnCloseMenu";
            btnCloseMenu.Size = new Size(101, 23);
            btnCloseMenu.TabIndex = 3;
            btnCloseMenu.Text = "Close Menu";
            btnCloseMenu.UseVisualStyleBackColor = true;
            btnCloseMenu.Click += btnCloseMenu_Click;
            // 
            // btnWarp
            // 
            btnWarp.Location = new Point(227, 40);
            btnWarp.Name = "btnWarp";
            btnWarp.Size = new Size(101, 23);
            btnWarp.TabIndex = 4;
            btnWarp.Text = "Warp";
            btnWarp.UseVisualStyleBackColor = true;
            btnWarp.Click += btnWarp_Click;
            // 
            // btnArena
            // 
            btnArena.Location = new Point(119, 154);
            btnArena.Name = "btnArena";
            btnArena.Size = new Size(101, 23);
            btnArena.TabIndex = 5;
            btnArena.Text = "Arena";
            btnArena.UseVisualStyleBackColor = true;
            btnArena.Click += btnArena_Click;
            // 
            // cmbBoss
            // 
            cmbBoss.FormattingEnabled = true;
            cmbBoss.Location = new Point(12, 41);
            cmbBoss.Name = "cmbBoss";
            cmbBoss.Size = new Size(209, 23);
            cmbBoss.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(cmbBoss);
            Controls.Add(btnArena);
            Controls.Add(btnWarp);
            Controls.Add(btnCloseMenu);
            Controls.Add(btnDSRDebug);
            Controls.Add(btnOpenMenu);
            Controls.Add(btnLaunchDSR);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnLaunchDSR;
        private Button btnOpenMenu;
        private Button btnDSRDebug;
        private Button btnCloseMenu;
        private Button btnWarp;
        private Button btnArena;
        private ComboBox cmbBoss;
    }
}