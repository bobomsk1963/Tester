using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tester
{
    public partial class FormProbaTest : Form
    {
        ClassWorkTest Work;

        public FormProbaTest(ClassPorter p)
        {
            InitializeComponent();

            ClassPorter Porter = new ClassPorterMono();
            ((ClassPorterMono)p).SetDistantPorter(Porter);
            ((ClassPorterMono)Porter).SetDistantPorter(p);
            Work = new ClassWorkTest(Porter,this,panel1);

        }

        private void FormProbaTest_Shown(object sender, EventArgs e)
        {
            panel1.Left = ((this.Width - panel1.Width) / 2)-2;
            panel1.Top = ((this.Height - panel1.Height) / 2)-2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int numb = 0;
            for (int i = 0; i < Work.Q.Elements.Count; i++)
            {
                numb = numb + Work.Q.Elements[i].ResultNumber(Work.Q.Elements[i].ResultObject());
            }
            MessageBox.Show(numb.ToString());

        }




    }
}
