using System;
using System.Windows.Forms;

namespace Hemming
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int k = 0;
            int[] ar = new int[13];
            // разделяем биты переданного сообщения и котрольные биты 
            for (int i = 0; i < textBox2.Text.Length; i++)
            {
                if ((i + 1) != 1 && (i + 1) != 2 && (i + 1) != 4 && (i + 1) != 8 && (i + 1) != 16)
                {
                    ar[k] = int.Parse(textBox2.Text[i].ToString());
                    k++;
                }
            }

            string s = "";
            foreach (int i in ar)
            {
                s += i.ToString();
            }

            // расчиываем контрольные биты
            int[] array_rith = encode(s);

            int[] array_wrong = new int[18];

            for (int i = 0; i < 18; i++)
            {
                array_wrong[i] = int.Parse(textBox2.Text[i].ToString());
            }

            // находим сумму индексов несовпадающих контрольных бит
            int sum = sum_wrong_bite(array_rith, array_wrong);

            // если совпадений не найденно
            if (sum == 0)
            {
                MessageBox.Show(
                "Раскодированная последовательность бит " + rith(array_wrong),
                "Все верно",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
            }
            // если ошибка не в контрольных битах
            // else if (sum - 1 != 0 && sum - 1 != 1 && sum - 1 != 3 && sum - 1 != 7 && sum - 1 != 15)
            // {
            //     array_wrong[sum - 1] = invers(array_wrong[sum - 1]);

            //     s = "";
            //     foreach (int i in array_wrong)
            //     {
            //         s += i.ToString();
            //     }

            //     MessageBox.Show(
            //     "Правильный вариант " + s + "\n" + "Раскодированная последовательность бит " + rith(array_wrong),
            //     "Ошибка сожержиться в " + sum + " бите",
            //     MessageBoxButtons.OK,
            //     MessageBoxIcon.Information,
            //     MessageBoxDefaultButton.Button1,
            //     MessageBoxOptions.DefaultDesktopOnly);
            // }
            else
            // если ошибка в контрольных битах
            {
                array_wrong[sum - 1] = invers(array_wrong[sum - 1]);

                s = "";
                foreach (int i in array_wrong)
                {
                    s += i.ToString();
                }

                MessageBox.Show(
                "Правильный вариант " + s + "\n" + "Раскодированная последовательность бит " + rith(array_wrong),
                "Ошибка сожержиться в дополнительном " + sum + "бите",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private string rith(int[] array_wrong)
        {
            // разделяем биты переданного сообщения и котрольные биты
            int k = 0;
            int[] decode = new int[13];
            for (int i = 0; i < array_wrong.Length; i++)
            {
                if ((i + 1) != 1 && (i + 1) != 2 && (i + 1) != 4 && (i + 1) != 8 && (i + 1) != 16)
                {
                    decode[k] = array_wrong[i];
                    k++;
                }
            }

            string s = "";

            foreach (int i in decode)
            {
                s += i.ToString();
            }

            return s;
        }

        private int invers(int v)
        {
            return (v == 1) ? 0 : 1;
        }

        private int sum_wrong_bite(int[] array_rith, int[] array_wrong)
        {
            // сравниваем контрольные биты в полученном сообщении и вычесленном
            // по сумме индексов несовпадающих контрольных бит 
            // можно определить место ошибки
            int sum = 0;
            for (int i = 0; i < 5; i++)
            {
                if (array_rith[(int)Math.Pow(2, i) - 1] != array_wrong[(int)Math.Pow(2, i) - 1])
                    sum += (int)Math.Pow(2, i);
            }
            return sum;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            in_lable(textBox1.Text.ToCharArray());
            ful_in_lable(encode(textBox1.Text));
        }

        private int[] encode(string text)
        {
            char[] ch = text.ToCharArray();
            int[] full_array = new int[18];

            // заполняем массив не трогая 0,1,3,7,15 места(биты)
            int k = 0;
            for (int i = 0; i < full_array.Length; i++)
            {
                if ((i + 1) != 1 && (i + 1) != 2 && (i + 1) != 4 && (i + 1) != 8 && (i + 1) != 16)
                {
                    full_array[i] = int.Parse(ch[k].ToString());
                    k++;
                }
            }

            // вычисляем сколько едениц контролирует 1 бит
            // исходя из этого определяем чему равен этот бит 0 или 1
            int sum = 0;
            for (int i = 0; i < full_array.Length; i += 2)
            {
                if (full_array[i] == 1)
                {
                    sum++;
                }
            }
            if (sum % 2 == 0 && sum != 0) full_array[0] = 1;
            else full_array[0] = 0;

            // вычисляем сколько едениц контролирует 2 бит
            // исходя из этого определяем чему равен этот бит 0 или 1
            sum = 0;
            for (int i = 1; i < full_array.Length; i += 4)
            {
                if (full_array[i] == 1)
                {
                    sum++;
                }
                if (i + 1 < full_array.Length && full_array[i + 1] == 1)
                {
                    sum++;
                }
            }
            if (sum % 2 == 0 && sum != 0) full_array[1] = 1;
            else full_array[1] = 0;

            // вычисляем сколько едениц контролирует 4 бит
            // исходя из этого определяем чему равен этот бит 0 или 1
            sum = 0;
            for (int i = 3; i < full_array.Length; i += 8)
            {
                for (int j = i; j < i + 4 && j < full_array.Length; j++)
                {
                    if (full_array[j] == 1)
                    {
                        sum++;
                    }
                }
            }
            if (sum % 2 == 0 && sum != 0) full_array[3] = 1;
            else full_array[3] = 0;

            // вычисляем сколько едениц контролирует 8 бит
            // исходя из этого определяем чему равен этот бит 0 или 1
            sum = 0;
            for (int i = 7; i < full_array.Length; i += 16)
            {
                for (int j = i; j < i + 8 && j < full_array.Length; j++)
                {
                    if (full_array[j] == 1)
                    {
                        sum++;
                    }
                }
            }
            if (sum % 2 == 0 && sum != 0) full_array[7] = 1;
            else full_array[7] = 0;

            // вычисляем сколько едениц контролирует 32 бит
            // исходя из этого определяем чему равен этот бит 0 или 1
            sum = 0;
            for (int i = 15; i < full_array.Length; i += 32)
            {
                for (int j = i; (j < i + 16) && j < full_array.Length; j++)
                {
                    if (full_array[j] == 1)
                    {
                        sum++;
                    }
                }
            }
            if (sum % 2 == 0 && sum != 0) full_array[15] = 1;
            else full_array[15] = 0;

            // возвращаем закодированный массив
            return full_array;
        }

        private void ful_in_lable(int[] full_array)
        {

            label17.Text = full_array[0].ToString();
            label36.Text = full_array[1].ToString();
            label37.Text = full_array[3].ToString();
            label38.Text = full_array[7].ToString();
            label40.Text = full_array[15].ToString();


            label43.Text = full_array[0].ToString();
            label47.Text = full_array[1].ToString();
            label60.Text = full_array[2].ToString();
            label61.Text = full_array[3].ToString();
            label55.Text = full_array[4].ToString();
            label58.Text = full_array[5].ToString();
            label59.Text = full_array[6].ToString();
            label62.Text = full_array[7].ToString();
            label63.Text = full_array[8].ToString();
            label64.Text = full_array[9].ToString();
            label65.Text = full_array[10].ToString();
            label66.Text = full_array[11].ToString();
            label67.Text = full_array[12].ToString();
            label69.Text = full_array[13].ToString();
            label70.Text = full_array[14].ToString();
            label71.Text = full_array[15].ToString();
            label72.Text = full_array[16].ToString();
            label73.Text = full_array[17].ToString();

        }

        private void in_lable(char[] ch)
        {
            label42.Text = ch[0].ToString();
            label44.Text = ch[1].ToString();
            label45.Text = ch[2].ToString();
            label46.Text = ch[3].ToString();
            label48.Text = ch[4].ToString();
            label49.Text = ch[5].ToString();
            label50.Text = ch[6].ToString();
            label51.Text = ch[7].ToString();
            label52.Text = ch[8].ToString();
            label53.Text = ch[9].ToString();
            label54.Text = ch[10].ToString();
            label56.Text = ch[11].ToString();
            label57.Text = ch[12].ToString();
        }
    }
}
