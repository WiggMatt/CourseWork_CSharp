using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Stage_3__C_Sharp_
{
    public partial class Form1 : Form
    {
        private Company com; 
        private bool flag = false; //флаг истинности выполнения метода загрузки файла (нужен для правильного заполнения таблицы, чтобы игнорировать метод SelectedChanged)
        //конструктор формы
        public Form1()
        {
            InitializeComponent();
            //создание компонента с локальным времененем, то есть только часы и минуты
            dateTimePicker1.CustomFormat = "HH:mm";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            trnsitDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Courier New", 12, FontStyle.Bold);
            truckDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Courier New", 12, FontStyle.Bold);
            //ограничение на ввод символов в соответвующие поля
            textBoxOfNumber.MaxLength = 6;
            textBoxOfSurname.MaxLength = 10;
            //создание структуры 
            com = new Company();
        }
        //преобразование значения трэкбара в числовое в реальном времени
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            numericUpDown1.Text = trackBar1.Value.ToString();
        }
        //валидатор для поля ввода гос. номера автомобиля
        private void textBoxOfNumber_TextChanged(object sender, EventArgs e)
        {
            //все допустимые символы для гос. номера в РФ
            if (new Regex(@"^[АВЕКМНОРСТУХ]{1}[0-9]{3}[АВЕКМНОРСТУХ]{2}$").IsMatch(textBoxOfNumber.Text))
            {
                textBoxOfNumber.ForeColor = Color.Black;
            }
            else 
            {
                textBoxOfNumber.ForeColor = Color.Red;
            }
        }
        //преобразование нажатых клавиш поля с гос.номером в верхний регистр
        private void textBoxOfNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = char.ToUpper(e.KeyChar);
        }
        //валидатор для поля ввода фамилии водителя
        private void textBoxOfSurname_TextChanged(object sender, EventArgs e)
        {
            //первый символ преобразуется в верхний регистр
            if (textBoxOfSurname.Text.Length == 1) textBoxOfSurname.Text = textBoxOfSurname.Text.ToUpper();
            //ввод символов друг за другом
            textBoxOfSurname.Select(textBoxOfSurname.Text.Length, 0);
            //допустимые значения
            if (new Regex(@"^[А-Я]{1}[а-я]{1,9}$").IsMatch(textBoxOfSurname.Text))
            {
                textBoxOfSurname.ForeColor = Color.Black;
            }
            else
            {
                textBoxOfSurname.ForeColor = Color.Red;
            }
        }
        //кнопка добавления авто
        private void addTruck_Click(object sender, EventArgs e)
        {
            //проверка на существование экземпляра с подобным гос. номером
            if (com.searchTruck(textBoxOfNumber.Text) != null)
            {
                MessageBox.Show("Автомобиль с данным гос. номером уже существует!");
            }
            else
            {
                //проверка массива на заполненность
                if (com.getMAX() == com.getCount())
                {
                    MessageBox.Show("Добавлено максимальное количество автомобилей!");
                    textBoxOfNumber.Clear();
                    textBoxOfSurname.Clear();
                }
                else
                {
                    //проверка на существование экземпляра с полностью схожими данными
                    if (truckDataGrid.Rows.Count != 0 && (com.searchTruck(textBoxOfNumber.Text) != null && com.searchTruck(textBoxOfNumber.Text).getDriversSurname() == textBoxOfSurname.Text))
                    {
                        MessageBox.Show("Данный автомобиль уже существует!");
                        textBoxOfNumber.Clear();
                        textBoxOfSurname.Clear();
                    }
                    else
                    {
                        //проврека на пустоту полей ввода
                        if (textBoxOfNumber.Text.Equals("") || textBoxOfSurname.Text.Equals(""))
                        {
                            MessageBox.Show("Введите все значения для авто!");
                            return;
                        }
                        //фильтрация введенных данных
                        if (textBoxOfNumber.ForeColor == Color.Red || textBoxOfSurname.ForeColor == Color.Red) MessageBox.Show("Введите правильные данные!");
                        else
                        {
                            //добавление в структуру
                            com.addTruck(textBoxOfNumber.Text, textBoxOfSurname.Text);
                            //добавление новой строки в таблицу
                            truckDataGrid.Rows.Add(com.getCount(), textBoxOfNumber.Text, textBoxOfSurname.Text);
                            //отчистка полей ввода
                            textBoxOfNumber.Clear();
                            textBoxOfSurname.Clear();
                        }
                    }
                }
            }
        }
        //кнопка изменения данных автомобиля
        private void changedName_Click(object sender, EventArgs e)
        {
            //проверка таблицы на пустоту
            if (truckDataGrid.Rows.Count == 0)
            {
                MessageBox.Show("Автомобилей не существует!");
            }
            else
            //проверка на пустоту полей ввода
            if (textBoxOfNumber.Text.Equals("") && textBoxOfSurname.Text.Equals(""))
            {
                MessageBox.Show("Заполните хотя бы одно поле!");
            }
            else
            //проверка полей ввода на уникальность
            if (com.searchTruck(textBoxOfNumber.Text) != null || com.searchTruck(textBoxOfSurname.Text) != null)
            {
                MessageBox.Show("Автомобиль со схожими данными уже существует!");
                textBoxOfNumber.Clear();
                textBoxOfSurname.Clear();
            }
            else
            {
                //извлечение гос. номера из текущей строки для дальнейшего поиска экземпляра
                string text = truckDataGrid.CurrentRow.Cells[1].Value.ToString(); ;
                //if (truckDataGrid.CurrentRow.Cells[1].Value.ToString().Equals(""))
                //{
                //    text = truckDataGrid.CurrentRow.Cells[2].Value.ToString();
                //}
                //else
                //{
                //    text = truckDataGrid.CurrentRow.Cells[1].Value.ToString();
                //}

                //фиксация индекса выбранной строки
                int index = truckDataGrid.CurrentRow.Index;
                //проверка на наличие данных в одном из полей ввода
                //и дальнейшее изменение соответвующих данных в структуре и строке таблицы
                if (textBoxOfNumber.Text.Equals("") == false && textBoxOfSurname.Text.Equals(""))
                {
                    com.searchTruck(text).setStateNumber(textBoxOfNumber.Text);
                    truckDataGrid.Rows[index].Cells[1].Value = textBoxOfNumber.Text;
                }
                else if (textBoxOfNumber.Text.Equals("") && textBoxOfSurname.Text.Equals("") == false)
                {
                    com.searchTruck(text).setDriversSurname(textBoxOfSurname.Text);
                    truckDataGrid.Rows[index].Cells[2].Value = textBoxOfSurname.Text;
                }
                else
                {
                    //вариант, когда оба поля ввода зполнены
                    Truck temp = com.searchTruck(text);
                    temp.setStateNumber(textBoxOfNumber.Text);
                    temp.setDriversSurname(textBoxOfSurname.Text);
                    truckDataGrid.Rows[index].Cells[1].Value = textBoxOfNumber.Text;
                    truckDataGrid.Rows[index].Cells[2].Value = textBoxOfSurname.Text;
                }
                //отчистка полей ввода
                textBoxOfNumber.Clear();
                textBoxOfSurname.Clear();
            }
        }
        //кнопка удаления автомобиля
        private void deleteTruck_Click(object sender, EventArgs e)
        {
            //проверка таблицы на пустоту
            if (truckDataGrid.Rows.Count == 0)
            {
                MessageBox.Show("Автомобилей не существует!");
            }
            else
            {
                //удаление элемента из структуры 
                com.deleteTruck();
                //удаление строки из начала таблицы (т.к. структура реализована в виде очереди)
                truckDataGrid.Rows.RemoveAt(0);
                //изменение нумерации строк в таблице
                for (int i = 0; i < com.getCount(); i++)
                {
                    truckDataGrid.Rows[i].Cells[0].Value = Convert.ToString(i + 1);
                }
                if (truckDataGrid.Rows.Count == 0)
                {
                    trnsitDataGrid.Rows.Clear();
                }
            }
        }
        //изменение значения трэкбара в соответсвии со значением его поля ввода
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            trackBar1.Value = (int)numericUpDown1.Value;
        }
        //кнопка добавления рейса
        private void addTransit_Click(object sender, EventArgs e)
        {
            //проверка таблицы с авто на пустоту
            if (truckDataGrid.Rows.Count == 0)
            {
                MessageBox.Show("Автомобилей не существует!");
                trackBar1.Value = 0;
                numericUpDown1.Value = 0;
                dateTimePicker1.Value = Convert.ToDateTime("00:00");
            }
            else
            //проверка поля ввода объема груза
            if (numericUpDown1.Value == 0)
            {
                MessageBox.Show("Рейс не может быть с пустым грузом!");
                trackBar1.Value = 0;
                numericUpDown1.Value = 0;
                dateTimePicker1.Value = Convert.ToDateTime("00:00");
            }
            else
            //проверка поля ввода времени на уникальность
            if (trnsitDataGrid.Rows.Count != 0 && com.searchTruck(truckDataGrid.CurrentRow.Cells[1].Value.ToString()).searchTransit(dateTimePicker1.Value.Hour, dateTimePicker1.Value.Minute) != null)
            {
                MessageBox.Show("Рейс с таким временем уже существует!");
                trackBar1.Value = 0;
                numericUpDown1.Value = 0;
                dateTimePicker1.Value = Convert.ToDateTime("00:00");
            }
            else
            {
                string data = truckDataGrid.CurrentRow.Cells[1].Value.ToString();

                //if (truckDataGrid.CurrentRow.Cells[1].Value.ToString().Equals(""))
                //{
                //    data = truckDataGrid.CurrentRow.Cells[2].Value.ToString();
                //}
                //else
                //{
                //    data = truckDataGrid.CurrentRow.Cells[1].Value.ToString();
                //}

                //поиск авто с данными текущей ячейки таблицы с авто
                Truck curr = com.searchTruck(data);
                //добавление в структуру нового рейса
                curr.addTransit((int)numericUpDown1.Value, dateTimePicker1.Value.Hour, dateTimePicker1.Value.Minute);
                //обрезание строки для отсечения даты, т.к. нам нужно только время!
                string time = Convert.ToString(dateTimePicker1.Value).Remove(0, 11);
                //разделение полученной строки на часы и минуты 
                string[] val = time.Split(new char[] { ':' });
                //добавление новой строки в таблицу рейсов
                trnsitDataGrid.Rows.Add(curr.getCount(), (int)numericUpDown1.Value, val[0] + ":" + val[1]);
                //отчистка полй ввода
                trackBar1.Value = 0;
                numericUpDown1.Value = 0;
                dateTimePicker1.Value = Convert.ToDateTime("00:00");
            }
        }
        //кнопка удаления рейса
        private void delTransit_Click(object sender, EventArgs e)
        {
            //проверка таблицы рейсов на пустоту
            if (trnsitDataGrid.Rows.Count == 0)
            {
                MessageBox.Show("Рейсов не существует!");
            }
            else
            {
                //проверка таблицы авто на пустоту
                if (truckDataGrid.Rows.Count == 0)
                {
                    MessageBox.Show("Автомобилей не существует!");
                }
                else
                {
                    string text = truckDataGrid.CurrentRow.Cells[1].Value.ToString();

                    //if (truckDataGrid.CurrentRow.Cells[1].Value.ToString().Equals(""))
                    //{
                    //    text = truckDataGrid.CurrentRow.Cells[2].Value.ToString();
                    //}
                    //else
                    //{
                    //    text = truckDataGrid.CurrentRow.Cells[1].Value.ToString();
                    //}

                    //поиск авто
                    Truck tmpTruck = com.searchTruck(text);
                    //разделение ячейки со временем удаляемого элементана часы и минуты
                    string[] val = Convert.ToString(trnsitDataGrid.CurrentRow.Cells[2].Value).Split(new char[] { ':' });
                    //удаление соответсвующей строки из таблицы
                    trnsitDataGrid.Rows.RemoveAt(trnsitDataGrid.CurrentRow.Index);
                    //удаление элемента из структуры 
                    tmpTruck.deleteTransit(Convert.ToInt32(val[0]), Convert.ToInt32(val[1]));
                    //изменение нумерации элементов таблицы
                    for (int i = 0; i < tmpTruck.getCount(); i++)
                    {
                        trnsitDataGrid.Rows[i].Cells[0].Value = Convert.ToString(i + 1);
                    }
                }
            }
        }
        //кнопка изменение данных рейса
        private void changeTransit_Click(object sender, EventArgs e)
        {
            //проверка таблицы с авто на пустоту
            if (truckDataGrid.Rows.Count == 0)
            {
                MessageBox.Show("Автомобилей не существует!");
                trackBar1.Value = 0;
                numericUpDown1.Value = 0;
                dateTimePicker1.Value = Convert.ToDateTime("00:00");
            }
            //проверка таблицы с рейсами на пустоту
            else if (trnsitDataGrid.Rows.Count == 0) { MessageBox.Show("Рейсов не существует!"); }
            else
            {
                string data = truckDataGrid.CurrentRow.Cells[1].Value.ToString();

                //if (truckDataGrid.CurrentRow.Cells[1].Value.ToString().Equals(""))
                //{
                //    data = truckDataGrid.CurrentRow.Cells[2].Value.ToString();
                //}
                //else
                //{
                //    data = truckDataGrid.CurrentRow.Cells[1].Value.ToString();
                //}

                //проверка наличия данных в ОБОИХ полях ввода, иначе данные не изменяются
                if (numericUpDown1.Value == 0 & dateTimePicker1.Text == "0:00:00")
                {
                    MessageBox.Show("Введите оба значения!"); ;
                }
                else if ((numericUpDown1.Value != 0 & dateTimePicker1.Text == "0:00:00") || (numericUpDown1.Value == 0 & dateTimePicker1.Text != "0:00:00"))
                {
                    MessageBox.Show("Введите оба значения!");
                }
                else
                {
                    //разделение строки времени из соответсвующей ячейки 
                    string[] val = Convert.ToString(trnsitDataGrid.CurrentRow.Cells[2].Value).Split(new char[] { ':' });
                    //поиск рейса в структуре
                    Transit temp = com.searchTruck(data).searchTransit(int.Parse(val[0]), int.Parse(val[1]));
                    //изменение объема груза
                    temp.setCargoVolume(Convert.ToInt32(numericUpDown1.Value));
                    //обрезка строки со временем, так как нам нужны только часы и минуты
                    string time = Convert.ToString(dateTimePicker1.Value).Remove(0, 11);
                    //разделение полученной строки на часы и минуты
                    string[] val1 = time.Split(new char[] { ':' });
                    //изменение часа
                    temp.setHours(Convert.ToInt32(val1[0]));
                    //изменение минут
                    temp.setMinutes(Convert.ToInt32(val1[1]));
                    //изменение данных в таблице
                    trnsitDataGrid.CurrentRow.Cells[1].Value = numericUpDown1.Value;
                    trnsitDataGrid.CurrentRow.Cells[2].Value = val1[0] + ":" + val1[1];
                }
                //отчисьтка полей ввода
                numericUpDown1.Value = 0;
                trackBar1.Value = 0;
                dateTimePicker1.Value = Convert.ToDateTime("00:00");
            }
        }
        //кнопка сохранения данных в файл
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path;
            //добавление к файлу специального ключевого слова, для дальнейшего определения правильности файла
            string text = "\tФайл\n";
            //Создание экземпляра диалогового окна c фильтром на создание текстовых файлов
            using (SaveFileDialog save = new SaveFileDialog() { Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*" })
            {
                if (save.ShowDialog() == DialogResult.OK)
                {
                    path = save.FileName;
                    //накопление результирующей строки с полезными данными
                    text += com.getTrucksInf() + "\n";
                    //Та же работа с экземплярами файла и записи файла
                    using (FileStream file = new FileStream(path, FileMode.OpenOrCreate))
                    using (StreamWriter stream = new StreamWriter(file))
                    {
                        //запись в вайл
                        stream.Write(text);
                        stream.Close();
                        file.Close();
                    }
                }
                else return;
            }
        }
        //кнопка загрузки данных из файла
        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //указание на то, что выполняется данный метод
            flag = true;
            //
            string path = "", check;
            int count;
            //создание диалогового окна с указанием фильтра на создание файлов
            using (OpenFileDialog openFile = new OpenFileDialog() { Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*" })
            {
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    path = openFile.FileName;
                }
                else return;
            }
            try
            {
                //использование объекта, который считывает строки из файла
                using (StreamReader stream3 = new StreamReader(path))
                {
                    check = stream3.ReadLine();
                    //Проверка на пригодность файла для работы с программой
                    if (check == "\tФайл")
                    {
                        //удаление существующей структуры
                        if (truckDataGrid.Rows.Count != 0)
                        {
                            for (int i = 0; i <= com.getCount(); i++)
                            {
                                com.deleteTruck();
                                truckDataGrid.Rows.Clear();
                                trnsitDataGrid.Rows.Clear();
                            }
                        }
                        //считывание строки с количеством авто
                        string filter = stream3.ReadLine().Remove(0, 17);
                        count = int.Parse(filter);
                        for (int i = 1; i <= count; i++)
                        {
                            //считывание строки с гос. номером
                            string num = stream3.ReadLine().Remove(0, 24);
                            //считывание строки с фамилией
                            string name = stream3.ReadLine().Remove(0, 18);
                            //добавление в структуру
                            com.addTruck(num, name);
                            //добавление новой строки
                            truckDataGrid.Rows.Add();
                            truckDataGrid.Rows[i - 1].Cells[0].Value = i;
                            truckDataGrid.Rows[i - 1].Cells[1].Value = num;
                            truckDataGrid.Rows[i - 1].Cells[2].Value = name;
                            //считывание строки с количеством рейсов данного авто
                            string filter1 = stream3.ReadLine().Remove(0, 19);
                            int count2 = int.Parse(filter1);
                            for (int j = 1; j <= count2; j++)
                            {
                                //считывание строки с объемом груза
                                int c = int.Parse(stream3.ReadLine().Remove(0, 13));
                                //считывание строки со временем
                                string time = stream3.ReadLine().Remove(0, 21);
                                //разделение ее на часы и минуты
                                string[] val = time.Split(new char[] { ':' });
                                int hour = int.Parse(val[0]);
                                int minute = int.Parse(val[1]);
                                //добавление в структуру
                                com.searchTruck(name).addTransit(c, hour, minute);
                            }
                        }
                    }
                    else 
                    {
                        for (int i = 0; i <= com.getCount(); i++)
                        {
                            com.deleteTruck();
                            truckDataGrid.Rows.Clear();
                            trnsitDataGrid.Rows.Clear();
                        }
                        MessageBox.Show("Данный файл не подходит для работы с программой"); 
                    }
                    stream3.Close();
                }
            } catch 
            {
                for (int i = 0; i <= com.getCount(); i++)
                {
                    com.deleteTruck();
                    truckDataGrid.Rows.Clear();
                    trnsitDataGrid.Rows.Clear();
                }
                MessageBox.Show("Невозможно открыть файл!"); 
            }
            flag = false;
        }
        //метод реагирования не события изменение выбора строки в таблице авто, то есть обновление таблицы рейсов
        private void truckDataGrid_SelectionChanged(object sender, EventArgs e)
        { 
            //если флаг переведен в режим выполнения метода загрузки файла
            //то пропускаем выполнение этого метода, для правильного формирования таблицы
            if (flag == true)
            {
                return;
            }
            if (truckDataGrid.Rows.Count != 0)
            {
                trnsitDataGrid.Rows.Clear();
                string data = truckDataGrid.CurrentRow.Cells[1].Value.ToString();
                //поиск авто
                Truck temp = com.searchTruck(data);
                if (temp != null)
                {
                    //обращение к первому элементу списка
                    Transit inc = temp.getFirst();
                    int num = 1;
                    while (inc != null)
                    {
                        //добавлние новой строки в таблицу
                        trnsitDataGrid.Rows.Add(num, inc.getCargoVolume(), inc.getTime());
                        //переход к след. элементу
                        inc = inc.getNext();
                        if (inc == temp.getFirst())
                        {
                            break;
                        }
                        num++;
                    }

                }
                else return;
            }
        }

        private void информацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int total = com.totalCargoOfAllCars();
            MessageBox.Show("Компания \"Грузовичок\"\nОбщий объем груза: " + total);
        }
    }
}
