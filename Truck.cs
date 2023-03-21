using System;
using System.Text;

namespace Stage_3__C_Sharp_
{
    internal class Truck
    {
        private String stateNumber; //гос. номер
        private String driversSurname; //фамилия водителя
        private tHead head; //заголовок списка рейсов
        private int count; //количество рейсов

        //класс-заголовок списка
        private class tHead
        {
            private Transit first; //сылка на первый элемент списка
            //констурктор                       
            public tHead()
            {
                first = null;
            }
            //получение первого элемента списка
            public Transit getFirst()
            { 
                return first;
            }
            //изменение первого элемента списка
            public void setFirst(Transit tFirst)
            { 
                first = tFirst;
            }
        }

        //конуструктор автомобиля
        public Truck(String number, String name)
        {
            stateNumber = number;
            driversSurname = name;
            head = new tHead();
            count = 0;
        }
        //получение первого элемента списка
        public Transit getFirst()
        {
            return head.getFirst();
        }
        //полчение кол-ва авто
        public int getCount()
        {
            return count;
        }

        //получение гос. номера
        public String getStateNumber()
        {
            return stateNumber;
        }
        //изменение гос. номера
        public void setStateNumber(String tNum)
        {
            stateNumber = tNum;
        }

        //получение фамилии водителя
        public String getDriversSurname()
        {
            return driversSurname;
        }
        //изменение фамилии водителя
        public void setDriversSurname(String tName)
        {
            driversSurname = tName;
        }

        //метод добавления рейса
        public void addTransit(int num, int tHours, int tMinutes)
        {
            Transit added = new Transit(num, tHours, tMinutes);
            if (head.getFirst() == null)
            {
                head.setFirst(added);
                added.setNext(head.getFirst());
            }
            else if (((head.getFirst().getHours() == tHours) && (head.getFirst().getMinutes() > tMinutes)) || (head.getFirst().getHours() > tHours))
            {
                added.setNext(head.getFirst());
                Transit temp = head.getFirst().getNext();
                while (temp.getNext() != head.getFirst())
                {
                    temp = temp.getNext();
                }
                head.setFirst(added);
                temp.setNext(head.getFirst());
            }
            else
            {
                Transit prev;
                Transit temp = head.getFirst().getNext();
                do
                {
                    prev = temp;
                    temp = temp.getNext();
                } while ((((temp.getHours() == tHours) && (temp.getMinutes() < tMinutes)) || (temp.getHours() < tHours)) && (temp != head.getFirst()));
                added.setNext(prev.getNext());
                prev.setNext(added);
            }
            count++;    
        }

        //удаление рейса
        public void deleteTransit(int tHours, int tMinutes)
        {
            Transit prev; //предыдущий элемент к удаляемому 
            Transit temp = head.getFirst();
            //поиск предыдущего элемента (для дальнейшей перенастройки ссылок)
            do
            {
                prev = temp;
                temp = temp.getNext();
            } while ((temp.getHours() != tHours) & (temp.getMinutes() != tMinutes));
            if (count == 1)
            {
                head.setFirst(null);
            }
            else
            {
                if (temp == head.getFirst())
                {
                    Transit t = head.getFirst();
                    while (t.getNext() != head.getFirst())
                    {
                        t = t.getNext();
                    }
                    t.setNext(temp.getNext());
                    head.setFirst(temp.getNext());
                }
                else
                {
                    prev.setNext(temp.getNext());
                }
            }
            count--;
        }

        //получение всей информации о рейсах
        public StringBuilder getAllInf()
        {
            StringBuilder result = new StringBuilder();
            Transit temp = head.getFirst();
            if (temp == null)
            {
                result.Append("Количество рейсов: 0");
                return result;
            }
            result.Append("Количество рейсов: ").Append(count).Append("\n");
            do
            {
                result.Append("\tОбъем груза: ").Append(temp.getCargoVolume()).Append("\n");
                result.Append("\t").Append("Время начала рейса: ").Append(temp.getTime()).Append("\n");
                temp = temp.getNext();
            } while (temp != head.getFirst());
            return result;
        }

        //подсчет суммарного количества груза по рейсам
        public int totalCargo()
        {
            int sum = 0;
            Transit temp = head.getFirst();
            do
            {
                sum += temp.getCargoVolume();
                temp = temp.getNext();
            } while (temp != head.getFirst());
            return sum;
        }

        //поиск рейса
        public Transit searchTransit(int tHours, int tMinutes)
        {
            Transit temp = head.getFirst();
            while ((temp.getHours() != tHours) || (temp.getMinutes() != tMinutes))
            {
                temp = temp.getNext();
                if (temp == head.getFirst())
                {
                    return null;
                }
            }
            return temp;
        }
    }
}
