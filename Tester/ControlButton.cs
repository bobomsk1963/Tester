using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
namespace Tester
{

    //******************************************************************************************************************* IDragControl
    interface IDragControl
    {
        Point DownPoint
        { get; set; }
        bool IsDragMode
        { get; set; }
        ControlSelection selectControl
        { get; set; }
        bool OnBase
        { get; set; }
        bool dYready
        { get; set; }
        bool dXready
        { get; set; }

    }

    //******************************************************************************************************************* ControlButton
    [Serializable]
    class ControlButton : Button,IDragControl
    {
        public Point DownPoint
        { get; set; }
        public bool IsDragMode
        { get; set; }        
        public ControlSelection selectControl
        { get; set; }
        public bool OnBase
        { get; set; }
        public bool dYready
        { get; set; }
        public bool dXready
        { get; set; }


        public ControlButton(ControlSelection scontrol, bool onbase = true)              // Отключение возможностит фокуса
        {
            IsDragMode = false;
            OnBase = onbase;
            dYready = true;
            dXready = true;

            selectControl = scontrol;

            if (!onbase)
            {
                this.SetStyle(ControlStyles.Selectable, false);
                this.SetStyle(ControlStyles.UserMouse, true);
                this.SetStyle(ControlStyles.StandardClick, false);

                TabStop = false;
            }
            Name = "Кнопка";
            Text = "Кнопка";
            this.Location = new Point(0, 0);

        }

        
        protected override void OnClick(EventArgs e)
        {
            if (OnBase)
            {
                base.OnClick(e);
            }
        }
        
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (OnBase)
            {
                base.OnMouseClick(e);
            }
        }
        
    }

    //******************************************************************************************************************* ControlRichTextBox

    class ControlRichTextBox : RichTextBox, IDragControl
    {
        public Point DownPoint
        { get; set; }
        public bool IsDragMode
        { get; set; }
        public ControlSelection selectControl
        { get; set; }
        public bool OnBase
        { get; set; }
        public bool dYready
        { get; set; }
        public bool dXready
        { get; set; }


        public ControlRichTextBox(ControlSelection selectcontrol)
        {
            IsDragMode = false;
            dYready = true;
            dXready = true;

            selectControl = selectcontrol;
            this.SetStyle(ControlStyles.Selectable, false); // Отключение возможностит фокуса
            this.SetStyle(ControlStyles.UserMouse, true);   // Отключить выделение текста по двойному щелчку
            this.ContextMenuStrip = new ContextMenuStrip(); // Отключение контекстного меню
            TabStop = false;
            Cursor = Cursors.Arrow;

            HideCaret();

            Name = "Текст";
            Text = "Текст";
            this.Location = new Point(0, 0);
        }

        [DllImport("user32")]
        public static extern bool HideCaret(IntPtr hWnd);


        public void HideCaret()
        {
            ControlRichTextBox.HideCaret(Handle);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }
    }

    //******************************************************************************************************************* ControlRadioButton

    class ControlRadioButton : RadioButton, IDragControl
    {

        public Point DownPoint
        { get; set; }
        public bool IsDragMode
        { get; set; }
        public ControlSelection selectControl
        { get; set; }
        public bool OnBase
        { get; set; }
        public bool dYready
        { get; set; }
        public bool dXready
        { get; set; }

        public ControlRadioButton(ControlSelection selectcontrol, bool onbase = true)              // Отключение возможностит фокуса
        {
            IsDragMode = false;
            OnBase = onbase;
            dYready = true;
            dXready = true;

            selectControl = selectcontrol;

            if (!onbase)
            {
                this.SetStyle(ControlStyles.Selectable, false);

                TabStop = false;
            }
            Name = "Переключатель";
            Text = "Переключатель";            

            //this.AutoSize = false;
            //this.TextAlign = ContentAlignment.BottomRight;
            this.AutoSize = true;
            this.Location = new Point(0, 0);
            //this.SetBounds(0, 0, Width, Height);
        }


        protected override void OnClick(EventArgs e)
        {
            if (OnBase)
            {
                base.OnClick(e);
            }
        }

    }

    //*******************************************************************************************************************  ControlCheckBox

    class ControlCheckBox : CheckBox, IDragControl
    {
        //Point DownPoint;
        //bool IsDragMode;

        //SelectionControl selectionControl;

        public Point DownPoint
        { get; set; }
        public bool IsDragMode
        { get; set; }
        public ControlSelection selectControl
        { get; set; }
        public bool OnBase
        { get; set; }
        public bool dYready
        { get; set; }
        public bool dXready
        { get; set; }

        public ControlCheckBox(ControlSelection selectcontrol, bool onbase = true)              // Отключение возможностит фокуса
        {
            IsDragMode = false;
            OnBase = onbase;
            dYready = true;
            dXready = true;

            selectControl = selectcontrol;
            if (!onbase)
            {
                this.SetStyle(ControlStyles.Selectable, false);

                TabStop = false;
            }
            Name = "Флаговая кнопка";
            Text = "Флаговая кнопка";

            this.AutoSize = true;
            this.Location = new Point(0, 0);

        }

        protected override void OnClick(EventArgs e)
        {
            if (OnBase)
            {
                base.OnClick(e);
            }
        }

    }

    //******************************************************************************************************************* ControlTextBox

    class ControlTextBox : TextBox, IDragControl
    {

        public Point DownPoint
        { get; set; }
        public bool IsDragMode
        { get; set; }
        public ControlSelection selectControl
        { get; set; }
        public bool OnBase
        { get; set; }
        public bool dYready
        { get; set; }
        public bool dXready
        { get; set; }

        public ControlTextBox(ControlSelection selectcontrol, bool onbase = true)
        {
            IsDragMode = false;
            OnBase = onbase;
            dYready = false;
            dXready = true;

            selectControl = selectcontrol;
            if (!onbase)
            {
                this.SetStyle(ControlStyles.Selectable, false); // Отключение возможностит фокуса
                this.SetStyle(ControlStyles.UserMouse, true);   // Отключить выделение текста по двойному щелчку
                this.SetStyle(ControlStyles.StandardClick, false);
                this.SetStyle(ControlStyles.StandardDoubleClick, false);
                this.ContextMenuStrip = new ContextMenuStrip(); // Отключение контекстного меню

                TabStop = false;
                Cursor = Cursors.Arrow;

                HideCaret();
            }
            Name = "Ввод текста";
            Text = "Ввод текста";

            this.Location = new Point(0, 0);

        }

        public void HideCaret()
        {
            ControlRichTextBox.HideCaret(Handle);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (OnBase)
            {
                base.OnKeyDown(e);
            }
            else
            {
                e.SuppressKeyPress = true;
            }

        }

        protected override void OnClick(EventArgs e)
        {
            if (OnBase)
            {
                base.OnClick(e);
            }
        }


    }

    //*******************************************************************************************************************
    class ControlPictureBox : PictureBox, IDragControl
    {
        public Point DownPoint
        { get; set; }
        public bool IsDragMode
        { get; set; }
        public ControlSelection selectControl
        { get; set; }
        public bool OnBase
        { get; set; }
        public bool dYready
        { get; set; }
        public bool dXready
        { get; set; }


        public ControlPictureBox(ControlSelection scontrol, bool onbase = true)              // Отключение возможностит фокуса
        {
            IsDragMode = false;
            OnBase = onbase;
            dYready = true;
            dXready = true;

            selectControl = scontrol;
            this.SetStyle(ControlStyles.Selectable, false);
            //this.SetStyle(ControlStyles.UserMouse, true);
            //this.SetStyle(ControlStyles.StandardClick, false);

            this.BorderStyle = BorderStyle.FixedSingle;
            
            TabStop = false;

            Name = "Рисунок";
            Text = "Рисунок";
            this.Location = new Point(0, 0);

        }

        protected override void OnClick(EventArgs e)
        {
            if (OnBase)
            {
                base.OnClick(e);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (OnBase)
            {
                base.OnMouseClick(e);
            }
        }

    }

    //*******************************************************************************************************************
    class ControlLabel : Label, IDragControl
    {
        public Point DownPoint
        { get; set; }
        public bool IsDragMode
        { get; set; }
        public ControlSelection selectControl
        { get; set; }
        public bool OnBase
        { get; set; }
        public bool dYready
        { get; set; }
        public bool dXready
        { get; set; }


        public ControlLabel(ControlSelection scontrol, bool onbase = true)              // Отключение возможностит фокуса
        {
            IsDragMode = false;
            OnBase = onbase;
            dYready = true;
            dXready = true;

            selectControl = scontrol;
            this.SetStyle(ControlStyles.Selectable, false);
            //this.SetStyle(ControlStyles.UserMouse, true);
            //this.SetStyle(ControlStyles.StandardClick, false);

            //this.BorderStyle = BorderStyle.FixedSingle;

            TabStop = false;

            Name = "Надпись";
            Text = "Надпись";

            this.AutoSize = false;
            this.Location = new Point(0, 0);
        }
    }
}
