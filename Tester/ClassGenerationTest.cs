using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tester
{
    public class ClassGenerationTest : IReceiver // Сервер Теста
    {
        ClassTest Test;
        int Index;
        ClassPorter Porter;

        int ResultData;
        // bool isResultWindow = false;

        public ClassGenerationTest(ClassPorter porter)
        {
            Porter = porter;
            Porter.Receiver = this;
            Index = 0;
            ResultData = 0;
        }

        public void InitTest(ClassTest test)
        {
            Test = test;
            Index = 0;
            ResultData = 0;
        }

        bool Next()
        {
            if ((Index + 1) < Test.ListQuestions.Count)
            {
                Index++;
                return true;
            }
            else { return false; }
        }

        public void TakeMessage(MemoryStream ms)
        {
            // Десериализация принятого

            //MessageBox.Show((Index+1).ToString());

            if (ms != null)
            {
                ms.Position = 0;

                ArrayList array = null;

                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    array = (ArrayList)formatter.Deserialize(ms);
                }
                catch (Exception e)
                {
                    MessageBox.Show("? - " + e.Message);
                    array = null;
                }

                if (array != null)   // Если принятый массив не равен нул то подсчитываем значения
                {
                    int n = 0;
                    for (int i = 0; i < array.Count; i++)
                    {
                        n = n + Test[Index].Elements[i].ResultNumber(array[i]);
                    }

                    ResultData = ResultData + n;
                    // MessageBox.Show(n.ToString());
                }

            }

            if (Next())
            {
                SendToWork();
            }
            else
            {


                MessageBox.Show("У вас "+ResultData.ToString()+" баллов.");

                Porter.SendTo(null);
                // Выдача результата                 
                // после чего Остановка или повторение теста

            }
        }

        public void SendToWork()
        {
            MemoryStream m = Test[Index].SerialBinary();
            if (m != null)
            {
                // Сериализация вопроса
                Porter.SendTo(m);
            }            
        }
    }

    class ClassWorkTest : IReceiver // Клиент Теста
    {
        //Question TakeQuestion = null;
        ClassPorter Porter;
        Form F;
        Panel P;
       public Question Q = null;

        public ClassWorkTest(ClassPorter porter,Form f,Panel p)
        {
            Porter = porter;
            Porter.Receiver = this;
            F = f;
            P = p;
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            ((ElementButton)((ControlButton)sender).Tag).isClick = true;
            if (((ElementButton)((ControlButton)sender).Tag).NextQuestion)
            {
                SendToGeneration();
            }
        }

        public void TakeMessage(MemoryStream ms)
        {            

            if (ms != null)
            {                
                ms.Position = 0; //Перевести на начало потока
                Q = Question.DeSerialBinary(ms);

                for (int i = 0; i < Q.Elements.Count; i++)
                {
                    ((ElementControl)Q.Elements[i]).OnBaseControl = true;
                    Q.Elements[i].CreateControl(true);
                    if (Q.Elements[i].EControl is ControlButton)
                    {
                        Q.Elements[i].EControl.Click += new EventHandler(ButtonClick);                        
                    }
                    ((ElementControl)Q.Elements[i]).SetToPanel(P);
                }
            }
            else
            {
                F.Close();
            }
            // Десериализация принятого
        }

        void NewQuestion()
        { 
        }

       public void SendToGeneration()
        {
            MemoryStream ms = new MemoryStream();

            //Формирование массива для передачи гененратору теста

            ArrayList array = new ArrayList();
            for (int i = 0; i < Q.Elements.Count; i++)
            {
                array.Add(Q.Elements[i].ResultObject());               
            }

            //MessageBox.Show(array.Count.ToString());

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, array);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                ms = null;
            }

           //Q // Очистить вопрос
           // Очистить панель на экране
            Q.Dispose();
            Q = null;       

            Porter.SendTo(ms);
        }
    }

    public abstract class ClassPorter
    {
        public IReceiver Receiver 
        { get; set; }               // Объект принимающий данные для принятых данных

        abstract public void SendTo(MemoryStream m);    // Передать
        abstract public void ReceiveTo(MemoryStream m); // Передать принятое
    }

    public interface IReceiver
    {
        void TakeMessage(MemoryStream m);
    }

    //**************
   public class ClassPorterMono : ClassPorter
    {

        public ClassPorter P; //  Для связи         

        public void SetDistantPorter(ClassPorter p)
        {
            P = p;
        }

        public override void SendTo(MemoryStream m)    // Передать
        {
            P.ReceiveTo(m);
        }
        public override void ReceiveTo(MemoryStream m) // Передать принятое
        {
            Receiver.TakeMessage(m);
        }
    }



}
