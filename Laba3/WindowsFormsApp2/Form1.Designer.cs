namespace WindowsFormsApp2
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.Generated = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.Result = new System.Windows.Forms.Button();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Группа 1 ",
            "Группа 2",
            "Группа 3",
            "Группа 4"});
            this.comboBox1.Location = new System.Drawing.Point(12, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(232, 24);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.Text = "Группа текстовых данных";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Группа 1 ",
            "Группа 2",
            "Группа 3"});
            this.comboBox2.Location = new System.Drawing.Point(12, 39);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(232, 24);
            this.comboBox2.TabIndex = 1;
            this.comboBox2.Text = "Группа алгоритмов сортировки";
            // 
            // Generated
            // 
            this.Generated.Location = new System.Drawing.Point(12, 66);
            this.Generated.Name = "Generated";
            this.Generated.Size = new System.Drawing.Size(128, 41);
            this.Generated.TabIndex = 2;
            this.Generated.Text = "Сгенерировать массивы";
            this.Generated.UseVisualStyleBackColor = true;
            this.Generated.Click += new System.EventHandler(this.Generated_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(146, 66);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(98, 41);
            this.button3.TabIndex = 3;
            this.button3.Text = "Запустить тесты";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Result
            // 
            this.Result.Location = new System.Drawing.Point(13, 113);
            this.Result.Name = "Result";
            this.Result.Size = new System.Drawing.Size(231, 23);
            this.Result.TabIndex = 4;
            this.Result.Text = "Сохранить результаты";
            this.Result.UseVisualStyleBackColor = true;
            this.Result.Click += new System.EventHandler(this.button4_Click);
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.IsEnableHPan = false;
            this.zedGraphControl1.Location = new System.Drawing.Point(267, 12);
            this.zedGraphControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(688, 398);
            this.zedGraphControl1.TabIndex = 5;
            this.zedGraphControl1.UseExtendedPrintDialog = true;
            this.zedGraphControl1.Load += new System.EventHandler(this.zedGraphControl1_Load);
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(968, 423);
            this.Controls.Add(this.zedGraphControl1);
            this.Controls.Add(this.Result);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.Generated);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button Generated;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button Result;
        private ZedGraph.ZedGraphControl zedGraphControl1;
    }
}

