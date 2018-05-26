namespace Sorting
{
    partial class ChooseGood
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
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.GoodName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DrugPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoodsNode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Untis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PuzzlePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.metroComboBox1 = new DMSkin.Metro.Controls.MetroComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 18F);
            this.label1.Location = new System.Drawing.Point(135, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择商品";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GoodName,
            this.DrugPrice,
            this.GoodsNode,
            this.Untis,
            this.PuzzlePrice});
            this.dataGridView1.Location = new System.Drawing.Point(45, 107);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(293, 150);
            this.dataGridView1.TabIndex = 1;
            // 
            // GoodName
            // 
            this.GoodName.HeaderText = "商品名称";
            this.GoodName.Name = "GoodName";
            this.GoodName.Width = 250;
            // 
            // DrugPrice
            // 
            this.DrugPrice.HeaderText = "商品价格";
            this.DrugPrice.Name = "DrugPrice";
            this.DrugPrice.Visible = false;
            // 
            // GoodsNode
            // 
            this.GoodsNode.HeaderText = "商品编号";
            this.GoodsNode.Name = "GoodsNode";
            this.GoodsNode.Visible = false;
            // 
            // Untis
            // 
            this.Untis.HeaderText = "物品单位";
            this.Untis.Name = "Untis";
            this.Untis.Visible = false;
            // 
            // PuzzlePrice
            // 
            this.PuzzlePrice.HeaderText = "拼团单价";
            this.PuzzlePrice.Name = "PuzzlePrice";
            this.PuzzlePrice.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(86, 271);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 32);
            this.button1.TabIndex = 2;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(211, 270);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 32);
            this.button2.TabIndex = 3;
            this.button2.Text = "确定";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // metroComboBox1
            // 
            this.metroComboBox1.DM_UseSelectable = true;
            this.metroComboBox1.FormattingEnabled = true;
            this.metroComboBox1.ItemHeight = 24;
            this.metroComboBox1.Location = new System.Drawing.Point(103, 52);
            this.metroComboBox1.Name = "metroComboBox1";
            this.metroComboBox1.Size = new System.Drawing.Size(167, 30);
            this.metroComboBox1.TabIndex = 4;
            this.metroComboBox1.SelectedIndexChanged += new System.EventHandler(this.metroComboBox1_SelectedIndexChanged);
            this.metroComboBox1.SelectionChangeCommitted += new System.EventHandler(this.metroComboBox1_SelectionChangeCommitted);
            // 
            // ChooseGood
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 320);
            this.Controls.Add(this.metroComboBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Name = "ChooseGood";
            this.Load += new System.EventHandler(this.ChooseGood_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DrugPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsNode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Untis;
        private System.Windows.Forms.DataGridViewTextBoxColumn PuzzlePrice;
        private DMSkin.Metro.Controls.MetroComboBox metroComboBox1;
    }
}