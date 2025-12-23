using PersonalPermission.Core.Entities;
using PersonalPermission.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalPermission.Service.Concrete
{
    public class CalculatingServiceTime : ICalculatingServiceTime
    {
        private readonly IService<User> _serviceUser;

        public CalculatingServiceTime(IService<User> serviceUser)
        {
            _serviceUser = serviceUser;
        }

        public void UpdateServiceTime()
        {
            var users = _serviceUser.GetAll();
            foreach (var user in users)
            {
                DateTime? today = null;
               // DateTime today2 = new DateTime(2025,12,19);
                DateTime nowDate = today ?? DateTime.Now;
              //  DateTime nowDate = today ?? today2;
                int year = nowDate.Year - user.StartingWorkDate.Year;
                int month = nowDate.Month - user.StartingWorkDate.Month;
                int day = nowDate.Day - user.StartingWorkDate.Day;

                if (day < 0)
                {
                    month--;
                    day += DateTime.DaysInMonth(nowDate.Year, (nowDate.Month == 1 ? 12 : nowDate.Month - 1));
                }

                if (month < 0)
                {
                    year--;
                    month += 12;
                }

                user.ServiceTimeYear = year;
                user.ServiceTimeMonth = month;
                user.ServiceTimeDay = day;

                _serviceUser.Update(user);
                _serviceUser.SaveChanges();

            }
        }

        //public (int Year, int Month, int Day) CalculatingServiceTime(DateTime StartingWorkDate, DateTime? today = null)
        //{
        //    DateTime nowDate = today ?? DateTime.Now;
        //    if (StartingWorkDate > nowDate)
        //        throw new ArgumentException("İşe giriş tarihi bugünden büyük olamaz.");

        //    int year = nowDate.Year - StartingWorkDate.Year;
        //    int month = nowDate.Month - StartingWorkDate.Month;
        //    int day = nowDate.Day - StartingWorkDate.Day;

        //    if (day < 0)
        //    {
        //        month--;
        //        day += DateTime.DaysInMonth(nowDate.Year, (nowDate.Month == 1 ? 12 : nowDate.Month - 1));
        //    }

        //    if (month < 0)
        //    {
        //        year--;
        //        month += 12;
        //    }

        //    return (year, month, day);
        //}
    }
}
