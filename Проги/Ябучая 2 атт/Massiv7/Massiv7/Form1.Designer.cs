﻿namespace Massiv7
{
  partial class frmMass7
  {
    /// <summary>
    /// Требуется переменная конструктора.
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
    /// Обязательный метод для поддержки конструктора - не изменяйте
    /// содержимое данного метода при помощи редактора кода.
    /// </summary>
    private void InitializeComponent()
    {
      this.lblTask = new System.Windows.Forms.Label();
      this.btnTorf = new System.Windows.Forms.Button();
      this.lblAns = new System.Windows.Forms.Label();
      this.lblMass = new System.Windows.Forms.Label();
      this.gv = new System.Windows.Forms.DataGridView();
      ((System.ComponentModel.ISupportInitialize)(this.gv)).BeginInit();
      this.SuspendLayout();
      // 
      // lblTask
      // 
      this.lblTask.Location = new System.Drawing.Point(12, 9);
      this.lblTask.Name = "lblTask";
      this.lblTask.Size = new System.Drawing.Size(259, 68);
      this.lblTask.TabIndex = 0;
      this.lblTask.Text = "7.\tПеременной t присвоить значение true, если в массиве нет нулевых элементов и п" +
          "ри этом положительные элементы чередуются с отрицательными и значение false в пр" +
          "отивном случае.";
      // 
      // btnTorf
      // 
      this.btnTorf.Location = new System.Drawing.Point(15, 317);
      this.btnTorf.Name = "btnTorf";
      this.btnTorf.Size = new System.Drawing.Size(160, 34);
      this.btnTorf.TabIndex = 1;
      this.btnTorf.Text = "Присвоить значение";
      this.btnTorf.UseVisualStyleBackColor = true;
      this.btnTorf.Click += new System.EventHandler(this.btnTorf_Click);
      // 
      // lblAns
      // 
      this.lblAns.AutoSize = true;
      this.lblAns.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.lblAns.Location = new System.Drawing.Point(181, 326);
      this.lblAns.Name = "lblAns";
      this.lblAns.Size = new System.Drawing.Size(57, 16);
      this.lblAns.TabIndex = 6;
      this.lblAns.Text = "Ответ:";
      // 
      // lblMass
      // 
      this.lblMass.Location = new System.Drawing.Point(181, 170);
      this.lblMass.Name = "lblMass";
      this.lblMass.Size = new System.Drawing.Size(75, 48);
      this.lblMass.TabIndex = 7;
      this.lblMass.Text = "Введите массив элементов";
      // 
      // gv
      // 
      this.gv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.gv.Location = new System.Drawing.Point(15, 80);
      this.gv.MultiSelect = false;
      this.gv.Name = "gv";
      this.gv.RowHeadersVisible = false;
      this.gv.RowHeadersWidth = 15;
      this.gv.Size = new System.Drawing.Size(160, 231);
      this.gv.TabIndex = 9;
      // 
      // frmMass7
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(291, 359);
      this.Controls.Add(this.gv);
      this.Controls.Add(this.lblMass);
      this.Controls.Add(this.lblAns);
      this.Controls.Add(this.btnTorf);
      this.Controls.Add(this.lblTask);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Name = "frmMass7";
      this.Text = "Добрый день! (Массивы - 7)";
      ((System.ComponentModel.ISupportInitialize)(this.gv)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblTask;
    private System.Windows.Forms.Button btnTorf;
    private System.Windows.Forms.Label lblAns;
    private System.Windows.Forms.Label lblMass;
    private System.Windows.Forms.DataGridView gv;
  }
}

