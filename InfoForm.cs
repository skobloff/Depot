using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DepotConcept
{
    public partial class InfoForm : Form
    {
        public InfoForm(IEnumerable<String> messages)
        {
            InitializeComponent();
            foreach(string s in messages)
            {
                richTextBoxMain.AppendText(s);
                richTextBoxMain.AppendText("\n");
            }
        }

        public static void ShowInfo(IEnumerable<String> messages)
        {
            if (messages.Count() > 0)
            {
                InfoForm form = new InfoForm(messages);
                form.ShowDialog();
            }
        }
    }
}
