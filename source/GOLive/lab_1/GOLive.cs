using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab_1
{
    public partial class Screen : Form
    {
        int[,] redbox; //двумерный массив, хранящий живые красные клетки
        int[,] tworedbox; //двумерный массив, в который заносятся новые поколения красных клеток
        int time = 1;
        int[,] bluebox; //двумерный массив, хранящий живые синие клетки
        int[,] twobluebox; //двумерный массив, в который заносятся новые поколения синих клеток
        bool SelCol = false; //переменная отвечающая какой цвет будет ставить пользователь
        //false - красный
        //true - синий
        int Box_map = 30;

        private void button1_Click(object sender, EventArgs e)
        {
            Timer.Enabled = false; //таймер выключается
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Timer.Enabled = true; //таймер выключается
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SelCol = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SelCol = true;
        }

        //функция размножения красных клеток
        void FunctionLifeIsRed()
        {
            //переменные определяющие индекс клеток с левой, правой, сверху, снизу
            int Left, Right, Up, Down, Live; //Live определяет количество живых клеток рядом с клеткой
            for (int k = 0; k < Box_map; k++)      
                for (int g = 0; g < Box_map; g++)
                { 
                    if (k != 0) //если не первый столбец в поле
                        Left = k - 1;
                    else
                        Left = Box_map - 1; //то присваивается самый парвый столбец
                    if (k != Box_map - 1) //если не последний столбец в поле
                        Right = k + 1;
                    else
                        Right = 0;
                    if (g != 0) //если не верхняя строка в поле
                        Up = g - 1;
                    else
                        Up = Box_map - 1;
                    if (g != Box_map - 1) //если не нижняя строка в поле
                        Down = g + 1;
                    else
                        Down = 0;
                    Live = 0;
                    //подсчет живых клеток вокруг клетки
                    Live += redbox[k, Up];
                    Live += redbox[k, Down];
                    Live += redbox[Left, g];
                    Live += redbox[Right, g];
                    Live += redbox[Left, Up];
                    Live += redbox[Left, Down];
                    Live += redbox[Right, Up];
                    Live += redbox[Right, Down];
                    //если вокруг три живых клетки и клетка мертва
                    if (Live == 3 && redbox[k, g] == 0)  
                        tworedbox[k, g] = 1; //клетка становится живой
                    //если вокруг меньше двух или больше трех клеток и клетка живая
                    else if ((Live < 2 || Live > 3) && (redbox[k, g] == 1))
                        tworedbox[k, g] = 0; //клетка становится мертвой
                    else
                        tworedbox[k, g] = redbox[k, g]; //иначе клетка остается без изменений
                }
        }

        //функция приравнивания элементов массива нынешнего поколения красных клеток к следующему
        void NewForOldRed()
        {
            for (int k = 0; k < Box_map; k++)
                for (int g = 0; g < Box_map; g++)
                    redbox[k, g] = tworedbox[k, g];
        }

        //функция размножения синих клеток с таким же алгоритмом, как и у красных
        void FunctionLifeIsBlue()
        {
            int Left, Right, Up, Down, Live;
            for (int i = 0; i < Box_map; i++)
                for (int j = 0; j < Box_map; j++)
                {
                    if (i != 0)
                        Left = i - 1;
                    else
                        Left = Box_map - 1;
                    if (i != Box_map - 1)
                        Right = i + 1;
                    else
                        Right = 0;
                    if (j != 0)
                        Up = j - 1;
                    else
                        Up = Box_map - 1;
                    if (j != Box_map - 1)
                        Down = j + 1;
                    else
                        Down = 0;
                    Live = 0;
                    Live += bluebox[i, Up];
                    Live += bluebox[i, Down];
                    Live += bluebox[Left, j];
                    Live += bluebox[Right, j];
                    Live += bluebox[Left, Up];
                    Live += bluebox[Left, Down];
                    Live += bluebox[Right, Up];
                    Live += bluebox[Right, Down];
                    if (Live == 3 && bluebox[i, j] == 0)
                        twobluebox[i, j] = 1;
                    else if ((Live < 2 || Live > 3) && (bluebox[i, j] == 1))
                        twobluebox[i, j] = 0;
                    else
                        twobluebox[i, j] = bluebox[i, j];
                }
        }

        //функция приравнивания элементов массива нынешнего поколения синих клеток к следующему
        void NewForOldBlue()
        {
            for (int i = 0; i < Box_map; i++)
                for (int j = 0; j < Box_map; j++)
                    bluebox[i, j] = twobluebox[i, j];
        }

        //отображение красных клеток
        void SeeRedToMap()
        {
            for (int i = 0; i < Box_map; i++)
                for (int j = 0; j < Box_map; j++)
                {
                    //если в данной ячейке поля красная клетка жива и синяя мертва
                    if (redbox[i, j] == 1 && bluebox[i, j] == 0) 
                        MapIsLife[j, i].Style.BackColor = System.Drawing.Color.Red; //ячейка красная
                    //если в ячейке красная мертва и синяя живы
                    else if (redbox[i, j] == 0 && bluebox[i, j] == 1)
                        MapIsLife[j, i].Style.BackColor = System.Drawing.Color.Blue; //ячейка синяя
                    //если в ячейке красная и синяя жива
                    else if (redbox[i, j] == 1 && bluebox[i, j] == 1) 
                    {
                        bluebox[i, j] = 0; //синяя клетка умирает
                        MapIsLife[j, i].Style.BackColor = System.Drawing.Color.Red; //ячейка красная
                    }
                    else
                        MapIsLife[j, i].Style.BackColor = System.Drawing.Color.White; //иначе ячейка белая

                }
            MapIsLife.Refresh();
        }

        //отображение синих клеток
        void SeeBlueToMap()
        {
            for (int i = 0; i < Box_map; i++)
                for (int j = 0; j < Box_map; j++)
                {
                    //если в данной ячейке поля синяя жива и красная мертва
                    if (bluebox[i, j] == 1 && redbox[i, j] == 0)
                        MapIsLife[j, i].Style.BackColor = System.Drawing.Color.Blue; //ячейка синяя
                    //если синяя клетка мертва и красная жива
                    else if(bluebox[i, j] == 0 && redbox[i, j] == 1)
                        MapIsLife[j, i].Style.BackColor = System.Drawing.Color.Red; //ячейка красная
                    //если красная и синяя живы
                    else if (redbox[i, j] == 1 && bluebox[i, j] == 1)
                    {
                        redbox[i, j] = 0; //красная клетка умирает
                        MapIsLife[j, i].Style.BackColor = System.Drawing.Color.Blue; //ячейка синяя
                    }
                    else
                        MapIsLife[j, i].Style.BackColor = System.Drawing.Color.White; //иначе ячейка белая

                }
        }

        public Screen()
        {
            InitializeComponent();
            redbox = new int[Box_map, Box_map]; //определение размера массива 
            bluebox = new int[Box_map, Box_map]; //определение размера массива
            tworedbox = new int[Box_map, Box_map]; //определение размера массива
            twobluebox = new int[Box_map, Box_map]; //определение размера массива
            MapIsLife.ColumnCount = Box_map; //количество столбцов поля
            MapIsLife.RowCount = Box_map; //количество строк в поле
            MapIsLife.ReadOnly = true; //запрет на изменение данных в ячейках 
            MapIsLife.AllowUserToDeleteRows = false; //запрещает пользователю удалять строки
            MapIsLife.RowHeadersVisible = false; //убирает столбец показыающий, что происходит со строками
            MapIsLife.AllowUserToResizeColumns = false; //запрещает пользователю менять размер столбцов
            MapIsLife.AllowUserToResizeRows = false;//запрещает пользователю менять размер полей
            for (int i = 0; i < Box_map; i++)
            {
                MapIsLife.Columns[i].Width = 20; //ширина ячеек в поле
                MapIsLife.Rows[i].Height = 20; //высота ячеек в поле
                MapIsLife.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable; //отключает сортировку столбца
                for (int j = 0; j < Box_map; j++) //приравнивание всех масивов к 0
                {
                    redbox[i, j] = 0;
                    bluebox[i, j] = 0;
                    tworedbox[i, j] = 0;
                    twobluebox[i, j] = 0;
                }
            }
            MapIsLife.ColumnHeadersVisible = false; //удаление заголовков
        }

        private void PoleLife_CellClick(object sender, DataGridViewCellEventArgs e) //событие клик по ячейке
        {
            if (SelCol == false) //если в Color текст
            {
                if (MapIsLife[e.ColumnIndex, e.RowIndex].Style.BackColor != System.Drawing.Color.Red) //если ячейка не красная
                {
                    MapIsLife[e.ColumnIndex, e.RowIndex].Style.BackColor = System.Drawing.Color.Red; //она становится красной
                    redbox[e.RowIndex, e.ColumnIndex] = 1; //ячейка в массиве с красными клетками становится живой
                    bluebox[e.RowIndex, e.ColumnIndex] = 0; //ячейка в массиве с синими клетками становится мёртвой
                }
                else
                {
                    MapIsLife[e.ColumnIndex, e.RowIndex].Style.BackColor = System.Drawing.Color.White; //она становится белой
                    redbox[e.RowIndex, e.ColumnIndex] = 0; //ячейка в массиве с красными клетками становится мёртвой
                    bluebox[e.RowIndex, e.ColumnIndex] = 0; //ячейка в массиве с синими клетками становится мёртвой
                }
            }
            else if (SelCol == true) //если в Color текст
            {
                if (MapIsLife[e.ColumnIndex, e.RowIndex].Style.BackColor != System.Drawing.Color.Blue) //если ячейка не синяя
                {
                    MapIsLife[e.ColumnIndex, e.RowIndex].Style.BackColor = System.Drawing.Color.Blue; //она становится синей
                    redbox[e.RowIndex, e.ColumnIndex] = 0; //ячейка в массиве с красными клетками становится мёртвой
                    bluebox[e.RowIndex, e.ColumnIndex] = 1; //ячейка в массиве с синими клетками становится живой
                }
                else
                {
                    MapIsLife[e.ColumnIndex, e.RowIndex].Style.BackColor = System.Drawing.Color.White; //она становится белой
                    redbox[e.RowIndex, e.ColumnIndex] = 0; //ячейка в массиве с красными клетками становится мёртвой
                    bluebox[e.RowIndex, e.ColumnIndex] = 0; //ячейка в массиве с синими клетками становится мёртвой
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e) //Что происходит каждую секунду
        {
            time += 1;
            Race.Text = "Время: " + Convert.ToString(time);
            FunctionLifeIsRed();
            NewForOldRed();
            FunctionLifeIsBlue();
            NewForOldBlue();
            SeeRedToMap();
            SeeBlueToMap();
        }
    }
}
