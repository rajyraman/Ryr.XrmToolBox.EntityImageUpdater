namespace Ryr.XrmToolBox.EntityImageUpdater
{
    partial class EntityImageUpdater
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntityImageUpdater));
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbLoadEntities = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbUpdateImages = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbFxbEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.gbEntities = new System.Windows.Forms.GroupBox();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.lbSearch = new System.Windows.Forms.Label();
            this.txtSearchEntity = new System.Windows.Forms.TextBox();
            this.lvEntities = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbSourceAndAttributes = new System.Windows.Forms.GroupBox();
            this.gbAttributes = new System.Windows.Forms.GroupBox();
            this.lvAttributes = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.scApiAndAttributes = new System.Windows.Forms.SplitContainer();
            this.gbLogoSource = new System.Windows.Forms.GroupBox();
            this.txtLogoFrom = new System.Windows.Forms.TextBox();
            this.lbLabelSource = new System.Windows.Forms.Label();
            this.rbFileSystem = new System.Windows.Forms.RadioButton();
            this.rbTwitter = new System.Windows.Forms.RadioButton();
            this.rbGravatar = new System.Windows.Forms.RadioButton();
            this.rbClearbit = new System.Windows.Forms.RadioButton();
            this.gbResults = new System.Windows.Forms.GroupBox();
            this.lvResults = new System.Windows.Forms.ListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.scSourceAndResults = new System.Windows.Forms.SplitContainer();
            this.tsMain.SuspendLayout();
            this.gbEntities.SuspendLayout();
            this.searchPanel.SuspendLayout();
            this.gbSourceAndAttributes.SuspendLayout();
            this.gbAttributes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scApiAndAttributes)).BeginInit();
            this.scApiAndAttributes.Panel1.SuspendLayout();
            this.scApiAndAttributes.SuspendLayout();
            this.gbLogoSource.SuspendLayout();
            this.gbResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scSourceAndResults)).BeginInit();
            this.scSourceAndResults.Panel1.SuspendLayout();
            this.scSourceAndResults.Panel2.SuspendLayout();
            this.scSourceAndResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.toolStripSeparator2,
            this.tsbLoadEntities,
            this.toolStripSeparator3,
            this.tsbUpdateImages,
            this.toolStripSeparator4,
            this.tsbFxbEdit,
            this.toolStripSeparator1});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(998, 25);
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.Image = ((System.Drawing.Image)(resources.GetObject("tsbClose.Image")));
            this.tsbClose.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(23, 22);
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbLoadEntities
            // 
            this.tsbLoadEntities.Image = ((System.Drawing.Image)(resources.GetObject("tsbLoadEntities.Image")));
            this.tsbLoadEntities.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbLoadEntities.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLoadEntities.Name = "tsbLoadEntities";
            this.tsbLoadEntities.Size = new System.Drawing.Size(94, 22);
            this.tsbLoadEntities.Text = "Load Entities";
            this.tsbLoadEntities.Click += new System.EventHandler(this.tsbLoadEntities_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbUpdateImages
            // 
            this.tsbUpdateImages.Enabled = false;
            this.tsbUpdateImages.Image = ((System.Drawing.Image)(resources.GetObject("tsbUpdateImages.Image")));
            this.tsbUpdateImages.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbUpdateImages.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUpdateImages.Name = "tsbUpdateImages";
            this.tsbUpdateImages.Size = new System.Drawing.Size(82, 22);
            this.tsbUpdateImages.Text = "Update All";
            this.tsbUpdateImages.Click += new System.EventHandler(this.tsbUpdateImages_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbFxbEdit
            // 
            this.tsbFxbEdit.Enabled = false;
            this.tsbFxbEdit.Image = ((System.Drawing.Image)(resources.GetObject("tsbFxbEdit.Image")));
            this.tsbFxbEdit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbFxbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFxbEdit.Name = "tsbFxbEdit";
            this.tsbFxbEdit.Size = new System.Drawing.Size(114, 22);
            this.tsbFxbEdit.Text = "Selective Update";
            this.tsbFxbEdit.Click += new System.EventHandler(this.tsbFxbEdit_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // gbEntities
            // 
            this.gbEntities.Controls.Add(this.searchPanel);
            this.gbEntities.Controls.Add(this.lvEntities);
            this.gbEntities.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbEntities.Enabled = false;
            this.gbEntities.Location = new System.Drawing.Point(0, 25);
            this.gbEntities.Name = "gbEntities";
            this.gbEntities.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.gbEntities.Size = new System.Drawing.Size(306, 575);
            this.gbEntities.TabIndex = 1;
            this.gbEntities.TabStop = false;
            this.gbEntities.Text = "Entities";
            // 
            // searchPanel
            // 
            this.searchPanel.Controls.Add(this.lbSearch);
            this.searchPanel.Controls.Add(this.txtSearchEntity);
            this.searchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchPanel.Location = new System.Drawing.Point(5, 18);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.searchPanel.Size = new System.Drawing.Size(296, 51);
            this.searchPanel.TabIndex = 3;
            // 
            // lbSearch
            // 
            this.lbSearch.AutoSize = true;
            this.lbSearch.Location = new System.Drawing.Point(9, 21);
            this.lbSearch.Name = "lbSearch";
            this.lbSearch.Size = new System.Drawing.Size(41, 13);
            this.lbSearch.TabIndex = 2;
            this.lbSearch.Text = "Search";
            // 
            // txtSearchEntity
            // 
            this.txtSearchEntity.Location = new System.Drawing.Point(52, 18);
            this.txtSearchEntity.Name = "txtSearchEntity";
            this.txtSearchEntity.Size = new System.Drawing.Size(236, 20);
            this.txtSearchEntity.TabIndex = 1;
            this.txtSearchEntity.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnSearchKeyUp);
            // 
            // lvEntities
            // 
            this.lvEntities.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvEntities.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvEntities.FullRowSelect = true;
            this.lvEntities.HideSelection = false;
            this.lvEntities.Location = new System.Drawing.Point(5, 75);
            this.lvEntities.MultiSelect = false;
            this.lvEntities.Name = "lvEntities";
            this.lvEntities.Size = new System.Drawing.Size(296, 495);
            this.lvEntities.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvEntities.TabIndex = 0;
            this.lvEntities.UseCompatibleStateImageBehavior = false;
            this.lvEntities.View = System.Windows.Forms.View.Details;
            this.lvEntities.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEntities_ColumnClick);
            this.lvEntities.SelectedIndexChanged += new System.EventHandler(this.lvEntities_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Display Name";
            this.columnHeader1.Width = 145;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Logical Name";
            this.columnHeader2.Width = 151;
            // 
            // gbSourceAndAttributes
            // 
            this.gbSourceAndAttributes.Controls.Add(this.gbAttributes);
            this.gbSourceAndAttributes.Controls.Add(this.scApiAndAttributes);
            this.gbSourceAndAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSourceAndAttributes.Location = new System.Drawing.Point(0, 0);
            this.gbSourceAndAttributes.Name = "gbSourceAndAttributes";
            this.gbSourceAndAttributes.Size = new System.Drawing.Size(692, 187);
            this.gbSourceAndAttributes.TabIndex = 2;
            this.gbSourceAndAttributes.TabStop = false;
            // 
            // gbAttributes
            // 
            this.gbAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAttributes.Controls.Add(this.lvAttributes);
            this.gbAttributes.Enabled = false;
            this.gbAttributes.Location = new System.Drawing.Point(230, 23);
            this.gbAttributes.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbAttributes.Name = "gbAttributes";
            this.gbAttributes.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.gbAttributes.Size = new System.Drawing.Size(457, 158);
            this.gbAttributes.TabIndex = 1;
            this.gbAttributes.TabStop = false;
            this.gbAttributes.Text = "Attribute to match";
            // 
            // lvAttributes
            // 
            this.lvAttributes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader5,
            this.columnHeader4});
            this.lvAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvAttributes.FullRowSelect = true;
            this.lvAttributes.HideSelection = false;
            this.lvAttributes.Location = new System.Drawing.Point(5, 18);
            this.lvAttributes.MultiSelect = false;
            this.lvAttributes.Name = "lvAttributes";
            this.lvAttributes.Size = new System.Drawing.Size(447, 135);
            this.lvAttributes.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvAttributes.TabIndex = 0;
            this.lvAttributes.UseCompatibleStateImageBehavior = false;
            this.lvAttributes.View = System.Windows.Forms.View.Details;
            this.lvAttributes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvAttributes_ColumnClick);
            this.lvAttributes.SelectedIndexChanged += new System.EventHandler(this.lvAttributes_SelectedIndexChanged);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Attribute Name";
            this.columnHeader3.Width = 217;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Schema Name";
            this.columnHeader5.Width = 228;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Description";
            this.columnHeader4.Width = 367;
            // 
            // scApiAndAttributes
            // 
            this.scApiAndAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scApiAndAttributes.Location = new System.Drawing.Point(3, 16);
            this.scApiAndAttributes.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.scApiAndAttributes.Name = "scApiAndAttributes";
            // 
            // scApiAndAttributes.Panel1
            // 
            this.scApiAndAttributes.Panel1.Controls.Add(this.gbLogoSource);
            this.scApiAndAttributes.Size = new System.Drawing.Size(686, 168);
            this.scApiAndAttributes.SplitterDistance = 218;
            this.scApiAndAttributes.SplitterWidth = 2;
            this.scApiAndAttributes.TabIndex = 1;
            // 
            // gbLogoSource
            // 
            this.gbLogoSource.Controls.Add(this.txtLogoFrom);
            this.gbLogoSource.Controls.Add(this.lbLabelSource);
            this.gbLogoSource.Controls.Add(this.rbFileSystem);
            this.gbLogoSource.Controls.Add(this.rbTwitter);
            this.gbLogoSource.Controls.Add(this.rbGravatar);
            this.gbLogoSource.Controls.Add(this.rbClearbit);
            this.gbLogoSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLogoSource.Enabled = false;
            this.gbLogoSource.Location = new System.Drawing.Point(0, 0);
            this.gbLogoSource.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbLogoSource.Name = "gbLogoSource";
            this.gbLogoSource.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbLogoSource.Size = new System.Drawing.Size(218, 168);
            this.gbLogoSource.TabIndex = 1;
            this.gbLogoSource.TabStop = false;
            this.gbLogoSource.Text = "Logo For";
            // 
            // txtLogoFrom
            // 
            this.txtLogoFrom.Location = new System.Drawing.Point(10, 119);
            this.txtLogoFrom.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtLogoFrom.Name = "txtLogoFrom";
            this.txtLogoFrom.ReadOnly = true;
            this.txtLogoFrom.Size = new System.Drawing.Size(207, 20);
            this.txtLogoFrom.TabIndex = 5;
            // 
            // lbLabelSource
            // 
            this.lbLabelSource.AutoSize = true;
            this.lbLabelSource.Location = new System.Drawing.Point(10, 90);
            this.lbLabelSource.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbLabelSource.Name = "lbLabelSource";
            this.lbLabelSource.Size = new System.Drawing.Size(98, 13);
            this.lbLabelSource.TabIndex = 4;
            this.lbLabelSource.Text = "Logo retrieved from";
            // 
            // rbFileSystem
            // 
            this.rbFileSystem.AutoSize = true;
            this.rbFileSystem.Location = new System.Drawing.Point(10, 60);
            this.rbFileSystem.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbFileSystem.Name = "rbFileSystem";
            this.rbFileSystem.Size = new System.Drawing.Size(83, 17);
            this.rbFileSystem.TabIndex = 3;
            this.rbFileSystem.TabStop = true;
            this.rbFileSystem.Tag = "Folder";
            this.rbFileSystem.Text = "Local Folder";
            this.rbFileSystem.UseVisualStyleBackColor = true;
            this.rbFileSystem.CheckedChanged += new System.EventHandler(this.LogoSource_Selected);
            // 
            // rbTwitter
            // 
            this.rbTwitter.AutoSize = true;
            this.rbTwitter.Location = new System.Drawing.Point(145, 28);
            this.rbTwitter.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbTwitter.Name = "rbTwitter";
            this.rbTwitter.Size = new System.Drawing.Size(57, 17);
            this.rbTwitter.TabIndex = 2;
            this.rbTwitter.TabStop = true;
            this.rbTwitter.Tag = "Twitter";
            this.rbTwitter.Text = "Twitter";
            this.rbTwitter.UseVisualStyleBackColor = true;
            this.rbTwitter.CheckedChanged += new System.EventHandler(this.LogoSource_Selected);
            // 
            // rbGravatar
            // 
            this.rbGravatar.AutoSize = true;
            this.rbGravatar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbGravatar.Location = new System.Drawing.Point(76, 28);
            this.rbGravatar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbGravatar.Name = "rbGravatar";
            this.rbGravatar.Size = new System.Drawing.Size(50, 17);
            this.rbGravatar.TabIndex = 1;
            this.rbGravatar.TabStop = true;
            this.rbGravatar.Tag = "Gravatar";
            this.rbGravatar.Text = "Email";
            this.rbGravatar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rbGravatar.UseVisualStyleBackColor = true;
            this.rbGravatar.CheckedChanged += new System.EventHandler(this.LogoSource_Selected);
            // 
            // rbClearbit
            // 
            this.rbClearbit.AutoSize = true;
            this.rbClearbit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbClearbit.Location = new System.Drawing.Point(10, 28);
            this.rbClearbit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbClearbit.Name = "rbClearbit";
            this.rbClearbit.Size = new System.Drawing.Size(47, 17);
            this.rbClearbit.TabIndex = 0;
            this.rbClearbit.TabStop = true;
            this.rbClearbit.Tag = "Clearbit";
            this.rbClearbit.Text = "URL";
            this.rbClearbit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rbClearbit.UseVisualStyleBackColor = true;
            this.rbClearbit.CheckedChanged += new System.EventHandler(this.LogoSource_Selected);
            // 
            // gbResults
            // 
            this.gbResults.Controls.Add(this.lvResults);
            this.gbResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbResults.Location = new System.Drawing.Point(0, 0);
            this.gbResults.Name = "gbResults";
            this.gbResults.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.gbResults.Size = new System.Drawing.Size(692, 386);
            this.gbResults.TabIndex = 3;
            this.gbResults.TabStop = false;
            this.gbResults.Text = "Results";
            // 
            // lvResults
            // 
            this.lvResults.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8});
            this.lvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvResults.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvResults.HideSelection = false;
            this.lvResults.Location = new System.Drawing.Point(5, 18);
            this.lvResults.MultiSelect = false;
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(682, 363);
            this.lvResults.TabIndex = 0;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.View = System.Windows.Forms.View.SmallIcon;
            this.lvResults.SelectedIndexChanged += new System.EventHandler(this.lvResults_SelectedIndexChanged);
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Url";
            this.columnHeader7.Width = 176;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Logo";
            this.columnHeader8.Width = 134;
            // 
            // scSourceAndResults
            // 
            this.scSourceAndResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scSourceAndResults.IsSplitterFixed = true;
            this.scSourceAndResults.Location = new System.Drawing.Point(306, 25);
            this.scSourceAndResults.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.scSourceAndResults.Name = "scSourceAndResults";
            this.scSourceAndResults.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scSourceAndResults.Panel1
            // 
            this.scSourceAndResults.Panel1.Controls.Add(this.gbSourceAndAttributes);
            // 
            // scSourceAndResults.Panel2
            // 
            this.scSourceAndResults.Panel2.Controls.Add(this.gbResults);
            this.scSourceAndResults.Size = new System.Drawing.Size(692, 575);
            this.scSourceAndResults.SplitterDistance = 187;
            this.scSourceAndResults.SplitterWidth = 2;
            this.scSourceAndResults.TabIndex = 4;
            // 
            // EntityImageUpdater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scSourceAndResults);
            this.Controls.Add(this.gbEntities);
            this.Controls.Add(this.tsMain);
            this.Name = "EntityImageUpdater";
            this.Size = new System.Drawing.Size(998, 600);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.gbEntities.ResumeLayout(false);
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            this.gbSourceAndAttributes.ResumeLayout(false);
            this.gbAttributes.ResumeLayout(false);
            this.scApiAndAttributes.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scApiAndAttributes)).EndInit();
            this.scApiAndAttributes.ResumeLayout(false);
            this.gbLogoSource.ResumeLayout(false);
            this.gbLogoSource.PerformLayout();
            this.gbResults.ResumeLayout(false);
            this.scSourceAndResults.Panel1.ResumeLayout(false);
            this.scSourceAndResults.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scSourceAndResults)).EndInit();
            this.scSourceAndResults.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripButton tsbLoadEntities;
        private System.Windows.Forms.ToolStripButton tsbUpdateImages;
        private System.Windows.Forms.GroupBox gbEntities;
        private System.Windows.Forms.GroupBox gbSourceAndAttributes;
        private System.Windows.Forms.GroupBox gbResults;
        private System.Windows.Forms.ListView lvEntities;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListView lvAttributes;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ListView lvResults;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.Label lbSearch;
        private System.Windows.Forms.TextBox txtSearchEntity;
        private System.Windows.Forms.SplitContainer scSourceAndResults;
        private System.Windows.Forms.GroupBox gbAttributes;
        private System.Windows.Forms.SplitContainer scApiAndAttributes;
        private System.Windows.Forms.GroupBox gbLogoSource;
        private System.Windows.Forms.RadioButton rbFileSystem;
        private System.Windows.Forms.RadioButton rbTwitter;
        private System.Windows.Forms.RadioButton rbGravatar;
        private System.Windows.Forms.RadioButton rbClearbit;
        private System.Windows.Forms.ToolStripButton tsbFxbEdit;
        private System.Windows.Forms.TextBox txtLogoFrom;
        private System.Windows.Forms.Label lbLabelSource;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}
