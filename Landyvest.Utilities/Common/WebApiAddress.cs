using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.Common
{
    public static class WebApiAddress
    {
        public static string LoginPost = "Account/Login/Login";
        public static string ForgetPasswordPost = "Account/ForgetPassword/ForgetPassword";
        public static string ChangePasswordPost = "Account/ChangePassword/ChangePassword";
        public static string UpdatePassword = "Account/UpdatePassword/UpdatePassword";
        public static string RegisterPost = "User/AddUpdateCustomer/Add-Update-Customer";

        public static string CreateUpdateCountryPost = "Country/AddUpdateCountry/Add-Update-Country";
        public static string FetchCountryGet = "Country/GetAllCountries/GetAllCountries";


        public static string DealerC = "Dealer/Dealer/GetAllCountries";

        public static string DealerReg = "Dealer/RegisterDealer/Add-Registration";

        public static string Paystack_InitializePayment = "Paystack/InitializePayment/Paystack";

        public static string confirmtransactionstatus = "confirmtransactionstatus";

        public static string FirstLevelApproval = "Dealer/GetFirstLevelApproval/GetFirstLevelApproval";
        public static string SecondLevelApproval = "Dealer/GetSecondLevelApproval/GetSecondLevelApproval";
        public static string ThirdLevelApproval = "Dealer/GetThirdLevelApproval/GetThirdLevelApproval";
        public static string Process = "Dealer/ProcessRequest/ProcessRequest";
        public static string GetRequest = "Dealer/GetRequest/GetRequest";
        public static string Decline = "Dealer/DeclineRequest/DeclineRequest";
        public static string BaseDocumentUrl = "Dealer/DeclineRequest/DeclineRequest";


        public static string DataPricingLookUp = "Data/DataPricingLookUp/DataPricingLookUp";
        public static string DataVending = "Data/DataVending/DataVending";
        public static string AirtimeVending = "Airtime/AirtimeVending/AirtimeVending";
        public static string DstvLookUp = "Dstv/LoadDstvPackages/LoadDstvPackages";
        public static string DstvPayment = "Dstv/PurchaseDstv/PurchaseDstv";
        public static string GotvPayment = "Gotv/PurchaseGotv/PurchaseGotv";
        public static string GotvLookUp = "Gotv/LoadGotvPackages/LoadGotvPackages";

        public static string StartimesLookUp = "StarTimes/LoadStarTimesPackages/LoadStarTimesPackages";
        public static string StartimesPayment = "StarTimes/PurchaseStarTimes/PurchaseStarTimes";









        public static string BetAccountValidation = "Bet/AccountValidation/AccountValidation";
        public static string BetVending = "Bet/BetPayment/BetPayment";
        public static string InsuranceVending = "Insurance/PurchaseLawAndUnionInsurance/PurchaseLawAndUnionInsurance";


        public static string LawAndUnionInsurancePricing = "insurance/LawAndUnionInsurancePricing/LawAndUnionInsurancePricing";
        //   public static string BetAccountValidation = "Bet/AccountValidation/AccountValidation";

        public static string ValidateCustomerTollAccount = "Toll/AccountValidation/AccountValidation";

        public static string TollPayment = "Toll/TollPayment/TollPayment";
        public static string FetchAllProducts = "Product/GetAllProduct/GetAllProduct";


        /// This apis are for Vatebra APIs
        public static string ValidateCustomerMeterNo = "Electricity/AccountValidationRespone/AccountValidationRespone";
        public static string VendCustomerMeterNo = "Electricity/PurchaseToken/PurchaseToken";

        public static string ValidateVatebraCustomerMeterNo = "verify-meter-updated";

        public static string ValidateVatebraCustomerMeterNoEKO = "verify-meter-updated_Eko";

        // Process Payment
        public static string ProcessPayment = "ProcessPayment/Process/ProcessPayment";
        public static string ReversePayment = "ProcessPayment/Reversal/ReversePayment";
        public static string UpdatePayment = "ProcessPayment/Update/UpdatePayment";

        //Wallet Funding
        public static string FundWallet = "Wallet/FundSubdealer/FundSubdealer";

        public static string FundSuperdealerWallet = "Wallet/ManualFundWallet/ManualFundWallet";


        //Transaction Log
        public static string LogAirtime = "Vending/LogAirtimeVending/LogAirtimeVending";

        public static string LogBet = "Vending/LogBetvending/LogBetvending";
        public static string LogToll = "Vending/LogLogTollvending/LogLogTollvending";
        public static string LogInsurance = "Vending/LogInsurancevending/LogInsurancevending";
        public static string LogData = "Vending/LogDataVending/LogDataVending";
        public static string LogCable = "Vending/LogCableVending/LogCableVending";
        public static string LogDisco = "Vending/LogDiscoVending/LogDiscoVending";








        // Hub url 
        public static string FetchCustomerTransaction = "Account/FettchTrxRecord?AccountNo={AccountNo}&dealercode={dealercode}&transType={transType}";

        public static string PostTransaction = "post-vtu-transaction";
        public static string RequeryEkoTransaction = "ReQueryTransaction";



        //monnify
        public static string GetAccessToken = "api/v1/auth/login";
        public static string GetReservedAccount = "api/v2/bank-transfer/reserved-accounts";
        public static string GetTransactionStatus = "api/v2/transactions/";

    }

    public static class ProcessStatus
    {
        public static int Pending = 1;
        public static int Processed = 2;
        public static int BusinessMananger = 3;

    }

    public static class Cables
    {
        public static string DSTV = "DSTV";
        public static string GOTV = "GOTV";
        public static string STARTIMES = "STARTIMES";
    }
}
