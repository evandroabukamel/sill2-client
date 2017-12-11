namespace sill2_client
{
	partial class sill2_client_form
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(sill2_client_form));
			this.label1 = new System.Windows.Forms.Label();
			this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblComputador = new System.Windows.Forms.Label();
			this.lblUsuario = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Malgun Gothic", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.label1.Location = new System.Drawing.Point(83, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(184, 41);
			this.label1.TabIndex = 0;
			this.label1.Text = "SILL2 Client";
			// 
			// trayIcon
			// 
			this.trayIcon.Text = "notifyIcon";
			this.trayIcon.Visible = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.label2.Location = new System.Drawing.Point(12, 116);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(86, 25);
			this.label2.TabIndex = 1;
			this.label2.Text = "Usuário:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.label3.Location = new System.Drawing.Point(12, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(131, 25);
			this.label3.TabIndex = 2;
			this.label3.Text = "Computador:";
			// 
			// lblComputador
			// 
			this.lblComputador.AutoSize = true;
			this.lblComputador.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblComputador.ForeColor = System.Drawing.SystemColors.WindowText;
			this.lblComputador.Location = new System.Drawing.Point(149, 72);
			this.lblComputador.Name = "lblComputador";
			this.lblComputador.Size = new System.Drawing.Size(27, 25);
			this.lblComputador.TabIndex = 3;
			this.lblComputador.Text = "...";
			// 
			// lblUsuario
			// 
			this.lblUsuario.AutoSize = true;
			this.lblUsuario.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblUsuario.ForeColor = System.Drawing.SystemColors.WindowText;
			this.lblUsuario.Location = new System.Drawing.Point(104, 116);
			this.lblUsuario.Name = "lblUsuario";
			this.lblUsuario.Size = new System.Drawing.Size(27, 25);
			this.lblUsuario.TabIndex = 4;
			this.lblUsuario.Text = "...";
			// 
			// sill2_client_form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(366, 168);
			this.Controls.Add(this.lblUsuario);
			this.Controls.Add(this.lblComputador);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "sill2_client_form";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SILL2 Client";
			this.Load += new System.EventHandler(this.sill2_client_form_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.Label lblComputador;
		public System.Windows.Forms.Label lblUsuario;
		public System.Windows.Forms.NotifyIcon trayIcon;
	}
}