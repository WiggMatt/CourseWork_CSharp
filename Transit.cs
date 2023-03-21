using System;

namespace Stage_3__C_Sharp_
{
    internal class Transit
    {
        private int cargoVolume; //объем груза (в единицах)
        private int hour; //время начала рейса
        private int minute;
        private Transit next; //ссылка на следующий элемент списка

        //конструктор рейса
        public Transit(int tNum, int tHour, int tMinute)
        {
            cargoVolume = tNum;
            hour = tHour;
            minute = tMinute;
            next = null;
        }

        //изменение ссылки на следующий элемент
        public void setNext(Transit temp)
        {
            next = temp;
        }

        //получение ссылки на след. элементz
        public Transit getNext()
        {
            return next;
        }

        //получение часов из даты
        public int getHours()
        {
            return hour;
        }

        //получение минут из даты
        public int getMinutes()
        {
            return minute;
        }

        //получение числа объема груза
        public int getCargoVolume()
        {
            return cargoVolume;
        }

        //изменение числа объема груза
        public void setCargoVolume(int newVolume)
        {
            cargoVolume = newVolume;
        }

        //изменение даты, а именно часа
        public void setHours(int tHour)
        {
            hour = tHour;
        }

        //изменение минут
        public void setMinutes(int tMinute)
        {

            minute = tMinute;
        }

        //получение даты целиком
        public String getTime()
        {
            return Convert.ToString(hour + ":" + minute);
        }
    }
}
