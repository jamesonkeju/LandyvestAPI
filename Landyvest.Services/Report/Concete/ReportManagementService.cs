using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Data;
using Landyvest.Data.Models.ViewModel;
using Landyvest.Services.DataAccess;
using Landyvest.Services.Report.DTO;
using Landyvest.Services.Report.Interface;
using Landyvest.Utilities.Common;

namespace Landyvest.Services.Report.Concete
{
   public class ReportManagementService : IReportManagement
    {
        private LandyvestAppContext _context;

        public ReportManagementService(LandyvestAppContext context)
        {
            _context = context;
        }

        
            public async Task<List<loadSummaryDashboardViewModel>> AdminLoadSummaryofDashboard()
        {
            var loadData = new List<loadSummaryDashboardViewModel>();


            try
            {

                var sd = _context.Database.GetDbConnection().ConnectionString;
                AccessDataLayer accessDataLayer = new AccessDataLayer(_context);
                DBManager dBManager = new DBManager(_context);

                var parameters = new List<IDbDataParameter>();
                
                var data = accessDataLayer.FetchRolePermissionsByRoleId(parameters.ToArray(), "MegaDealerDashboard");

                if (data.Rows.Count > 0)
                {

                    foreach (DataRow row in data.Rows)
                    {
                        var single = new loadSummaryDashboardViewModel();
                        single.Description = row["Description"].ToString();
                        single.MappingItem = row["MappingItem"].ToString();
                        single.Total = Convert.ToDecimal(row["Total"]);

                        loadData.Add(single);
                    }


                }
                return loadData;
            }
            catch (Exception ex)
            {
                return loadData;

            }
        }
        public async Task<List<loadSummaryDashboardViewModel>> LoadSummaryofDashboard(string UserId)
        {
            var loadData = new List<loadSummaryDashboardViewModel>();


            try
            {
              
                var sd = _context.Database.GetDbConnection().ConnectionString;
                AccessDataLayer accessDataLayer = new AccessDataLayer(_context);
                DBManager dBManager = new DBManager(_context);

                var parameters = new List<IDbDataParameter>();
                parameters.Add(dBManager.CreateParameter("@UserId", UserId, DbType.String));

                var data = accessDataLayer.FetchRolePermissionsByRoleId(parameters.ToArray(), "DashboardReport");

                if (data.Rows.Count > 0)
                {

                    foreach (DataRow row in data.Rows)
                    {
                        var single = new loadSummaryDashboardViewModel();
                        single.Description = row["Description"].ToString();
                        single.MappingItem = row["MappingItem"].ToString();
                        single.Total = Convert.ToDecimal(row["Total"]);

                        single.RequestId = row["RequestId"].ToString();
                        if (row["CreatedDate"] != null)
                        {
                            single.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                        }
                        
                        loadData.Add(single);
                    }


                }
                return loadData;
            }
            catch (Exception ex)
            {
                return loadData;

            }
        }

        public async Task<List<loadSummaryDashboardViewModel>> Top10SuccessfulTransaction(string UserId, int Today)
        {
            var loadData = new List<loadSummaryDashboardViewModel>();


            try
            {

                var sd = _context.Database.GetDbConnection().ConnectionString;
                AccessDataLayer accessDataLayer = new AccessDataLayer(_context);
                DBManager dBManager = new DBManager(_context);

                var parameters = new List<IDbDataParameter>();
                parameters.Add(dBManager.CreateParameter("@UserId", UserId, DbType.String));
                parameters.Add(dBManager.CreateParameter("@Today", Today, DbType.Int32));
                var data = accessDataLayer.FetchRolePermissionsByRoleId(parameters.ToArray(), "spTop10TransactionsTodayAndYesterday");

                if (data.Rows.Count > 0)
                {

                    foreach (DataRow row in data.Rows)
                    {
                        var single = new loadSummaryDashboardViewModel();
                        single.Description = row["Description"].ToString();
                        single.Total = Convert.ToDecimal(row["Amount"]);
                        single.RequestId = row["ReferenceNumber"].ToString();
                        if (row["CreatedDate"] != null)
                        {
                            single.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                        }

                        loadData.Add(single);
                    }


                }
                return loadData;
            }
            catch (Exception ex)
            {
                return loadData;

            }
        }

