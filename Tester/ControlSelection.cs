using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
namespace Tester
{
    public class ControlSelection
        // Элемент этого класса размещается на панели проектироавания
    {
        static ControlSelection instance;
        public static ControlSelection Instance
        { get { return instance; } }

        Panel[] panels = new Panel[8]; // Панельки отмечающие контрол

        static int width = 5;  // Размер стороны панельки

        Panel PanelEdit;

        PropertyGrid PropertyGridElement; // Редактор свойств элемента
        PropertyGrid PropertyGridQuestion; // Редактор свойств вопроса
        PropertyGrid PropertyGridTest; // Редактор свойств теста

        public Control C = null; // Выделенный контрол

        ElementControl cd; // Выделенный элемент вопроса

        public ElementControl CD
        {
            get { return cd; }
        }
        /*
        ElementControl CD
        {
            get { return cd; }
        }

         */

        ClassTest Test;
        ListBox ListBoxTest;

        public object SelectQuestion // Текущий вопрос
        {
            get 
            {
                return //Test.ListQuestions[IndexSelectQuestion]; 
                       (Question)ListBoxTest.SelectedItem; 
            }
        }

        public int IndexSelectQuestion //Индекс текущего вопроса
        { 
            get { return ListBoxTest.SelectedIndex; }
            set { ListBoxTest.SelectedIndex = value; }
        }

        public void ResetPrpertyQuestion()
        {
            //propertyGrid3.SelectedObject = SelectQuestion;
        }

        void SetQuestionToPanel(Question q)
        { 

        }

        void ResetTest(ClassTest test)
        {
            Test = test;
        }

        TextBox textbox; // Для фокусировки при буксировании клавиатурой

        ContextMenuStrip ContextMenu; //Контекстное меню для контролов


        public ControlSelection(Panel panel, PropertyGrid propGE, 
                                             PropertyGrid propGQ, 
                                             PropertyGrid propGT, 
                                                          ListBox listboxtest, TextBox tb, ContextMenuStrip contextMenu)
        {
            ContextMenu = contextMenu;
            textbox = tb;
            PanelEdit = panel;                // Установить обработчик события
            PropertyGridElement = propGE;
            PropertyGridQuestion = propGQ;
            PropertyGridTest = propGT;
            ListBoxTest = listboxtest;
            for (int i = 0; i < 8; i++)
            {
                panels[i] = null;
            }

            instance = this;

        }

                                 // Передовать объект который редактируетсяся  
                                 //ElementControl
        public void ResetControl(Control c)
        {
            if (C != c)
            {
                if (C != null)
                {
                    cd.GetCtrl().ContextMenuStrip = new ContextMenuStrip(); 
                }

                C = c;

                ResetPanel();

                if (C != null)
                {
                             //ElementControl
                    cd = (ElementControl)C.Tag;//new ControlData(C);

                    cd.GetCtrl().ContextMenuStrip = ContextMenu;
                    
                }
                else
                { cd = null; }
                PropertyGridElement.SelectedObject = cd;
            }
        }
        

        void ResetPanel()
        {
                for (int i = 0; i < 8; i++)
                {
                    if (panels[i]!=null)
                    panels[i].Dispose();
                    panels[i] = null;
                }

                if (C != null)
                {

                    for (int i = 0; i < 8; i++)
                    {
                        panels[i] = new DeltaPanel(i, (ControlSubstrate)C, width);
                    }   
                }
        }

        void FreePanel()
        {
            for (int i = 0; i < 8; i++)
            {
                if (panels[i] != null)
                    panels[i].Dispose();
                panels[i] = null;
            }
        }


        public void RefreshPG()
        {
            PropertyGridElement.Refresh();            
        }

        public void DelElement()
        {
            if (C != null)
            {
                FreePanel();
                C.Dispose();
                C = null;
                PropertyGridElement.SelectedObject = null;
                ((Question)SelectQuestion).Elements.Remove(cd);                
                cd.Dispose();
                cd = null;
            }

        }

        public void BringToFront()
        {
            if (C != null)
            {
                C.BringToFront();

                // Перемещение на первую позицию в списке
                ((Question)SelectQuestion).Elements.Remove(cd);
                ((Question)SelectQuestion).Elements.Insert(0, cd);
                
            }
        }

        public void SandToBack()
        {
            if (C != null)
            {
                C.SendToBack();
                // Перемещение на послкднюю позицию в списке
                ((Question)SelectQuestion).Elements.Remove(cd);
                ((Question)SelectQuestion).Elements.Add(cd);
            }
        }

        // Обработчики событий мышки ствятся на объекты устанавливаемые на панель вопроса
        public static void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                if (sender is ControlTextBox)
                {
                    ((ControlTextBox)sender).HideCaret();
                }
                ((IDragControl)sender).selectControl.ResetControl(((Control)sender).Parent); // Установка выделенного элемента в редактор и отметка

                ((Control)sender).Cursor = Cursors.SizeAll;
                ((IDragControl)sender).DownPoint = e.Location;
                ((IDragControl)sender).IsDragMode = true;

                //textbox2.Focus();
                //textbox2.Select();
            }
        }

        public static void MouseUp(object sender, MouseEventArgs e)
        {
            ((Control)sender).Cursor = Cursors.Arrow;
            ((IDragControl)sender).IsDragMode = false;

            ((IDragControl)sender).selectControl.RefreshPG();
        }

        public static void MouseMove(object sender, MouseEventArgs e)
        {
            if (((IDragControl)sender).IsDragMode)
            {
                Point p = e.Location;
                Point dp = new Point(p.X - ((IDragControl)sender).DownPoint.X, p.Y - ((IDragControl)sender).DownPoint.Y);
                //((Control)sender).Parent.Location = new Point(((Control)sender).Parent.Location.X + dp.X, ((Control)sender).Parent.Location.Y + dp.Y);

                ElementControl elc = (ElementControl)((Control)sender).Tag;
                elc.Location = new Point(elc.Location.X + dp.X, elc.Location.Y + dp.Y);
                    //new Point(((Control)sender).Parent.Location.X + dp.X, ((Control)sender).Parent.Location.Y + dp.Y);

                //((IDragControl)sender).selectionControl.RefreshPG();
            }
            else
            {
                ((Control)sender).Cursor = Cursors.Arrow;
            }
        }

    }

    //******************************************************************************************************************* ControlSelection

    class DeltaPanel : Panel  // Класс панель отмечающих выделенныый элемент
    {
        Point DownPoint;
        bool IsDragMode;

        int Idx = 0;
        ControlSubstrate Ctrl;
        int width = 5;

        public DeltaPanel(int idx, ControlSubstrate ctrl, int w)
            : base()
        {
            Ctrl = ctrl;
            Idx = idx;
            width = w;

            Name = "Delta" + Idx.ToString();
            BackColor = Color.LightGray;
            BorderStyle = BorderStyle.FixedSingle;

            switch (Idx)
            {
                case 0:
                    {
                        SetBounds(0, 0, width, width);
                        Anchor = AnchorStyles.Left | AnchorStyles.Top;
                        Cursor = Cursors.SizeNWSE;
                        break;
                    }
                case 2:
                    {
                        SetBounds(Ctrl.Width - width, Ctrl.Height - width, width, width);
                        Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                        Cursor = Cursors.SizeNWSE;
                        break;
                    }
                case 1:
                    {
                        SetBounds(Ctrl.Width - width, 0, width, width);
                        Anchor = AnchorStyles.Right | AnchorStyles.Top;
                        Cursor = Cursors.SizeNESW;
                        break;
                    }
                case 3:
                    {
                        SetBounds(0, Ctrl.Height - width, width, width);
                        Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                        Cursor = Cursors.SizeNESW;
                        break;
                    }
                case 4:
                    {
                        SetBounds((Ctrl.Width / 2) - (width / 2), 0, width, width);
                        Anchor = AnchorStyles.Top;
                        Cursor = Cursors.SizeNS;
                        break;
                    }
                case 6:
                    {
                        SetBounds((Ctrl.Width / 2) - (width / 2), Ctrl.Height - width, width, width);
                        Anchor = AnchorStyles.Bottom;
                        Cursor = Cursors.SizeNS;
                        break;
                    }
                case 5:
                    {
                        SetBounds(Ctrl.Width - width, (Ctrl.Height / 2) - (width / 2), width, width);
                        Anchor = AnchorStyles.Right;
                        Cursor = Cursors.SizeWE;
                        break;
                    }
                case 7:
                    {
                        SetBounds(0, (Ctrl.Height / 2) - (width / 2), width, width);
                        Anchor = AnchorStyles.Left;
                        Cursor = Cursors.SizeWE;
                        break;
                    }
            }
            Ctrl.Controls.Add(this);
            BringToFront();

            TabStop = false;
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            DownPoint = mevent.Location;
            IsDragMode = true;
            base.OnMouseDown(mevent);  // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            IsDragMode = false;
            base.OnMouseUp(mevent);    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            Ctrl.selectControl.RefreshPG();
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            if (IsDragMode)
            {
                int dy = 0;
                if (((IDragControl)Ctrl.Ctrl).dYready)
                {
                    dy = mevent.Location.Y - DownPoint.Y;
                }
                int dx = 0;
                if (((IDragControl)Ctrl.Ctrl).dXready)
                {
                    dx = mevent.Location.X - DownPoint.X;
                }

                switch (Idx)
                {
                    case 0:
                        {
                            this.Parent.SetBounds(this.Parent.Left + dx, this.Parent.Top + dy, this.Parent.Width - dx, this.Parent.Height - dy);
                            break;
                        }
                    case 2:
                        {
                            this.Parent.SetBounds(this.Parent.Left, this.Parent.Top, this.Parent.Width + dx, this.Parent.Height + dy);
                            break;
                        }
                    case 1:
                        {
                            this.Parent.SetBounds(this.Parent.Left, this.Parent.Top + dy, this.Parent.Width + dx, this.Parent.Height - dy);
                            break;
                        }
                    case 3:
                        {
                            this.Parent.SetBounds(this.Parent.Left + dx, this.Parent.Top, this.Parent.Width - dx, this.Parent.Height + dy);
                            break;
                        }
                    case 4:
                        {
                            this.Parent.SetBounds(this.Parent.Left, this.Parent.Top + dy, this.Parent.Width, this.Parent.Height - dy);
                            break;
                        }
                    case 6:
                        {
                            this.Parent.SetBounds(this.Parent.Left, this.Parent.Top, this.Parent.Width, this.Parent.Height + dy);
                            //Ctrl.Ctrl.SetBounds(0, 0, Ctrl.Ctrl.Width, Ctrl.Ctrl.Height + dy);
                            //Ctrl.Ctrl.Height = Ctrl.Ctrl.Height + dy;
                            break;
                        }
                    case 5:
                        {
                            this.Parent.SetBounds(this.Parent.Left, this.Parent.Top, this.Parent.Width + dx, this.Parent.Height);
                            break;
                        }
                    case 7:
                        {
                            this.Parent.SetBounds(this.Parent.Left + dx, this.Parent.Top, this.Parent.Width - dx, this.Parent.Height);
                            break;
                        }
                }

                //Ctrl.selectionControl.RefreshPG();

            }
            base.OnMouseMove(mevent);  // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
    }

    //******************************************************************************************************************* DeltaPanel

    public partial class ControlSubstrate : UserControl
    // Подложка под размещеный контрол - нужна для размещения на ней панелей отмечающих выделенный элемент 
    // Может быть панель         
    {

        public ControlSelection selectControl;
        public Control Ctrl;
        public int idx;

        public ControlSubstrate(ControlSelection selectcontrol, Control ctrl, int idxcontrol)
            : base()
        {
            this.SetStyle(ControlStyles.Selectable, false);
            this.SetStyle(ControlStyles.UserMouse, true);

            idx = idxcontrol;
            Name = "uc" + idx.ToString();
            selectControl = selectcontrol;
            Ctrl = ctrl;
            ((IDragControl)Ctrl).selectControl = selectcontrol;            

            this.Controls.Add(Ctrl);

            this.SetBounds(0, 0, Ctrl.Width, Ctrl.Height);            

            Ctrl.Dock = DockStyle.Fill;
            
            TabStop = false;

            //BackColor = Color.Transparent;
            //AutoSize = true;
        }
    }
}
