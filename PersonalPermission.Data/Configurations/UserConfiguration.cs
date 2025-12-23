using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalPermission.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalPermission.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            int years = DateTime.Now.Year - Convert.ToDateTime("24.05.2023").Year;
            int months = DateTime.Now.Month - Convert.ToDateTime("24.05.2023").Month;
            int days = DateTime.Now.Day - Convert.ToDateTime("24.05.2023").Day;

            if (days < 0)
            {
                months--;
                days += DateTime.DaysInMonth(Convert.ToDateTime("24.05.2023").Year, Convert.ToDateTime("24.05.2023").Month);
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            builder.HasData(new User
            {
                Id = 1,
                Name = "Velat",
                Surname = "BARAN",
                RegistryNo = "SP-1578",
                TitleId = 1,
                DepartmentId = 1,
                StartingWorkDate = Convert.ToDateTime("24.05.2023"),
                ServiceTimeYear = years,
                ServiceTimeMonth = months,
                ServiceTimeDay = days,
                Password = "212121",
                IsAdmin = true,
                UserGuid = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
            });
        }
    }
}
