namespace RXReport2HTML
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
            this.panelDropZone = new System.Windows.Forms.Panel();
            this.lblDropInstruction = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panelDropZone
            // 
            this.panelDropZone.AllowDrop = true;
            this.panelDropZone.BackColor = System.Drawing.Color.LightGray;
            this.panelDropZone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDropZone.Location = new System.Drawing.Point(20, 20);
            this.panelDropZone.Name = "panelDropZone";
            this.panelDropZone.Size = new System.Drawing.Size(760, 150);
            this.panelDropZone.TabIndex = 0;
            this.panelDropZone.DragDrop += new System.Windows.Forms.DragEventHandler(this.panelDropZone_DragDrop);
            this.panelDropZone.DragEnter += new System.Windows.Forms.DragEventHandler(this.panelDropZone_DragEnter);
            // 
            // lblDropInstruction
            // 
            this.lblDropInstruction.AutoSize = true;
            this.lblDropInstruction.BackColor = System.Drawing.Color.Transparent;
            this.lblDropInstruction.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDropInstruction.Location = new System.Drawing.Point(250, 80);
            this.lblDropInstruction.Name = "lblDropInstruction";
            this.lblDropInstruction.Size = new System.Drawing.Size(300, 21);
            this.lblDropInstruction.TabIndex = 1;
            this.lblDropInstruction.Text = "Drag and Drop .rxlog file here";
            this.panelDropZone.Controls.Add(this.lblDropInstruction);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(20, 190);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(650, 23);
            this.txtFilePath.TabIndex = 2;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(680, 190);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(100, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Enabled = false;
            this.btnGenerate.Location = new System.Drawing.Point(20, 230);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(760, 35);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "Generate HTML Report";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(20, 280);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(760, 23);
            this.progressBar.TabIndex = 5;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(20, 315);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 15);
            this.lblStatus.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 350);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.panelDropZone);
            this.Name = "Form1";
            this.Text = "Ranorex Report Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelDropZone;
        private System.Windows.Forms.Label lblDropInstruction;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
    }
}
