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
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Location = new System.Drawing.Point(123, 11);
            this.zedGraphControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
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
            this.pointTestCount.Location = new System.Drawing.Point(1231, 32);
            this.pointTestCount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pointTestCount.Name = "pointTestCount";
            this.pointTestCount.Size = new System.Drawing.Size(129, 22);
            this.pointTestCount.TabIndex = 3;
            this.pointTestCount.Text = "500";
            this.pointTestCount.TextChanged += new System.EventHandler(this.pointTestCount_TextChanged);
            // 
            // firstTask
            // 
            this.firstTask.Location = new System.Drawing.Point(12, 11);
            this.firstTask.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.firstTask.Name = "firstTask";
            this.firstTask.Size = new System.Drawing.Size(103, 23);
            this.firstTask.TabIndex = 4;
            this.firstTask.Text = "Первая";
            this.firstTask.UseVisualStyleBackColor = true;
            this.firstTask.Click += new System.EventHandler(this.firstTask_Click);
            // 
            // secondTask
            // 
            this.secondTask.Location = new System.Drawing.Point(12, 55);
            this.secondTask.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.secondTask.Name = "secondTask";
            this.secondTask.Size = new System.Drawing.Size(103, 23);
            this.secondTask.TabIndex = 5;
            this.secondTask.Text = "Вторая";
            this.secondTask.UseVisualStyleBackColor = true;
            this.secondTask.Click += new System.EventHandler(this.secondTask_Click);
            // 
            // thirdTask
            // 
            this.thirdTask.Location = new System.Drawing.Point(12, 100);
            this.thirdTask.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.thirdTask.Name = "thirdTask";
            this.thirdTask.Size = new System.Drawing.Size(103, 23);
            this.thirdTask.TabIndex = 6;
            this.thirdTask.Text = "Третья";
            this.thirdTask.UseVisualStyleBackColor = true;
            this.thirdTask.Click += new System.EventHandler(this.thirdTask_Click);
            // 
            // fourthTask
            // 
            this.fourthTask.Location = new System.Drawing.Point(12, 151);
            this.fourthTask.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fourthTask.Name = "fourthTask";
            this.fourthTask.Size = new System.Drawing.Size(103, 23);
            this.fourthTask.TabIndex = 7;
            this.fourthTask.Text = "Четвёртая";
            this.fourthTask.UseVisualStyleBackColor = true;
            this.fourthTask.Click += new System.EventHandler(this.fourthTask_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1228, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Кол-во точек:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1232, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Кол-во точек, лежащих внутри:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // inPointsCountLabel
            // 
            this.inPointsCountLabel.AutoSize = true;
            this.inPointsCountLabel.Location = new System.Drawing.Point(1235, 100);
            this.inPointsCountLabel.Name = "inPointsCountLabel";
            this.inPointsCountLabel.Size = new System.Drawing.Size(14, 16);
            this.inPointsCountLabel.TabIndex = 10;
            this.inPointsCountLabel.Text = "0";
            this.inPointsCountLabel.Click += new System.EventHandler(this.inPointsCountLabel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1228, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "Приближённая площадь:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // areaLabel
            // 
            this.areaLabel.AutoSize = true;
            this.areaLabel.Location = new System.Drawing.Point(1235, 158);
            this.areaLabel.Name = "areaLabel";
            this.areaLabel.Size = new System.Drawing.Size(14, 16);
            this.areaLabel.TabIndex = 12;
            this.areaLabel.Text = "0";
            this.areaLabel.Click += new System.EventHandler(this.areaLabel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1232, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 16);
            this.label4.TabIndex = 13;
            this.label4.Text = "Погрешность:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // absoluteErrorLabel
            // 
            this.absoluteErrorLabel.AutoSize = true;
            this.absoluteErrorLabel.Location = new System.Drawing.Point(1235, 220);
            this.absoluteErrorLabel.Name = "absoluteErrorLabel";
            this.absoluteErrorLabel.Size = new System.Drawing.Size(14, 16);
            this.absoluteErrorLabel.TabIndex = 14;
            this.absoluteErrorLabel.Text = "0";
            this.absoluteErrorLabel.Click += new System.EventHandler(this.absoluteErrorLabel_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1232, 254);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 16);
            this.label5.TabIndex = 15;
            this.label5.Text = "Пи";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1235, 283);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 16);
            this.label6.TabIndex = 16;
            this.label6.Text = "label6";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1443, 820);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
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
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DoubleClick += new System.EventHandler(this.Form1_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}

