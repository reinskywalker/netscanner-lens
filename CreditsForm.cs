namespace netscan_lens
{
    public partial class CreditsForm : Form
    {
        public CreditsForm()
        {
            InitializeComponent();
        }

        private string SetName(string name, string studentID)
        {
            return $"{name}\nStudent ID: {studentID}\n\n";
        }

        private void InitializeComponent()
        {
            lbl = new Label();
            SuspendLayout();

            lbl.AutoSize = false;
            lbl.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            lbl.Location = new Point(20, 20);
            lbl.Name = "lbl";
            lbl.Size = new Size(360,360 );
            lbl.TabIndex = 0;
            lbl.TextAlign = ContentAlignment.TopCenter;


            var sb = new System.Text.StringBuilder();
            sb.Append(SetName("none", "none"));

            lbl.Text = sb.ToString();

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 320);
            Controls.Add(lbl);
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Credits";

            ResumeLayout(false);
        }

        private Label lbl;
    }
}
