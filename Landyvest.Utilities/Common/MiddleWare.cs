using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Json;
using System.Threading;
using Microsoft.Extensions.Configuration;
using RestSharp;
using Newtonsoft.Json;



namespace Landyvest.Utilities.Common
{
    public class MiddleWare
    {

        private static string Baseurl = ConfigHelper.AppSetting("baseurl");

        private static string HubBaseurlProduction = ConfigHelper.AppSetting("HubBaseurl");

        private static string shagoProduction = ConfigHelper.AppSetting("ShagoProduction", "shagoParameter");

        private static string monnifyBaseUrlTest = ConfigHelper.AppSetting("MonnifyBaseUrlTest", "MonnifyParameter");

        private static string ContractCode = ConfigHelper.AppSetting("ContractCode", "MonnifyParameter");













        public static async Task<string> GetAsync(string ApiCall, string token, bool addToken)
        {

            string buildurl = Baseurl + ApiCall;

            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();

                if (addToken == true)
                {
                    client.DefaultRequestHeaders.Add("Authorization", token);
                }
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage resultRespone = await client.GetAsync(ApiCall);

                using (var response = await client
                                                            .GetAsync(buildurl)
                                                            .ConfigureAwait(false))
                {
                    var data = await response.Content.ReadAsStringAsync();

                    return data;
                }
            }
        }



