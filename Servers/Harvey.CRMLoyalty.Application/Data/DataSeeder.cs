using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Data
{
    public class DataSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            try
            {
                Policy.Handle<Exception>()
               .WaitAndRetry(new[] {
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(10)
               }, (exception, timeSpan, retryCount, context) =>
               {

               }).Execute(() =>
               {
                   var harveyCRMLoyaltyDbContext = serviceProvider.GetRequiredService<HarveyCRMLoyaltyDbContext>();

                   harveyCRMLoyaltyDbContext.Database.Migrate();
                   SeedStaffAsync(harveyCRMLoyaltyDbContext).Wait();
                   SeedOutletAsync(harveyCRMLoyaltyDbContext).Wait();
                   //SeedCustomerAsync(harveyCRMLoyaltyDbContext).Wait();
                   SeedMembershipTypeAsync(harveyCRMLoyaltyDbContext).Wait();
                   //SeedMembershipTransactionrAsync(harveyCRMLoyaltyDbContext).Wait();
                   //SeedPointTransactionrAsync(harveyCRMLoyaltyDbContext).Wait();
                   SeedStaffOutletAsync(harveyCRMLoyaltyDbContext).Wait();
                   //SeedWalletTransactionrAsync(harveyCRMLoyaltyDbContext).Wait();
                   SeedPointTransactionTypeAsync(harveyCRMLoyaltyDbContext).Wait();
                   SeedAppSettingTypeAsync(harveyCRMLoyaltyDbContext).Wait();
                   SeedAppSettingAsync(harveyCRMLoyaltyDbContext).Wait();
                   SeedErrorSourceAsync(harveyCRMLoyaltyDbContext).Wait();
               });
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<DataSeeder>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }

        #region Seed Staff
        private static async Task SeedStaffAsync(HarveyCRMLoyaltyDbContext context)
        {
            if (context.Staffs.Any())
                return;

            var staffs = new List<Staff>()
            {
                 new Staff
                {
                    Id = "76af5bea-6af1-4a18-b38e-1396875518e5",
                    FirstName = "Migration",
                    LastName = "Staff",
                    Email = "Migration Staff",
                    Phone = "Migration Staff",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy="admin",
                    DateOfBirth=DateTime.UtcNow,
                    Gender=1,
                    MembershipTransactions = null,
                    PointTransactions = null,
                    ProfileImage=null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    WalletTransactions=null,
                    TypeOfStaff = TypeOfStaff.StoreAccount
                },
                new Staff
                {
                    Id = "f6e021af-a6a0-4039-83f4-152595b4671a",
                    FirstName = "TOG",
                    LastName = "Bugis+",
                    Email = "bgp@tog.com.sg",
                    Phone = "9563476",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy="admin",
                    DateOfBirth=DateTime.UtcNow,
                    Gender=1,
                    MembershipTransactions = null,
                    PointTransactions = null,
                    ProfileImage=null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    WalletTransactions=null,
                    TypeOfStaff = TypeOfStaff.StoreAccount
                },
                new Staff
                {
                    Id = "1f552daf-b628-4ab8-8294-be535af7ed7e",
                    FirstName = "TOG",
                    LastName = "Tiong Bahru Plaza",
                    Email = "tb@tog.com.sg",
                    Phone = "95726754",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy="admin",
                    DateOfBirth=DateTime.UtcNow,
                    Gender=1,
                    MembershipTransactions = null,
                    PointTransactions = null,
                    ProfileImage=null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    WalletTransactions=null,
                    TypeOfStaff = TypeOfStaff.StoreAccount
                },
                new Staff
                {
                    Id = "d2d6fd97-fedc-4298-8c3c-464e68d06193",
                    FirstName = "TOG",
                    LastName = "WaterWay Point",
                    Email = "wp@tog.com.sg",
                    Phone = "8237613",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy="admin",
                    DateOfBirth=DateTime.UtcNow,
                    Gender=1,
                    MembershipTransactions = null,
                    PointTransactions = null,
                    ProfileImage=null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    WalletTransactions=null,
                    TypeOfStaff = TypeOfStaff.StoreAccount
                },
                new Staff
                {
                    Id = "e17c1c0d-0da7-49fe-9556-1f1a286a607e",
                    FirstName = "TOG",
                    LastName = "Jurong Point Shopping Centre",
                    Email = "jp@tog.com.sg",
                    Phone = "97317583",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy="admin",
                    DateOfBirth=DateTime.UtcNow,
                    Gender=1,
                    MembershipTransactions = null,
                    PointTransactions = null,
                    ProfileImage=null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    WalletTransactions=null,
                    TypeOfStaff = TypeOfStaff.StoreAccount
                },
                new Staff
                {
                    Id = "065edde7-5620-48c7-9060-7fbddfe4c2c6",
                    FirstName = "TOG",
                    LastName = "Lot 1 Shopper's Mall",
                    Email = "lt1@tog.com.sg",
                    Phone = "89256821",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy="admin",
                    DateOfBirth=DateTime.UtcNow,
                    Gender=1,
                    MembershipTransactions = null,
                    PointTransactions = null,
                    ProfileImage=null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    WalletTransactions=null,
                    TypeOfStaff = TypeOfStaff.StoreAccount
                },
                new Staff
                {
                    Id = "b157896b-c807-4a09-9dce-f482e0fde6e3",
                    FirstName = "TOG",
                    LastName = "Clementi Mall",
                    Email = "cm@tog.com.sg",
                    Phone = "92376165",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy="admin",
                    DateOfBirth=DateTime.UtcNow,
                    Gender=1,
                    MembershipTransactions = null,
                    PointTransactions = null,
                    ProfileImage=null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    WalletTransactions=null,
                    TypeOfStaff = TypeOfStaff.StoreAccount
                },
                new Staff
                {
                    Id = "e96a19b7-e59f-41f4-9eb2-32d38d19adea",
                    FirstName = "TOG",
                    LastName = "Plaza Singapura",
                    Email = "ps@tog.com.sg",
                    Phone = "98346982",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy="admin",
                    DateOfBirth=DateTime.UtcNow,
                    Gender=1,
                    MembershipTransactions = null,
                    PointTransactions = null,
                    ProfileImage=null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    WalletTransactions=null,
                    TypeOfStaff = TypeOfStaff.StoreAccount
                },
                new Staff
                {
                    Id = "4fdce97f-207c-42ad-879f-87b2fee87aec",
                    FirstName = "TOG",
                    LastName = "Causeway Point Shopping Centre",
                    Email = "cw@tog.com.sg",
                    Phone = "95823457",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy="admin",
                    DateOfBirth=DateTime.UtcNow,
                    Gender=1,
                    MembershipTransactions = null,
                    PointTransactions = null,
                    ProfileImage=null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    WalletTransactions=null,
                    TypeOfStaff = TypeOfStaff.StoreAccount
                },
                new Staff
                {
                    Id = "aa156fcb-250c-4f23-b661-c8c858bfe94d",
                    FirstName = "TOG",
                    LastName = "JEM",
                    Email = "jm@tog.com.sg",
                    Phone = "85846174",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy="admin",
                    DateOfBirth=DateTime.UtcNow,
                    Gender=1,
                    MembershipTransactions = null,
                    PointTransactions = null,
                    ProfileImage=null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    WalletTransactions=null,
                    TypeOfStaff = TypeOfStaff.StoreAccount
                },

                new Staff
                {
                    Id = "3437cbff-cc72-4f25-b0d5-cf46f2d066c6",
                    FirstName = "TOG",
                    LastName = "Tampines Mall",
                    Email = "tm@tog.com.sg",
                    Phone = "91678356",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy="admin",
                    DateOfBirth=DateTime.UtcNow,
                    Gender=1,
                    MembershipTransactions = null,
                    PointTransactions = null,
                    ProfileImage=null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    WalletTransactions=null,
                    TypeOfStaff = TypeOfStaff.StoreAccount
                },

                new Staff
                {
                    Id = "068e6bda-d830-4e65-bb3f-aaa8beda2981",
                    FirstName = "Gan",
                    LastName = "Leong",
                    Email = "gan.leong@toyorgame.com.sg",
                    Phone = "xxxxx356",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy="admin",
                    DateOfBirth=DateTime.UtcNow,
                    Gender=1,
                    MembershipTransactions = null,
                    PointTransactions = null,
                    ProfileImage=null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    WalletTransactions=null,
                    TypeOfStaff = TypeOfStaff.StaffAdminAccount
                },
                new Staff
                {
                    Id = "ffb7f35f-e19f-4c61-aeb7-da0c91e12158",
                    FirstName = "Eleen",
                    LastName = "Tan",
                    Email = "eleen.tan@toyorgame.com.sg",
                    Phone = "xxxxx357",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy="admin",
                    DateOfBirth=DateTime.UtcNow,
                    Gender=1,
                    MembershipTransactions = null,
                    PointTransactions = null,
                    ProfileImage=null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    WalletTransactions=null,
                    TypeOfStaff = TypeOfStaff.StaffAdminAccount
                },
                new Staff
                {
                    Id = "cbb6f8e5-3a46-41cf-b9a1-dbb163dda690",
                    FirstName = "Adeline",
                    LastName = "Ngu",
                    Email = "adeline.ngu@toyorgame.com.sg",
                    Phone = "xxxxx358",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy="admin",
                    DateOfBirth=DateTime.UtcNow,
                    Gender=1,
                    MembershipTransactions = null,
                    PointTransactions = null,
                    ProfileImage=null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    WalletTransactions=null,
                    TypeOfStaff = TypeOfStaff.StaffAdminAccount
                },
                new Staff
                {
                    Id = "6f84ce5d-4da1-4969-86ad-e66631ef89bc",
                    FirstName = "Hiew",
                    LastName = "Yuhang",
                    Email = "hiew.yuhang@toyorgame.com.sg",
                    Phone = "xxxxx359",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy="admin",
                    DateOfBirth=DateTime.UtcNow,
                    Gender=1,
                    MembershipTransactions = null,
                    PointTransactions = null,
                    ProfileImage=null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    WalletTransactions=null,
                    TypeOfStaff = TypeOfStaff.StaffAdminAccount
                },
                 new Staff
                {
                    Id = "4a0679d0-83ed-49c2-b2f3-7017e7218ba2",
                    FirstName = "Tech",
                    LastName = "Admin",
                    Email = "tech.admin@tog.com.sg",
                    Phone = "xxxxx359",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy="admin",
                    DateOfBirth=DateTime.UtcNow,
                    Gender=1,
                    MembershipTransactions = null,
                    PointTransactions = null,
                    ProfileImage=null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    WalletTransactions=null,
                    TypeOfStaff = TypeOfStaff.StaffAdminAccount
                },
            };
            context.AddRange(staffs);
            await context.SaveChangesAsync();
        }
        #endregion

        #region Seed Outlet
        private static async Task SeedOutletAsync(HarveyCRMLoyaltyDbContext context)
        {
            if (context.Outlets.Any())
                return;

            var outlets = new List<Outlet>()
            {
                 new Outlet
                {
                    Id = "10ecc00f-6e24-49bc-b735-c18127433f8b",
                    Name = "Migration Outlet",
                    Code = "MO",
                    Address = "Migration Outlet",
                    Email = "Migration Outlet",
                    Phone = "Migration Outlet",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    Status = Status.InActive,
                    CreatedBy="admin",
                    UpdatedBy=null,
                    UpdatedDate=null,
                },
                new Outlet
                {
                    Id = "f5134054-e657-42bf-bcf0-3d80a9e76470",
                    Name = "TOG Bugis+",
                    Code = "BP",
                    Address = "201 Victoria Street, S188067",
                    Email = "bugis.plus@toyorgame.com.sg",
                    Phone = "6634 2637",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    Status = Status.Active,
                    CreatedBy="admin",
                    UpdatedBy=null,
                    UpdatedDate=null,
                },
                new Outlet
                {
                    Id = "1a130aed-2262-4d7f-abd5-3b4f0b303289",
                    Name = "TOG Tiong Bahru Plaza",
                    Code = "TP",
                    Address = "302 Tiong Bahru Rd, S168732",
                    Email = "tiong.bahru.plaza@toyorgame.com.sg",
                    Phone = "6352 2672",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    Status = Status.Active,
                    CreatedBy="admin",
                    UpdatedBy=null,
                    UpdatedDate=null,
                },
                new Outlet
                {
                    Id = "e336ba38-ee22-44e9-a55c-05c5133b1ead",
                    Name = "TOG WaterWay Point",
                    Code = "WP",
                    Address = "83 Punggol Central #02-18, S828761",
                    Email = "waterway.point@toyorgame.com.sg",
                    Phone = "6385 9172",
                    PhoneCountryCode = "65",
                    Status = Status.Active,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy="admin",
                    UpdatedBy=null,
                    UpdatedDate=null,
                },
                new Outlet
                {
                    Id = "0500c213-88f5-4138-b271-01c2c28443b5",
                    Name = "TOG Jurong Point Shopping Centre",
                    Code = "JC",
                    Address = "1 Jurong West Central 2 S648886",
                    Email = "djurong.point@toyorgame.com.sg",
                    Phone = "6316 5984",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    Status = Status.Active,
                    CreatedBy="admin",
                    UpdatedBy=null,
                    UpdatedDate=null,
                },
                new Outlet
                {
                    Id = "67958fb9-a54e-45b1-a0ef-ddeae3b31d99",
                    Code = "LM",
                    Name = "TOG Lot 1 Shopper's Mall",
                    Address = "21 Chua Chu Kang Ave 4 #04-13 S689812",
                    Email = "lot.1@toyorgames.com.sg",
                    Phone = "6769 6129",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    Status = Status.Active,
                    CreatedBy="admin",
                    UpdatedBy=null,
                    UpdatedDate=null,
                },
                new Outlet
                {
                    Id = "54029213-7fd5-47e7-9989-7b729e7bf7b6",
                    Name = "TOG Clementi Mall",
                    Code = "CM",
                    Address = "3155 Commonwealth Avenue West, S129855",
                    Email = "clementi.mall@toyorgame.com.sg",
                    Phone = "6570 6084",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    Status = Status.Active,
                    CreatedBy="admin",
                    UpdatedBy=null,
                    UpdatedDate=null,
                },
                new Outlet
                {
                    Id = "47cce686-f17b-46ed-ae4a-033bf012c27e",
                    Name = "TOG Plaza Singapura",
                    Code = "PS",
                    Address = "68 Orchard Road, S238839",
                    Email = "plaza.singapura@toyorgame.com.sg",
                    Phone = "6238 7632",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    Status = Status.Active,
                    CreatedBy="admin",
                    UpdatedBy=null,
                    UpdatedDate=null,
                },
                new Outlet
                {
                    Id = "68a41dd1-e17c-438b-bfb6-15d47a56fb54",
                    Name = "TOG Causeway Point Shopping Centre",
                    Code = "CC",
                    Address = "1 Woodlands Square #04-05 S738099",
                    Email = "causeway.point@toyorgame.com.sg",
                    Phone = "6891 2836",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    Status = Status.Active,
                    CreatedBy="admin",
                    UpdatedBy=null,
                    UpdatedDate=null,
                },
                new Outlet
                {
                    Id = "07ead259-d662-4390-93c5-7711b85fd38e",
                    Name = "TOG JEM",
                    Code = "JM",
                    Address = "50 Jurong Gateway Road #04-29 S608549",
                    Email = "jem@toyorgame.com.sg",
                    Phone = "6334 6753",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    Status = Status.Active,
                    CreatedBy="admin",
                    UpdatedBy=null,
                    UpdatedDate=null,
                },
                new Outlet
                {
                    Id = "3f738c80-e4e3-4208-a6c2-194ea536653d",
                    Name = "TOG Tampines Mall",
                    Code = "TM",
                    Address = "4 Tampines Central 5, S529510",
                    Email = "tampines.mall@toyorgame.com.sg",
                    Phone = "6538 0144",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    Status = Status.Active,
                    CreatedBy="admin",
                    UpdatedBy=null,
                    UpdatedDate=null,
                },
            };
            context.AddRange(outlets);
            await context.SaveChangesAsync();
        }
        #endregion

        #region Seed StaffOutlet
        private static async Task SeedStaffOutletAsync(HarveyCRMLoyaltyDbContext context)
        {
            if (context.Staff_Outlets.Any())
                return;

            var staffOutlet = new List<Staff_Outlet>()
            {
                new Staff_Outlet
                {
                    Id = "750ee067-2ad5-43d9-9ee1-7988ccf1af26",
                    StaffId ="f6e021af-a6a0-4039-83f4-152595b4671a",
                    OutletId = "f5134054-e657-42bf-bcf0-3d80a9e76470"
                },
                new Staff_Outlet
                {
                    Id = "e37cea5d-8eba-438a-ba3e-9a91d12b2d34",
                    StaffId ="1f552daf-b628-4ab8-8294-be535af7ed7e",
                    OutletId = "1a130aed-2262-4d7f-abd5-3b4f0b303289"
                },
                new Staff_Outlet
                {
                    Id = "d1c77b14-0473-4b65-ad10-fcd0bdac06f3",
                    StaffId ="d2d6fd97-fedc-4298-8c3c-464e68d06193",
                    OutletId = "e336ba38-ee22-44e9-a55c-05c5133b1ead"
                },
                new Staff_Outlet
                {
                    Id = "80a33897-a500-4c1b-a5ea-12bcbddd4198",
                    StaffId ="e17c1c0d-0da7-49fe-9556-1f1a286a607e",
                    OutletId = "0500c213-88f5-4138-b271-01c2c28443b5"
                },
                new Staff_Outlet
                {
                    Id = "f509bd2e-db1b-44ae-ac8d-d0e54e18cb2b",
                    StaffId ="065edde7-5620-48c7-9060-7fbddfe4c2c6",
                    OutletId = "67958fb9-a54e-45b1-a0ef-ddeae3b31d99"
                },
                new Staff_Outlet
                {
                    Id = "6324a6a0-6562-4fc3-b934-49a3989757a3",
                    StaffId ="b157896b-c807-4a09-9dce-f482e0fde6e3",
                    OutletId = "54029213-7fd5-47e7-9989-7b729e7bf7b6"
                },
                new Staff_Outlet
                {
                    Id = "a8b1be17-02c8-457b-b3ff-7267e74a966e",
                    StaffId ="e96a19b7-e59f-41f4-9eb2-32d38d19adea",
                    OutletId = "47cce686-f17b-46ed-ae4a-033bf012c27e"
                },
                new Staff_Outlet
                {
                    Id = "9b926f4d-c72c-4208-8e30-b1f94cf8ae10",
                    StaffId ="4fdce97f-207c-42ad-879f-87b2fee87aec",
                    OutletId = "68a41dd1-e17c-438b-bfb6-15d47a56fb54"
                },
                new Staff_Outlet
                {
                    Id = "b195f9ad-59f0-45c5-aa33-5dd2df80a11b",
                    StaffId ="aa156fcb-250c-4f23-b661-c8c858bfe94d",
                    OutletId = "07ead259-d662-4390-93c5-7711b85fd38e"
                },
                new Staff_Outlet
                {
                    Id = "d8527ec7-ad02-4da6-bb5a-87724381d04b",
                    StaffId ="3437cbff-cc72-4f25-b0d5-cf46f2d066c6",
                    OutletId = "3f738c80-e4e3-4208-a6c2-194ea536653d"
                },
                //---admin staff
                //---Hiew.Yuhang@toyorgame.com.sg
                new Staff_Outlet
                {
                    Id = Guid.NewGuid().ToString(),
                    StaffId ="6f84ce5d-4da1-4969-86ad-e66631ef89bc",
                    OutletId = "f5134054-e657-42bf-bcf0-3d80a9e76470"
                },
                new Staff_Outlet
                {
                   Id = Guid.NewGuid().ToString(),
                    StaffId ="6f84ce5d-4da1-4969-86ad-e66631ef89bc",
                    OutletId = "1a130aed-2262-4d7f-abd5-3b4f0b303289"
                },
                new Staff_Outlet
                {
                    Id = Guid.NewGuid().ToString(),
                    StaffId ="6f84ce5d-4da1-4969-86ad-e66631ef89bc",
                    OutletId = "e336ba38-ee22-44e9-a55c-05c5133b1ead"
                },
                new Staff_Outlet
                {
                    Id = Guid.NewGuid().ToString(),
                    StaffId ="6f84ce5d-4da1-4969-86ad-e66631ef89bc",
                    OutletId = "0500c213-88f5-4138-b271-01c2c28443b5"
                },
                new Staff_Outlet
                {
                    Id = Guid.NewGuid().ToString(),
                    StaffId ="6f84ce5d-4da1-4969-86ad-e66631ef89bc",
                    OutletId = "67958fb9-a54e-45b1-a0ef-ddeae3b31d99"
                },
                new Staff_Outlet
                {
                    Id = Guid.NewGuid().ToString(),
                    StaffId ="6f84ce5d-4da1-4969-86ad-e66631ef89bc",
                    OutletId = "54029213-7fd5-47e7-9989-7b729e7bf7b6"
                },
                new Staff_Outlet
                {
                    Id = Guid.NewGuid().ToString(),
                    StaffId ="6f84ce5d-4da1-4969-86ad-e66631ef89bc",
                    OutletId = "47cce686-f17b-46ed-ae4a-033bf012c27e"
                },
                new Staff_Outlet
                {
                    Id = Guid.NewGuid().ToString(),
                    StaffId ="6f84ce5d-4da1-4969-86ad-e66631ef89bc",
                    OutletId = "68a41dd1-e17c-438b-bfb6-15d47a56fb54"
                },
                new Staff_Outlet
                {
                    Id = Guid.NewGuid().ToString(),
                    StaffId ="6f84ce5d-4da1-4969-86ad-e66631ef89bc",
                    OutletId = "07ead259-d662-4390-93c5-7711b85fd38e"
                },
                new Staff_Outlet
                {
                    Id = Guid.NewGuid().ToString(),
                    StaffId ="6f84ce5d-4da1-4969-86ad-e66631ef89bc",
                    OutletId = "3f738c80-e4e3-4208-a6c2-194ea536653d"
                },
                //---adeline.ngu@toyorgame.com.sg
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="cbb6f8e5-3a46-41cf-b9a1-dbb163dda690",
                    OutletId = "f5134054-e657-42bf-bcf0-3d80a9e76470"
                },
                new Staff_Outlet
                {
                    Id = Guid.NewGuid().ToString(),
                    StaffId ="cbb6f8e5-3a46-41cf-b9a1-dbb163dda690",
                    OutletId = "1a130aed-2262-4d7f-abd5-3b4f0b303289"
                },
                new Staff_Outlet
                {
                   Id = Guid.NewGuid().ToString(),
                    StaffId ="cbb6f8e5-3a46-41cf-b9a1-dbb163dda690",
                    OutletId = "e336ba38-ee22-44e9-a55c-05c5133b1ead"
                },
                new Staff_Outlet
                {
                   Id = Guid.NewGuid().ToString(),
                    StaffId ="cbb6f8e5-3a46-41cf-b9a1-dbb163dda690",
                    OutletId = "0500c213-88f5-4138-b271-01c2c28443b5"
                },
                new Staff_Outlet
                {
                   Id = Guid.NewGuid().ToString(),
                    StaffId ="cbb6f8e5-3a46-41cf-b9a1-dbb163dda690",
                    OutletId = "67958fb9-a54e-45b1-a0ef-ddeae3b31d99"
                },
                new Staff_Outlet
                {
                   Id = Guid.NewGuid().ToString(),
                    StaffId ="cbb6f8e5-3a46-41cf-b9a1-dbb163dda690",
                    OutletId = "54029213-7fd5-47e7-9989-7b729e7bf7b6"
                },
                new Staff_Outlet
                {
                   Id = Guid.NewGuid().ToString(),
                    StaffId ="cbb6f8e5-3a46-41cf-b9a1-dbb163dda690",
                    OutletId = "47cce686-f17b-46ed-ae4a-033bf012c27e"
                },
                new Staff_Outlet
                {
                   Id = Guid.NewGuid().ToString(),
                    StaffId ="cbb6f8e5-3a46-41cf-b9a1-dbb163dda690",
                    OutletId = "68a41dd1-e17c-438b-bfb6-15d47a56fb54"
                },
                new Staff_Outlet
                {
                   Id = Guid.NewGuid().ToString(),
                    StaffId ="cbb6f8e5-3a46-41cf-b9a1-dbb163dda690",
                    OutletId = "07ead259-d662-4390-93c5-7711b85fd38e"
                },
                new Staff_Outlet
                {
                   Id = Guid.NewGuid().ToString(),
                    StaffId ="cbb6f8e5-3a46-41cf-b9a1-dbb163dda690",
                    OutletId = "3f738c80-e4e3-4208-a6c2-194ea536653d"
                },
                //---eleen.tan@toyorgame.com.sg
                new Staff_Outlet
                {
                    Id = Guid.NewGuid().ToString(),
                    StaffId ="ffb7f35f-e19f-4c61-aeb7-da0c91e12158",
                    OutletId = "f5134054-e657-42bf-bcf0-3d80a9e76470"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="ffb7f35f-e19f-4c61-aeb7-da0c91e12158",
                    OutletId = "1a130aed-2262-4d7f-abd5-3b4f0b303289"
                },
                new Staff_Outlet
                {
                    Id = Guid.NewGuid().ToString(),
                    StaffId ="ffb7f35f-e19f-4c61-aeb7-da0c91e12158",
                    OutletId = "e336ba38-ee22-44e9-a55c-05c5133b1ead"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="ffb7f35f-e19f-4c61-aeb7-da0c91e12158",
                    OutletId = "0500c213-88f5-4138-b271-01c2c28443b5"
                },
                new Staff_Outlet
                {
                    Id = Guid.NewGuid().ToString(),
                    StaffId ="ffb7f35f-e19f-4c61-aeb7-da0c91e12158",
                    OutletId = "67958fb9-a54e-45b1-a0ef-ddeae3b31d99"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="ffb7f35f-e19f-4c61-aeb7-da0c91e12158",
                    OutletId = "54029213-7fd5-47e7-9989-7b729e7bf7b6"
                },
                new Staff_Outlet
                {
                    Id = Guid.NewGuid().ToString(),
                    StaffId ="ffb7f35f-e19f-4c61-aeb7-da0c91e12158",
                    OutletId = "47cce686-f17b-46ed-ae4a-033bf012c27e"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="ffb7f35f-e19f-4c61-aeb7-da0c91e12158",
                    OutletId = "68a41dd1-e17c-438b-bfb6-15d47a56fb54"
                },
                new Staff_Outlet
                {
                    Id = Guid.NewGuid().ToString(),
                    StaffId ="ffb7f35f-e19f-4c61-aeb7-da0c91e12158",
                    OutletId = "07ead259-d662-4390-93c5-7711b85fd38e"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="ffb7f35f-e19f-4c61-aeb7-da0c91e12158",
                    OutletId = "3f738c80-e4e3-4208-a6c2-194ea536653d"
                },
                //---gan.leong@toyorgame.com.sg
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="068e6bda-d830-4e65-bb3f-aaa8beda2981",
                    OutletId = "f5134054-e657-42bf-bcf0-3d80a9e76470"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="068e6bda-d830-4e65-bb3f-aaa8beda2981",
                    OutletId = "1a130aed-2262-4d7f-abd5-3b4f0b303289"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="068e6bda-d830-4e65-bb3f-aaa8beda2981",
                    OutletId = "e336ba38-ee22-44e9-a55c-05c5133b1ead"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="068e6bda-d830-4e65-bb3f-aaa8beda2981",
                    OutletId = "0500c213-88f5-4138-b271-01c2c28443b5"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="068e6bda-d830-4e65-bb3f-aaa8beda2981",
                    OutletId = "67958fb9-a54e-45b1-a0ef-ddeae3b31d99"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="068e6bda-d830-4e65-bb3f-aaa8beda2981",
                    OutletId = "54029213-7fd5-47e7-9989-7b729e7bf7b6"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="068e6bda-d830-4e65-bb3f-aaa8beda2981",
                    OutletId = "47cce686-f17b-46ed-ae4a-033bf012c27e"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="068e6bda-d830-4e65-bb3f-aaa8beda2981",
                    OutletId = "68a41dd1-e17c-438b-bfb6-15d47a56fb54"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="068e6bda-d830-4e65-bb3f-aaa8beda2981",
                    OutletId = "07ead259-d662-4390-93c5-7711b85fd38e"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="068e6bda-d830-4e65-bb3f-aaa8beda2981",
                    OutletId = "3f738c80-e4e3-4208-a6c2-194ea536653d"
                },
                //tech.store 
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="4a0679d0-83ed-49c2-b2f3-7017e7218ba2",
                    OutletId = "f5134054-e657-42bf-bcf0-3d80a9e76470"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="4a0679d0-83ed-49c2-b2f3-7017e7218ba2",
                    OutletId = "1a130aed-2262-4d7f-abd5-3b4f0b303289"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="4a0679d0-83ed-49c2-b2f3-7017e7218ba2",
                    OutletId = "e336ba38-ee22-44e9-a55c-05c5133b1ead"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="4a0679d0-83ed-49c2-b2f3-7017e7218ba2",
                    OutletId = "0500c213-88f5-4138-b271-01c2c28443b5"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="4a0679d0-83ed-49c2-b2f3-7017e7218ba2",
                    OutletId = "67958fb9-a54e-45b1-a0ef-ddeae3b31d99"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="4a0679d0-83ed-49c2-b2f3-7017e7218ba2",
                    OutletId = "54029213-7fd5-47e7-9989-7b729e7bf7b6"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="4a0679d0-83ed-49c2-b2f3-7017e7218ba2",
                    OutletId = "47cce686-f17b-46ed-ae4a-033bf012c27e"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="4a0679d0-83ed-49c2-b2f3-7017e7218ba2",
                    OutletId = "68a41dd1-e17c-438b-bfb6-15d47a56fb54"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="4a0679d0-83ed-49c2-b2f3-7017e7218ba2",
                    OutletId = "07ead259-d662-4390-93c5-7711b85fd38e"
                },
                new Staff_Outlet
                {
                     Id = Guid.NewGuid().ToString(),
                    StaffId ="4a0679d0-83ed-49c2-b2f3-7017e7218ba2",
                    OutletId = "3f738c80-e4e3-4208-a6c2-194ea536653d"
                },


            };
            context.AddRange(staffOutlet);
            await context.SaveChangesAsync();
        }
        #endregion

        #region Seed customer
        private static async Task SeedCustomerAsync(HarveyCRMLoyaltyDbContext context)
        {
            if (context.Customers.Any())
                return;

            var customers = new List<Customer>()
            {
                new Customer
                {
                    Id = "5b236842-e30b-460a-9aad-771a8c5aec11",
                    FirstName = "Chee",
                    LastName = "LeongWong",
                    Email = "chee.leongwong@gmail.com",
                    Phone = "95463217",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "admin",
                    DateOfBirth = DateTime.UtcNow,
                    JoinedDate = DateTime.UtcNow,
                    LastUsed = DateTime.UtcNow.AddHours(2),
                    ProfileImage = null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    Status = Status.Active
                },
                new Customer
                {
                    Id = "f6e076af-a6a0-4939-83f4-152595b4698g",
                    FirstName = "Leo",
                    LastName = "Mar",
                    Email = "leo@gmail.com",
                    Phone = "942960154",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "admin",
                    DateOfBirth = DateTime.UtcNow,
                    JoinedDate = DateTime.UtcNow,
                    LastUsed = DateTime.UtcNow.AddHours(2),
                    ProfileImage = null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    Status = Status.Active
                },
                 new Customer
                {
                    Id = "91338a0d-9fa5-4f9c-b17b-9943ec9adf2c",
                    FirstName = "Saiful",
                    LastName = "Nizam",
                    Email = "Nizam@test.com",
                    Phone = "98542600",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "admin",
                    DateOfBirth = DateTime.UtcNow,
                    JoinedDate = DateTime.UtcNow,
                    LastUsed = DateTime.UtcNow.AddHours(2),
                    ProfileImage = null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    Status = Status.Active
                },
                 new Customer
                {
                    Id = "7e1291c7-999d-46ef-97a2-9f44d7a8e234",
                    FirstName = "Adam",
                    LastName = "Gerrard",
                    Email = "Gerrard@test.com",
                    Phone = "87523961",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "admin",
                    DateOfBirth = DateTime.UtcNow,
                    JoinedDate = DateTime.UtcNow,
                    LastUsed = DateTime.UtcNow.AddHours(2),
                    ProfileImage = null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    Status = Status.Active
                },
                 new Customer
                {
                    Id = "e74066e0-f90b-4858-b0ab-f5ae09b6c125",
                    FirstName = "Firdaus",
                    LastName = "Azzhar",
                    Email = "Azzhar@test.com",
                    Phone = "86913161",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "admin",
                    DateOfBirth = DateTime.UtcNow,
                    JoinedDate = DateTime.UtcNow,
                    LastUsed = DateTime.UtcNow.AddHours(2),
                    ProfileImage = null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    Status = Status.Active
                },
                  new Customer
                {
                    Id = "316c7583-fcbf-4545-8538-5737c8ce9ee3",
                    FirstName = "Chee Hong",
                    LastName = "Chang",
                    Email = "Chang@test.com",
                    Phone = "84022691",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "admin",
                    DateOfBirth = DateTime.UtcNow,
                    JoinedDate = DateTime.UtcNow,
                    LastUsed = DateTime.UtcNow.AddHours(2),
                    ProfileImage = null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    Status = Status.Active
                },
                  new Customer
                {
                    Id = "4ca6bb58-f986-4085-9d75-fdc996b29068",
                    FirstName = "Shashi",
                    LastName = "Jeganathan",
                    Email = "Jeganathan@test.com",
                    Phone = "98324445",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "admin",
                    DateOfBirth = DateTime.UtcNow,
                    JoinedDate = DateTime.UtcNow,
                    LastUsed = DateTime.UtcNow.AddHours(2),
                    ProfileImage = null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    Status = Status.Active
                },
                  new Customer
                {
                    Id = "10232822-990c-4d7b-9b4f-db8f2dcbdc11",
                    FirstName = "Sufi",
                    LastName = "Khamis",
                    Email = "Khamis@test.com",
                    Phone = "90018845",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "admin",
                    DateOfBirth = DateTime.UtcNow,
                    JoinedDate = DateTime.UtcNow,
                    LastUsed = DateTime.UtcNow.AddHours(2),
                    ProfileImage = null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    Status = Status.Active
                },
                  new Customer
                {
                    Id = "5b234672-e30b-460a-9aad-771a8c5aec11",
                    FirstName = "Christopher",
                    LastName = "Wong",
                    Email = "christopher_wong@gmail.com",
                    Phone = "97964603",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "admin",
                    DateOfBirth = DateTime.UtcNow,
                    JoinedDate = DateTime.UtcNow,
                    LastUsed = DateTime.UtcNow.AddHours(2),
                    ProfileImage = null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    Status = Status.Active
                },
                new Customer
                {
                    Id = "f6e076af-a6a0-5465-83f4-152595b4698g",
                    FirstName = "Leon",
                    LastName = "Trinity",
                    Email = "leon_trinity@gmail.com",
                    Phone = "84636534",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "admin",
                    DateOfBirth = DateTime.UtcNow,
                    JoinedDate = DateTime.UtcNow,
                    LastUsed = DateTime.UtcNow.AddHours(2),
                    ProfileImage = null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    Status = Status.Active
                },
                 new Customer
                {
                    Id = "91338a0d-9fa5-4f9c-b17b-8543ec9adf2c",
                    FirstName = "Gavriel",
                    LastName = "Teo",
                    Email = "gavriel_teo@test.com",
                    Phone = "98544524",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "admin",
                    DateOfBirth = DateTime.UtcNow,
                    JoinedDate = DateTime.UtcNow,
                    LastUsed = DateTime.UtcNow.AddHours(2),
                    ProfileImage = null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    Status = Status.Active
                },
                 new Customer
                {
                    Id = "7e1291c7-842d-46ef-97a2-9f44d7a8e234",
                    FirstName = "Wilson",
                    LastName = "Lee",
                    Email = "wilson_lee@test.com",
                    Phone = "87467157",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "admin",
                    DateOfBirth = DateTime.UtcNow,
                    JoinedDate = DateTime.UtcNow,
                    LastUsed = DateTime.UtcNow.AddHours(2),
                    ProfileImage = null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    Status = Status.Active
                },
                 new Customer
                {
                    Id = "e74066e0-f90b-4858-b0ab-f5ae09b6c475",
                    FirstName = "Damion",
                    LastName = "Lee",
                    Email = "damion_lee@test.com",
                    Phone = "86913161",
                    PhoneCountryCode = "65",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "admin",
                    DateOfBirth = DateTime.UtcNow,
                    JoinedDate = DateTime.UtcNow,
                    LastUsed = DateTime.UtcNow.AddHours(2),
                    ProfileImage = null,
                    UpdatedBy=null,
                    UpdatedDate=null,
                    Status = Status.InActive
                }
            };
            context.AddRange(customers);
            await context.SaveChangesAsync();
        }
        #endregion

        #region Seed MembershipTransaction
        private static async Task SeedMembershipTransactionrAsync(HarveyCRMLoyaltyDbContext context)
        {
            if (context.MembershipTransactions.Any())
                return;

            var membershipTransactions = new List<MembershipTransaction>()
            {
                new MembershipTransaction
                {
                    Id = "f6e996af-a6a0-4939-83f4-157595b4698g",
                    MembershipTypeId = 2,
                    Comment = "No comment",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.UtcNow,
                    CustomerId = "7e1291c7-999d-46ef-97a2-9f44d7a8e234",
                    OutletId = "f5134054-e657-42bf-bcf0-3d80a9e76470",
                    StaffId = "92cba2fc-ddb2-419b-b3f5-915e896814fe",
                    ExpiredDate = DateTime.UtcNow.AddDays(7),
                    Customer = null,
                    Outlet = null,
                    Staff = null,
                }
            };
            context.AddRange(membershipTransactions);
            await context.SaveChangesAsync();
        }
        #endregion

        #region Seed MembershipType
        private static async Task SeedMembershipTypeAsync(HarveyCRMLoyaltyDbContext context)
        {
            if (context.MembershipTypes.Any())
                return;

            var membershipTypes = new List<MembershipType>()
            {
                 new MembershipType
                {
                    Id = 1,
                    TypeName = "Basic"
                },
                  new MembershipType
                {
                    Id = 2,
                    TypeName = "Premium+"
                }
            };
            context.AddRange(membershipTypes);
            await context.SaveChangesAsync();
        }
        #endregion

        #region pointtransaction-type
        private static async Task SeedPointTransactionTypeAsync(HarveyCRMLoyaltyDbContext context)
        {
            if (context.PointTransactionType.Any())
                return;

            var pointTransactionTypes = new List<PointTransactionType>()
            {
                new PointTransactionType
                {
                    Id = 1,
                    TypeName = "AddPoint"
                },
                 new PointTransactionType
                {
                    Id = 2,
                    TypeName = "RedeemPoint"
                },
                  new PointTransactionType
                {
                    Id = 3,
                    TypeName = "ExpiryPoint"
                },
                   new PointTransactionType
                {
                    Id = 4,
                    TypeName = "Void"
                }
            };
            context.AddRange(pointTransactionTypes);
            await context.SaveChangesAsync();
        }
        #endregion

        #region Seed PointTransaction
        private static async Task SeedWalletTransactionrAsync(HarveyCRMLoyaltyDbContext context)
        {
            if (context.PointTransactions.Any())
                return;

            var walletTransactions = new List<WalletTransaction>()
            {
                new WalletTransaction
                {
                    Id = Guid.NewGuid().ToString(),
                    BalanceTotal = 500,
                    Debit = 300,
                    Credit = 0,
                    Comment = "No comment",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.UtcNow.AddDays(-1),
                    CustomerId = "5b236842-e30b-460a-9aad-771a8c5aec11",
                    OutletId = "f5134054-e657-42bf-bcf0-3d80a9e76470",
                    StaffId = "92cba2fc-ddb2-419b-b3f5-915e896814fe",
                    Customer = null,
                    Outlet = null,
                    Staff = null,
                },
                new WalletTransaction
                {
                    Id = Guid.NewGuid().ToString(),
                    BalanceTotal = 300,
                    Debit = 0,
                    Credit = 200,
                    Comment = "No comment",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    CustomerId = "5b236842-e30b-460a-9aad-771a8c5aec11",
                    OutletId = "f5134054-e657-42bf-bcf0-3d80a9e76470",
                    StaffId = "92cba2fc-ddb2-419b-b3f5-915e896814fe",
                    Customer = null,
                    Outlet = null,
                    Staff = null,
                },
                new WalletTransaction
                {
                    Id = Guid.NewGuid().ToString(),
                    BalanceTotal = 150,
                    Debit = 100,
                    Credit = 0,
                    Comment = "No comment",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.UtcNow.AddDays(-3),
                    CustomerId = "5b236842-e30b-460a-9aad-771a8c5aec11",
                    OutletId = "f5134054-e657-42bf-bcf0-3d80a9e76470",
                    StaffId = "92cba2fc-ddb2-419b-b3f5-915e896814fe",
                    Customer = null,
                    Outlet = null,
                    Staff = null,
                },
                new WalletTransaction
                {
                    Id = Guid.NewGuid().ToString(),
                    BalanceTotal = 600,
                    Debit = 0,
                    Credit = 250,
                    Comment = "No comment",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.UtcNow,
                    CustomerId = "5b236842-e30b-460a-9aad-771a8c5aec11",
                    OutletId = "f5134054-e657-42bf-bcf0-3d80a9e76470",
                    StaffId = "92cba2fc-ddb2-419b-b3f5-915e896814fe",
                    Customer = null,
                    Outlet = null,
                    Staff = null,
                },

                new WalletTransaction
                {
                    Id = Guid.NewGuid().ToString(),
                    BalanceTotal = 500,
                    Debit = 0,
                    Credit = 200,
                    Comment = "No comment",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.UtcNow.AddDays(-1),
                    CustomerId = "e74066e0-f90b-4858-b0ab-f5ae09b6c125",
                    OutletId = "f5134054-e657-42bf-bcf0-3d80a9e76470",
                    StaffId = "92cba2fc-ddb2-419b-b3f5-915e896814fe",
                    Customer = null,
                    Outlet = null,
                    Staff = null,
                },
                new WalletTransaction
                {
                    Id = Guid.NewGuid().ToString(),
                    BalanceTotal = 300,
                    Debit = 100,
                    Credit = 0,
                    Comment = "No comment",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    CustomerId = "e74066e0-f90b-4858-b0ab-f5ae09b6c125",
                    OutletId = "f5134054-e657-42bf-bcf0-3d80a9e76470",
                    StaffId = "92cba2fc-ddb2-419b-b3f5-915e896814fe",
                    Customer = null,
                    Outlet = null,
                    Staff = null,
                },
                new WalletTransaction
                {
                    Id = Guid.NewGuid().ToString(),
                    BalanceTotal = 150,
                    Debit = 0,
                    Credit = 150,
                    Comment = "No comment",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.UtcNow.AddDays(-3),
                    CustomerId = "e74066e0-f90b-4858-b0ab-f5ae09b6c125",
                    OutletId = "f5134054-e657-42bf-bcf0-3d80a9e76470",
                    StaffId = "92cba2fc-ddb2-419b-b3f5-915e896814fe",
                    Customer = null,
                    Outlet = null,
                    Staff = null,
                },
                new WalletTransaction
                {
                    Id = Guid.NewGuid().ToString(),
                    BalanceTotal = 600,
                    Debit = 0,
                    Credit = 600,
                    Comment = "No comment",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.UtcNow,
                    CustomerId = "e74066e0-f90b-4858-b0ab-f5ae09b6c125",
                    OutletId = "f5134054-e657-42bf-bcf0-3d80a9e76470",
                    StaffId = "92cba2fc-ddb2-419b-b3f5-915e896814fe",
                    Customer = null,
                    Outlet = null,
                    Staff = null,
                }
            };


            context.AddRange(walletTransactions);
            await context.SaveChangesAsync();
        }
        #endregion

        #region App Setting Type
        private static async Task SeedAppSettingTypeAsync(HarveyCRMLoyaltyDbContext context)
        {
            if (context.AppSettingTypes.Any())
                return;

            var appSettingTypes = new List<AppSettingType>()
            {
                new AppSettingType
                {
                    Id = 1,
                    TypeName = "AdminApp"
                },
                new AppSettingType
                {
                    Id = 2,
                    TypeName = "StoreApp"
                },
                new AppSettingType
                {
                    Id = 3,
                    TypeName = "MemberApp"
                },
                new AppSettingType
                {
                    Id = 4,
                    TypeName = "ValidatePhone"
                },
                new AppSettingType
                {
                    Id = 5,
                    TypeName = "WorkingTime"
                }
            };
            context.AddRange(appSettingTypes);
            await context.SaveChangesAsync();
        }
        #endregion

        #region Seed App Setting
        private static async Task SeedAppSettingAsync(HarveyCRMLoyaltyDbContext context)
        {
            if (context.AppSettings.Any())
                return;

            var appSettings = new List<AppSetting>()
            {
                new AppSetting
                {
                    Id = "1",
                    AppSettingTypeId = 2,
                    Name = "ExchangeRate",
                    Value="1",
                    GroupName = null
                },
                new AppSetting
                {
                    Id="2",
                    AppSettingTypeId=2,
                    Name="100",
                    Value="100",
                    GroupName="OptionRedeem"
                },
                new AppSetting
                {
                    Id="3",
                    AppSettingTypeId=2,
                    Name="200",
                    Value="200",
                    GroupName="OptionRedeem"
                },
                new AppSetting
                {
                    Id="4",
                    AppSettingTypeId=2,
                    Name="300",
                    Value="300",
                    GroupName="OptionRedeem"
                },
                new AppSetting
                {
                    Id="5",
                    AppSettingTypeId=2,
                    Name="500",
                    Value="500",
                    GroupName="OptionRedeem"
                },
                new AppSetting
                {
                    Id="6",
                    AppSettingTypeId=2,
                    Name="1000",
                    Value="1000",
                    GroupName="OptionRedeem"
                },
                new AppSetting
                {
                    Id="7",
                    AppSettingTypeId=2,
                    Name="2000",
                    Value="2000",
                    GroupName="OptionRedeem"
                },new AppSetting
                {
                    Id="8",
                    AppSettingTypeId=2,
                    Name="5000",
                    Value="5000",
                    GroupName="OptionRedeem"
                },
                new AppSetting
                {
                    Id="9",
                    AppSettingTypeId=4,
                    Name="65",
                    Value = "SG ^(8|9)\\d{7}$",
                    GroupName="ValidatePhone"
                },
                new AppSetting
                {
                    Id="10",
                    AppSettingTypeId=4,
                    Name="66",
                    Value ="TH ^(6|8|9)\\d{8}$",
                    GroupName="ValidatePhone"
                },
                new AppSetting
                {
                    Id="11",
                    AppSettingTypeId=4,
                    Name="63",
                    Value="PH ^(9)\\d{9}$",
                    GroupName="ValidatePhone"
                },
                new AppSetting
                {
                    Id="12",
                    AppSettingTypeId=4,
                    Name="852",
                    Value="HK ^(5|6|9)\\d{7}$",
                    GroupName="ValidatePhone"
                },
                new AppSetting
                {
                    Id="13",
                    AppSettingTypeId=4,
                    Name="60",
                    Value="MY ^(1)[0-46-9]\\d{7}$",
                    GroupName="ValidatePhone"
                },
                new AppSetting
                {
                    Id="14",
                    AppSettingTypeId=4,
                    Name="62",
                    Value="ID ^(8)\\d{10}$",
                    GroupName="ValidatePhone"
                },
                new AppSetting
                {
                    Id="15",
                    AppSettingTypeId=4,
                    Name="84",
                    Value="VN ^[3-67-9]\\d{8}$",
                    GroupName="ValidatePhone"
                },
                new AppSetting
                {
                    Id="16",
                    AppSettingTypeId=4,
                    Name="61",
                    Value="AU ^(4|5)\\d{8}$",
                    GroupName="ValidatePhone"
                },
                new AppSetting
                {
                    Id="17",
                    AppSettingTypeId=4,
                    Name="673",
                    Value="BN ^(2|7|8)\\d{7}$",
                    GroupName="ValidatePhone"
                },
                new AppSetting
                {
                    Id="18",
                    AppSettingTypeId=4,
                    Name="86",
                    Value="CN ^(1)[3-57-8]\\d{9}$",
                    GroupName="ValidatePhone"
                },
                new AppSetting
                {
                    Id="19",
                    AppSettingTypeId=4,
                    Name="91",
                    Value="IN ^[6-9]\\d{9}$",
                    GroupName="ValidatePhone"
                },
                new AppSetting
                {
                    Id="20",
                    AppSettingTypeId=5,
                    Name="StartWorkingTime",
                    Value= "10:00 AM",
                    GroupName ="WorkingTime"
                },
                new AppSetting
                {
                    Id="21",
                    AppSettingTypeId=5,
                    Name="EndWorkingTime",
                    Value= "11:00 PM",
                    GroupName="WorkingTime"
                },
                new AppSetting
                {
                    Id="22",
                    AppSettingTypeId=2,
                    Name="AutoTopupWalletValue",
                    Value= "10",
                    GroupName="RateValue"
                },
                new AppSetting
                {
                    Id="23",
                    AppSettingTypeId=2,

                    Name="IsAutoTopupWallet",
                    Value= "true",
                    GroupName="RateValue"
                }
                ,new AppSetting
                {
                    Id="24",
                    AppSettingTypeId=2,
                    Name="800",
                    Value="800",
                    GroupName="OptionRedeem"
                }
                ,new AppSetting
                {
                    Id="25",
                    AppSettingTypeId=2,
                    Name="RedeemRateValue",
                    Value="0.01",
                    GroupName="RateValue"
                },
                new AppSetting
                {
                    Id="26",
                    AppSettingTypeId=1,
                    Name="AdminAppTitle",
                    Value="TOG - Toy Or Game Executive Dashboard",
                    GroupName="AppTitleSetting"
                },
                new AppSetting
                {
                    Id="27",
                    AppSettingTypeId=2,
                    Name="StoreAppTitle",
                    Value="TOG - Toy Or Game At Your Service",
                    GroupName="AppTitleSetting"
                },
                new AppSetting
                {
                    Id="28",
                    AppSettingTypeId=3,
                    Name="MemberAppTitle",
                    Value="Trading Cards | Collectible Toys | Video Games | TOG - Toy Or Game",
                    GroupName="AppTitleSetting"
                }

            };
            context.AddRange(appSettings);
            await context.SaveChangesAsync();
        }
        #endregion

        #region Log Error Source
        private static async Task SeedErrorSourceAsync(HarveyCRMLoyaltyDbContext context)
        {
            if (context.ErrorLogSources.Any())
                return;

            var errorLogSources = new List<ErrorLogSource>()
            {
                new ErrorLogSource
                {
                    Id = 1,
                    SourceName = "AdminApp"
                },
                new ErrorLogSource
                {
                    Id =2,
                    SourceName = "MemberApp"
                },
                new ErrorLogSource
                {
                    Id =3,
                    SourceName = "StoreApp"
                },
                new ErrorLogSource
                {
                    Id = 4,
                    SourceName = "BackEnd"
                },
                new ErrorLogSource
                {
                    Id = 5,
                    SourceName = "FrontEnd"
                }
            };
            context.AddRange(errorLogSources);
            await context.SaveChangesAsync();
        }
        #endregion

    }
}
