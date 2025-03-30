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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ButtonBackground = new Panel();
            utilGroup = new GroupBox();
            browserList = new ComboBox();
            useCookies = new CheckBox();
            generatePlaylist = new CheckBox();
            setOutputButton = new Button();
            updateYTDLP = new Button();
            executeButton = new Button();
            generateCommand = new Button();
            loadList = new Button();
            dlList = new GroupBox();
            URLs = new DataGridView();
            readyLabel = new Label();
            readyStatus = new Label();
            pathLabel = new Label();
            pathLabelValue = new Label();
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
            ButtonBackground.Location = new Point(12, 12);
            ButtonBackground.Name = "ButtonBackground";
            ButtonBackground.Size = new Size(1489, 859);
            ButtonBackground.TabIndex = 0;
            // 
            // utilGroup
            // 
            utilGroup.Controls.Add(browserList);
            utilGroup.Controls.Add(useCookies);
            utilGroup.Controls.Add(generatePlaylist);
            utilGroup.Controls.Add(setOutputButton);
            utilGroup.Location = new Point(414, 3);
            utilGroup.Name = "utilGroup";
            utilGroup.Size = new Size(322, 61);
            utilGroup.TabIndex = 6;
            utilGroup.TabStop = false;
            utilGroup.Text = "Utils";
            // 
            // browserList
            // 
            browserList.FormattingEnabled = true;
            browserList.Items.AddRange(new object[] { "Firefox", "Chrome", "Edge" });
            browserList.Location = new Point(195, 32);
            browserList.Name = "browserList";
            browserList.Size = new Size(112, 23);
            browserList.TabIndex = 4;
            // 
            // useCookies
            // 
            useCookies.AutoSize = true;
            useCookies.Location = new Point(102, 32);
            useCookies.Name = "useCookies";
            useCookies.Size = new Size(90, 19);
            useCookies.TabIndex = 3;
            useCookies.Text = "Use Cookies";
            useCookies.UseVisualStyleBackColor = true;
            useCookies.CheckedChanged += useCookies_CheckedChanged;
            // 
            // generatePlaylist
            // 
            generatePlaylist.AutoSize = true;
            generatePlaylist.Location = new Point(102, 13);
            generatePlaylist.Name = "generatePlaylist";
            generatePlaylist.Size = new Size(95, 19);
            generatePlaylist.TabIndex = 2;
            generatePlaylist.Text = "Make Playlist";
            generatePlaylist.UseVisualStyleBackColor = true;
            generatePlaylist.CheckedChanged += generatePlaylist_CheckedChanged;
            // 
            // setOutputButton
            // 
            setOutputButton.Location = new Point(6, 22);
            setOutputButton.Name = "setOutputButton";
            setOutputButton.Size = new Size(90, 26);
            setOutputButton.TabIndex = 1;
            setOutputButton.Text = "Set Output";
            setOutputButton.UseVisualStyleBackColor = true;
            setOutputButton.Click += setOutputButton_Click;
            // 
            // updateYTDLP
            // 
            updateYTDLP.Location = new Point(1355, 3);
            updateYTDLP.Name = "updateYTDLP";
            updateYTDLP.Size = new Size(131, 43);
            updateYTDLP.TabIndex = 3;
            updateYTDLP.Text = "Update Tools";
            updateYTDLP.UseVisualStyleBackColor = true;
            updateYTDLP.Click += updateYTDLP_Click;
            // 
            // executeButton
            // 
            executeButton.Location = new Point(277, 3);
            executeButton.Name = "executeButton";
            executeButton.Size = new Size(131, 43);
            executeButton.TabIndex = 2;
            executeButton.Text = "Execute list";
            executeButton.UseVisualStyleBackColor = true;
            executeButton.Click += executeButton_Click;
            // 
            // generateCommand
            // 
            generateCommand.Location = new Point(140, 3);
            generateCommand.Name = "generateCommand";
            generateCommand.Size = new Size(131, 43);
            generateCommand.TabIndex = 1;
            generateCommand.Text = "Prep Download";
            generateCommand.UseVisualStyleBackColor = true;
            generateCommand.Click += generateCommand_Click;
            // 
            // loadList
            // 
            loadList.Location = new Point(3, 3);
            loadList.Name = "loadList";
            loadList.Size = new Size(131, 43);
            loadList.TabIndex = 0;
            loadList.Text = "Impot List";
            loadList.UseVisualStyleBackColor = true;
            loadList.Click += loadList_Click;
            // 
            // dlList
            // 
            dlList.Controls.Add(URLs);
            dlList.Location = new Point(2, 57);
            dlList.Name = "dlList";
            dlList.Size = new Size(1487, 799);
            dlList.TabIndex = 5;
            dlList.TabStop = false;
            dlList.Text = "URLs";
            // 
            // URLs
            // 
            URLs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            URLs.Location = new Point(6, 22);
            URLs.Name = "URLs";
            URLs.Size = new Size(1475, 771);
            URLs.TabIndex = 0;
            // 
            // readyLabel
            // 
            readyLabel.AutoSize = true;
            readyLabel.Location = new Point(14, 877);
            readyLabel.Name = "readyLabel";
            readyLabel.Size = new Size(45, 15);
            readyLabel.TabIndex = 1;
            readyLabel.Text = "Ready :";
            // 
            // readyStatus
            // 
            readyStatus.AutoSize = true;
            readyStatus.Location = new Point(65, 877);
            readyStatus.Name = "readyStatus";
            readyStatus.Size = new Size(0, 15);
            readyStatus.TabIndex = 2;
            // 
            // pathLabel
            // 
            pathLabel.AutoSize = true;
            pathLabel.Location = new Point(152, 877);
            pathLabel.Name = "pathLabel";
            pathLabel.Size = new Size(37, 15);
            pathLabel.TabIndex = 3;
            pathLabel.Text = "Path: ";
            // 
            // pathLabelValue
            // 
            pathLabelValue.AutoSize = true;
            pathLabelValue.Location = new Point(195, 877);
            pathLabelValue.Name = "pathLabelValue";
            pathLabelValue.Size = new Size(0, 15);
            pathLabelValue.TabIndex = 4;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1513, 897);
            Controls.Add(pathLabelValue);
            Controls.Add(pathLabel);
            Controls.Add(readyStatus);
            Controls.Add(readyLabel);
            Controls.Add(ButtonBackground);
            Name = "MainForm";
            Text = "YLYL Downloader";
            Load += Form1_Load;
            ButtonBackground.ResumeLayout(false);
            utilGroup.ResumeLayout(false);
            utilGroup.PerformLayout();
            dlList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)URLs).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel ButtonBackground;
        private Button executeButton;
        private Button generateCommand;
        private Button loadList;
        private Button updateYTDLP;
        private GroupBox dlList;
        private DataGridView URLs;
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
