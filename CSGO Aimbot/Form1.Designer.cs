namespace CSGO_Aimbot
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
            aimbot_checkbox = new CheckBox();
            smoothing = new NumericUpDown();
            label1 = new Label();
            team_checkbox = new CheckBox();
            targetBoneBox = new ComboBox();
            label2 = new Label();
            visible_checkbox = new CheckBox();
            label3 = new Label();
            label4 = new Label();
            keybox = new ComboBox();
            boxespbox = new CheckBox();
            teamespbox = new CheckBox();
            tracersespbox = new CheckBox();
            aimbotfovd = new NumericUpDown();
            skelespbox = new CheckBox();
            bunnyhopbox = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)smoothing).BeginInit();
            ((System.ComponentModel.ISupportInitialize)aimbotfovd).BeginInit();
            SuspendLayout();
            // 
            // aimbot_checkbox
            // 
            aimbot_checkbox.AutoSize = true;
            aimbot_checkbox.Location = new Point(43, 23);
            aimbot_checkbox.Name = "aimbot_checkbox";
            aimbot_checkbox.Size = new Size(64, 19);
            aimbot_checkbox.TabIndex = 0;
            aimbot_checkbox.Text = "aimbot";
            aimbot_checkbox.UseVisualStyleBackColor = true;
            // 
            // smoothing
            // 
            smoothing.Location = new Point(115, 99);
            smoothing.Name = "smoothing";
            smoothing.Size = new Size(49, 23);
            smoothing.TabIndex = 1;
            smoothing.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(39, 102);
            label1.Name = "label1";
            label1.Size = new Size(68, 15);
            label1.TabIndex = 2;
            label1.Text = "smoothing:";
            // 
            // team_checkbox
            // 
            team_checkbox.AutoSize = true;
            team_checkbox.Checked = true;
            team_checkbox.CheckState = CheckState.Checked;
            team_checkbox.Location = new Point(43, 131);
            team_checkbox.Name = "team_checkbox";
            team_checkbox.Size = new Size(87, 19);
            team_checkbox.TabIndex = 3;
            team_checkbox.Text = "team check";
            team_checkbox.UseVisualStyleBackColor = true;
            // 
            // targetBoneBox
            // 
            targetBoneBox.FormattingEnabled = true;
            targetBoneBox.Items.AddRange(new object[] { "head", "neck", "chest", "stomach", "pelvis" });
            targetBoneBox.Location = new Point(87, 195);
            targetBoneBox.Name = "targetBoneBox";
            targetBoneBox.Size = new Size(93, 23);
            targetBoneBox.TabIndex = 4;
            targetBoneBox.Text = "head";
            targetBoneBox.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(41, 198);
            label2.Name = "label2";
            label2.Size = new Size(40, 15);
            label2.TabIndex = 5;
            label2.Text = "bone: ";
            // 
            // visible_checkbox
            // 
            visible_checkbox.AutoSize = true;
            visible_checkbox.Location = new Point(41, 161);
            visible_checkbox.Name = "visible_checkbox";
            visible_checkbox.Size = new Size(93, 19);
            visible_checkbox.TabIndex = 6;
            visible_checkbox.Text = "visible check";
            visible_checkbox.UseVisualStyleBackColor = true;
            visible_checkbox.CheckedChanged += visible_checkbox_CheckedChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(43, 57);
            label3.Name = "label3";
            label3.Size = new Size(27, 15);
            label3.TabIndex = 8;
            label3.Text = "fov:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(41, 227);
            label4.Name = "label4";
            label4.Size = new Size(31, 15);
            label4.TabIndex = 10;
            label4.Text = "key: ";
            // 
            // keybox
            // 
            keybox.FormattingEnabled = true;
            keybox.Items.AddRange(new object[] { "mouse1", "mouse2", "mouse3", "mouse4", "mouse5", "shift" });
            keybox.Location = new Point(87, 224);
            keybox.Name = "keybox";
            keybox.Size = new Size(93, 23);
            keybox.TabIndex = 9;
            keybox.Text = "mouse1";
            keybox.SelectedIndexChanged += keybox_SelectedIndexChanged;
            // 
            // boxespbox
            // 
            boxespbox.AutoSize = true;
            boxespbox.Location = new Point(39, 273);
            boxespbox.Name = "boxespbox";
            boxespbox.Size = new Size(67, 19);
            boxespbox.TabIndex = 11;
            boxespbox.Text = "box esp";
            boxespbox.UseVisualStyleBackColor = true;
            boxespbox.CheckedChanged += boxespbox_CheckedChanged;
            // 
            // teamespbox
            // 
            teamespbox.AutoSize = true;
            teamespbox.Location = new Point(39, 325);
            teamespbox.Name = "teamespbox";
            teamespbox.Size = new Size(74, 19);
            teamespbox.TabIndex = 12;
            teamespbox.Text = "team esp";
            teamespbox.UseVisualStyleBackColor = true;
            teamespbox.CheckedChanged += teamespbox_CheckedChanged;
            // 
            // tracersespbox
            // 
            tracersespbox.AutoSize = true;
            tracersespbox.Location = new Point(39, 350);
            tracersespbox.Name = "tracersespbox";
            tracersespbox.Size = new Size(61, 19);
            tracersespbox.TabIndex = 13;
            tracersespbox.Text = "tracers";
            tracersespbox.UseVisualStyleBackColor = true;
            tracersespbox.CheckedChanged += tracersespbox_CheckedChanged;
            // 
            // aimbotfovd
            // 
            aimbotfovd.Location = new Point(87, 57);
            aimbotfovd.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            aimbotfovd.Name = "aimbotfovd";
            aimbotfovd.Size = new Size(49, 23);
            aimbotfovd.TabIndex = 14;
            aimbotfovd.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // skelespbox
            // 
            skelespbox.AutoSize = true;
            skelespbox.Location = new Point(37, 301);
            skelespbox.Name = "skelespbox";
            skelespbox.Size = new Size(172, 19);
            skelespbox.TabIndex = 15;
            skelespbox.Text = "skeleton esp (coming soon)";
            skelespbox.UseVisualStyleBackColor = true;
            skelespbox.CheckedChanged += skelespbox_CheckedChanged;
            // 
            // bunnyhopbox
            // 
            bunnyhopbox.AutoSize = true;
            bunnyhopbox.Location = new Point(37, 380);
            bunnyhopbox.Name = "bunnyhopbox";
            bunnyhopbox.Size = new Size(81, 19);
            bunnyhopbox.TabIndex = 16;
            bunnyhopbox.Text = "bunnyhop";
            bunnyhopbox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(241, 416);
            Controls.Add(bunnyhopbox);
            Controls.Add(skelespbox);
            Controls.Add(aimbotfovd);
            Controls.Add(tracersespbox);
            Controls.Add(teamespbox);
            Controls.Add(boxespbox);
            Controls.Add(label4);
            Controls.Add(keybox);
            Controls.Add(label3);
            Controls.Add(visible_checkbox);
            Controls.Add(label2);
            Controls.Add(targetBoneBox);
            Controls.Add(team_checkbox);
            Controls.Add(label1);
            Controls.Add(smoothing);
            Controls.Add(aimbot_checkbox);
            Name = "Form1";
            Text = "CSGO Hack";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)smoothing).EndInit();
            ((System.ComponentModel.ISupportInitialize)aimbotfovd).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private CheckBox aimbot_checkbox;
        private NumericUpDown smoothing;
        private Label label1;
        private CheckBox team_checkbox;
        private ComboBox targetBoneBox;
        private Label label2;
        private CheckBox visible_checkbox;
        private Label label3;
        private Label label4;
        private ComboBox keybox;
        private CheckBox boxespbox;
        private CheckBox teamespbox;
        private CheckBox tracersespbox;
        private NumericUpDown aimbotfovd;
        private CheckBox skelespbox;
        private CheckBox bunnyhopbox;
        private Label label5;
    }
}