using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

using System.Drawing.Design;
//using System.Windows.Forms.Design;
//using System.Design;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tester
{
    //***************************************************************************************************************
    [Serializable]
    public class ClassTest : IDisposable
    {
        string name;
        [DisplayName("Наименование")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string description;
        [DisplayName("Описание")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        string autor;
        [DisplayName("Автор")]
        public string Autor
        {
            get { return autor; }
            set { autor = value; }
        }


        List<Question> Questions;

        public ClassTest()
        {
            Questions=new List<Question>();
        }

        public Question this[int i]
        {
            get { return Questions[i]; }
            set { Questions[i] = value; }
        }


        [Browsable(false)]
        public List<Question> ListQuestions
        { get { return Questions; } }

        public void Dispose()
        {
            // Удалить все элементы !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        public Question AddQuestion()
        {
            Question q = new Question(Questions.Count+1);
            Questions.Add(q);
            return q;
        }

        public void DelQuestion(Question q)
        {
            Questions.Remove(q);
        }

        public void DelQuestion(int i)
        {
            // Удалить все элементы !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            Questions.RemoveAt(i);
        }

        public void ReIndexQuestions()
        {
            for (int i = 0; i < Questions.Count; i++)
            {
                Questions[i].Idx = i + 1;
            }
        }


        public bool SerialBinary(string fname)
        {
            bool ret = false;
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(fname, FileMode.Create))//OpenOrCreate)) //Create ??????????????????????????????????/
                {
                    formatter.Serialize(fs, this);
                }
                ret = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(fname + "  -  " + e.Message);
            }
            return ret;
        }

        static public ClassTest DeSerialBinary(string fname, bool MessageBoxShow = true)
        {
            ClassTest smp = null;
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(fname, FileMode.Open))//OpenOrCreate))// Open?????????????????????????????????????/
                {
                    smp = (ClassTest)formatter.Deserialize(fs);
                }
            }
            catch (Exception e)
            {
                if (MessageBoxShow)
                {
                    MessageBox.Show(fname + " - " + e.Message);//"+ e.Message);
                }
                smp = null; //new ClassTest();//new ListSMSCommand(0, "", "", new List<SMSCommand>());
            }
            return smp;
        }

        public bool SerialXML(string fname)
        {
            // Для сериализации в XML необходимо определить се типы записываемых классов если они наследуются
            Type[] extraTypes = new Type[9];
            extraTypes[0] = typeof(Element);
            extraTypes[1] = typeof(ElementControl);
            extraTypes[2] = typeof(ElementButton);
            extraTypes[3] = typeof(ElementRichTextBox);
            extraTypes[4] = typeof(ElementRadioButton);
            extraTypes[5] = typeof(ElementCheckBox);
            extraTypes[6] = typeof(ElementTextBox);
            extraTypes[7] = typeof(ElementPictureBox);
            extraTypes[8] = typeof(Image);

            bool ret = false;
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(ClassTest), extraTypes);
                using (FileStream fs = new FileStream(fname, FileMode.Create))//OpenOrCreate)) //Create ??????????????????????????????????/
                {
                    formatter.Serialize(fs, this);
                }
                ret = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(fname + "  -  " + e.Message);
            }
            return ret;
        }

        static public ClassTest DeSerialXML(string fname, bool MessageBoxShow = true)
        {
            ClassTest smp = null;
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(ClassTest));
                using (FileStream fs = new FileStream(fname, FileMode.Open))//OpenOrCreate))// Open?????????????????????????????????????/
                {
                    smp = (ClassTest)formatter.Deserialize(fs);
                }
            }
            catch (Exception e)
            {

                if (MessageBoxShow)
                {
                    MessageBox.Show(fname + " - Ошибка загрузки!");//"+ e.Message);
                }
                smp = new ClassTest();//new ListSMSCommand(0, "", "", new List<SMSCommand>());

            }
            return smp;
        }

    }

    //***************************************************************************************************************
    [Serializable]
    public class Question : IDisposable
    {
        int idx = 0;

        [ReadOnly(true)] 
        [DisplayName("Номер вопроса")]
        public int Idx 
        {
            get { return idx; }
            set { idx = value; }
        }

        List<Element> elements=new List< Element>();
        [Browsable(false)]
        public List< Element> Elements
        {
            get { return elements; }
            //set { }
        }

        [DisplayName("Тема вопроса")]
        public string Name
        {
            get;
            set;
        }

        [DisplayName("Уровень сложности вопроса")]
        public int Uroven
        {
            get;
            set;
        }


        [Browsable(false)]
        public string EcranName      // Для вывода в строку ListBox
        {
            get { return ToString(); }            
        }

        public Question()
        {
        }

        public Question(int i)
        {
            idx = i;
            Name = "Вопрос";
        }

        public void Dispose()
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Dispose();
            }
            Elements.Clear();
        }

        void AddElement(Element e)
        {
            Elements.Add(e);            
        }


        public MemoryStream SerialBinary()  // Сериализаци и гаоборот для передачи вопроса в исполнение теста
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                ms = null;
            }
            return ms;
        }

        static public Question DeSerialBinary(MemoryStream ms,bool MessageBoxShow = true)
        {
            Question q = null;

            // Поставить на первую позицию в ms ???

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                q = (Question)formatter.Deserialize(ms);
            }
            catch (Exception e)
            {
                if (MessageBoxShow)
                {
                    MessageBox.Show("11111111111111111 - "+e.Message);
                }
                q = null; 
            }
            return q;
        }


        public void DeSerializeQuestion(ControlSelection selection) // Для загрузки из файла вопроса
        {
            for (int i = 0; i < elements.Count; i++)
            {
                DeSerializeElement(selection, elements[i]);
            }
        }

        void DeSerializeElement(ControlSelection selection, Element el)
        {
            //this.Elements.Insert(0, el);
            el.CreateControl(true);
            ElementControl elc = (ElementControl)el;
            Control ccc = elc.CreateSubstrate(selection, null);
        }

        bool DelElement(Element e)
        {
            return Elements.Remove(e);
        }

        public override string ToString()
        {
            return idx.ToString().PadLeft(3,' ')+". "+Name;//base.ToString();
        }
    }

    //***************************************************************************************************************
    [Serializable]
    public abstract class Element : IDisposable
    {
        protected int index = 0;
        [ReadOnly(true)]
        [DisplayName("Номер элемента")]
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        [NonSerialized] 
        protected Control control = null;
        
        [Browsable(false)]
        public Control EControl
        {
            get { return control; }
        }

        public Control GetCtrl()
        {return control;}

        public abstract //Control 
                        void CreateControl(bool isDeSerialize = false);
        //public abstract void CreateSubstrate() // Создать подложку
        
        public abstract void ResetData();   // Пере загрузка данных из контрола
        //public abstract void Dispose();
        public void Dispose()
        {
            control.Dispose();
            GC.SuppressFinalize(this);
        }

        public Element()
        { }

        public abstract object ResultObject();

        public abstract int ResultNumber(object o);
    }

    [Serializable]
   public class ElementControl : Element
    {

        //bool OnLocation;

        public ElementControl():base()
        { }

        bool CreateWithSubstrate = true;
        public void WithSubstrate(bool b)
        {
            CreateWithSubstrate = b;
        }

        Point location = new Point(0, 0);

        public void setLocation(Point p)
        {
            location = p;
        }

        [Browsable(false)]
        [NonSerialized]
        protected bool onbasecontrol = false;
        [Browsable(false)]
        public bool OnBaseControl
        {
            get { return onbasecontrol; }
            set { onbasecontrol = value; }
        }       


        [DisplayName("Положенме")]
        public Point Location
        {
            get 
            {
                /*
                if (control != null)
                {
                    if (control.Parent != null)
                    {
                        return control.Parent.Location;
                    }
                    else
                    {
                        return control.Location;
                    }
                }
                else
                {
                    return location;
                }
                */
                
                if (control != null)
                {                    
                    if (control.Parent != null)
                    {
                        location = control.Parent.Location;
                    }
                    else 
                    {
                        location = control.Location;
                    }
                }
                return location; 
                 
            }
            set
            {
                location = value;
                if (control != null)
                {
                    if (control.Parent != null)
                    {
                        control.Parent.Location = location;//value;
                        //location = control.Parent.Location;
                    }
                    else 
                    {
                        control.Location = location;//value;
                        //location = control.Location;
                    }
                }
            }
        }

        public void OutLocation(Point p)
        {
            if (control != null)
            {
                if (control.Parent != null)
                {
                    control.Parent.Location = p;
                    location = control.Parent.Location;
                }
                else
                {
                    control.Location = p;
                    location = control.Location;
                }
            }
        }

        public void InLocation()
        {
            if (control != null)
            {
                if (control.Parent != null)
                {
                    location = control.Parent.Location;
                }
                else
                {
                    location = control.Location;
                }
            }
        }

        

        protected Size size;
        [DisplayName("Размер")]
        public Size Size
        {
            get 
            {
                if (control != null)
                {
                    if (control.Parent != null)
                    {
                        size = control.Parent.Size;
                    }
                    else
                    {
                        size = control.Size;
                    }
                }
                return size; 
                //return control.Parent.Size; 
            }
            set            
            {
                size = value;
                if (control != null)
                {
                    if (control.Parent != null)
                    {
                        control.Parent.Size = size;
                        //size = control.Parent.Size;
                    }
                    else
                    {
                        control.Size = size;
                        //size = control.Size;
                    }
                }
            }

        }


        public override //Control 
                         void CreateControl(bool isDeSerialize = false)
        {
            if (!isDeSerialize)
            {
                this.size = control.Size;       // По умолчанию размеры при созданиии online  
                this.location = control.Location;
            }
            else
            {
                control.Size = this.size;       // По умолчанию размеры при созданиии online  
                control.Location = this.location;
            }
            control.Tag = this;
        }

        public override void ResetData()
        { 

        }

        public Control CreateSubstrate(ControlSelection cs, Panel panel) // Создать подложку
        {
            ControlSubstrate sub = new ControlSubstrate(cs, control, Index);
            this.size = sub.Size;
            sub.Tag = control.Tag;
            control.MouseDown += new MouseEventHandler(ControlSelection.MouseDown);
            control.MouseUp += new MouseEventHandler(ControlSelection.MouseUp);
            control.MouseMove += new MouseEventHandler(ControlSelection.MouseMove);
            SetToPanel(panel);
            return sub;
        }

        public void SetToPanel(Panel panel) // Создать подложку
        { 
            if (control.Parent!=null)
            {
                control.Parent.Location = location;
                control.Parent.SetBounds(location.X,location.Y,size.Width,size.Height);
                if (panel != null)
                { panel.Controls.Add(control.Parent); }
            }
            else
            {
                control.SetBounds(location.X, location.Y, size.Width, size.Height);
                if (panel != null)
                { panel.Controls.Add(control); }
            }
        }

        public override object ResultObject()
        {
            return null;
        }

        public override int ResultNumber(object o)
        {
            return 0;
        }
    }

    [Serializable]
    public class ElementButton : ElementControl
    {
        //static int index = 0;

                                        // Добавить возможность вставку картинок на кнопку
        string text="";
        [DisplayName("Текст")]
        public string Text
        {
            get 
            {
                if (control != null)
                { return control.Text; }
                else
                { return text; }
            }
            set
            {
                if (control != null)
                { control.Text = value; }
                text = value;
            }
        }

        //[XmlIgnore]
        Font fontchar;
        //[Browsable(false)]
        //public string FontSerializable
        //{
        //    get { return TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(FontChar); }//fontchar); }
        //    set { fontchar = (Font)TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(value); }
        //}
        //[XmlIgnore]
        [DisplayName("Шрифт")]
        public Font FontChar
        {
            get
            {
                if (control != null)
                { return control.Font; }
                else
                { return fontchar; }
            }
            set
            {
                if (control != null)
                { control.Font = value; }
                fontchar = value;
            }
        }

        //[XmlIgnore]
        Color colorchar;
        //[Browsable(false)]
        //public int ARGBColorChar
        //{
        //    get { return ColorChar.ToArgb(); }
        //    set { colorchar = Color.FromArgb(value); }
        //}
        //[XmlIgnore]
        [DisplayName("Цвет символов")]
        public Color ColorChar
        {
            get
            {
                if (control != null)
                { return control.ForeColor; }
                else
                { return colorchar; }
            }
            set
            {
                if (control != null)
                { control.ForeColor = value; }
                colorchar = value;
            }
        }

        //[XmlIgnore]
        Color colorback;
        //[Browsable(false)]
        //public int ARGBColorBack
        //{
        //    get { return ColorBack.ToArgb(); }
        //    set { colorback = Color.FromArgb(value); }
        //}
        //[XmlIgnore]
        [DisplayName("Цвет фона")]
        public Color ColorBack
        {
            get
            {
                if (control != null)
                { return control.BackColor; }
                else
                { return colorback; }
            }
            set
            {
                if (control != null)
                { control.BackColor = value; }
                colorback = value;
            }
        }

        //[XmlEnum]
        ContentAlignment textalign;
        [DisplayName("Выравнивание текста")]
        public ContentAlignment TextAlign
        {
            get
            {
                if (control != null)
                { return ((Button)control).TextAlign; }
                else
                { return textalign; }
            }
            set
            {
                if (control != null)
                { ((Button)control).TextAlign = value; }
                textalign = value;
            }
        }

                                                             // Может переход на предидущий на первый вопрос и тд
        bool nextquestion=true;
        [DisplayName("Переход на следующий вопрос")]
        public bool NextQuestion
        {
            get { return nextquestion; }
            set { nextquestion = value; }
        }

        [NonSerialized]
        public bool isClick = false;   // Для вщзврата нажата или нет кнопка

        int numericvalueclick;
        [DisplayName("Кнопка нажималась")]
        public int NumericValueClick
        {
            get { return numericvalueclick; }
            set { numericvalueclick = value; }
        }

        int numericvaluenoclick;
        [DisplayName("Нажатия небыло")]
        public int NumericValueNoClick
        {
            get { return numericvaluenoclick; }
            set { numericvaluenoclick = value; }
        }


        public ElementButton()
            : base()
        {
        }

        public override //Control 
                       void CreateControl(bool isDeSerialize = false)            
        {
            ControlButton b = new ControlButton(null, onbasecontrol);
            if (!isDeSerialize)
            {
                this.fontchar = b.Font;
                this.colorchar = b.ForeColor;
                this.colorback = b.BackColor;
                this.textalign = b.TextAlign;
                this.text = b.Text;
            }
            else
            {
                b.Font = this.fontchar;
                b.ForeColor = this.colorchar;
                b.BackColor = this.colorback;
                b.TextAlign = this.textalign;
                b.Text = this.text;
            }
            //this.Size = b.Size;       // По умолчанию размеры при созданиии online     
            b.Parent = null;            
            b.Name = b.Name;
            //text = b.Text;
            control = b;            
            base.CreateControl(isDeSerialize);            
        }

        public override object ResultObject()
        {
            bool b = isClick;  // Должна быть истина при нажатии кнопки
            return b;
        }

        public override int ResultNumber(object o)
        {
            int n = 0;
            if ((bool)o)
            {
                n = numericvalueclick;
            }
            else
            {
                n = numericvaluenoclick;
            }
            return n;
        }

    }

    [Serializable]
    public class ElementRichTextBox : ElementControl
    {

        //static int index = 0;

        public ElementRichTextBox()
        {
        }


        string rtftext;
        //[ReadOnly(true)]
        [DisplayName("Изменение текста")]
        [Editor(typeof(EditRichText), typeof(UITypeEditor))]
        public string RtfText
        {
            get
            {
                if (control != null)
                { return ((ControlRichTextBox)control).Rtf; }
                else
                { return rtftext; }
            }
            set
            {
                if (control != null)
                {
                    try
                    {
                        ((ControlRichTextBox)control).Rtf = value;
                        rtftext = value;
                    }
                    catch
                    { }
                }
                else
                { rtftext = value; }
            }
        }

        //[XmlIgnore]
        Color colorback;
        //[Browsable(false)]
        //public int ARGBColorBack
        //{
        //    get { return ColorBack.ToArgb(); }
        //    set { colorback = Color.FromArgb(value); }
        //}
        //[XmlIgnore]
        [DisplayName("Цвет фона")]
        public Color ColorBack
        {
            get
            {
                if (control != null)
                { return control.BackColor; }
                else
                { return colorback; }
            }
            set
            {
                if (control != null)
                { control.BackColor = value; }
                colorback = value;
            }
        }

        [XmlEnum]
        BorderStyle borderstyle;
        [DisplayName("Бордюр")]
        public BorderStyle BorderStyle
        {
            get
            {
                if (control != null)
                { return ((RichTextBox)control).BorderStyle; }
                else
                { return this.borderstyle; }
            }
            set
            {
                if (control != null)
                { ((RichTextBox)control).BorderStyle = value; }
                this.borderstyle = value;
            }
        }

        public override //Control 
                        void CreateControl(bool isDeSerialize = false)
        {
            //index++;
            ControlRichTextBox b = new ControlRichTextBox(null);
            if (!isDeSerialize)
            {
                this.rtftext = b.Rtf;                
                this.colorback = b.BackColor;
                this.borderstyle = b.BorderStyle;
            }
            else
            {
                b.Rtf = this.rtftext;
                b.BackColor = this.colorback;
                b.BorderStyle = this.borderstyle;
            }
            //this.Size = b.Size;       // По умолчанию размеры при созданиии online 
            b.Parent = null;
            b.Name = b.Name;
            //b.Tag = this;
            control = b;
            //return b;
            base.CreateControl(isDeSerialize);
        }

    }

    [Serializable]
    public class ElementRadioButton : ElementControl
    {
        //static int index = 0;        

        string text = "";
        [DisplayName("Текст")]
        public string Text
        {
            get 
            {
                if (control != null)
                { return control.Text; }
                else
                { return text; }
            }
            set
            {                
                if (control != null)
                { control.Text = value; }
                text = value;
            }
        }

        //[XmlIgnore]
        Font fontchar;
        //[Browsable(false)]
        //public string FontSerializable
        //{
        //    get { return TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(FontChar); }
        //    set { fontchar = (Font)TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(value); }
        //}
        //[XmlIgnore]
        [DisplayName("Шрифт")]
        public Font FontChar
        {
            get
            {
                if (control != null)
                { return control.Font; }
                else
                { return fontchar; }
            }
            set
            {
                if (control != null)
                { control.Font = value; }
                fontchar = value;
            }
        }

        //[XmlIgnore]
        Color colorchar;
        //[Browsable(false)]
        //public int ARGBColorChar
        //{
        //    get { return ColorChar.ToArgb(); }
        //    set { colorchar = Color.FromArgb(value); }
        //}
        //[XmlIgnore]
        [DisplayName("Цвет символов")]
        public Color ColorChar
        {
            get
            {
                if (control != null)
                { return control.ForeColor; }
                else
                { return colorchar; }
            }
            set
            {
                if (control != null)
                { control.ForeColor = value; }
                colorchar = value;
            }
        }

        //[XmlIgnore]
        Color colorback;
        //[Browsable(false)]
        //public int ARGBColorBack
        //{
        //    get { return ColorBack.ToArgb(); }
        //    set { colorback = Color.FromArgb(value); }
        //}
        //[XmlIgnore]
        [DisplayName("Цвет фона")]
        public Color ColorBack
        {
            get
            {
                if (control != null)
                { return control.BackColor; }
                else
                { return colorback; }
            }
            set
            {
                if (control != null)
                { control.BackColor = value; }
                colorback = value;
            }
        }

        //[XmlEnum]
        ContentAlignment textalign;
        [DisplayName("Выравнивание текста")]
        public ContentAlignment TextAlign
        {
            get
            {
                if (control != null)
                { return ((RadioButton)control).TextAlign; }
                else
                { return textalign; }
            }
            set
            {
                if (control != null)
                { ((RadioButton)control).TextAlign = value; }
                textalign = value;
            }
        }

        //[XmlEnum]
        ContentAlignment checkalign;
        [DisplayName("Выравнивание кнопки")]
        public ContentAlignment CheckAlign
        {
            get
            {
                if (control != null)
                { return ((RadioButton)control).CheckAlign; }
                else
                { return checkalign; }
            }
            set
            {
                if (control != null)
                { ((RadioButton)control).CheckAlign = value; }
                checkalign = value;
            }
        }

        int numericvaluecheck;
        [DisplayName("Значение если есть отметка")]
        public int NumericValueCheck
        {
            get { return numericvaluecheck; }
            set { numericvaluecheck = value; }
        }

        int numericvalueoff;
        [DisplayName("Значение если нет отметки")]
        public int NumericValueOff
        {
            get { return numericvalueoff; }
            set { numericvalueoff = value; }
        }


        public ElementRadioButton()
        {
        }

        public override //Control 
                        void CreateControl(bool isDeSerialize = false)
        {
            //index++;
            ControlRadioButton b = new ControlRadioButton(null, onbasecontrol);
            if (!isDeSerialize)
            {
                this.fontchar = b.Font;
                this.colorchar = b.ForeColor;
                this.colorback = b.BackColor;
                this.textalign = b.TextAlign;
                this.checkalign = b.CheckAlign;
                this.text = b.Text;
            }
            else
            {
                b.Font = this.fontchar;
                b.ForeColor = this.colorchar;
                b.BackColor = this.colorback;
                b.TextAlign = this.textalign;
                b.CheckAlign = this.checkalign;
                b.Text = this.text;
            }
            //this.Size = b.Size;       // По умолчанию размеры при созданиии online 
            b.Parent = null;
            b.Name = b.Name;
            //text = b.Text;
            //b.Tag = this;
            control = b;
            //return b;
            base.CreateControl(isDeSerialize);
        }

        public override object ResultObject()
        {
            bool b = ((ControlRadioButton)control).Checked;  // 
            return b;
        }

        public override int ResultNumber(object o)
        {
            int n = 0;
            if ((bool)o)
            {
                n = numericvaluecheck;
            }
            else
            {
                n = numericvalueoff;
            }
            return n;
        }
    }

    [Serializable]
    public class ElementCheckBox : ElementControl
    {
        //static int index = 0;

        /*
        RightToLeft rightToleft = RightToLeft.No;
        [DisplayName("С права на лево")]
        public RightToLeft rightToLeft
        {
            get { return rightToleft; }
            set
            {
                rightToleft = value;
                if (control != null)
                {
                    control.RightToLeft = rightToleft;
                }
            }
        }
        */

        string text = "";
        [DisplayName("Текст")]
        public string Text
        {
            get
            {
                if (control != null)
                { return control.Text; }
                else
                { return text; }
            }
            set
            {
                if (control != null)
                { control.Text = value; }
                text = value;
            }
        }

        //[XmlIgnore]
        Font fontchar;
        //[Browsable(false)]
        // public string FontSerializable
        //{
        //    get { return TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(FontChar); }
        //    set { fontchar = (Font)TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(value); }
        //}
        //[XmlIgnore]
        [DisplayName("Шрифт")]
        public Font FontChar
        {
            get
            {
                if (control != null)
                { return control.Font; }
                else
                { return fontchar; }
            }
            set
            {
                if (control != null)
                { control.Font = value; }
                fontchar = value;
            }
        }

        //[XmlIgnore]
        Color colorchar;
        //[Browsable(false)]
        //public int ARGBColorChar
        //{
        //    get { return ColorChar.ToArgb(); }
        //    set { colorchar = Color.FromArgb(value); }
        //}
        //[XmlIgnore]
        [DisplayName("Цвет символов")]
        public Color ColorChar
        {
            get
            {
                if (control != null)
                { return control.ForeColor; }
                else
                { return colorchar; }
            }
            set
            {
                if (control != null)
                { control.ForeColor = value; }
                colorchar = value;
            }
        }

        //[XmlIgnore]
        Color colorback;
        //[Browsable(false)]
        //public int ARGBColorBack
        //{
        //    get { return ColorBack.ToArgb(); }
        //    set { colorback = Color.FromArgb(value); }
        //}
        //[XmlIgnore]
        [DisplayName("Цвет фона")]
        public Color ColorBack
        {
            get
            {
                if (control != null)
                { return control.BackColor; }
                else
                { return colorback; }
            }
            set
            {
                if (control != null)
                { control.BackColor = value; }
                colorback = value;
            }
        }

        //[XmlEnum]
        ContentAlignment textalign;
        [DisplayName("Выравнивание текста")]
        public ContentAlignment TextAlign
        {
            get
            {
                if (control != null)
                { return ((CheckBox)control).TextAlign; }
                else
                { return textalign; }
            }
            set
            {
                if (control != null)
                { ((CheckBox)control).TextAlign = value; }
                textalign = value;
            }
        }

        //[XmlEnum]
        ContentAlignment checkalign;
        [DisplayName("Выравнивание кнопки")]
        public ContentAlignment CheckAlign
        {
            get
            {
                if (control != null)
                { return ((CheckBox)control).CheckAlign; }
                else
                { return checkalign; }
            }
            set
            {
                if (control != null)
                { ((CheckBox)control).CheckAlign = value; }
                checkalign = value;
            }
        }

        int numericvaluecheck;
        [DisplayName("Значение если есть отметка")]
        public int NumericValueCheck
        {
            get { return numericvaluecheck; }
            set { numericvaluecheck = value; }
        }

        int numericvalueoff;
        [DisplayName("Значение если нет отметки")]
        public int NumericValueOff
        {
            get { return numericvalueoff; }
            set { numericvalueoff = value; }
        }


        public ElementCheckBox()
        {
        }

        public override //Control 
                        void CreateControl(bool isDeSerialize = false)
        {
            //index++;
            ControlCheckBox b = new ControlCheckBox(null, onbasecontrol);
            if (!isDeSerialize)
            {
                this.fontchar = b.Font;
                this.colorchar = b.ForeColor;
                this.colorback = b.BackColor;
                this.textalign = b.TextAlign;
                this.checkalign = b.CheckAlign;
                this.text = b.Text;
            }
            else
            {
                b.Font = this.fontchar;
                b.ForeColor = this.colorchar;
                b.BackColor = this.colorback;
                b.TextAlign = this.textalign;
                b.CheckAlign = this.checkalign;
                b.Text = this.text;
            }
            //this.Size = b.Size;       // По умолчанию размеры при созданиии online 
            b.Parent = null;
            b.Name = b.Name;
            //text = b.Text;
            //b.Tag = this;
            control = b;
            //return b;
            base.CreateControl(isDeSerialize);
        }

        public override object ResultObject()
        {
            bool b = ((ControlCheckBox)control).Checked;  // 
            return b;
        }

        public override int ResultNumber(object o)
        {
            int n = 0;
            if ((bool)o)
            {
                n = numericvaluecheck;
            }
            else
            {
                n = numericvalueoff;
            }
            return n;
        }
    }

    [Serializable]
    public class ElementTextBox : ElementControl
    {
        //static int index = 0;
        

        string text = "";
        [DisplayName("Текст")]
        public string Text
        {
            get
            {
                if (control != null)
                { return control.Text; }
                else
                { return text; }
            }
            set
            {
                if (control != null)
                { control.Text = value; }
                text = value;
            }
        }

        //[XmlIgnore]
        Font fontchar;
        //[Browsable(false)]
        //public string FontSerializable
        //{
        //    get { return TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(FontChar); }
        //    set { fontchar = (Font)TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(value); }
        //}
        //[XmlIgnore]
        [DisplayName("Шрифт")]
        public Font FontChar
        {
            get
            {
                if (control != null)
                { return control.Font; }
                else
                { return fontchar; }
            }
            set
            {
                // Пересчитать высотк подложки при измеении шрифта !!!!!!!!!!!!!!!!!!!!!!!
                if (control != null)
                {
                    control.Font = value;
                    if ((control.Parent != null) && (control.Parent is ControlSubstrate))
                    {
                        control.Parent.Size = control.Size;
                        size = control.Size;
                    }
                }
                fontchar = value;
            }
        }

        //[XmlIgnore]
        Color colorchar;
        //[Browsable(false)]
        //public int ARGBColorChar
        //{
        //    get { return ColorChar.ToArgb(); }
        //    set { colorchar = Color.FromArgb(value); }
        //}
        //[XmlIgnore]
        [DisplayName("Цвет символов")]
        public Color ColorChar
        {
            get
            {
                if (control != null)
                { return control.ForeColor; }
                else
                { return colorchar; }
            }
            set
            {
                if (control != null)
                { control.ForeColor = value; }
                colorchar = value;
            }
        }

        //[XmlIgnore]
        Color colorback;
        //[Browsable(false)]
        //public int ARGBColorBack
        //{
        //    get { return ColorBack.ToArgb(); }
        //    set { colorback = Color.FromArgb(value); }
        //}
        //[XmlIgnore]
        [DisplayName("Цвет фона")]
        public Color ColorBack
        {
            get
            {
                if (control != null)
                { return control.BackColor; }
                else
                { return colorback; }
            }
            set
            {
                if (control != null)
                { control.BackColor = value; }
                colorback = value;
            }
        }

        //[XmlEnum]
        BorderStyle borderstyle;
        [DisplayName("Бордюр")]
        public BorderStyle BorderStyle
        {
            get
            {
                if (control != null)
                { return ((TextBox)control).BorderStyle; }
                else
                { return this.borderstyle; }
            }
            set
            {
                // Пересчитать высотк подложки при измеении бордюра !!!!!!!!!!!!!!!!!!!!!!!
                if (control != null)
                {
                    ((TextBox)control).BorderStyle = value;
                    if ((control.Parent != null) && (control.Parent is ControlSubstrate))
                    {
                        control.Parent.Size = control.Size;
                        size = control.Size;
                    }
                }
                this.borderstyle = value;
            }
        }

        //[XmlEnum]
        HorizontalAlignment textalign;
        [DisplayName("Выравнивание текста")]
        public HorizontalAlignment TextAlign
        {
            get
            {
                if (control != null)
                { return ((TextBox)control).TextAlign; }
                else
                { return textalign; }
            }
            set
            {
                if (control != null)
                { ((TextBox)control).TextAlign = value; }
                textalign = value;
            }
        }

        string textaetalon;                            // Список эталонов и значений им соответствущих
        [DisplayName("Эталон текста")]
        public string TextaEtalon
        {
            get { return textaetalon; }
            set { textaetalon = value; }
        }

        int numericvalueequal;
        [DisplayName("Текст равен введеному")]
        public int NumericValueEqual
        {
            get { return numericvalueequal; }
            set { numericvalueequal = value; }
        }

        int numericvaluenoequal;
        [DisplayName("Текст не равен введеному")]
        public int NumericValueNoEqual
        {
            get { return numericvalueequal; }
            set { numericvalueequal = value; }
        }


        public ElementTextBox()
        {
        }

        public override //Control 
                        void CreateControl(bool isDeSerialize = false)
        {
            //index++;
            ControlTextBox b = new ControlTextBox(null, onbasecontrol);
            if (!isDeSerialize)
            {
                this.fontchar = b.Font;
                this.colorchar = b.ForeColor;
                this.colorback = b.BackColor;
                this.textalign = b.TextAlign;
                this.borderstyle = b.BorderStyle;
                this.text = b.Text;
            }
            else
            {
                b.Font = this.fontchar;
                b.ForeColor = this.colorchar;
                b.BackColor = this.colorback;
                b.TextAlign = this.textalign;
                b.BorderStyle = this.borderstyle;
                b.Text = this.text;
            }
//            this.Size = b.Size;       // По умолчанию размеры при созданиии online 
            b.Parent = null;
            b.Name = b.Name;
            //text = b.Text;
            //b.Tag = this;
            control = b;
            //return b;
            base.CreateControl(isDeSerialize);
        }

        public override object ResultObject()
        {
            string s = ((ControlTextBox)control).Text;  // 
            return s;
        }

        public override int ResultNumber(object o)
        {
            int n = 0;
            if ((string)o== textaetalon)
            {
                n = numericvalueequal;
            }
            else
            {
                n = numericvaluenoequal;
            }
            return n;
        }

    }

    [Serializable]
    public class ElementPictureBox : ElementControl
    {
        //static int index = 0;
        /*
        string text = "";
        [DisplayName("Рисунок")]
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                if (control != null)
                {
                    control.Text = text;
                }
            }
        }
        */

        //private string filename;
        
        //[Browsable(false)]
        Image image;

        [Browsable(false)]
        public Image Im
        {
            get
            {
                if (control != null)
                { return ((ControlPictureBox)control).Image; }
                else { return image; }
            }
            set 
            {
                if (control != null)
                { ((ControlPictureBox)control).Image = value; }
                image = value;
            }
        }
        
        //[Browsable(false)]
        //public string ImageSerializable
        //{
        //    get { return TypeDescriptor.GetConverter(typeof(Image)).ConvertToString(imagefile); }
        //    set { imagefile = (Image)TypeDescriptor.GetConverter(typeof(Image)).ConvertFromString(value); }
        //}

        //[XmlIgnore]
        [DisplayName("Загрузка из файла")]
        [Editor(typeof(OpenDlg), typeof(UITypeEditor))]
        public string Filename
        {
            get
            {
                return "";//filename;
            }
            set
            {
                try
                {
                    //image = Image.FromFile(value);
                    //((ControlPictureBox)control).Image = image;
                    Im = Image.FromFile(value);                    
                }
                catch (Exception e)
                {
                    MessageBox.Show(value+" - "+e.Message);
                }
            }
        }

        //[XmlIgnore]
        [DisplayName("Загрузка из буфера обмена")]
        [Editor(typeof(NullDlg), typeof(UITypeEditor))]
        public string Clip
        {
            get
            {
                return "";//filename;
            }
            set
            {
                if (value == "1")
                {
                    Image im = Form1.PastePictureFromClipbord();
                    if (im != null)
                    {
                        //im.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        //image = im;
                        //((ControlPictureBox)control).Image = im;
                        Im = im;
                    }
                }
            }
        }

        //[XmlEnum]
        PictureBoxSizeMode sizemode;
        [DisplayName("Вид отображения")]
        public PictureBoxSizeMode SizeMode
        {
            get
            {
                if (control != null)
                { return ((ControlPictureBox)control).SizeMode; }
                else
                { return sizemode; }
            }
            set
            {
                if (control != null)
                { ((ControlPictureBox)control).SizeMode = value; }
                sizemode = value;
            }
        }

        //[XmlIgnore]
        Color colorback;
        [Browsable(false)]
        //public int ARGBColorBack
        //{
        //    get { return ColorBack.ToArgb(); }
        //    set { colorback = Color.FromArgb(value); }
        //}
        //[XmlIgnore]
        [DisplayName("Цвет фона")]
        public Color ColorBack
        {
            get
            {
                if (control != null)
                { return control.BackColor; }
                else
                { return colorback; }
            }
            set
            {
                if (control != null)
                { control.BackColor = value; }
                colorback = value;
            }
        }

        [XmlEnum]
        BorderStyle borderstyle;
        [DisplayName("Бордюр")]
        public BorderStyle BorderStyle
        {
            get
            {
                if (control != null)
                { return ((PictureBox)control).BorderStyle; }
                else
                { return this.borderstyle; }
            }
            set
            {
                // Пересчитать высотк подложки при измеении бордюра !!!!!!!!!!!!!!!!!!!!!!!
                if (control != null)
                { ((PictureBox)control).BorderStyle = value; }
                this.borderstyle = value;
            }
        }

        public ElementPictureBox()
        {
        }

        public override //Control 
                       void CreateControl(bool isDeSerialize = false)
        {
            ControlPictureBox b = new ControlPictureBox(null, false);
            if (!isDeSerialize)
            {
                this.sizemode = b.SizeMode;
                this.colorback = b.BackColor;
                this.borderstyle = b.BorderStyle;
                this.image = b.Image;
            }
            else
            {
                b.SizeMode = this.sizemode;
                b.BackColor = this.colorback;
                b.BorderStyle = this.borderstyle;
                b.Image = this.image;
            }
//            this.Size = b.Size;       // По умолчанию размеры при созданиии online     
            b.Parent = null;
            b.Name = b.Name;
            //text = b.Text;
            control = b;
            base.CreateControl(isDeSerialize);
        }
    }


