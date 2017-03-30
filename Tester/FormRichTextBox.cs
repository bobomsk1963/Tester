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
    public partial class FormRichTextBox : Form
    {
        public string text
        {
            get { return richTextBox1.Rtf; }
            set { richTextBox1.Rtf = value; }
        }

        public FormRichTextBox(Color c)
        {
            InitializeComponent();
            richTextBox1.BackColor = c;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void Font_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = richTextBox1.SelectionFont;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
                if (!richTextBox1.SelectionFont.Equals(fontDialog1.Font))
                {
                    Font f = richTextBox1.SelectionFont;
                    richTextBox1.SelectionFont = fontDialog1.Font;
                    f.Dispose();
                    //bold10.Checked = bold1.Checked = fontDialog1.Font.Bold;
                    //italic10.Checked = italic1.Checked = fontDialog1.Font.Italic;
                    //underline10.Checked = underline1.Checked = fontDialog1.Font.Underline;
                }
        }

        private void Color_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = richTextBox1.SelectionColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                richTextBox1.SelectionColor = colorDialog1.Color;
        }

        private void отменаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.CanUndo)
            {
                richTextBox1.Undo();
            }
        }

        private void восстановитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.CanRedo)
            {
                richTextBox1.Redo();
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            отменаToolStripMenuItem.Enabled = richTextBox1.CanUndo;
            восстановитьToolStripMenuItem.Enabled = richTextBox1.CanRedo;
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void копироватьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
                richTextBox1.Paste();
        }

        private void кЛевойСторонеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void поЦентруToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void кПравойСторонеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void жирныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem mi = sender as ToolStripMenuItem;
            FontStyle fs = richTextBox1.SelectionFont.Style;
            fs = fs | mi.Font.Style;
            Font f = richTextBox1.SelectionFont;
            richTextBox1.SelectionFont = new Font(f, fs);
            f.Dispose();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog1.FileName;
                richTextBox1.SaveFile(path, GetFileType(path));
                richTextBox1.Modified = false;
            }
        }

        private RichTextBoxStreamType GetFileType(string path)
        {
            string s = System.IO.Path.GetExtension(path).ToUpper();
            return s == ".RTF" ? RichTextBoxStreamType.RichText : RichTextBoxStreamType.PlainText;
        }

        private void Load_Click(object sender, EventArgs e)
        {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string path = openFileDialog1.FileName;
                    richTextBox1.LoadFile(path, GetFileType(path));
                    saveFileDialog1.FileName = path;
                    openFileDialog1.FileName = "";
                    richTextBox1.Modified = false;
                }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = richTextBox1.SelectionBackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                richTextBox1.SelectionBackColor = colorDialog1.Color;
        }
    }
}
