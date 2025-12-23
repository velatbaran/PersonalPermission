using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PersonalPermission.Core.Entities;
using PersonalPermission.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalPermission.Service.Concrete
{
    public class CalculatingPermision : ICalculatingPermision
    {
        private readonly IService<User> _serviceUser;
        private readonly IService<PermissionState> _servicePermissionState;

        public CalculatingPermision(IService<PermissionState> servicePermissionState, IService<User> serviceUser)
        {
            _servicePermissionState = servicePermissionState;
            _serviceUser = serviceUser;
        }

        public async void UpdateYearlyAndAdministrivePermision()
        {
           // DateTime today2 = new DateTime(2026, 7, 10);
            int gainedYearlyPermission = 0;
            int gainedAdministrativePermission = 0;
            var users = _serviceUser.GetQueryable().Include(x=>x.Department).Include(x=>x.Title).ToList();
            //   bool isActive = false;
            foreach (var user in users)
            {

                if (user.Title.Name != "İşçi")
                {
                    //   int years = DateTime.Now.Year - user.StartingWorkDate.Year;
                    var _userPermissionState = _servicePermissionState.GetQueryable().Include(x => x.User).Where(x => x.UserGuid == user.UserGuid).OrderByDescending(m => m.WhichYears).FirstOrDefault();
                    var __user = _serviceUser.Get(x => x.UserGuid == _userPermissionState.UserGuid);

                    if (_userPermissionState != null && _userPermissionState.WhichYears != DateTime.Now.Year)
                    {
                        // ToDo List : service time year controlling (< 1)
                        if ((DateTime.Now.Month == 01 && DateTime.Now.Day == 01))
                        {
                            //if (user.ServiceTimeYear < 1)
                            //{
                            //    gainedYearlyPermission = 0;
                            //    gainedAdministrativePermission = 40;
                            //}
                            if (user.ServiceTimeYear >= 1 || user.ServiceTimeYear < 10)
                            {
                                gainedYearlyPermission = 20;
                                gainedAdministrativePermission = 40;
                            }
                            else if (user.ServiceTimeYear >= 10)
                            {
                                gainedYearlyPermission = 30;
                                gainedAdministrativePermission = 40;
                            }

                            PermissionState permissionState = new PermissionState()
                            {
                                UserId = user.Id,
                                UserGuid = user.UserGuid,
                                GainedYearlyPermission = gainedYearlyPermission,
                                //       GainedAdministrativePermission = gainedAdministrativePermission,
                                RemainYearlyPermission = gainedYearlyPermission,
                                //        RemainAdministrativePermission = gainedAdministrativePermission,
                                UsedYearlyPermission = 0,
                                UsedAdministrativePermission = 0,
                                WhichYears = DateTime.Now.Year,
                                //  WhichYears = today2.Year,
                                IsActive = true,
                                //  AddedDate = today2,
                                AddedDate = DateTime.Now,
                            };
                            _servicePermissionState.Add(permissionState);
                            _servicePermissionState.SaveChanges();

                            var allUsers = _servicePermissionState.GetAll(x => x.UserGuid == user.UserGuid);
                            var inActiveUsers = allUsers.Where(x => x.UserGuid == user.UserGuid).OrderByDescending(x => x.WhichYears).Skip(2).ToList();
                            foreach (var _user in inActiveUsers)
                            {
                                _user.IsActive = false;
                                _servicePermissionState.Update(_user);
                                _servicePermissionState.SaveChanges();
                            }
                        }

                        // if (_userPermissionState.User.ServiceTimeYear == 1 && _userPermissionState.User.StartingWorkDate.Month == today2.Month &&  _userPermissionState.User.StartingWorkDate.Day == today2.Day)
                        if (_userPermissionState.User.ServiceTimeYear == 1 && _userPermissionState.User.StartingWorkDate.Month == DateTime.Now.Month && _userPermissionState.User.StartingWorkDate.Day == DateTime.Now.Day)
                        {
                            gainedYearlyPermission = 20;
                            gainedAdministrativePermission = 40;

                            PermissionState permissionState = new PermissionState()
                            {
                                UserId = user.Id,
                                UserGuid = user.UserGuid,
                                GainedYearlyPermission = gainedYearlyPermission,
                                //    GainedAdministrativePermission = gainedAdministrativePermission,
                                RemainYearlyPermission = gainedYearlyPermission,
                                //    RemainAdministrativePermission = gainedAdministrativePermission,
                                UsedYearlyPermission = 0,
                                UsedAdministrativePermission = 0,
                                WhichYears = DateTime.Now.Year,
                                //  WhichYears = today2.Year,
                                IsActive = true,
                                AddedDate = DateTime.Now,
                                //  AddedDate = today2,
                            };
                            _servicePermissionState.Add(permissionState);
                            _servicePermissionState.SaveChanges();
                        }
                    }
                }

                else
                {
                    //   int years = DateTime.Now.Year - user.StartingWorkDate.Year;
                    var _userPermissionState = _servicePermissionState.GetQueryable().Include(x => x.User).Where(x => x.UserGuid == user.UserGuid).OrderByDescending(m => m.WhichYears).FirstOrDefault();
                    var __user = _serviceUser.Get(x => x.UserGuid == _userPermissionState.UserGuid);

                    if (_userPermissionState != null && _userPermissionState.WhichYears != DateTime.Now.Year)
                    {
                        // ToDo List : service time year controlling (< 1)
                        if ((DateTime.Now.Month == 01 && DateTime.Now.Day == 01))
                        {
                            //if (user.ServiceTimeYear < 1)
                            //{
                            //    gainedYearlyPermission = 0;
                            //    gainedAdministrativePermission = 40;
                            //}

                            if (user.ServiceTimeYear >= 1 && user.ServiceTimeYear < 5)
                            {
                                gainedYearlyPermission = 18;
                                //     gainedAdministrativePermission = 0;
                            }
                            if (user.ServiceTimeYear >= 5 && user.ServiceTimeYear < 5)
                            {
                                gainedYearlyPermission = 25;
                                //  gainedAdministrativePermission = 0;
                            }
                            if (user.ServiceTimeYear >= 10)
                            {
                                gainedYearlyPermission = 30;
                                //  gainedAdministrativePermission = 0;
                            }

                            PermissionState permissionState = new PermissionState()
                            {
                                UserId = user.Id,
                                UserGuid = user.UserGuid,
                                GainedYearlyPermission = gainedYearlyPermission,
                                //       GainedAdministrativePermission = gainedAdministrativePermission,
                                RemainYearlyPermission = gainedYearlyPermission,
                                //        RemainAdministrativePermission = gainedAdministrativePermission,
                                UsedYearlyPermission = 0,
                                UsedAdministrativePermission = 0,
                                WhichYears = DateTime.Now.Year,
                                //  WhichYears = today2.Year,
                                IsActive = true,
                                //  AddedDate = today2,
                                AddedDate = DateTime.Now,
                            };
                            _servicePermissionState.Add(permissionState);
                            _servicePermissionState.SaveChanges();

                            //var allUsers = _servicePermissionState.GetAll(x => x.UserGuid == user.UserGuid);
                            //var inActiveUsers = allUsers.Where(x => x.UserGuid == user.UserGuid).OrderByDescending(x => x.WhichYears).Skip(2).ToList();
                            //foreach (var _user in inActiveUsers)
                            //{
                            //    _user.IsActive = false;
                            //    _servicePermissionState.Update(_user);
                            //    _servicePermissionState.SaveChanges();
                            //}
                        }

                        // if (_userPermissionState.User.ServiceTimeYear == 1 && _userPermissionState.User.StartingWorkDate.Month == today2.Month &&  _userPermissionState.User.StartingWorkDate.Day == today2.Day)
                        if (_userPermissionState.User.ServiceTimeYear == 1 && _userPermissionState.User.StartingWorkDate.Month == DateTime.Now.Month && _userPermissionState.User.StartingWorkDate.Day == DateTime.Now.Day)
                        {
                            gainedYearlyPermission = 18;
                       //     gainedAdministrativePermission = 0;

                            PermissionState permissionState = new PermissionState()
                            {
                                UserId = user.Id,
                                UserGuid = user.UserGuid,
                                GainedYearlyPermission = gainedYearlyPermission,
                                //    GainedAdministrativePermission = gainedAdministrativePermission,
                                RemainYearlyPermission = gainedYearlyPermission,
                                //    RemainAdministrativePermission = gainedAdministrativePermission,
                                UsedYearlyPermission = 0,
                                UsedAdministrativePermission = 0,
                                WhichYears = DateTime.Now.Year,
                                //  WhichYears = today2.Year,
                                IsActive = true,
                                AddedDate = DateTime.Now,
                                //  AddedDate = today2,
                            };
                            _servicePermissionState.Add(permissionState);
                            _servicePermissionState.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}