        public static async Task<string> PostBasicAsync(object content, CancellationToken cancellationToken, string token, string apiurl, bool addToken = true)
        {
            string buildurl = Baseurl + apiurl;


            var client = new HttpClient();


            using (var request = new HttpRequestMessage(HttpMethod.Post, buildurl))
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(content);


                if (addToken == true)
                {

                    client.DefaultRequestHeaders.Add("token", token);
                    client.DefaultRequestHeaders.Add("Authorization", token);

                }

                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    using (var response = await client
                                       .PostAsync(buildurl, stringContent)
                                       .ConfigureAwait(false))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        return data;
                    }

                }
            }
        }

        public static async Task<string> PostBasicAsync(object content, CancellationToken cancellationToken, string apiurl)
        {

            string buildurl = Baseurl + apiurl;

            var client = new HttpClient();

            using (var request = new HttpRequestMessage(HttpMethod.Post, buildurl))
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(content);



                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    using (var response = await client
                                                             .PostAsync(buildurl, stringContent)
                                                             .ConfigureAwait(false))
                    {

                        var data = await response.Content.ReadAsStringAsync();

                        return data;
                    }

                }
            }
        }







        public static async Task<IRestResponse> IRestPostAsync(object payload)
        {
            

            try
            {
                string shagoUrl = "";
                string shagoHashkey = "";

                if (shagoProduction.ToUpper().ToUpper() == "NO")
                {
                   shagoUrl = ConfigHelper.AppSetting("shagoUrlTest", "shagoParameter");
                   shagoHashkey = ConfigHelper.AppSetting("shagoHashKeyTest","shagoParameter");
                }
                else
                {
                    shagoUrl = ConfigHelper.AppSetting("shagoUrlProduction","shagoParameter");
                    shagoHashkey = ConfigHelper.AppSetting("shagoHashKeyProduction","shagoParameter");
                }


                string payloadtoJson = JsonConvert.SerializeObject(payload);

                var client = new RestClient(shagoUrl);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);

                request.AddHeader("Content-Type", "application/json");

                request.AddHeader("hashKey", shagoHashkey);
            

                request.AddParameter("application/json", payloadtoJson, ParameterType.RequestBody);
                IRestResponse responsetask = client.Execute(request);
                return responsetask;


            }


            catch (Exception ex)
            {
                return null;
            }

        }

        public static async Task<string> IRestEkoRequeryVatebraAsync(string ApiCall, string token, bool addToken)
        {
            string buildurl = "";

            if (ConfigHelper.AppSetting("Production").ToUpper() == "YES")
            {
                buildurl = ConfigHelper.AppSetting("MproxyBaseurlproduction") + ApiCall;
            }
            else
            {
                buildurl = ConfigHelper.AppSetting("MproxyBaseurlTest") + ApiCall;
            }

            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(buildurl);
                client.DefaultRequestHeaders.Clear();

                if (addToken == true)
                {
                    client.DefaultRequestHeaders.Add("Authorization", token);
                }
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage resultRespone = await client.GetAsync(ApiCall);

                using (var response = await client.GetAsync(buildurl).ConfigureAwait(false))
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return data;
                }
            }
        }

        public static async Task<IRestResponse> IRestEkoRequeryVatebraAsync(object payload, string url)
        {

            try
            {
                string dataurl = "";

                if (ConfigHelper.AppSetting("Production").ToUpper() == "YES")
                {

                    dataurl = ConfigHelper.AppSetting("MproxyBaseurlproduction") + "requeryucgtransaction?transRefNum=" + payload;
                }
                else
                {
                    dataurl = ConfigHelper.AppSetting("MproxyBaseurlTest") + "requeryucgtransaction?transRefNum=" + payload;

                }

                string payloadtoJson = JsonConvert.SerializeObject(payload);

                var client = new RestClient(dataurl);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);

                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", payloadtoJson, ParameterType.GetOrPost);
                IRestResponse responsetask = client.Execute(request);
                return responsetask;
            }


            catch (Exception ex)
            {
                return null;
            }

        }


        public static async Task<IRestResponse> IRestPostVatebraAsync(object payload, string url)
        {

            try
            {
                string dataurl = "";

                if (ConfigHelper.AppSetting("Production").ToUpper() == "YES")
                {
                    dataurl = ConfigHelper.AppSetting("HubBaseurl") + url;
                }
                else
                {
                    dataurl = ConfigHelper.AppSetting("HubBaseurlTest") + url;
                }

                string payloadtoJson = JsonConvert.SerializeObject(payload);

                var client = new RestClient(dataurl);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);

                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", payloadtoJson, ParameterType.RequestBody);
                IRestResponse responsetask = client.Execute(request);
                return responsetask;
            }


            catch (Exception ex)
            {
                return null;
            }

        }

        public static async Task<IRestResponse> IRestPosturlAsync(string url)
        {
            string dataurl = "";

            if (ConfigHelper.AppSetting("Production").ToUpper() == "YES")
            {
                dataurl = ConfigHelper.AppSetting("HubBaseurl") + url;
            }
            else
            {
                dataurl = ConfigHelper.AppSetting("HubBaseurlTest") + url;
            }
            try
            {
                var client = new RestClient(dataurl);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/xml");
                IRestResponse responsetask = client.Execute(request);
                return responsetask;


            }


            catch (Exception ex)
            {
                return null;
            }

        }


        public static async Task<string> PostMonnifyAsync(object content, CancellationToken cancellationToken, string token, string apiurl, bool addToken = true)
        {
            string buildurl = monnifyBaseUrlTest + apiurl;


            var client = new HttpClient();


            using (var request = new HttpRequestMessage(HttpMethod.Post, buildurl))
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(content);


                if (addToken == false)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Basic "+ token);
                }

                if (addToken == true)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }

                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    using (var response = await client
                                       .PostAsync(buildurl, stringContent)
                                       .ConfigureAwait(false))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        return data;
                    }

                }
            }
        }


        public static async Task<string> GetMonnifyAsync(string content, CancellationToken cancellationToken, string token, string apiurl, bool addToken)
        {

            string buildurl = monnifyBaseUrlTest + apiurl+content;

            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(buildurl);
                client.DefaultRequestHeaders.Clear();
                if (addToken == false)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Basic " + token);
                }

                if (addToken == true)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }

                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               

                using (var response = await client.GetAsync(buildurl).ConfigureAwait(false))
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return data;
                }
            }
           
        }
    }
}