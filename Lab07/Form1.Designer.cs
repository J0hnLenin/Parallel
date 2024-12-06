namespace Lab07
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
            tabControl1 = new TabControl();
            pageTask1 = new TabPage();
            output1 = new TextBox();
            input1 = new TextBox();
            task1Button = new Button();
            pageTask2 = new TabPage();
            task2Button = new Button();
            pageTask3 = new TabPage();
            task3Button = new Button();
            output2 = new TextBox();
            input2 = new TextBox();
            tabControl1.SuspendLayout();
            pageTask1.SuspendLayout();
            pageTask2.SuspendLayout();
            pageTask3.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(pageTask1);
            tabControl1.Controls.Add(pageTask2);
            tabControl1.Controls.Add(pageTask3);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 450);
            tabControl1.TabIndex = 0;
            // 
            // pageTask1
            // 
            pageTask1.Controls.Add(output1);
            pageTask1.Controls.Add(input1);
            pageTask1.Controls.Add(task1Button);
            pageTask1.Location = new Point(4, 30);
            pageTask1.Name = "pageTask1";
            pageTask1.Padding = new Padding(3);
            pageTask1.Size = new Size(792, 416);
            pageTask1.TabIndex = 0;
            pageTask1.Text = "Task1";
            pageTask1.UseVisualStyleBackColor = true;
            // 
            // output1
            // 
            output1.Location = new Point(208, 74);
            output1.Multiline = true;
            output1.Name = "output1";
            output1.ReadOnly = true;
            output1.ScrollBars = ScrollBars.Vertical;
            output1.Size = new Size(576, 342);
            output1.TabIndex = 2;
            output1.TextChanged += output1_TextChanged;
            // 
            // input1
            // 
            input1.Location = new Point(8, 74);
            input1.Multiline = true;
            input1.Name = "input1";
            input1.Size = new Size(194, 342);
            input1.TabIndex = 1;
            input1.TextChanged += input1_TextChanged;
            // 
            // task1Button
            // 
            task1Button.Location = new Point(6, 6);
            task1Button.Name = "task1Button";
            task1Button.Size = new Size(90, 59);
            task1Button.TabIndex = 0;
            task1Button.Text = "Run";
            task1Button.UseVisualStyleBackColor = true;
            task1Button.Click += task1Button_Click;
            // 
            // pageTask2
            // 
            pageTask2.Controls.Add(output2);
            pageTask2.Controls.Add(input2);
            pageTask2.Controls.Add(task2Button);
            pageTask2.Location = new Point(4, 30);
            pageTask2.Name = "pageTask2";
            pageTask2.Padding = new Padding(3);
            pageTask2.Size = new Size(792, 416);
            pageTask2.TabIndex = 1;
            pageTask2.Text = "Task2";
            pageTask2.UseVisualStyleBackColor = true;
            // 
            // task2Button
            // 
            task2Button.Location = new Point(6, 6);
            task2Button.Name = "task2Button";
            task2Button.Size = new Size(90, 59);
            task2Button.TabIndex = 1;
            task2Button.Text = "Run";
            task2Button.UseVisualStyleBackColor = true;
            task2Button.Click += task2Button_Click;
            // 
            // pageTask3
            // 
            pageTask3.Controls.Add(task3Button);
            pageTask3.Location = new Point(4, 30);
            pageTask3.Name = "pageTask3";
            pageTask3.Size = new Size(792, 416);
            pageTask3.TabIndex = 2;
            pageTask3.Text = "Task3";
            pageTask3.UseVisualStyleBackColor = true;
            // 
            // task3Button
            // 
            task3Button.Location = new Point(6, 6);
            task3Button.Name = "task3Button";
            task3Button.Size = new Size(90, 59);
            task3Button.TabIndex = 2;
            task3Button.Text = "Run";
            task3Button.UseVisualStyleBackColor = true;
            // 
            // output2
            // 
            output2.Location = new Point(208, 74);
            output2.Multiline = true;
            output2.Name = "output2";
            output2.ReadOnly = true;
            output2.ScrollBars = ScrollBars.Vertical;
            output2.Size = new Size(576, 342);
            output2.TabIndex = 4;
            // 
            // input2
            // 
            input2.Location = new Point(8, 74);
            input2.Multiline = true;
            input2.Name = "input2";
            input2.Size = new Size(194, 342);
            input2.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Name = "Form1";
            tabControl1.ResumeLayout(false);
            pageTask1.ResumeLayout(false);
            pageTask1.PerformLayout();
            pageTask2.ResumeLayout(false);
            pageTask2.PerformLayout();
            pageTask3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage pageTask1;
        private TabPage pageTask2;
        private TabPage pageTask3;
        private Button task1Button;
        private Button task2Button;
        private Button task3Button;
        private TextBox input1;
        private TextBox output1;
        private TextBox output2;
        private TextBox input2;
    }
}
