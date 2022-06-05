using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.Common
{
    public class ApplicationResponseCode
    {



        public static ErrorMessage LoadErrorMessageByCode(string Code)
        {

            try
            {

                var dataResult = ErrorMessageDataBank().ToList();
                return dataResult.Where(a => a.Code == Code).FirstOrDefault();

            }
            catch (Exception ex)
            {
                var mess = new ErrorMessage
                {
                    Code = "Unknowing",
                    Name = ex.Message
                };
                return mess;
            }
        }



        public class ErrorMessage
        {
            public string Name { get; set; }
            public string Code { get; set; }

        }


        private static List<ErrorMessage> ErrorMessageDataBank()
        {
            var listData = new List<ErrorMessage>();

            var data = new ErrorMessage();


            try
            {

                var data01 = new ErrorMessage
                {
                    Code = "1000",
                    Name = "Inner Error"
                };
                listData.Add(data01);


                var data02 = new ErrorMessage
                {
                    Code = "100",
                    Name = "Successful"
                };
                listData.Add(data02);


                var data03 = new ErrorMessage
                {
                    Code = "101",
                    Name = "Some properties are not valid"
                };
                listData.Add(data03);



                var data04 = new ErrorMessage
                {
                    Code = "102",
                    Name = "Invalid UserName or Password"
                };
                listData.Add(data04);

     

                var data103 = new ErrorMessage
                {
                    Code = "103",
                    Name = "Your account is turn off on this portal, kindly contact the system adminstrator"
                };
                listData.Add(data103);


                var data014 = new ErrorMessage
                {

                    Code = "104",
                    Name = "Your account is disabled on this portal, kindly contact the system adminstrator"
                };
                listData.Add(data014);


                var data105 = new ErrorMessage
                {
                    Code = "105",
                    Name = "You are yet to confirm your email, kindly check your email for the confirmation email"
                };
                listData.Add(data105);

                var data106 = new ErrorMessage
                {

                    Code = "106",
                    Name = "For security reasons, you would be required to change your system generated password"
                };
                listData.Add(data106);


                var data107 = new ErrorMessage
                {

                    Code = "107",
                    Name = "You have logged in successfully"
                };
                listData.Add(data107);


                var data108 = new ErrorMessage
                {
                    Code = "108",
                    Name = "You are not permitted to perform this operation"
                }
            ;
                listData.Add(data108);


                var data109 = new ErrorMessage
                {
                    Code = "109",
                    Name = "Password change was successful, please logIn with your new detail"
                };

                listData.Add(data109);


                var data110 = new ErrorMessage
                {
                    Code = "110",
                    Name = "The operation was not successful"
                };
                listData.Add(data110);



                var data11 = new ErrorMessage
                {

                    Code = "111",
                    Name = "Password doesn't match."
                };
                listData.Add(data11);


                var data112 = new ErrorMessage
                {
                    Code = "112",
                    Name = "Password change was successful, please logIn with your new detail."
                };
                listData.Add(data112);


                var data113 = new ErrorMessage
                {
                    Code = "113",
                    Name = "Your old password didn't match, please try again."
                };
                listData.Add(data113);


                var data114 = new ErrorMessage
                {
                    Code = "114",
                    Name = "You can't use any of your old password(s), please try with a new password"
                };
                listData.Add(data114);

                var data115 = new ErrorMessage
                {
                    Code = "115",
                    Name = "No record was found."
                };
                listData.Add(data115);


                var data116 = new ErrorMessage
                {
                    Code = "116",
                    Name = "Dealer not provisioned for disco."
                };
                listData.Add(data116);

              
                
                
                
                
                
                
                /// 

                var data200 = new ErrorMessage
                {
                    Code = "200",
                    Name = "Your record was saved successfully"
                };
                listData.Add(data200);



                var data201 = new ErrorMessage
                {
                    Code = "201",
                    Name = "Your record was updated successfully"
                };
                listData.Add(data201);




                var data700 = new ErrorMessage
                {
                    Code = "700",
                    Name = "User is already existing!"
                };
                listData.Add(data700);





                var data701 = new ErrorMessage
                {
                    Code = "701",
                    Name = "User Id is required for this operation!"
                };
                listData.Add(data700);


                var data702 = new ErrorMessage
                {
                    Code = "702",
                    Name = "Unable to retrieve balance at the moment, please try again"
                };
                listData.Add(data702);



                var data800 = new ErrorMessage
                {
                    Code = "800",
                    Name = "You are not permitted to create a sub dealer under this customer"
                };
                listData.Add(data800);

                var data801 = new ErrorMessage
                {
                    Code = "801",
                    Name = "Insufficient balance"
                };
                listData.Add(data801);

                var data802 = new ErrorMessage
                {
                    Code = "802",
                    Name = "Error processing payment"
                };
                listData.Add(data802);

                var data803 = new ErrorMessage
                {
                    Code = "803",
                    Name = "Payment processed successfully"
                };
                listData.Add(data803);

                var data804 = new ErrorMessage
                {
                    Code = "804",
                    Name = "Error updating payment"
                };
                listData.Add(data804);

                var data805 = new ErrorMessage
                {
                    Code = "805",
                    Name = "Payment updated successfully"
                };
                listData.Add(data805);

                var data806 = new ErrorMessage
                {
                    Code = "806",
                    Name = "Payment reversed successfully"
                };
                listData.Add(data806);

                var data807 = new ErrorMessage
                {
                    Code = "807",
                    Name = "Error reversing payment"
                };
                listData.Add(data807);

                var data808 = new ErrorMessage
                {
                    Code = "808",
                    Name = "Error while Creating User"
                };
                listData.Add(data808);

                var data3000 = new ErrorMessage
                {
                    Code = "3000",
                    Name = "Invalid Token"
                };
                listData.Add(data3000);

                var data3001 = new ErrorMessage
                {
                    Code = "3001",
                    Name = "Expired Token"
                };
                listData.Add(data3001);

                var data3002 = new ErrorMessage
                {
                    Code = "3002",
                    Name = "Kindly validate your account with OTP"
                };
                listData.Add(data3002);


              

                var data3003 = new ErrorMessage
                {
                    Code = "3003",
                    Name = "Requery was not successful"
                };
                listData.Add(data3003);


                var data3004 = new ErrorMessage
                {
                    Code = "3004",
                    Name = "Your account was not debited"
                };
                listData.Add(data3004);


                var data3005 = new ErrorMessage
                {
                    Code = "3005",
                    Name = "No active commission found for the selected product, please contact the system administrator"
                };
                listData.Add(data3005);



                var data3006 = new ErrorMessage
                {
                    Code = "3006",
                    Name = "Token Generated Successful"
                };

                listData.Add(data3006);

                var data3007 = new ErrorMessage
                {
                    Code = "3007",
                    Name = "Wallet Funding Not Successful"
                };
                listData.Add(data3007);


                var data3008 = new ErrorMessage
                {
                    Code = "3008",
                    Name = "No Valid card was found found"
                };
                listData.Add(data3008);


                var data3009 = new ErrorMessage
                {
                    Code = "3009",
                    Name = "User not valid"
                };
                listData.Add(data3009);


                var data3010 = new ErrorMessage
                {
                    Code = "3010",
                    Name = "Transaction failed"
                };
                listData.Add(data3010);

                var data3011 = new ErrorMessage
                {
                    Code = "3011",
                    Name = "Commission Was successfullly Transfered"
                };
                listData.Add(data3011);


                var data3012 = new ErrorMessage
                {
                    Code = "3012",
                    Name = "Dealer does not have such agent under him, kindly check the agent id and check again"
                };
                listData.Add(data3012);

                var data3013 = new ErrorMessage
                {
                    Code = "3013",
                    Name = "User Created Successfully "
                };
                listData.Add(data3013);

                var data3014 = new ErrorMessage
                {
                    Code = "3014",
                    Name = "Amount must be greater than 0"
                };
                listData.Add(data3014);


                var data600 = new ErrorMessage
                {
                    Code = "600",
                    Name = "The input is not a valid Base - 64 string as it contains a non - base 64 character, more than two padding characters, or an illegal character among the padding characters."
                };
                listData.Add(data600);

                var data601 = new ErrorMessage
                {
                    Code = "601",
                    Name = "Transaction Reference is Required"
                };
                listData.Add(data601);

                var data602 = new ErrorMessage
                {
                    Code = "602",
                    Name = "Transaction Reference Already Exist"
                };
                listData.Add(data602);

                var data604 = new ErrorMessage
                {
                    Code = "604",
                    Name = "Error Occured While Creating Your Virtual Account, Kindly contact the Landyvest Adminsitrator"
                };
                listData.Add(data604);

                var data606 = new ErrorMessage
                {
                    Code = "606",
                    Name = "Unauthorizes Access"
                };
                listData.Add(data606);

                var data608 = new ErrorMessage
                {
                    Code = "608",
                    Name = "Error Validating Transaction"
                };
                listData.Add(data608);

                var data609 = new ErrorMessage
                {
                    Code = "609",
                    Name = "Error Occured while funding wallet"
                };
                listData.Add(data609);

                var data707 = new ErrorMessage
                {
                    Code = "707",
                    Name = "The Password must contain at least 8 digit, a combination of lowercase, uppercase and a special chracter"
                };
                listData.Add(data707);



                return listData.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
