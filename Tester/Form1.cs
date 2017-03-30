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
    public partial class Form1 : Form
    {

        RadioButton radioButton = null; //Выделенная радиокнопка

        ArrayCreateElement ArrayCreate;

        ControlSelection Selection;

        ClassTest Test;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Selection = new ControlSelection(panel2, propertyGrid1, propertyGrid3, propertyGrid2, listBox1, null, contextMenuStrip1);

            ArrayCreate = new ArrayCreateElement();
            ArrayCreate.Add(null);
            ArrayCreate.Add(new CreateElementButton());
            ArrayCreate.Add(new CreateElementRichTextBox());
            ArrayCreate.Add(new CreateElementRadioButton());
            ArrayCreate.Add(new CreateElementCheckBox());
            ArrayCreate.Add(new CreateElementTextBox());
            ArrayCreate.Add(new CreateElementPictureBox());
            ArrayCreate.Add(new CreateElementLabel());

            radioButtonPanel0.Text = "";
            radioButtonPanel1.Text = ArrayCreate[1].ToString();
            radioButtonPanel2.Text = ArrayCreate[2].ToString();
            radioButtonPanel3.Text = ArrayCreate[3].ToString();
            radioButtonPanel4.Text = ArrayCreate[4].ToString();
            radioButtonPanel5.Text = ArrayCreate[5].ToString();
            radioButtonPanel6.Text = ArrayCreate[6].ToString();
            radioButtonPanel7.Text = ArrayCreate[7].ToString();

            Test = new ClassTest();
            Test.AddQuestion();

            propertyGrid2.SelectedObject = Test;

            BindingSource binding1;
            binding1 = new BindingSource();
            binding1.DataSource = Test.ListQuestions;
            listBox1.DisplayMember = "EcranName";
            listBox1.DataSource = binding1;

            this.Height = 900;
            this.Location = new Point(50, 50);
            tabControl1.SelectedIndex = 3;
        }

        private void radioButtonPanel1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                radioButton = (RadioButton)sender;
            }
        }

        private void panel2_Click(object sender, EventArgs e)
        {

        }

        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            //e.
            if (radioButton != null)
            {
                // Проверить на клик левой кнопкой

                //MessageBox.Show(radioButton.Tag.ToString());
                string s = radioButton.Tag.ToString();
                int n = Convert.ToInt16(s);
                CreateElement ce = ArrayCreate[n];

                if (ce != null)
                {
                    Element el = ce.Create();

                    Question q = (Question)Selection.SelectQuestion;
                    q.Elements.Insert(0, el);

                    el.CreateControl();

                    ElementControl elc = (ElementControl)el;

                    elc.Location = e.Location;//new Point(e.X, e.Y);

                    Control ccc = elc.CreateSubstrate(Selection, panel2);
                    //((ElementControl)el).Location = new Point(e.X, e.Y);
                    //Control ccc =((ElementControl)el).CreateSubstrate(Selection, panel2);

                    Selection.ResetControl(ccc);
                    Selection.BringToFront();

                    // Переместить этот элемент на нулевое место
                }


                // Создание объкта и установка его на панели

            }

            radioButtonPanel0.Checked = true;              // Переход на нулевую кнопку
        }

        private void наПереднийПланToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Selection.BringToFront();
        }

        private void наЗаднийПланToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Selection.SandToBack();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Selection.DelElement();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Test.AddQuestion();
            ((BindingSource)listBox1.DataSource).ResetBindings(true);
            listBox1.SelectedIndex = Test.ListQuestions.Count - 1;
            //binding1.ResetBindings(true);//Insert
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (Test.ListQuestions.Count > 1)
            {
                                    // Удалить все элементы

                int i = listBox1.SelectedIndex;
                Test.DelQuestion(i);
                Test.ReIndexQuestions();
                ((BindingSource)listBox1.DataSource).ResetBindings(true);

                int n = listBox1.SelectedIndex; // Для отображения в редакторе свойств после удаления элемента
                listBox1.SelectedIndex = -1;
                listBox1.SelectedIndex = n;
            }
            else
            {
                MessageBox.Show(this, "Должен остаться хотябы один вопрос.", "Внимание!");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((Question)listBox1.SelectedItem != null)
            {
                Question q = (Question)listBox1.SelectedItem;
                propertyGrid3.SelectedObject = q;


                // снять с текущего элемнта панельки 
                // сделать текущий null

                Selection.ResetControl(null);

                while (panel2.Controls.Count > 0)    // Снятие всех контролов с панели при смене вопроса
                {
                    //MessageBox.Show(panel2.Controls[0].ToString());

                    panel2.Controls[0].Parent = null;
                }

                // Установка текущих контролов

                for (int i = 0; i < q.Elements.Count; i++)
                {
                    panel2.Controls.Add(q.Elements[i].EControl.Parent);
                }
            }
        }

        public static Image PastePictureFromClipbord()  //Получение изображения из буфера обмена
        {

            //Проверяем есть ли в буфере изображение
            Image im = null;

            if (System.Windows.Forms.Clipboard.GetDataObject() != null)
            {
                IDataObject oDataObj = Clipboard.GetDataObject();
                if (oDataObj.GetDataPresent(System.Windows.Forms.DataFormats.Bitmap))
                {
                    object oObj = oDataObj.GetData(DataFormats.Bitmap, true);
                    im = (Image)oObj;
                }
            }
            return im;
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string fname = "";

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Сохранить";
            //dialog.InitialDirectory = (string)value;
            dialog.Filter = "Файл теста (*.tst)|*.tst";
            //openFileDialog1.FilterIndex = 2;
            //openFileDialog1.RestoreDirectory = true;           
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fname = dialog.FileName;
                dialog.Dispose();
            }
            else
            {
                dialog.Dispose();
                return;
            }

            Test.SerialBinary(fname);
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fname="";

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Открыть";
            //dialog.InitialDirectory = (string)value;
            dialog.Filter = "Файл теста (*.tst)|*.tst";
            //openFileDialog1.FilterIndex = 2;
            //openFileDialog1.RestoreDirectory = true;           
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fname = dialog.FileName;
                dialog.Dispose();
            }
            else
            {
                dialog.Dispose();
                return;                
            }


            try
            {
                Selection.ResetControl(null);

                ClassTest tttt = ClassTest.DeSerialBinary(fname);

                for (int i = 0; i < tttt.ListQuestions.Count; i++)
                {
                    tttt.ListQuestions[i].DeSerializeQuestion(Selection);
                }

                // Удаление старого объекта тест

                Test = tttt;

                propertyGrid2.SelectedObject = Test;

                BindingSource binding1;
                binding1 = new BindingSource();
                binding1.DataSource = Test.ListQuestions;
                listBox1.DisplayMember = "EcranName";
                listBox1.DataSource = binding1;
                listBox1.SelectedIndex = -1;
                listBox1.SelectedIndex = 0;
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.Message);
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout formAbout = new FormAbout();
            formAbout.ShowDialog(this);
        }

        private void проверкаТестаToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ClassPorter Porter = new ClassPorterMono();
            ClassGenerationTest Generation = new ClassGenerationTest(Porter);

            FormProbaTest t = new FormProbaTest(Porter);

            t.FormBorderStyle = FormBorderStyle.None;

            //t.Height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
            //t.Width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
            
            t.WindowState = FormWindowState.Maximized;
            //t.TopMost = true;

            Generation.InitTest(Test);
            Generation.SendToWork();

            t.ShowDialog();            


            t.Dispose();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

    }



}
