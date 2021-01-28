using System.Diagnostics;
// ReSharper disable CheckNamespace

namespace System.Windows.Forms
{
	/// <summary>
	/// Used by InputBox.Show().
	/// </summary>
	internal sealed class InputBoxDialog : Form 
	{
		private Label _lblPrompt;
		public TextBox TxtInput;
		private Button _btnOk;
		private Button _btnCancel;
	
		public InputBoxDialog(string prompt, string title) : this(prompt, title, int.MinValue, int.MinValue) {} 

 		public InputBoxDialog(string prompt, string title, int xPos, int yPos)
		{
			if (xPos != int.MinValue && yPos != int.MinValue) {
				StartPosition = FormStartPosition.Manual;
				Location = new Drawing.Point(xPos, yPos);
			}

			InitializeComponent();

			_lblPrompt.Text = prompt;
			Text = title;

			var g = CreateGraphics();
			var size = g.MeasureString(prompt, _lblPrompt.Font, _lblPrompt.Width);
			Debug.WriteLine("PROMPT SIZE: " + size);
			if (size.Height > _lblPrompt.Height)
				Height += (int)size.Height - _lblPrompt.Height;

			TxtInput.SelectionStart = 0;
			TxtInput.SelectionLength = TxtInput.Text.Length;
			TxtInput.Focus();
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._lblPrompt = new System.Windows.Forms.Label();
			this.TxtInput = new System.Windows.Forms.TextBox();
			this._btnOk = new System.Windows.Forms.Button();
			this._btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblPrompt
			// 
			this._lblPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this._lblPrompt.BackColor = System.Drawing.SystemColors.Control;
			this._lblPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._lblPrompt.Location = new System.Drawing.Point(12, 9);
			this._lblPrompt.Name = "_lblPrompt";
			this._lblPrompt.Size = new System.Drawing.Size(302, 71);
			this._lblPrompt.TabIndex = 3;
			// 
			// txtInput
			// 
			this.TxtInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.TxtInput.Location = new System.Drawing.Point(8, 88);
			this.TxtInput.Name = "TxtInput";
			this.TxtInput.Size = new System.Drawing.Size(381, 20);
			this.TxtInput.TabIndex = 0;
			this.TxtInput.Text = "";
			// 
			// btnOK
			// 
			this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._btnOk.Location = new System.Drawing.Point(326, 8);
			this._btnOk.Name = "_btnOk";
			this._btnOk.Size = new System.Drawing.Size(64, 24);
			this._btnOk.TabIndex = 1;
			this._btnOk.Text = "&OK";
			// 
			// btnCancel
			// 
			this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._btnCancel.Location = new System.Drawing.Point(326, 40);
			this._btnCancel.Name = "_btnCancel";
			this._btnCancel.Size = new System.Drawing.Size(64, 24);
			this._btnCancel.TabIndex = 2;
			this._btnCancel.Text = "&Cancel";
			// 
			// InputBoxDialog
			// 
			this.AcceptButton = this._btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this._btnCancel;
			this.ClientSize = new System.Drawing.Size(398, 117);
			this.Controls.Add(this.TxtInput);
			this.Controls.Add(this._btnCancel);
			this.Controls.Add(this._btnOk);
			this.Controls.Add(this._lblPrompt);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InputBoxDialog";
			this.ResumeLayout(false);

		}
		#endregion
	}

	/// <summary>
	/// This static class contains methods named Show() to display a dialog box 
	/// with an input field, similar in appearance to the one in Visual Basic.
	/// The Show() method returns null if the user clicks Cancel, and non-null
	/// if the user clicks OK.
	/// </summary>
	public static class InputBox
	{
        /*
        public static string Show(string prompt, string title, string @default)
			{ return Show(prompt, title, @default, int.MinValue); }
			*/
		
		public static string Show(string prompt, string title = null, string @default = null, int xPos = int.MinValue, int yPos = int.MinValue, bool isPassword = false)
		{
			title ??= Application.ProductName;
			var dlg = new InputBoxDialog(prompt, title, xPos, yPos);
			if (isPassword)
				dlg.TxtInput.UseSystemPasswordChar = true;
			if (@default != null)
				dlg.TxtInput.Text = @default;
			var result = dlg.ShowDialog();
			return result == DialogResult.Cancel ? null : dlg.TxtInput.Text;
		}
		public static string ShowPasswordBox(string prompt, string title)
			{ return Show(prompt, title, "", int.MinValue, int.MinValue, true); }
	}
}
