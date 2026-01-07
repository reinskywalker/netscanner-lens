namespace netscan_lens
{
    public partial class CreditsForm : Form
    {
        public CreditsForm()
        {
            InitializeComponent();
        }

        private string SetName(string name, string studentID) //reuse for credit name list
        {
            return $"{name}\nStudent ID: {studentID}\n\n";
        }

        private void InitializeComponent()
        {
            lbl = new Label();
            SuspendLayout();
            // 
            // lbl
            // 
            lbl.Font = new Font("Segoe UI", 11F);
            lbl.Location = new Point(20, 20);
            lbl.Name = "lbl";
            lbl.Size = new Size(360, 360);
            lbl.TabIndex = 0;
            lbl.TextAlign = ContentAlignment.TopCenter;
            lbl.Click += lbl_Click;
            // 
            // CreditsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 320);
            Controls.Add(lbl);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CreditsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Credits";
            ResumeLayout(false);
        }

        private Label lbl;

        private void lbl_Click(object sender, EventArgs e)
        {

        }
    }
}