        public async Task<List<TransactionView>> TransactionHistoryView(string UserId)
        {
            var loadData = new List<TransactionView>();


            try
            {

                var sd = _context.Database.GetDbConnection().ConnectionString;
                AccessDataLayer accessDataLayer = new AccessDataLayer(_context);
                DBManager dBManager = new DBManager(_context);

                var parameters = new List<IDbDataParameter>();
                parameters.Add(dBManager.CreateParameter("@UserId", UserId, DbType.String));

                var data = accessDataLayer.FetchRolePermissionsByRoleId(parameters.ToArray(), "TransactionReport");

                if (data.Rows.Count > 0)
                {

                    foreach (DataRow row in data.Rows)
                    {
                        var single = new TransactionView();
                        single.Description = row["Description"].ToString();
                        single.Amount =  Convert.ToDecimal(row["Total"].ToString());

                        loadData.Add(single);
                    }


                }
                return loadData;
            }
            catch (Exception ex)
            {
                return loadData;

            }
        }

        public async Task<List<InFlowOutFlowReportView>> InFlowOutFlowView(string UserId)
        {
            var loadData = new List<InFlowOutFlowReportView>();


            try
            {

                var sd = _context.Database.GetDbConnection().ConnectionString;
                AccessDataLayer accessDataLayer = new AccessDataLayer(_context);
                DBManager dBManager = new DBManager(_context);

                var parameters = new List<IDbDataParameter>();
                parameters.Add(dBManager.CreateParameter("@UserId", UserId, DbType.String));

                var data = accessDataLayer.FetchRolePermissionsByRoleId(parameters.ToArray(), "InFlowOutFlowReport");

                if (data.Rows.Count > 0)
                {

                    foreach (DataRow row in data.Rows)
                    {
                        var single = new InFlowOutFlowReportView();
                        single.Description = (row["Description"].ToString());
                        single.Amount = Convert.ToDecimal(row["Total"].ToString());

                        loadData.Add(single);
                    }


                }
                return loadData;
            }
            catch (Exception ex)
            {
                return loadData;

            }
        }

        public async Task<List<loadSummaryDashboardViewModel>> Top10WalletTransaction(string UserId)
        {
            var loadData = new List<loadSummaryDashboardViewModel>();


            try
            {

                var sd = _context.Database.GetDbConnection().ConnectionString;
                AccessDataLayer accessDataLayer = new AccessDataLayer(_context);
                DBManager dBManager = new DBManager(_context);

                var parameters = new List<IDbDataParameter>();
                parameters.Add(dBManager.CreateParameter("@UserId", UserId, DbType.String));
                //parameters.Add(dBManager.CreateParameter("@Today", Today, DbType.Int32));
                var data = accessDataLayer.FetchRolePermissionsByRoleId(parameters.ToArray(), "sp_WalletActivitesTransactionsTop10");

                if (data.Rows.Count > 0)
                {

                    foreach (DataRow row in data.Rows)
                    {
                        var single = new loadSummaryDashboardViewModel();
                        single.Status = row["WalletTransactionType"].ToString();
                        single.Amount = CurrencyUtil.formatAmount(Convert.ToDecimal(row["Amount"]).ToString());
                        single.Transtype =  row["TransactionType"].ToString();

                        if (row["CreatedDate"] != null)
                        {
                            single.TransDate =  Convert.ToDateTime(row["CreatedDate"]).ToShortDateString() + " " + Convert.ToDateTime(row["CreatedDate"]).ToShortTimeString();
                        }

                        if (row["WalletTransactionType"].ToString() == "Credit")
                        {
                            single.icon = "images/arrow-up2.png";
                            single.Cssclass = "text-success";
                        }
                        else
                        {
                            single.icon = "images/arrow-down2.jpg";
                            single.Cssclass = "text-danger";
                        }

                        loadData.Add(single);
                    }

                    

                }
                return loadData;
            }
            catch (Exception ex)
            {
                return loadData;

            }
        }

       
    }
}