//**********
    [Serializable]
    public class ElementLabel : ElementControl
    {
        //static int index = 0;
/*
        RightToLeft rightToleft = RightToLeft.No;
        [DisplayName("С права на лево")]
        public RightToLeft rightToLeft
        {
            get { return rightToleft; }
            set
            {
                rightToleft = value;
                if (control != null)
                {
                    control.RightToLeft = rightToleft;
                }
            }
        }
        */
        string text = "";
        [DisplayName("Текст")]
        public string Text
        {
            get
            {
                if (control != null)
                { return control.Text; }
                else
                { return text; }
            }
            set
            {
                if (control != null)
                { control.Text = value; }
                text = value;
            }
        }

        Font fontchar;
        [DisplayName("Шрифт")]
        public Font FontChar
        {
            get
            {
                if (control != null)
                { return control.Font; }
                else
                { return fontchar; }
            }
            set
            {
                if (control != null)
                { control.Font = value; }
                fontchar = value;
            }
        }

        Color colorchar;
        [DisplayName("Цвет символов")]
        public Color ColorChar
        {
            get
            {
                if (control != null)
                { return control.ForeColor; }
                else
                { return colorchar; }
            }
            set
            {
                if (control != null)
                { control.ForeColor = value; }
                colorchar = value;
            }
        }

        Color colorback;
        [DisplayName("Цвет фона")]
        public Color Colorbackr
        {
            get
            {
                if (control != null)
                { return control.BackColor; }
                else
                { return colorback; }
            }
            set
            {
                if (control != null)
                { control.BackColor = value; }
                colorback = value;
            }
        }

        ContentAlignment textalign;
        [DisplayName("Выравнивание текста")]
        public ContentAlignment TextAlign
        {
            get
            {
                if (control != null)
                { return ((Label)control).TextAlign; }
                else
                { return textalign; }
            }
            set
            {
                if (control != null)
                { ((Label)control).TextAlign = value; }
                textalign = value;
            }
        }

        BorderStyle borderstyle;
        [DisplayName("Бордюр")]
        public BorderStyle BorderStyle
        {
            get
            {
                if (control != null)
                { return ((ControlLabel)control).BorderStyle; }
                else
                { return this.borderstyle; }
            }
            set
            {
                if (control != null)
                { ((ControlLabel)control).BorderStyle = value; }
                this.borderstyle = value;
            }
        }

        public ElementLabel()
        {
        }

        public override //Control 
                        void CreateControl(bool isDeSerialize = false)
        {
            //index++;
            ControlLabel b = new ControlLabel(null, false);
            if (!isDeSerialize)
            {
                this.fontchar = b.Font;
                this.colorchar = b.ForeColor;
                this.colorback = b.BackColor;
                this.textalign = b.TextAlign;
                this.borderstyle = b.BorderStyle;
                this.text = b.Text;
            }
            else
            {
                b.Font = this.fontchar;
                b.ForeColor = this.colorchar;
                b.BackColor = this.colorback;
                b.TextAlign = this.textalign;
                b.BorderStyle = this.borderstyle;
                b.Text = this.text;
            }
            //this.Size = b.Size;       // По умолчанию размеры при созданиии online 
            b.Parent = null;
            b.Name = b.Name;
            //text = b.Text;            
            control = b;
            base.CreateControl(isDeSerialize);
        }
    }


    //***************************************************************************************************************

    abstract class CreateElement
    {
        protected string name = null;
        public string Name 
        {
            get { return name; }
        }

        public abstract Element Create();
        public override string ToString()
        {
            return Name;//base.ToString();
        }
    }

    class CreateElementButton : CreateElement
    {
        public CreateElementButton()
        {
            name = "Кнопка";
        }

        public override Element Create()
        {
            return new ElementButton();
        }
    }

    class CreateElementRichTextBox : CreateElement
    {
        public CreateElementRichTextBox()
        {
            name = "Текст";
        }

        public override Element Create()
        {
            return new ElementRichTextBox();
        }
    }

    class CreateElementRadioButton : CreateElement
    {
        public CreateElementRadioButton()
        {
            name = "Переключатель";
        }

        public override Element Create()
        {
            return new ElementRadioButton();
        }
    }

    class CreateElementCheckBox : CreateElement
    {
        public CreateElementCheckBox()
        {
            name = "Флаговая кнопка";
        }

        public override Element Create()
        {
            return new ElementCheckBox();
        }
    }

    class CreateElementTextBox : CreateElement
    {
        public CreateElementTextBox()
        {
            name = "Ввод текста";
        }

        public override Element Create()
        {
            return new ElementTextBox();
        }
    }

    class CreateElementPictureBox : CreateElement
    {
        public CreateElementPictureBox()
        {
            name = "Рисунок";
        }

        public override Element Create()
        {
            return new ElementPictureBox();
        }
    }

    class CreateElementLabel : CreateElement
    {
        public CreateElementLabel()
        {
            name = "Надпись";
        }

        public override Element Create()
        {
            return new ElementLabel();
        }
    }

    //***************************************************************************************************************

    class ArrayCreateElement 
    {
        List<CreateElement> ListCreate=null;

        public CreateElement this[int idx]
        {
            get { return ListCreate[idx]; }
            set { ListCreate[idx] = value; }
        }

        public ArrayCreateElement()
        {
            ListCreate = new List<CreateElement>();
        }

        public void Add(CreateElement e)
        {
            ListCreate.Add(e);
        }

        public void Del(int idx)
        {
            ListCreate.RemoveAt(idx);
        }
    }
}
