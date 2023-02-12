namespace LR2
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
            this.components = new System.ComponentModel.Container();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.pointTestCount = new System.Windows.Forms.TextBox();
            this.firstTask = new System.Windows.Forms.Button();
            this.secondTask = new System.Windows.Forms.Button();
            this.thirdTask = new System.Windows.Forms.Button();
            this.fourthTask = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.inPointsCountLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.areaLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.absoluteErrorLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.relativeErrorLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Location = new System.Drawing.Point(-1, -2);
            this.zedGraphControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(1097, 602);
            this.zedGraphControl1.TabIndex = 0;
            this.zedGraphControl1.UseExtendedPrintDialog = true;
            // 
            // pointTestCount
            // 
            this.pointTestCount.Location = new System.Drawing.Point(1099, 28);
            this.pointTestCount.Name = "pointTestCount";
            this.pointTestCount.Size = new System.Drawing.Size(130, 22);
            this.pointTestCount.TabIndex = 3;
            // 
            // firstTask
            // 
            this.firstTask.Location = new System.Drawing.Point(28, 607);
            this.firstTask.Name = "firstTask";
            this.firstTask.Size = new System.Drawing.Size(75, 23);
            this.firstTask.TabIndex = 4;
            this.firstTask.Text = "firstTask";
            this.firstTask.UseVisualStyleBackColor = true;
            this.firstTask.Click += new System.EventHandler(this.firstTask_Click);
            // 
            // secondTask
            // 
            this.secondTask.Location = new System.Drawing.Point(119, 607);
            this.secondTask.Name = "secondTask";
            this.secondTask.Size = new System.Drawing.Size(103, 23);
            this.secondTask.TabIndex = 5;
            this.secondTask.Text = "secondTask";
            this.secondTask.UseVisualStyleBackColor = true;
            this.secondTask.Click += new System.EventHandler(this.secondTask_Click);
            // 
            // thirdTask
            // 
            this.thirdTask.Location = new System.Drawing.Point(240, 607);
            this.thirdTask.Name = "thirdTask";
            this.thirdTask.Size = new System.Drawing.Size(103, 23);
            this.thirdTask.TabIndex = 6;
            this.thirdTask.Text = "thirdTask";
            this.thirdTask.UseVisualStyleBackColor = true;
            this.thirdTask.Click += new System.EventHandler(this.thirdTask_Click);
            // 
            // fourthTask
            // 
            this.fourthTask.Location = new System.Drawing.Point(365, 607);
            this.fourthTask.Name = "fourthTask";
            this.fourthTask.Size = new System.Drawing.Size(103, 23);
            this.fourthTask.TabIndex = 7;
            this.fourthTask.Text = "fourthTask";
            this.fourthTask.UseVisualStyleBackColor = true;
            this.fourthTask.Click += new System.EventHandler(this.fourthTask_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1100, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Количество точек:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1104, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(294, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Количество точек, лежащих внутри фигуры:";
            // 
            // inPointsCountLabel
            // 
            this.inPointsCountLabel.AutoSize = true;
            this.inPointsCountLabel.Location = new System.Drawing.Point(1107, 95);
            this.inPointsCountLabel.Name = "inPointsCountLabel";
            this.inPointsCountLabel.Size = new System.Drawing.Size(14, 16);
            this.inPointsCountLabel.TabIndex = 10;
            this.inPointsCountLabel.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1100, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = " Площадь (приближённо):";
            // 
            // areaLabel
            // 
            this.areaLabel.AutoSize = true;
            this.areaLabel.Location = new System.Drawing.Point(1107, 152);
            this.areaLabel.Name = "areaLabel";
            this.areaLabel.Size = new System.Drawing.Size(14, 16);
            this.areaLabel.TabIndex = 12;
            this.areaLabel.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1104, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(177, 16);
            this.label4.TabIndex = 13;
            this.label4.Text = "Абсолютная погрешность:";
            // 
            // absoluteErrorLabel
            // 
            this.absoluteErrorLabel.AutoSize = true;
            this.absoluteErrorLabel.Location = new System.Drawing.Point(1107, 215);
            this.absoluteErrorLabel.Name = "absoluteErrorLabel";
            this.absoluteErrorLabel.Size = new System.Drawing.Size(14, 16);
            this.absoluteErrorLabel.TabIndex = 14;
            this.absoluteErrorLabel.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1104, 245);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(198, 16);
            this.label6.TabIndex = 15;
            this.label6.Text = "Относительная погрешность:";
            // 
            // relativeErrorLabel
            // 
            this.relativeErrorLabel.AutoSize = true;
            this.relativeErrorLabel.Location = new System.Drawing.Point(1107, 274);
            this.relativeErrorLabel.Name = "relativeErrorLabel";
            this.relativeErrorLabel.Size = new System.Drawing.Size(14, 16);
            this.relativeErrorLabel.TabIndex = 16;
            this.relativeErrorLabel.Text = "0";
            this.relativeErrorLabel.Click += new System.EventHandler(this.label7_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1442, 820);
            this.Controls.Add(this.relativeErrorLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.absoluteErrorLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.areaLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.inPointsCountLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fourthTask);
            this.Controls.Add(this.thirdTask);
            this.Controls.Add(this.secondTask);
            this.Controls.Add(this.firstTask);
            this.Controls.Add(this.pointTestCount);
            this.Controls.Add(this.zedGraphControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.TextBox pointTestCount;
        private System.Windows.Forms.Button firstTask;
        private System.Windows.Forms.Button secondTask;
        private System.Windows.Forms.Button thirdTask;
        private System.Windows.Forms.Button fourthTask;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label inPointsCountLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label areaLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label absoluteErrorLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label relativeErrorLabel;
    }
}

