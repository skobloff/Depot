using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syntax;
using Eto.Parse;
using Depot;

namespace DepotConcept
{
    public partial class DepotDemoForm : Form
    {
        internal string fileName;
        internal Depot.Depot depot;

        public DepotDemoForm()
        {
            InitializeComponent();
            depot = new Depot.Depot();
        }

        private void richTextBoxAbout_TextChanged(object sender, EventArgs e)
        {

        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            fileName = openFileDialog.FileName; 
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }

        private void toolStripButtonStart_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            var Grammar = new Syntax.Syntax();
            Grammar.EnableMatchEvents = true;

            var match = Grammar.Match(richTextBoxInput.Text);

            sb.Append(match.ErrorMessage);
            sb.Append("\n\n");
            sb.AppendLine(match.Empty.ToString());
            foreach (Match _match in match.Matches)
            {
                sb.AppendLine(_match.ToString());
                var namedic = _match["name"].Value;
                sb.AppendLine(namedic.ToString());
                
            }
            
            richTextBoxOutput.Text = sb.ToString();
        }

        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            fileName = null;
            openFileDialog.CheckFileExists = false;
            openFileDialog.ShowDialog();

            if (fileName != null)
            {
                depot.CreateNewBase(fileName);
                richTextBoxGeneral.Text = depot.GetDescription();
            }

            InfoForm.ShowInfo(Info.Instance.GetAllMessages());
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            fileName = null;
            openFileDialog.CheckFileExists = true;
            openFileDialog.ShowDialog();

            if (fileName != null)
            {
                depot.OpenExistsBase(fileName);
                richTextBoxGeneral.Text = depot.GetDescription();
            }

            InfoForm.ShowInfo(Info.Instance.GetAllMessages());
        }
    }
}
