using AutoMapper;
using Harvey.CRMLoyalty.Application.Domain.Customers.Commands.UpdateMemberProfileCommandHandler;
using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Models;

namespace Harvey.CRMLoyalty.Application
{
    public static class MappingConfiguration
    {
        public static void Execute()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Staff, StaffModel>();
                cfg.CreateMap<Outlet, OutletModel>();
                cfg.CreateMap<Customer, CustomerModel>();
                cfg.CreateMap<MembershipTransaction, MembershipTransactionModel>();
                cfg.CreateMap<PointTransaction, PointTransactionModel>();
                cfg.CreateMap<WalletTransaction, WalletTransactionModel>();
                cfg.CreateMap<ErrorLogEntry, ErrorLogEntryModel>();
                cfg.CreateMap<Staff_Outlet, Staff_Outlet_Model>();
                cfg.CreateMap<UpdatememberProfileCommand, Customer>();
                cfg.CreateMap<AppSetting, AppSettingModel>();
            });
        }
    }
}
