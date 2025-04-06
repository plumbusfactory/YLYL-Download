namespace YLYL_Download
{
    partial class MainForm
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            ButtonBackground = new System.Windows.Forms.Panel();
            utilGroup = new System.Windows.Forms.GroupBox();
            browserList = new System.Windows.Forms.ComboBox();
            useCookies = new System.Windows.Forms.CheckBox();
            generatePlaylist = new System.Windows.Forms.CheckBox();
            setOutputButton = new System.Windows.Forms.Button();
            updateYTDLP = new System.Windows.Forms.Button();
            executeButton = new System.Windows.Forms.Button();
            generateCommand = new System.Windows.Forms.Button();
            loadList = new System.Windows.Forms.Button();
            dlList = new System.Windows.Forms.GroupBox();
            URLs = new System.Windows.Forms.DataGridView();
            readyLabel = new System.Windows.Forms.Label();
            readyStatus = new System.Windows.Forms.Label();
            pathLabel = new System.Windows.Forms.Label();
            pathLabelValue = new System.Windows.Forms.Label();
            totalCountLabel = new System.Windows.Forms.Label();
            totalCount = new System.Windows.Forms.Label();
            ButtonBackground.SuspendLayout();
            utilGroup.SuspendLayout();
            dlList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)URLs).BeginInit();
            SuspendLayout();
            // 
            // ButtonBackground
            // 
            ButtonBackground.Controls.Add(utilGroup);
            ButtonBackground.Controls.Add(updateYTDLP);
            ButtonBackground.Controls.Add(executeButton);
            ButtonBackground.Controls.Add(generateCommand);
            ButtonBackground.Controls.Add(loadList);
            ButtonBackground.Controls.Add(dlList);
            ButtonBackground.Location = new System.Drawing.Point(12, 12);
            ButtonBackground.Name = "ButtonBackground";
            ButtonBackground.Size = new System.Drawing.Size(1489, 859);
            ButtonBackground.TabIndex = 0;
            // 
            // utilGroup
            // 
            utilGroup.Controls.Add(browserList);
            utilGroup.Controls.Add(useCookies);
            utilGroup.Controls.Add(generatePlaylist);
            utilGroup.Controls.Add(setOutputButton);
            utilGroup.Location = new System.Drawing.Point(414, 3);
            utilGroup.Name = "utilGroup";
            utilGroup.Size = new System.Drawing.Size(322, 61);
            utilGroup.TabIndex = 6;
            utilGroup.TabStop = false;
            utilGroup.Text = "Utils";
            // 
            // browserList
            // 
            browserList.FormattingEnabled = true;
            browserList.Items.AddRange(new object[] { "Firefox", "Chrome", "Edge" });
            browserList.Location = new System.Drawing.Point(195, 32);
            browserList.Name = "browserList";
            browserList.Size = new System.Drawing.Size(112, 23);
            browserList.TabIndex = 4;
            // 
            // useCookies
            // 
            useCookies.AutoSize = true;
            useCookies.Location = new System.Drawing.Point(102, 32);
            useCookies.Name = "useCookies";
            useCookies.Size = new System.Drawing.Size(90, 19);
            useCookies.TabIndex = 3;
            useCookies.Text = "Use Cookies";
            useCookies.UseVisualStyleBackColor = true;
            useCookies.CheckedChanged += useCookies_CheckedChanged;
            // 
            // generatePlaylist
            // 
            generatePlaylist.AutoSize = true;
            generatePlaylist.Location = new System.Drawing.Point(102, 13);
            generatePlaylist.Name = "generatePlaylist";
            generatePlaylist.Size = new System.Drawing.Size(95, 19);
            generatePlaylist.TabIndex = 2;
            generatePlaylist.Text = "Make Playlist";
            generatePlaylist.UseVisualStyleBackColor = true;
            generatePlaylist.CheckedChanged += generatePlaylist_CheckedChanged;
            // 
            // setOutputButton
            // 
            setOutputButton.Location = new System.Drawing.Point(6, 22);
            setOutputButton.Name = "setOutputButton";
            setOutputButton.Size = new System.Drawing.Size(90, 26);
            setOutputButton.TabIndex = 1;
            setOutputButton.Text = "Set Output";
            setOutputButton.UseVisualStyleBackColor = true;
            setOutputButton.Click += setOutputButton_Click;
            // 
            // updateYTDLP
            // 
            updateYTDLP.Location = new System.Drawing.Point(1355, 3);
            updateYTDLP.Name = "updateYTDLP";
            updateYTDLP.Size = new System.Drawing.Size(131, 43);
            updateYTDLP.TabIndex = 3;
            updateYTDLP.Text = "Update Tools";
            updateYTDLP.UseVisualStyleBackColor = true;
            updateYTDLP.Click += updateYTDLP_Click;
            // 
            // executeButton
            // 
            executeButton.Location = new System.Drawing.Point(277, 3);
            executeButton.Name = "executeButton";
            executeButton.Size = new System.Drawing.Size(131, 43);
            executeButton.TabIndex = 2;
            executeButton.Text = "Execute list";
            executeButton.UseVisualStyleBackColor = true;
            executeButton.Click += executeButton_Click;
            // 
            // generateCommand
            // 
            generateCommand.Location = new System.Drawing.Point(140, 3);
            generateCommand.Name = "generateCommand";
            generateCommand.Size = new System.Drawing.Size(131, 43);
            generateCommand.TabIndex = 1;
            generateCommand.Text = "Prep Download";
            generateCommand.UseVisualStyleBackColor = true;
            generateCommand.Click += generateCommand_Click;
            // 
            // loadList
            // 
            loadList.Location = new System.Drawing.Point(3, 3);
            loadList.Name = "loadList";
            loadList.Size = new System.Drawing.Size(131, 43);
            loadList.TabIndex = 0;
            loadList.Text = "Impot List";
            loadList.UseVisualStyleBackColor = true;
            loadList.Click += loadList_Click;
            // 
            // dlList
            // 
            dlList.Controls.Add(URLs);
            dlList.Dock = System.Windows.Forms.DockStyle.Bottom;
            dlList.Location = new System.Drawing.Point(0, 60);
            dlList.Name = "dlList";
            dlList.Size = new System.Drawing.Size(1489, 799);
            dlList.TabIndex = 5;
            dlList.TabStop = false;
            dlList.Text = "URLs";
            // 
            // URLs
            // 
            URLs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            URLs.Dock = System.Windows.Forms.DockStyle.Fill;
            URLs.Location = new System.Drawing.Point(3, 19);
            URLs.Name = "URLs";
            URLs.ReadOnly = true;
            URLs.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            URLs.Size = new System.Drawing.Size(1483, 777);
            URLs.TabIndex = 0;
            // 
            // readyLabel
            // 
            readyLabel.AutoSize = true;
            readyLabel.Location = new System.Drawing.Point(14, 877);
            readyLabel.Name = "readyLabel";
            readyLabel.Size = new System.Drawing.Size(45, 15);
            readyLabel.TabIndex = 1;
            readyLabel.Text = "Ready :";
            // 
            // readyStatus
            // 
            readyStatus.AutoSize = true;
            readyStatus.Location = new System.Drawing.Point(65, 877);
            readyStatus.Name = "readyStatus";
            readyStatus.Size = new System.Drawing.Size(0, 15);
            readyStatus.TabIndex = 2;
            // 
            // pathLabel
            // 
            pathLabel.AutoSize = true;
            pathLabel.Location = new System.Drawing.Point(152, 877);
            pathLabel.Name = "pathLabel";
            pathLabel.Size = new System.Drawing.Size(37, 15);
            pathLabel.TabIndex = 3;
            pathLabel.Text = "Path: ";
            // 
            // pathLabelValue
            // 
            pathLabelValue.AutoSize = true;
            pathLabelValue.Location = new System.Drawing.Point(195, 877);
            pathLabelValue.Name = "pathLabelValue";
            pathLabelValue.Size = new System.Drawing.Size(0, 15);
            pathLabelValue.TabIndex = 4;
            // 
            // label2
            // 
            totalCountLabel.AutoSize = true;
            totalCountLabel.Location = new System.Drawing.Point(1341, 877);
            totalCountLabel.Name = "totalCountLabel";
            totalCountLabel.Size = new System.Drawing.Size(77, 15);
            totalCountLabel.TabIndex = 6;
            totalCountLabel.Text = "Total Videos: ";
            // 
            // label1
            // 
            totalCount.AutoSize = true;
            totalCount.Location = new System.Drawing.Point(1424, 877);
            totalCount.Name = "totalCount";
            totalCount.Size = new System.Drawing.Size(77, 15);
            totalCount.TabIndex = 7;
            totalCount.Text = "";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1513, 897);
            Controls.Add(totalCount);
            Controls.Add(totalCountLabel);
            Controls.Add(pathLabelValue);
            Controls.Add(pathLabel);
            Controls.Add(readyStatus);
            Controls.Add(readyLabel);
            Controls.Add(ButtonBackground);
            Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
            Text = "GrittyEnergys\'s YLYL Downloader";
            ButtonBackground.ResumeLayout(false);
            utilGroup.ResumeLayout(false);
            utilGroup.PerformLayout();
            dlList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)URLs).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label totalCount;

        private System.Windows.Forms.Label totalCountLabel;

        #endregion

        private Panel ButtonBackground;
        private Button executeButton;
        private Button generateCommand;
        private Button loadList;
        private Button updateYTDLP;
        private System.Windows.Forms.GroupBox dlList;
        private System.Windows.Forms.DataGridView URLs;
        private Label readyLabel;
        private Label readyStatus;
        private Label pathLabel;
        private Label pathLabelValue;
        private GroupBox utilGroup;
        private Button setOutputButton;
        private CheckBox generatePlaylist;
        private CheckBox useCookies;
        private ComboBox browserList;
    }
}
