using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Курсачёк__Графы
{



    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Random rnd = new Random();

        public static int count;
        public int[] NEW;//0-новая веришна, 1-старая вершина, 2-стартовая вершина
        public List<int> contour = new List<int>();//список для обхода и записи вершин, которые образуют контур
        public List<Contour> contours = new List<Contour>();//список контуров
        public List<Arch> lst_arch_gf = new List<Arch>();//список дуг графа
        public List<string> str_cont = new List<string>();//список найденыых контуров


        //Заполнение графа: начало
        private void String_Graph(object sender, NumericUpDown numeric, DataGridView matrix)
        {
            count = Convert.ToInt32(numeric.Value);
            NEW = new int[count];

            if (count == 0) return;


            matrix.RowCount = count;
            matrix.ColumnCount = count;



            print_Graph(matrix);

            matrix.AutoResizeColumns();
            matrix.AutoResizeRows();

        }

        private void print_Graph(DataGridView matrix)
        {
            for (int i = 0; i < count; i++)
            {
                NEW[i] = 0;
                for (int j = 0; j < count; j++)
                {
                    if (i == j)
                    {
                        matrix.Rows[i].Cells[j].Value = 0;
                        matrix.Rows[i].Cells[j].ReadOnly = false;

                    }
                    else { matrix.Rows[i].Cells[j].Value = ""; matrix.Rows[i].Cells[j].ReadOnly = false; }
                    matrix.Columns[j].Width = 30;

                }
            }

        }
        //Заполнение графа: конец

        private void Search(DataGridView matrix)
        {
            int[,] matr = new int[count, count];
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    matr[i, j] = Convert.ToInt32(matrix.Rows[i].Cells[j].Value);
                    Arch a = new Arch(i, j, matr[i, j]);
                    if (matr[i, j] != 0) { lst_arch_gf.Add(a); }
                }
            }

            for (int i = 0; i < count; i++)
            {

                NEW[i] = 2;//присваиваем стартовой вершине значение 2

                walk(i, matr, i);
                contour.Clear();
            }
        }

        private void print_contour(List<int> list)//передаём список вершин контура
        {
            string str = "";
            for (int i = 0; i < list.Count; i++)
            {
                str += (list[i] + 1) + "-";
            }
            str += (list[0] + 1);
            str_cont.Add(str);
         //   listBox1.Items.Add(str);

            List<Arch> archs = new List<Arch>();//создание списка дуг контура

            //формирую список дуг контура
            for (int i = 0; i < list.Count - 1; i++)
            {
                Arch a = new Arch(list[i], list[i + 1], Convert.ToInt32(graph.Rows[list[i]].Cells[list[i + 1]].Value));
                archs.Add(a);
            }

            Arch a1 = new Arch(list[list.Count - 1], list[0], Convert.ToInt32(graph.Rows[list[list.Count - 1]].Cells[list[0]].Value));
            archs.Add(a1);

            Contour c = new Contour(list, archs);
            c.versh_of_cont = list;
            contours.Add(c);
            listBox1.Items.Add(c);


        }

        private void walk(int x_start, int[,] matr, int l)//Обходим смежные для l вершины
        {
            contour.Add(l); //запись контуров в список
            if (x_start != l) NEW[l] = 1;
            for (int i = 0; i < count; i++)
            {
                if (matr[l, i] != 0)
                {
                    if (NEW[i] == 0) walk(x_start, matr, i);
                    else if (x_start == i) print_contour(contour);


                }
            }
            if (x_start != l) NEW[l] = 0;
            contour.Remove(l);
        }

        private void dimension_ValueChanged(object sender, EventArgs e)
        {

            String_Graph(this, dimension, graph);

        }





        private void button1_Click(object sender, EventArgs e)
        {

            listBox2.Items.Clear();
            listBox1.Items.Clear();
            contours.Clear();
            contour.Clear();
            str_cont.Clear();

            DateTime start = DateTime.Now;
            Search(graph);

            List<Contour> min_max_cont = new List<Contour>();
            int min_max = Int32.MaxValue;
            int min_length = int.MaxValue;
            //полный перебор
            for (int i = 0; i < contours.Count; i++)
            {
                if (contours[i].Max_Arch() < min_max)
                {
                    min_max = contours[i].Max_Arch();
                    min_length = contours[i].archs.Count;
                    min_max_cont.Clear();
                    min_max_cont.Add(contours[i]);
                }
                else if (contours[i].Max_Arch() == min_max)
                {
                    if (contours[i].archs.Count < min_length)
                    {
                        min_length = contours[i].archs.Count;
                        min_max_cont.Clear();
                        min_max_cont.Add(contours[i]);
                    }
                    else if (contours[i].archs.Count == min_length)
                    {
                        min_max_cont.Add(contours[i]);
                    }


                }
            }
            foreach (Contour c in min_max_cont)
            {
                //if (c.Max_Arch() == min_max && c.archs.Count == min_length)
                    listBox2.Items.Add(c);
            }

            DateTime now = DateTime.Now;
            TimeSpan time = now - start;
            textBox1.Text = Convert.ToString(time.TotalMilliseconds / 1000.0) + " секунд";


        }


        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < count; i++)
            {

                for (int j = 0; j < count; j++)
                {
                    if (i == j)
                    {
                        graph.Rows[i].Cells[j].Value = 0;
                        graph.Rows[i].Cells[j].ReadOnly = true;

                    }
                    else { graph.Rows[i].Cells[j].Value = rnd.Next(1, 100); graph.Rows[i].Cells[j].ReadOnly = false; }
                    graph.Columns[j].Width = 30;

                }
            }
        }
    }
}


