//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Landyvest.Data;
//using Landyvest.Services.Emailing.Interface;
//using Landyvest.Services.ProcessPayment.Concrete;
//using Landyvest.Services.ProcessPayment.Interface;
//using Landyvest.Services.ShagoPayment.Electricty.Interface;
//using Landyvest.Services.Vatebra.Electricty.EKO.DTO;
//using Landyvest.Utilities.Common;
//using Landyvest.Utilities.Enums;

//namespace Landyvest.API.HangFireService
//{
//    public class BackgroundJobs
//    {
//        private readonly IConfiguration _configuration;
//        private IProcessPayment _processPayment;
//        private readonly IElectricityShago _electricityShago;
//        private readonly LandyvestAppContext _context;
//        public IEmailing _emailManager;
//        public BackgroundJobs(IEmailing emailManager, IConfiguration configuration, IElectricityShago electricityShago, LandyvestAppContext context, IProcessPayment processPayment)
//        {
//            _emailManager = emailManager;
//            _configuration = configuration;
//            _electricityShago = electricityShago;
//            _context = context;
//            _processPayment = processPayment;
//        }

//        public void ProcessEmail()
//        {
//            if (_configuration["EnableHangFire_Email"].ToLower() == "yes")
//            {
//                _emailManager.ProcessPendingEmails();
//            }
//        }

//        public async Task<int> ProcessEkoRequery()
//        {
//            try
//            {
//                 var innerJoin = from t in _context.Transactions
//                                join d in _context.DiscoTopUps on t.ID equals d.PaymentTransactionId
//                                where t.IsActive == true && t.Status == Status.SUCCESS && d.Token == null && d.ThirdPartyRefId == null && t.ProductServiceId == 15
//                                select new
//                                {
//                                    trans = t,
//                                    topup = d,

//                                };
//                //var innerJoin = from t in _context.Transactions
//                //                join d in _context.DiscoTopUps on t.ID equals d.PaymentTransactionId
//                //                where t.IsActive == true && t.Status == Status.SUCCESS && d.Token == null && d.ThirdPartyRefId == null && t.ProductServiceId == 15
//                //                select new
//                //                {
//                //                    trans = t,
//                //                    topup = d,

//                //                };

//                if (innerJoin.Count() > 0)
//                {
//                    foreach (var item in innerJoin)
//                    {
//                        var checkifDebited = await Utilities.Common.MiddleWare.IRestEkoRequeryVatebraAsync(item.topup.ReferenceNo, WebApiAddress.confirmtransactionstatus);

//                        var checkTransaction = JsonConvert.DeserializeObject<TransactionData>(checkifDebited.Content);

//                        if (checkTransaction != null &&
//                            checkTransaction.Token != null && checkTransaction.IsCompleted == true
//                            && checkTransaction.IsDebited == true
//                            && checkTransaction.IsReversed == false && checkTransaction.TransType == 1)
//                        {
//                            var gettrans = await _context.Transactions.Where(a => a.ReferenceNumber == item.topup.ReferenceNo).FirstOrDefaultAsync();

//                            gettrans.TransactionToken = checkTransaction.Token;

//                            var log = await _context.DiscoTopUps.Where(a => a.PaymentTransactionId == gettrans.ID).FirstOrDefaultAsync();

//                            log.Token = checkTransaction.Token;
//                            log.IsTokenGenerated = true;
//                            log.ThirdPartyRefId = checkTransaction.Id.ToString();
//                            log.Unit = Convert.ToString(checkTransaction.TokenUnit);

//                        }

//                        else if (checkTransaction != null &&
//                           checkTransaction.Token != null && checkTransaction.IsCompleted == true
//                           && checkTransaction.IsDebited == true
//                           && checkTransaction.IsReversed == false && checkTransaction.TransType == 2 && (checkTransaction.UCGResponse == "AWAITING_SERVICE_PROVIDER" || checkTransaction.UCGResponse == "CONFIRMED"))
//                        {
//                            var gettrans = await _context.Transactions.Where(a => a.ReferenceNumber == item.topup.ReferenceNo).FirstOrDefaultAsync();
//                            gettrans.Status = Status.SUCCESS;
//                            gettrans.TransactionToken = "N/A";

//                            var log = await _context.DiscoTopUps.Where(a => a.PaymentTransactionId == gettrans.ID).FirstOrDefaultAsync();

//                            log.IsTokenGenerated = true;
//                            log.Token = "N/A";
//                            log.ThirdPartyRefId = checkTransaction.Id.ToString();
//                        }

//                        else if (checkTransaction == null)
//                        {
//                            return 0;
//                        }
//                        else if(checkTransaction.IsReversed == true &&  checkTransaction.IsCompleted==true)
//                        {

//                            var getRole = await  (from t in   _context.Users
//                                          join d in _context.ApplicationUserRoles on t.Id equals d.UserId
//                                            where t.Id == item.trans.CreatedBy
//                                            select new
//                                            {
//                                                RoleName = d.Role.Name

//                                            }).FirstOrDefaultAsync();

//                            // revers the money back
//                            var reversalpayload = new ReversalPayload
//                            {
//                                TransactionId = item.topup.PaymentTransactionId,
//                                CustomerId = item.trans.CustomerId,
//                                CustomerType = getRole.RoleName,
//                                Amount = item.trans.TotalAmount,
//                                CommissionAmount = item.trans.Amount - item.trans.TotalAmount,
//                                ActualAmount = item.trans.Amount,
//                            };
                           
//                            var reversePayment=  await  _processPayment.ReversalAnsyc(reversalpayload);
//                            var gettrans = await _context.Transactions.Where(a => a.ReferenceNumber == item.topup.ReferenceNo).FirstOrDefaultAsync();
                            
//                            gettrans.Status = Status.FAILED;
                            
//                            var log = await _context.DiscoTopUps.Where(a => a.PaymentTransactionId == gettrans.ID).FirstOrDefaultAsync();

//                        }

//                        else
//                        {


//                        }
                        


//                    }
//                    await _context.SaveChangesAsync();
                   
//                }
//                return 1;
//            }
//            catch (Exception ex)
//            {
//                string d = ex.Message;
//                return 1;
//            }
//        }
//    }
//}