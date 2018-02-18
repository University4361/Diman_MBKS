namespace Pick_Client_lab1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.PathTB = new System.Windows.Forms.TextBox();
            this.FileName = new System.Windows.Forms.TextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.Content = new System.Windows.Forms.TextBox();
            this.ToPublic = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PathTB
            // 
            this.PathTB.Location = new System.Drawing.Point(12, 12);
            this.PathTB.Name = "PathTB";
            this.PathTB.Size = new System.Drawing.Size(207, 20);
            this.PathTB.TabIndex = 0;
            // 
            // FileName
            // 
            this.FileName.Location = new System.Drawing.Point(71, 38);
            this.FileName.Name = "FileName";
            this.FileName.Size = new System.Drawing.Size(148, 20);
            this.FileName.TabIndex = 1;
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(225, 10);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(75, 23);
            this.Browse.TabIndex = 2;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // Content
            // 
            this.Content.Location = new System.Drawing.Point(12, 64);
            this.Content.Multiline = true;
            this.Content.Name = "Content";
            this.Content.Size = new System.Drawing.Size(207, 80);
            this.Content.TabIndex = 3;
            // 
            // ToPublic
            // 
            this.ToPublic.Location = new System.Drawing.Point(144, 150);
            this.ToPublic.Name = "ToPublic";
            this.ToPublic.Size = new System.Drawing.Size(75, 23);
            this.ToPublic.TabIndex = 4;
            this.ToPublic.Text = "To Public";
            this.ToPublic.UseVisualStyleBackColor = true;
            this.ToPublic.Click += new System.EventHandler(this.ToPublic_Click);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(12, 150);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 5;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "File name:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 186);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.ToPublic);
            this.Controls.Add(this.Content);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.FileName);
            this.Controls.Add(this.PathTB);
            this.Name = "Form1";
            this.Text = "Client file worker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox PathTB;
        private System.Windows.Forms.TextBox FileName;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.TextBox Content;
        private System.Windows.Forms.Button ToPublic;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Label label1;
    }
}

