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
            DateTime today = DateTime.Today;

            foreach (var user in users)
            {
                DateTime start = user.StartingWorkDate;

                int years = today.Year - start.Year;
                int months = today.Month - start.Month;
                int days = today.Day - start.Day;

                // Gün negatif ise bir ay geri git
                if (days < 0)
                {
                    months--;

                    var previousMonth = today.AddMonths(-1);
                    days += DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
                }

                // Ay negatif ise bir yıl geri git
                if (months < 0)
                {
                    years--;
                    months += 12;
                }

                user.ServiceTimeYear = years;
                user.ServiceTimeMonth = months;
                user.ServiceTimeDay = days;

                _serviceUser.Update(user);
            }

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
