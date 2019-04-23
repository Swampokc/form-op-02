using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

using Application = Microsoft.Office.Interop.Excel.Application;

namespace form_op_02
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 15; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1[0, i].ReadOnly = true;
                dataGridView1[2, i].ReadOnly = true;
                dataGridView1[3, i].ReadOnly = true;
                dataGridView1[4, i].ReadOnly = true;
                dataGridView1[6, i].ReadOnly = true;
                dataGridView1[7, i].ReadOnly = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                textBox2.Text = "07384";
                textBox3.Text = "5520102";
            }
            else
            {
                textBox2.Text = "93425";
                textBox3.Text = "5520104";
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (e.ColumnIndex == 1)
            {
                if (Convert.ToString(dgv[1, e.RowIndex].Value) == "Борщ")
                    setParamsInRows(e.RowIndex, 254, 91645, 300, 45);
                if (Convert.ToString(dgv[1, e.RowIndex].Value) == "Салат \"Витаминный\"")
                    setParamsInRows(e.RowIndex, 134, 12435, 170, 34);
                if (Convert.ToString(dgv[1, e.RowIndex].Value) == "Окрошка")
                    setParamsInRows(e.RowIndex, 612, 73462, 410, 67);
                if (Convert.ToString(dgv[1, e.RowIndex].Value) == "Пюре")
                    setParamsInRows(e.RowIndex, 923, 27310, 200, 27);
                if (Convert.ToString(dgv[1, e.RowIndex].Value) == "Отбивная из курицы")
                    setParamsInRows(e.RowIndex, 362, 23804, 220, 77);
                if (Convert.ToString(dgv[1, e.RowIndex].Value) == "Салат \"Цезарь\"")
                    setParamsInRows(e.RowIndex, 354, 38362, 255, 58);
                if (Convert.ToString(dgv[1, e.RowIndex].Value) == "Пирожок с картошкой")
                    setParamsInRows(e.RowIndex, 932, 78342, 120, 17);
                if (Convert.ToString(dgv[1, e.RowIndex].Value) == "Солянка")
                    setParamsInRows(e.RowIndex, 836, 73625, 320, 55);
                if (Convert.ToString(dgv[1, e.RowIndex].Value) == "Спагетти")
                    setParamsInRows(e.RowIndex, 834, 54637, 240, 23);
                if (Convert.ToString(dgv[1, e.RowIndex].Value) == "Пицца \"Студенческая\"")
                    setParamsInRows(e.RowIndex, 534, 25536, 140, 34);
            }

            if (e.ColumnIndex == 5)
            {
                calcSum(e.RowIndex);
            }
        }

        private void setParamsInRows(int row, int kod, int kodTTK, int weight, int cost)
        {
            dataGridView1[0, row].Value = row + 1;
            dataGridView1[2, row].Value = kod;
            dataGridView1[3, row].Value = kodTTK;
            dataGridView1[4, row].Value = weight;
            dataGridView1[6, row].Value = cost;
            calcSum(row);
        }

        private void calcSum(int row)
        {
            dataGridView1[7, row].Value = Convert.ToInt32(dataGridView1[5, row].Value) * Convert.ToInt32(dataGridView1[6, row].Value);
            int sum = 0;
            for (int i = 0; i < 15; i++)
                sum += Convert.ToInt32(dataGridView1[7, i].Value);
            label12.Text = Convert.ToString(sum);
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(tb_KeyPress);
        }

        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!(Char.IsDigit(e.KeyChar)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application application;
            Workbook workBook;
            Worksheet worksheet;
            string[] str;

            // Открываем приложение
            application = new Application
                {
                    DisplayAlerts = false
                };

            // Файл шаблона
            const string template = "forma_op-02.xls";

            // Открываем книгу
            workBook = application.Workbooks.Open(Path.Combine(Environment.CurrentDirectory, template));

            // Получаем активную таблицу
            worksheet = workBook.ActiveSheet as Worksheet;

            // Записываем данные
            worksheet.Range["A6"].Value = comboBox1.Text;
            worksheet.Range["A8"].Value = comboBox2.Text;

            worksheet.Range["AN6"].Value = textBox2.Text;
            worksheet.Range["AN9"].Value = textBox3.Text;

            worksheet.Range["AN14"].Value = "Директор";
            str = comboBox3.Text.Split(' ');
            if (checkBox1.Checked)
                worksheet.Range["AK16"].Value = str[0];
            char[] tmp1 = str[1].ToCharArray();
            char[] tmp2 = str[2].ToCharArray();
            worksheet.Range["AQ16"].Value = str[0] + " " + tmp1[0] + ". " + tmp2[0] + ".";

            worksheet.Range["V18"].Value = maskedTextBox1.Text;

            string day = Convert.ToString(dateTimePicker1.Value.Day);
            if (dateTimePicker1.Value.Day < 10)
                day = "0" + Convert.ToString(dateTimePicker1.Value.Day);

            string month = Convert.ToString(dateTimePicker1.Value.Month);
            if (dateTimePicker1.Value.Month < 10)
                month = "0" + Convert.ToString(dateTimePicker1.Value.Month);

            worksheet.Range["AC18"].Value = day + "." + month + "." + dateTimePicker1.Value.Year;

            int initIndex = 30;

            for (int i = 0; i < 15; i++)
            {
                string curIndex = Convert.ToString(initIndex + i);
                worksheet.Range["A" + curIndex].Value = dataGridView1[0, i].Value;
                worksheet.Range["D" + curIndex].Value = dataGridView1[1, i].Value;
                worksheet.Range["Q" + curIndex].Value = dataGridView1[2, i].Value;
                worksheet.Range["T" + curIndex].Value = dataGridView1[3, i].Value;
                worksheet.Range["AC" + curIndex].Value = dataGridView1[4, i].Value;
                worksheet.Range["AG" + curIndex].Value = dataGridView1[5, i].Value;
                worksheet.Range["AK" + curIndex].Value = dataGridView1[6, i].Value;
                worksheet.Range["AQ" + curIndex].Value = dataGridView1[7, i].Value;
            }

            str = comboBox4.Text.Split(' ');
            if (checkBox1.Checked)
                worksheet.Range["P55"].Value = str[0];
            tmp1 = str[1].ToCharArray();
            tmp2 = str[2].ToCharArray();
            worksheet.Range["AC55"].Value = str[0] + " " + tmp1[0] + ". " + tmp2[0] + ".";

            worksheet.Range["AQ53"].Value = label12.Text;

            day = Convert.ToString(dateTimePicker2.Value.Day);
            if (dateTimePicker2.Value.Day < 10)
                day = "0" + Convert.ToString(dateTimePicker2.Value.Day);
            worksheet.Range["AL18"].Value = day;
            worksheet.Range["AO18"].Value = getMonth(dateTimePicker2.Value.Month);
            worksheet.Range["AU18"].Value = dateTimePicker2.Value.Year;

            day = Convert.ToString(dateTimePicker3.Value.Day);
            if (dateTimePicker3.Value.Day < 10)
                day = "0" + Convert.ToString(dateTimePicker3.Value.Day);
            worksheet.Range["K20"].Value = day;
            worksheet.Range["M20"].Value = getMonth(dateTimePicker3.Value.Month);
            worksheet.Range["W20"].Value = dateTimePicker3.Value.Year;

            // Показываем приложение
            application.Visible = true;


            string savedFileName = "План-меню по форме № ОП-2.xlsx";
            workBook.SaveAs(Path.Combine(Environment.CurrentDirectory, savedFileName));
        }

        private string getMonth(int m)
        {
            if (m == 1)
                return "января";
            if (m == 2)
                return "февраля";
            if (m == 3)
                return "марта";
            if (m == 4)
                return "апреля";
            if (m == 5)
                return "мая";
            if (m == 6)
                return "июня";
            if (m == 7)
                return "июля";
            if (m == 8)
                return "августа";
            if (m == 9)
                return "сентября";
            if (m == 10)
                return "октября";
            if (m == 11)
                return "ноября";
            if (m == 12)
                return "декабря";
            return null;
        }
    }
}
