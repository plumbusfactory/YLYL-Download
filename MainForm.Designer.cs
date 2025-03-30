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
            updateYTDLP = new Button();
            executeButton = new Button();
            generateCommand = new Button();
            loadList = new Button();
            dlList = new GroupBox();
            URLs = new DataGridView();
            readyLabel = new Label();
            readyStatus = new Label();
            ButtonBackground.SuspendLayout();
            dlList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)URLs).BeginInit();
            SuspendLayout();
            // 
            // ButtonBackground
            // 
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
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1513, 897);
            Controls.Add(readyStatus);
            Controls.Add(readyLabel);
            Controls.Add(ButtonBackground);
            Name = "MainForm";
            Text = "YLYL Downloader";
            Load += Form1_Load;
            ButtonBackground.ResumeLayout(false);
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
    }
}
