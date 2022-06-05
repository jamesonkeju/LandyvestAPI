using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.Enums
{
    public enum TransactionType
    {
        FUNDWALLET = 1,
        AIRTIME = 2,
        DATATOPUP = 3,
        ELECTRICITY = 4,
        WALLETTRANSFER = 5,
        WALLETDEPOSIT = 6,
        BET = 7,
        TOLL = 8,
        CABLE = 9,
        INSURANCE = 10,
        COMMISIONTRANSFER = 11

    }

    public enum PaymentChannel
    {

        [Display(Name = "Card")]
        CARD,
        [Display(Name = "Bank")]
        BANK,
        [Display(Name = "Wallet")]
        WALLET,
        [Display(Name = "Commission")]
        COMMISSION,
            [Display(Name = "Manual")]
        MANUAL



    }

    public enum WalletTransactionType
    {
        CREDIT=1,
        DEBIT=2
    }

    public enum WalletType
    {
        WALLET=1,
        COMMISSION=2
    }

    public enum MeterType
    {
        Prepaid = 1, Postpaid
    }

    public enum Status
    {
        [Display(Name = "Pending")]
        PENDING = 100,
        [Display(Name = "Successful")]
        SUCCESS = 200,
        [Display(Name = "Failed")]
        FAILED = 300,
        INVALID_CUSTOMER = 400,
        AMOUNT_BELOW_MINIMUM_SINGLE_TRANSACTION_LIMIT = 500,
        AMOUNT_EXCEEDED_MAXIMUM_SINGLE_TRANSACTION_LIMIT = 600,
        AMOUNT_EXCEEDED_MAXIMUM_WALLET_LIMIT = 700,
        AMOUNT_PAYMENT_FAILED = 800,
        RECORD_NOT_FOUND = 900,
        HAS_SUFFICIENT_BALANCE = 901,
        INSUFFICIENT_BALANCE = 902,
        INVALID_TRANSACTION = 903,
        CREDIT_TOPUP_FAIL = 609,
        CREDIT_TOPUP_SUCCEED = 700

    }



}
