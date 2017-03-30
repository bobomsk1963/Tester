using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
namespace Tester
{
    // Вспомогательные объекты

    class RadioButtonPanel : RadioButton  // Отключение возможностит фокуса
    {
        public RadioButtonPanel()   
        {
            this.SetStyle(ControlStyles.Selectable, false);
            TabStop = false;
        }
    }

    public class OpenDlg : UITypeEditor
    {
        public OpenDlg()
        {
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            //dialog.Title = "Открыть";
            //dialog.InitialDirectory = (string)value;
            dialog.Filter = "Все файлы изображений (*.png)|*.jpg;*.bmp;*.gif;*.tif;*.png|" +
                            "JPEG (*.jpg)|*.jpg|" +
                            "Точечный рисунок (*.bmp)|*.bmp|" +
                            "Gif (*.gif)|*.gif|" +
                            "TIFF (*.tif)|*.tif|" +
                            "PNG (*.png)|*.png";

            //openFileDialog1.FilterIndex = 2;
            //openFileDialog1.RestoreDirectory = true;           

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                dialog.Dispose();
                return dialog.FileName;
            }
            dialog.Dispose();
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {

            return UITypeEditorEditStyle.Modal; //чтобы было [...]
        }
    }

    public class NullDlg : UITypeEditor
    {
        public NullDlg()
        {
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if ((string)value != "")
            { return ""; }
            else
            {
                return "1";
            }
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {

            return UITypeEditorEditStyle.Modal; //чтобы было [...]
        }
    }

    public class EditRichText : UITypeEditor
    {
        public EditRichText()
        {
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {

            ControlSelection sel = ControlSelection.Instance;


            FormRichTextBox dialog = new FormRichTextBox(sel.CD.EControl.BackColor);

            //ControlSelection sel = ControlSelection.Instance;
            System.Drawing.Rectangle r = sel.C.RectangleToScreen(sel.C.DisplayRectangle);

            dialog.SetBounds(r.Left-1,r.Top-1,r.Width+26,r.Height+2);//sel.C.Location;            

            dialog.text = (string)value;

            string rtf;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                rtf = dialog.text;
            }
            else
            {
                rtf = (string)value;
            }
            
            dialog.Dispose();
            return rtf;
            //}
            //dialog.Dispose();
            //return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {

            return UITypeEditorEditStyle.Modal; //чтобы было [...]
        }
    }


}
