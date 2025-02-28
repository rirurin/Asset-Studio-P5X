using AssetStudio.P5X;
using AssetStudio.P5X.ConfDataBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace AssetStudioGUI
{
    public partial class P5XFieldSelector : Form
    {
        private List<DatabaseLocation> DBTables;
        private string DBFolderName;
        private ConnectionManager DBConnections;

        private ListView FieldList;
        private ColumnHeader Column1;
        private ColumnHeader Column2;
        public P5XFieldSelector()
        {
            InitializeComponent();

            /*
            // Column1
            Column1 = new ColumnHeader();
            Column1.Text = "ID";
            Column1.Width = 200;
            // Column2
            Column2 = new ColumnHeader();
            Column2.Text = "Name";
            Column2.Width = 100;
            */

            // FieldList
            FieldList = new ListView();
            //FieldList.Columns.AddRange(new ColumnHeader[] { Column1, Column2 });
            FieldList.Dock = DockStyle.Fill;
            FieldList.FullRowSelect = true;
            FieldList.GridLines = true;
            FieldList.Location = new Point(0, 23);
            FieldList.Name = "FieldList";
            FieldList.Size = new Size(472, 584);
            FieldList.TabIndex = 0;
            FieldList.View = View.Details;
            FieldList.VirtualMode = false;

            Controls.Add(FieldList);

            this.FormClosing += P5XFieldSelector_FormClosing;
            this.Icon = Properties.Resources._as;
        }

        private void P5XFieldSelector_FormClosing(object sender, FormClosingEventArgs e)
        {
            // close DB connections
            DBConnections.CloseConnections();
        }

        public void LoadFields(string folderName)
        {
            DBFolderName = folderName;
            DBTables = new List<DatabaseLocation>();
            var ConfigDBMap = new XmlDocument();
            ConfigDBMap.Load($"{DBFolderName}\\ConfigDBMap.bytes");
            // First child is a single <xml> tag to define UTF-8 encoding
            // We already know that, so skip to the second node
            // <xml> -> Sibling -> s<ConfigDBMapRecord> -> Child -> <Map>
            var rootNode = ConfigDBMap.FirstChild.NextSibling.FirstChild;
            for (int i = 0; i < rootNode.ChildNodes.Count; i++)
                DBTables.Add(new DatabaseLocation(rootNode.ChildNodes[i]));
            // Make connection to ConfDataBasic
            DBConnections = new ConnectionManager(DBFolderName);
            var confDataBasic = DBConnections.GetDatabase("ConfDataBasic");
            var scenes = confDataBasic.GetEntries<ConfScene>();
            // Create Columns for FieldList based on schema of ConfScene
            var columnNames = Utility.GetColumnNames<ConfScene>();
            ColumnHeader[] columns = new ColumnHeader[columnNames.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = new ColumnHeader();
                columns[i].Text = columnNames[i];
                columns[i].Width = -2;
                columns[i].TextAlign = HorizontalAlignment.Left;
            }
            FieldList.Columns.AddRange(columns);
            // Fill ConfScene columns
            foreach (var scene in scenes)
            {
                ListViewItem sceneItem = new ListViewItem(scene.sn.ToString());
                sceneItem.SubItems.AddRange(scene.AsStrings(1)); // ID, name
                FieldList.Items.Add(sceneItem);
            }
        }
    }
}
