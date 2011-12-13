using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SharpChess
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmDifficulty : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox grpClock;
		private System.Windows.Forms.Label lblClockMovesIn;
		private System.Windows.Forms.Label lblClockMinutes;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox grpLevel;
		private System.Windows.Forms.TrackBar trkLevel;
		private System.Windows.Forms.Label lblLevel;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RadioButton radLevel;
		private System.Windows.Forms.RadioButton radCustom;
		private System.Windows.Forms.GroupBox grpCustom;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblMaximumSeconds;
		private System.Windows.Forms.NumericUpDown numMoves;
		private System.Windows.Forms.NumericUpDown numMinutes;
		private System.Windows.Forms.Label lblAverageSeconds;
		private System.Windows.Forms.NumericUpDown numMaximumSearchDepth;
		private System.Windows.Forms.CheckBox chkRestrictSearchDepth;
		private System.Windows.Forms.CheckBox chkEnablePondering;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox chkUseRandomOpeningMoves;

		private bool m_blnConfirmed = false;

		public frmDifficulty()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public bool Confirmed
		{
			get { return m_blnConfirmed; }
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmDifficulty));
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.chkEnablePondering = new System.Windows.Forms.CheckBox();
			this.grpClock = new System.Windows.Forms.GroupBox();
			this.lblMaximumSeconds = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.numMoves = new System.Windows.Forms.NumericUpDown();
			this.numMinutes = new System.Windows.Forms.NumericUpDown();
			this.lblAverageSeconds = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblClockMinutes = new System.Windows.Forms.Label();
			this.lblClockMovesIn = new System.Windows.Forms.Label();
			this.grpLevel = new System.Windows.Forms.GroupBox();
			this.trkLevel = new System.Windows.Forms.TrackBar();
			this.lblLevel = new System.Windows.Forms.Label();
			this.radLevel = new System.Windows.Forms.RadioButton();
			this.grpCustom = new System.Windows.Forms.GroupBox();
			this.chkUseRandomOpeningMoves = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.numMaximumSearchDepth = new System.Windows.Forms.NumericUpDown();
			this.chkRestrictSearchDepth = new System.Windows.Forms.CheckBox();
			this.radCustom = new System.Windows.Forms.RadioButton();
			this.grpClock.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMoves)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numMinutes)).BeginInit();
			this.grpLevel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkLevel)).BeginInit();
			this.grpCustom.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMaximumSearchDepth)).BeginInit();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(288, 352);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(368, 352);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// chkEnablePondering
			// 
			this.chkEnablePondering.Location = new System.Drawing.Point(16, 160);
			this.chkEnablePondering.Name = "chkEnablePondering";
			this.chkEnablePondering.Size = new System.Drawing.Size(296, 16);
			this.chkEnablePondering.TabIndex = 4;
			this.chkEnablePondering.Text = "&Enable Pondering (Thinking during other player\'s turn)";
			// 
			// grpClock
			// 
			this.grpClock.Controls.Add(this.lblMaximumSeconds);
			this.grpClock.Controls.Add(this.label4);
			this.grpClock.Controls.Add(this.numMoves);
			this.grpClock.Controls.Add(this.numMinutes);
			this.grpClock.Controls.Add(this.lblAverageSeconds);
			this.grpClock.Controls.Add(this.label2);
			this.grpClock.Controls.Add(this.label1);
			this.grpClock.Controls.Add(this.lblClockMinutes);
			this.grpClock.Controls.Add(this.lblClockMovesIn);
			this.grpClock.Location = new System.Drawing.Point(16, 32);
			this.grpClock.Name = "grpClock";
			this.grpClock.Size = new System.Drawing.Size(392, 80);
			this.grpClock.TabIndex = 4;
			this.grpClock.TabStop = false;
			this.grpClock.Text = "Thinking Time";
			// 
			// lblMaximumSeconds
			// 
			this.lblMaximumSeconds.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblMaximumSeconds.Location = new System.Drawing.Point(216, 48);
			this.lblMaximumSeconds.Name = "lblMaximumSeconds";
			this.lblMaximumSeconds.Size = new System.Drawing.Size(32, 20);
			this.lblMaximumSeconds.TabIndex = 16;
			this.lblMaximumSeconds.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label4
			// 
			this.label4.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.label4.Location = new System.Drawing.Point(264, 48);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(120, 16);
			this.label4.TabIndex = 15;
			this.label4.Text = "maximum secs / move";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numMoves
			// 
			this.numMoves.Location = new System.Drawing.Point(8, 24);
			this.numMoves.Maximum = new System.Decimal(new int[] {
																	 999,
																	 0,
																	 0,
																	 0});
			this.numMoves.Minimum = new System.Decimal(new int[] {
																	 1,
																	 0,
																	 0,
																	 0});
			this.numMoves.Name = "numMoves";
			this.numMoves.Size = new System.Drawing.Size(40, 20);
			this.numMoves.TabIndex = 10;
			this.numMoves.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numMoves.Value = new System.Decimal(new int[] {
																   1,
																   0,
																   0,
																   0});
			this.numMoves.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numMoves_KeyUp);
			this.numMoves.ValueChanged += new System.EventHandler(this.numMoves_ValueChanged);
			// 
			// numMinutes
			// 
			this.numMinutes.Location = new System.Drawing.Point(104, 24);
			this.numMinutes.Maximum = new System.Decimal(new int[] {
																	   999,
																	   0,
																	   0,
																	   0});
			this.numMinutes.Minimum = new System.Decimal(new int[] {
																	   1,
																	   0,
																	   0,
																	   0});
			this.numMinutes.Name = "numMinutes";
			this.numMinutes.Size = new System.Drawing.Size(40, 20);
			this.numMinutes.TabIndex = 14;
			this.numMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numMinutes.Value = new System.Decimal(new int[] {
																	 1,
																	 0,
																	 0,
																	 0});
			this.numMinutes.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numMinutes_KeyUp);
			this.numMinutes.ValueChanged += new System.EventHandler(this.numMinutes_ValueChanged);
			// 
			// lblAverageSeconds
			// 
			this.lblAverageSeconds.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblAverageSeconds.Location = new System.Drawing.Point(216, 24);
			this.lblAverageSeconds.Name = "lblAverageSeconds";
			this.lblAverageSeconds.Size = new System.Drawing.Size(32, 20);
			this.lblAverageSeconds.TabIndex = 13;
			this.lblAverageSeconds.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.label2.Location = new System.Drawing.Point(264, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 16);
			this.label2.TabIndex = 12;
			this.label2.Text = "average secs / move";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.label1.Location = new System.Drawing.Point(192, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(8, 16);
			this.label1.TabIndex = 10;
			this.label1.Text = "=";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblClockMinutes
			// 
			this.lblClockMinutes.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.lblClockMinutes.Location = new System.Drawing.Point(144, 24);
			this.lblClockMinutes.Name = "lblClockMinutes";
			this.lblClockMinutes.Size = new System.Drawing.Size(48, 16);
			this.lblClockMinutes.TabIndex = 9;
			this.lblClockMinutes.Text = "minutes";
			this.lblClockMinutes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblClockMovesIn
			// 
			this.lblClockMovesIn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.lblClockMovesIn.Location = new System.Drawing.Point(40, 24);
			this.lblClockMovesIn.Name = "lblClockMovesIn";
			this.lblClockMovesIn.Size = new System.Drawing.Size(64, 16);
			this.lblClockMovesIn.TabIndex = 7;
			this.lblClockMovesIn.Text = "moves in";
			this.lblClockMovesIn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// grpLevel
			// 
			this.grpLevel.Controls.Add(this.trkLevel);
			this.grpLevel.Location = new System.Drawing.Point(16, 24);
			this.grpLevel.Name = "grpLevel";
			this.grpLevel.Size = new System.Drawing.Size(424, 72);
			this.grpLevel.TabIndex = 6;
			this.grpLevel.TabStop = false;
			// 
			// trkLevel
			// 
			this.trkLevel.LargeChange = 1;
			this.trkLevel.Location = new System.Drawing.Point(8, 24);
			this.trkLevel.Maximum = 16;
			this.trkLevel.Minimum = 1;
			this.trkLevel.Name = "trkLevel";
			this.trkLevel.Size = new System.Drawing.Size(408, 45);
			this.trkLevel.TabIndex = 6;
			this.trkLevel.Value = 1;
			this.trkLevel.Scroll += new System.EventHandler(this.trkLevel_Scroll);
			// 
			// lblLevel
			// 
			this.lblLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblLevel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblLevel.Location = new System.Drawing.Point(160, 16);
			this.lblLevel.Name = "lblLevel";
			this.lblLevel.Size = new System.Drawing.Size(32, 24);
			this.lblLevel.TabIndex = 7;
			this.lblLevel.Text = "16";
			this.lblLevel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// radLevel
			// 
			this.radLevel.Checked = true;
			this.radLevel.Location = new System.Drawing.Point(24, 16);
			this.radLevel.Name = "radLevel";
			this.radLevel.Size = new System.Drawing.Size(144, 24);
			this.radLevel.TabIndex = 15;
			this.radLevel.TabStop = true;
			this.radLevel.Text = "General Difficulty Level:";
			this.radLevel.CheckedChanged += new System.EventHandler(this.radLevel_CheckedChanged);
			// 
			// grpCustom
			// 
			this.grpCustom.Controls.Add(this.chkUseRandomOpeningMoves);
			this.grpCustom.Controls.Add(this.label5);
			this.grpCustom.Controls.Add(this.numMaximumSearchDepth);
			this.grpCustom.Controls.Add(this.chkRestrictSearchDepth);
			this.grpCustom.Controls.Add(this.grpClock);
			this.grpCustom.Controls.Add(this.chkEnablePondering);
			this.grpCustom.Enabled = false;
			this.grpCustom.Location = new System.Drawing.Point(16, 112);
			this.grpCustom.Name = "grpCustom";
			this.grpCustom.Size = new System.Drawing.Size(424, 224);
			this.grpCustom.TabIndex = 16;
			this.grpCustom.TabStop = false;
			// 
			// chkUseRandomOpeningMoves
			// 
			this.chkUseRandomOpeningMoves.Location = new System.Drawing.Point(16, 192);
			this.chkUseRandomOpeningMoves.Name = "chkUseRandomOpeningMoves";
			this.chkUseRandomOpeningMoves.Size = new System.Drawing.Size(352, 16);
			this.chkUseRandomOpeningMoves.TabIndex = 12;
			this.chkUseRandomOpeningMoves.Text = "&Use Random Opening Moves (Opening Book)";
			// 
			// label5
			// 
			this.label5.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.label5.Location = new System.Drawing.Point(192, 128);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(32, 16);
			this.label5.TabIndex = 11;
			this.label5.Text = "ply(s)";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numMaximumSearchDepth
			// 
			this.numMaximumSearchDepth.Location = new System.Drawing.Point(152, 128);
			this.numMaximumSearchDepth.Maximum = new System.Decimal(new int[] {
																				  32,
																				  0,
																				  0,
																				  0});
			this.numMaximumSearchDepth.Minimum = new System.Decimal(new int[] {
																				  1,
																				  0,
																				  0,
																				  0});
			this.numMaximumSearchDepth.Name = "numMaximumSearchDepth";
			this.numMaximumSearchDepth.Size = new System.Drawing.Size(40, 20);
			this.numMaximumSearchDepth.TabIndex = 9;
			this.numMaximumSearchDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.numMaximumSearchDepth.Value = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				0});
			// 
			// chkRestrictSearchDepth
			// 
			this.chkRestrictSearchDepth.Location = new System.Drawing.Point(16, 128);
			this.chkRestrictSearchDepth.Name = "chkRestrictSearchDepth";
			this.chkRestrictSearchDepth.Size = new System.Drawing.Size(144, 16);
			this.chkRestrictSearchDepth.TabIndex = 10;
			this.chkRestrictSearchDepth.Text = "Restrict search depth to";
			this.chkRestrictSearchDepth.CheckedChanged += new System.EventHandler(this.chkRestrictSearchDepth_CheckedChanged);
			// 
			// radCustom
			// 
			this.radCustom.Location = new System.Drawing.Point(24, 104);
			this.radCustom.Name = "radCustom";
			this.radCustom.Size = new System.Drawing.Size(64, 24);
			this.radCustom.TabIndex = 17;
			this.radCustom.Text = "Custom";
			this.radCustom.CheckedChanged += new System.EventHandler(this.radCustom_CheckedChanged);
			// 
			// frmDifficulty
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(458, 384);
			this.Controls.Add(this.lblLevel);
			this.Controls.Add(this.radLevel);
			this.Controls.Add(this.radCustom);
			this.Controls.Add(this.grpLevel);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.grpCustom);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmDifficulty";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Game Difficulty Settings";
			this.Load += new System.EventHandler(this.frmDifficulty_Load);
			this.grpClock.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numMoves)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numMinutes)).EndInit();
			this.grpLevel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.trkLevel)).EndInit();
			this.grpCustom.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numMaximumSearchDepth)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void frmDifficulty_Load(object sender, System.EventArgs e)
		{
			m_blnConfirmed = false;

			this.radLevel.Checked = Game.DifficultyLevel > 0;
			if (radLevel.Checked)
			{
				trkLevel.Value = Game.DifficultyLevel;
			}
			this.radCustom.Checked = Game.DifficultyLevel == 0;
			this.numMoves.Value = Math.Max(Game.ClockMoves,1);
			this.numMinutes.Value = Convert.ToDecimal(Math.Max(Math.Round(Game.ClockTime.TotalMinutes,0),1));
			this.chkRestrictSearchDepth.Checked = Game.MaximumSearchDepth>0;
			this.numMaximumSearchDepth.Value = Math.Max(Game.MaximumSearchDepth,1);
			this.chkEnablePondering.Checked = Game.EnablePondering;
			this.chkUseRandomOpeningMoves.Checked = Game.UseRandomOpeningMoves;
			SetFormState();
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{

			Game.DifficultyLevel = this.radLevel.Checked ? this.trkLevel.Value : 0;
			Game.EnablePondering  = this.chkEnablePondering.Checked;
			Game.ClockMoves = (int)this.numMoves.Value;
			Game.ClockTime = new TimeSpan(0, (int)this.numMinutes.Value, 0);
			Game.ClockIncrementPerMove = new TimeSpan(0, 0, 0);
			Game.ClockFixedTimePerMove = new TimeSpan(0, 0, 0);
			Game.MaximumSearchDepth = this.chkRestrictSearchDepth.Checked ? (int)this.numMaximumSearchDepth.Value : 0;
			Game.EnablePondering = this.chkEnablePondering.Checked;
			Game.UseRandomOpeningMoves = this.chkUseRandomOpeningMoves.Checked;

			m_blnConfirmed = true;

			this.Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void trkLevel_Scroll(object sender, System.EventArgs e)
		{
			SetGeneralDifficulty();
		}

		private void radLevel_CheckedChanged(object sender, System.EventArgs e)
		{
			SetFormState();
			SetGeneralDifficulty();
		}

		private void radCustom_CheckedChanged(object sender, System.EventArgs e)
		{
			SetFormState();
		}

		private void SetFormState()
		{
			grpLevel.Enabled = radLevel.Checked;
			grpCustom.Enabled = radCustom.Checked;
			trkLevel.Visible = grpLevel.Enabled;
			lblLevel.Visible = grpLevel.Enabled;
			numMaximumSearchDepth.Enabled = chkRestrictSearchDepth.Checked;
		}

		private void SetGeneralDifficulty()
		{
			lblLevel.Text = trkLevel.Value.ToString();
			switch (trkLevel.Value)
			{
				case 1:
					numMoves.Value = 120;
					numMinutes.Value = 2;
					chkRestrictSearchDepth.Checked = true;
					numMaximumSearchDepth.Value = 1;
					chkEnablePondering.Checked = false;
					break;

				case 2:
					numMoves.Value = 120;
					numMinutes.Value = 4;
					chkRestrictSearchDepth.Checked = true;
					numMaximumSearchDepth.Value = 2;
					chkEnablePondering.Checked = false;
					break;

				case 3:
					numMoves.Value = 120;
					numMinutes.Value = 6;
					chkRestrictSearchDepth.Checked = true;
					numMaximumSearchDepth.Value = 3;
					chkEnablePondering.Checked = false;
					break;

				case 4:
					numMoves.Value = 120;
					numMinutes.Value = 8;
					chkRestrictSearchDepth.Checked = true;
					numMaximumSearchDepth.Value = 4;
					chkEnablePondering.Checked = false;
					break;

				case 5:
					numMoves.Value = 120;
					numMinutes.Value = 10;
					chkRestrictSearchDepth.Checked = true;
					numMaximumSearchDepth.Value = 5;
					chkEnablePondering.Checked = false;
					break;

				case 6:
					numMoves.Value = 120;
					numMinutes.Value = 12;
					chkRestrictSearchDepth.Checked = true;
					numMaximumSearchDepth.Value = 6;
					chkEnablePondering.Checked = false;
					break;

				case 7:
					numMoves.Value = 120;
					numMinutes.Value = 14;
					chkRestrictSearchDepth.Checked = true;
					numMaximumSearchDepth.Value = 7;
					chkEnablePondering.Checked = true;
					break;

				case 8:
					numMoves.Value = 120;
					numMinutes.Value = 16;
					chkRestrictSearchDepth.Checked = true;
					numMaximumSearchDepth.Value = 8;
					chkEnablePondering.Checked = true;
					break;

				case 9:
					numMoves.Value = 120;
					numMinutes.Value = 20;
					chkRestrictSearchDepth.Checked = false;
					numMaximumSearchDepth.Value = 32;
					chkEnablePondering.Checked = false;
					break;

				case 10:
					numMoves.Value = 120;
					numMinutes.Value = 30;
					chkRestrictSearchDepth.Checked = false;
					numMaximumSearchDepth.Value = 32;
					chkEnablePondering.Checked = false;
					break;

				case 11:
					numMoves.Value = 120;
					numMinutes.Value = 40;
					chkRestrictSearchDepth.Checked = false;
					numMaximumSearchDepth.Value = 32;
					chkEnablePondering.Checked = false;
					break;

				case 12:
					numMoves.Value = 120;
					numMinutes.Value = 60;
					chkRestrictSearchDepth.Checked = false;
					numMaximumSearchDepth.Value = 32;
					chkEnablePondering.Checked = false;
					break;

				case 13:
					numMoves.Value = 120;
					numMinutes.Value = 120;
					chkRestrictSearchDepth.Checked = false;
					numMaximumSearchDepth.Value = 32;
					chkEnablePondering.Checked = false;
					break;

				case 14:
					numMoves.Value = 120;
					numMinutes.Value = 60;
					chkRestrictSearchDepth.Checked = false;
					numMaximumSearchDepth.Value = 32;
					chkEnablePondering.Checked = true;
					break;

				case 15:
					numMoves.Value = 120;
					numMinutes.Value = 120;
					chkRestrictSearchDepth.Checked = false;
					numMaximumSearchDepth.Value = 32;
					chkEnablePondering.Checked = true;
					break;

				case 16:
					numMoves.Value = 120;
					numMinutes.Value = 600;
					chkRestrictSearchDepth.Checked = false;
					numMaximumSearchDepth.Value = 32;
					chkEnablePondering.Checked = true;
					break;

			}
		}

		private void CalculateMoveTimes()
		{
			lblAverageSeconds.Text = Convert.ToInt32(Math.Max(numMinutes.Value*60 / numMoves.Value, 1)).ToString();
			lblMaximumSeconds.Text = (int.Parse(lblAverageSeconds.Text)*2).ToString();
			lblLevel.Text = trkLevel.Value.ToString();
		}

		private void numMoves_ValueChanged(object sender, System.EventArgs e)
		{
			CalculateMoveTimes();
		}

		private void numMinutes_ValueChanged(object sender, System.EventArgs e)
		{
			CalculateMoveTimes();
		}

		private void numMoves_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			CalculateMoveTimes();
		}

		private void numMinutes_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			CalculateMoveTimes();
		}

		private void chkRestrictSearchDepth_CheckedChanged(object sender, System.EventArgs e)
		{
			SetFormState();
		}
	}
}
