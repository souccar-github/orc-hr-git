namespace Souccar.NHibernateManager
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
            this.butDrop = new System.Windows.Forms.Button();
            this.butCreate = new System.Windows.Forms.Button();
            this.butUpdate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // butDrop
            // 
            this.butDrop.Location = new System.Drawing.Point(12, 12);
            this.butDrop.Name = "butDrop";
            this.butDrop.Size = new System.Drawing.Size(75, 23);
            this.butDrop.TabIndex = 0;
            this.butDrop.Text = "Drop";
            this.butDrop.UseVisualStyleBackColor = true;
            this.butDrop.Click += new System.EventHandler(this.butDrop_Click);
            // 
            // butCreate
            // 
            this.butCreate.Location = new System.Drawing.Point(12, 41);
            this.butCreate.Name = "butCreate";
            this.butCreate.Size = new System.Drawing.Size(75, 23);
            this.butCreate.TabIndex = 0;
            this.butCreate.Text = "Create";
            this.butCreate.UseVisualStyleBackColor = true;
            this.butCreate.Click += new System.EventHandler(this.butCreate_Click);
            // 
            // butUpdate
            // 
            this.butUpdate.Location = new System.Drawing.Point(12, 70);
            this.butUpdate.Name = "butUpdate";
            this.butUpdate.Size = new System.Drawing.Size(75, 23);
            this.butUpdate.TabIndex = 1;
            this.butUpdate.Text = "Update";
            this.butUpdate.UseVisualStyleBackColor = true;
            this.butUpdate.Click += new System.EventHandler(this.butUpdate_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.butUpdate);
            this.Controls.Add(this.butDrop);
            this.Controls.Add(this.butCreate);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button butDrop;
        private System.Windows.Forms.Button butCreate;
        private System.Windows.Forms.Button butUpdate;
    }
}

