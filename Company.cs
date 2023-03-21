using System;
using System.Text;

namespace Stage_3__C_Sharp_
{
    internal class Company
    {
        private const int MAX = 4; //максимальный размер массива
        private Truck[] trucks; //массив автомобилей
        private int first, last; //указатели на первый и последний элемент массива соответсвенно
        private int count; //счетчик числа автомобилей

        //конструктор
        public Company()
        {
            trucks = new Truck[MAX];
            count = 0;
            first = last = 0;
        }
        //метод получения макс. количества элементов
        public int getMAX()
        {
            return MAX;
        }
        //добавление автомобиля
        public void addTruck(String number, String name)
        {
            //если массив автомобилей пуст
            if (count == 0)
            {
                trucks[0] = new Truck(number, name);
                first = last = 0;
            }
            else
            {

                if ((last == (trucks.Length - 1)) && (count != trucks.Length))
                {
                    last = -1;
                }
                trucks[last + 1] = new Truck(number, name);
                last++;
            }
            count++;
        }
        //поиск автомобиля
        public Truck searchTruck(String text)
        {
            foreach(Truck truck in trucks)
            {
                if (truck == null)
                {
                    continue;
                }
                else
                if (truck.getStateNumber().Equals(text) || truck.getDriversSurname().Equals(text))
                {
                    return truck;
                }
            }
            return null;
        }

        //удаление автомобиля
        public void deleteTruck()
        {
            if (first == last)
            {
                trucks[first] = null;
                first = last = 0;
            }
            else
            {
                trucks[first] = null;
                first++;
                if (first == trucks.Length)
                {
                    first = 0;
                }
            }
            count--;
        }

        //суммарное количество груза по всей компании
        public int totalCargoOfAllCars()
        {
            int sum = 0;
            foreach (Truck truck in trucks)
            {
                if (truck == null) break;
                sum += truck.totalCargo();
            }
            return sum;
        }

        //получение кол-ва автомобилей
        public int getCount()
        {
            return count;
        }

        //получение элемента массива по индексу
        public Truck getTruckByIndex(int index)
        {
            return trucks[index];
        }

        //получение инедкса первого элемента массива
        public int getFirst()
        {
            return first;
        }

        //получение инедкса последнего элемента массива
        public int getLast()
        {
            return last;
        }

        public Truck[] getTrucks()
        {
            return trucks;
        }

        //получение всей информации о автомобилях
        public StringBuilder getTrucksInf()
        {
            StringBuilder result = new StringBuilder();
            result.Append("Количество авто: ").Append(count).Append("\n");
            foreach (Truck truck in trucks)
            {
                if (truck == null) continue;
                result.Append("Гос. номер авмтомобиля: ").Append(truck.getStateNumber()).Append("\n");
                result.Append("Фамилия водителя: ").Append(truck.getDriversSurname()).Append("\n");
                result.Append(truck.getAllInf());
            }
            return result;
        }
    }
}
