namespace MapUpgrader
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.BtnLoadMap = new System.Windows.Forms.Button();
            this.BtnUpgrade = new System.Windows.Forms.Button();
            this.BtnSelectFolder = new System.Windows.Forms.Button();
            this.BtnTrash = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.RichLogBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // BtnLoadMap
            // 
            this.BtnLoadMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLoadMap.Location = new System.Drawing.Point(12, 12);
            this.BtnLoadMap.Name = "BtnLoadMap";
            this.BtnLoadMap.Size = new System.Drawing.Size(154, 34);
            this.BtnLoadMap.TabIndex = 0;
            this.BtnLoadMap.Text = "Load Map";
            this.BtnLoadMap.UseVisualStyleBackColor = true;
            this.BtnLoadMap.Click += new System.EventHandler(this.BtnLoadMap_Click);
            // 
            // BtnUpgrade
            // 
            this.BtnUpgrade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnUpgrade.Enabled = false;
            this.BtnUpgrade.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnUpgrade.Location = new System.Drawing.Point(518, 12);
            this.BtnUpgrade.Name = "BtnUpgrade";
            this.BtnUpgrade.Size = new System.Drawing.Size(154, 34);
            this.BtnUpgrade.TabIndex = 1;
            this.BtnUpgrade.Text = "Add Triggers!";
            this.BtnUpgrade.UseVisualStyleBackColor = true;
            this.BtnUpgrade.Click += new System.EventHandler(this.BtnUpgrade_Click);
            // 
            // BtnSelectFolder
            // 
            this.BtnSelectFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSelectFolder.Location = new System.Drawing.Point(172, 12);
            this.BtnSelectFolder.Name = "BtnSelectFolder";
            this.BtnSelectFolder.Size = new System.Drawing.Size(154, 34);
            this.BtnSelectFolder.TabIndex = 3;
            this.BtnSelectFolder.Text = "Select Folder";
            this.BtnSelectFolder.UseVisualStyleBackColor = true;
            this.BtnSelectFolder.Click += new System.EventHandler(this.BtnSelectFolder_Click);
            // 
            // BtnTrash
            // 
            this.BtnTrash.AccessibleDescription = "";
            this.BtnTrash.BackgroundImage = global::MapUpgrader.Properties.Resources.trash;
            this.BtnTrash.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnTrash.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnTrash.Location = new System.Drawing.Point(332, 12);
            this.BtnTrash.Name = "BtnTrash";
            this.BtnTrash.Size = new System.Drawing.Size(34, 34);
            this.BtnTrash.TabIndex = 4;
            this.BtnTrash.Tag = "";
            this.BtnTrash.UseVisualStyleBackColor = true;
            this.BtnTrash.Click += new System.EventHandler(this.BtnTrash_Click);
            // 
            // RichLogBox
            // 
            this.RichLogBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RichLogBox.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.RichLogBox.Location = new System.Drawing.Point(13, 64);
            this.RichLogBox.Name = "RichLogBox";
            this.RichLogBox.ReadOnly = true;
            this.RichLogBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.RichLogBox.Size = new System.Drawing.Size(659, 335);
            this.RichLogBox.TabIndex = 5;
            this.RichLogBox.Text = "";
            this.RichLogBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.RichLogBox_LinkClicked);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Controls.Add(this.RichLogBox);
            this.Controls.Add(this.BtnTrash);
            this.Controls.Add(this.BtnSelectFolder);
            this.Controls.Add(this.BtnUpgrade);
            this.Controls.Add(this.BtnLoadMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 350);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnLoadMap;
        private System.Windows.Forms.Button BtnUpgrade;
        private System.Windows.Forms.Button BtnSelectFolder;
        private System.Windows.Forms.Button BtnTrash;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.RichTextBox RichLogBox;
    }
}

