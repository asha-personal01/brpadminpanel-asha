using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Web;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using D1WebApp.Models;
using D1WebApp.ViewModels;
using RestSharp;

namespace D1WebApp.Utilities.Classes
{
    public class WebApiRequest : IDisposable
    {
        public WebApiRequest()
        {
        }


        public void SetConfigurations(string CUST_ID_PREFIX, string ApiEndPoint, string AuthonticationToken, string client, string company, string username, string password, string AppPaths)
        {
            string urls = string.Empty;
            try
            {
                HttpClient clients = new HttpClient();
                var UrlConfig = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("UserApiPath");
                urls = UrlConfig.ConfigurationValue + CUST_ID_PREFIX + "API";
                urls = urls + "/Ecommerce/InsertUpdateConfiguration?CUST_ID_PREFIX=" + CUST_ID_PREFIX + "&ApiEndPoint=" + ApiEndPoint + "&AuthonticationToken=" + AuthonticationToken + "&client=" + client + "&company=" + company + "&username=" + username + "&password=" + password + "&AppPath=" + AppPaths;
                //clients.BaseAddress = new Uri(urls);
                HttpResponseMessage response = clients.GetAsync(urls).Result;
                JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                ErrorLogs.ErrorLog(0, "SetConfigurations", DateTime.Now, "SetConfigurations", ex.ToString() + urls, "SetConfigurations", 2);

            }

        }

        public void DataPullMehod(string CUST_ID_PREFIX)
        {
            string urls = string.Empty;
            try
            {
                HttpClient clients = new HttpClient();
                var UrlConfig = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("UserApiPath");
                urls = UrlConfig.ConfigurationValue + CUST_ID_PREFIX + "API";
                urls = urls + "/Ecommerce/DataMigration";
                //clients.BaseAddress = new Uri(urls);
                HttpResponseMessage response = clients.GetAsync(urls).Result;
                JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                ErrorLogs.ErrorLog(0, "DataPullMehod", DateTime.Now, "DataPullMehod", ex.ToString() + urls, "DataPullMehod", 2);

            }

        }


        public string GetAuthonticationToken(string clients, string company, string username, string password, string ApiEndPoint)
        {
            string cacheList = string.Empty;
            try
            {
                string urls = ApiEndPoint + "/distone/rest/service/authorize/grant?client=" + clients + "&company=" + company + "&username=" + username + "&password=" + password;
                var client = new RestClient(urls);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "3bfe68f2-3553-ed2a-c580-56ac85a550cf");
                request.AddHeader("cache-control", "no-cache");
                IRestResponse response = client.Execute(request);
                AuthTokenViewModel getobj = new AuthTokenViewModel();
                getobj = JsonConvert.DeserializeObject<AuthTokenViewModel>(response.Content.ToString());
                return getobj.grant_token;
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetAuthonticationToken", DateTime.Now, "GetAuthonticationToken", ed.ToString(), "GetAuthonticationToken", 2);
                return cacheList;
            }

        }

        public dynamic InsertTrackingNo(string ApiEndpoint, string Token, string Company, string Manifest_id, string Order, string rec_type, string tracking_no, string manifest_carrier, string service_type, string ship_via_code, string ship_date, string pkg_wgt, string pkg_no, string pack_type, string descr)
        {
            try
            {
                rec_type = "P";
                string voided = "N";
                var client = new RestClient(ApiEndpoint + "/distone/rest/service/data/create");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "23955322-fd26-0bea-6ba6-f47d3363ea3a");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", Token);
                request.AddParameter("application/json", "{\r\n                \"request\": {\r\n                                \"records\": [{\r\n                                                \"company_oe\": \"" + Company + "\",\r\n                                                \"company_it\": \"" + Company + "\",\r\n                                                \"Manifest_id\": \"" + Manifest_id + "\",\r\n                                                \"order\":\"" + Order + "\",      \r\n                                                \"rec_type\":\"P\",\r\n                                                \"tracking_no\": \"" + tracking_no + "\",\r\n                                                \"manifest_carrier\":\"" + manifest_carrier + "\",\r\n                                                \"service_type\":\"" + service_type + "\",\r\n                                                \"ship_via_code\":\"" + ship_via_code + "\",\r\n                                                \"ship_date\":\"" + ship_date + "\",\r\n                                                \"pack_weight\":\"" + pkg_wgt + "\",\r\n                                                \"pkg_wgt\":\"" + pkg_wgt + "\",\r\n                                                \"pkg_no\":\"" + pkg_no + "\",\r\n                                                \"pack_type\":\"" + pack_type + "\",\r\n                                                \"descr\":\"" + descr + "\",\r\n                                                \"voided\":\"n\"\r\n                                                }],\r\n                                \"table\": \"oe_ship_pack\",\r\n                                \"triggers\": false\r\n                }\r\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetAuthonticationToken", DateTime.Now, "GetAuthonticationToken", ed.ToString(), "GetAuthonticationToken", 2);
                return null;
            }
        }

        public dynamic DataSyncNow(string domain, string MemRefNo, string day, string month, string year)
        {
            try
            {
                var client = new RestClient(domain + "/" + MemRefNo + "/Ecommerce/DataSync?day=" + day + "&month=" + month + "&year=" + year);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Postman-Token", "8498c898-58a0-48b3-907d-bc3617351b19");
                request.AddHeader("cache-control", "no-cache");
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }catch(Exception ed)
            {
                ErrorLogs.ErrorLog(0, "DataSyncNow", DateTime.Now, "DataSyncNow", ed.ToString(), "DataSyncNow", 2);
                return ed.ToString();
            }
        }
        public dynamic getPOList(string ApiEndpoint, string CompanyID, string RecType, string vendor, string Token, string PO, string PartNo, string VendorPart, string UPC, string Date, int PageNo)
        {
            try
            {
                string skip = ((PageNo - 1) * 10).ToString();

                string query = "/distone/rest/service/data/read?query=FOR%20EACH%20po_head%20NO-LOCK%20WHERE%20%20po_head.company_it%20%3D%20%22" + CompanyID + "%22%20and%20%20po_head.rec_type%20%3D%20%22" + RecType + "%22%20and%20%20po_head.vendor%20%3D%20%22" + vendor + "%22and%20opn%20%3D%22yes%22%20and%20po_head.po%20%3E%200%2C%20each%20po_line%20NO-LOCK%20WHERE%20po_line.po%20%3D%20po_head.po%20and%20po_line.rec_seq%20%3D%20po_head.rec_seq%20and%20po_line.rec_type%20%3D%20po_head.rec_type%20and%20po_line.company_it%20%3D%20po_head.company_it%20";

                if (!string.IsNullOrEmpty(PartNo))
                {
                    query += "and%20po_line.item%20%3D%22" + PartNo + "%22%20%20";
                }
                else if (!string.IsNullOrEmpty(PO))
                {
                    query += "and%20po_head.po%3D" + PO + "%20";
                }
                else if (!string.IsNullOrEmpty(Date))
                {
                    string[] dd = Date.Split('-');
                    string d = dd[1] + "-" + dd[2] + "-" + dd[0];
                    query += "and%20po_head.ord_date%3D%22" + d + "%22%20";
                }
                else if (!string.IsNullOrEmpty(VendorPart))
                {
                    query += "and%20po_line.vendor_item%3D%22" + VendorPart + "%22%20";
                }
                else if (!string.IsNullOrEmpty(UPC))
                {
                    query += "and%20po_line.UPC1%3D%22" + UPC + "%22";
                }


                var client = new RestClient(ApiEndpoint + query + "&skip=" + skip + "&take=" + 10);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "2c261dd3-2db4-a48e-aa19-3025fe96175e");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Token);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }catch(Exception ed)
            {
                ErrorLogs.ErrorLog(0, "getPOList", DateTime.Now, "getPOList", ed.ToString(), "getPOList", 2);
                return ed.ToString();
            }
        }

        public dynamic getItemList(string ApiEndpoint, string CompanyID, string vendor, string item, string vendorItem, string warehouse, string Token, int PageNo)
        {
            try
            {
                string skip = ((PageNo - 1) * 10).ToString();
                var client = new RestClient(ApiEndpoint + "/distone/rest/service/data/read?query=FOR%20EACH%20item%20NO-LOCK%20WHERE%20%20item.company_it%20%3D%20%22" + CompanyID + "%22%20and%20%20item.is_deleted%20%3D%20%22no%22%20and%20item.vendor%20%3D%20%22" + vendor + "%22%20and%20item.item%20matches%20%22*" + item + "*%22%20and%20item.vendor_item%20matches%20%20%22*" + vendorItem + "*%22%2C%20%0AEACH%20wa_item%20NO-LOCK%20WHERE%20%0Awa_item.company_it%20%3D%20item.company_it%20AND%20%0Awa_item.item%20%3D%20item.item%20and%20wa_item.warehouse%20matches%20%22*" + warehouse + "*%22&columns=item.item%2Citem.vendor_item%2Citem.stat%2Citem.um_po%2Cwa_item.warehouse%2Cwa_item.um_display%2Cwa_item.max%2Cwa_item.min%2Cwa_item.eoq%2Cwa_item.order_point%2Cwa_item.line_point%2Cwa_item.buy_type&skip=" + skip + "&take=" + 10);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "03857816-75c2-5c7c-05ef-8a04c19809a1");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Token);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }catch(Exception ed)
            {
                ErrorLogs.ErrorLog(0, "getItemList", DateTime.Now, "getItemList", ed.ToString(), "getItemList", 2);
                return ed.ToString();
            }
        }

        public dynamic getItemListCount(string ApiEndpoint, string CompanyID, string vendor, string item, string vendorItem, string warehouse, string Token)
        {
            try
            {
                var client = new RestClient(ApiEndpoint + "/distone/rest/service/data/read?query=FOR%20EACH%20item%20NO-LOCK%20WHERE%20%20item.company_it%20%3D%20%22" + CompanyID + "%22%20and%20%20item.is_deleted%20%3D%20%22no%22%20and%20item.vendor%20%3D%20%22" + vendor + "%22%20and%20item.item%20matches%20%22*" + item + "*%22%20and%20item.vendor_item%20matches%20%20%22*" + vendorItem + "*%22%2C%20%0AEACH%20wa_item%20NO-LOCK%20WHERE%20%0Awa_item.company_it%20%3D%20item.company_it%20AND%20%0Awa_item.item%20%3D%20item.item%20and%20wa_item.warehouse%20matches%20%22*" + warehouse + "*%22&columns=item.item%2Citem.vendor_item%2Citem.stat%2Citem.um_po%2Cwa_item.warehouse%2Cwa_item.um_display%2Cwa_item.max%2Cwa_item.min%2Cwa_item.eoq%2Cwa_item.order_point%2Cwa_item.line_point%2Cwa_item.buy_type");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "03857816-75c2-5c7c-05ef-8a04c19809a1");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Token);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }catch(Exception ed)
            {
                ErrorLogs.ErrorLog(0, "getItemListCount", DateTime.Now, "getItemListCount", ed.ToString(), "getItemListCount", 2);
                return ed.ToString();
            }
        }

        public dynamic getPOListCount(string ApiEndpoint, string CompanyID, string RecType, string vendor, string Token)
        {
            try
            {
                var client = new RestClient(ApiEndpoint + "/distone/rest/service/data/count?query=FOR%20EACH%20po_head%20NO-LOCK%20WHERE%20%20po_head.company_it%20%3D%20%22" + CompanyID + "%22%20and%20%20po_head.rec_type%20%3D%20%22" + RecType + "%22%20and%20%20po_head.vendor%20%3D%20%22" + vendor + "%22and%20opn%20%3D%22yes%22");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "2c261dd3-2db4-a48e-aa19-3025fe96175e");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Token);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }catch(Exception ed)
            {
                ErrorLogs.ErrorLog(0, "getPOListCount", DateTime.Now, "getPOListCount", ed.ToString(), "getPOListCount", 2);
                return ed.ToString();
            }
        }


        public dynamic getProductLinePO(string ApiEndPoint, string Token, string CompanyID, string RecType, string vendor, int PageNo)
        {
            try
            {
                string skip = ((PageNo - 1) * 10).ToString();
                var client = new RestClient(ApiEndPoint + "/distone/rest/service/data/read?query=FOR%20EACH%20po_head%20NO-LOCK%20WHERE%20%20po_head.company_it%20%3D%20%22" + CompanyID + "%22%20and%20%20po_head.rec_type%20%3D%20%22" + RecType + "%22%20and%20%20po_head.vendor%20%3D%20%22" + vendor + "%22%20and%20opn%20%3D%22yes%22%2CEACH%20po_line%20NO-LOCK%20WHERE%20po_line.po%20%3D%20po_head.po%20AND%20po_line.rec_seq%20%3D%20po_head.rec_seq%20AND%20po_line.rec_type%20%3D%20po_head.rec_type%20AND%20po_line.company_it%20%3D%20po_head.company_it&columns=po%2Cpo_line.line%2Cpo_line.line_add%2Cpo_line.item%2Cpo_line.descr%2Cpo_line.q_ord_d%2Cpo_line.um_o%2Cpo_line.price%2Cpo_line.o_ext%2Cpo_line.req_date%2Cpo_line.wanted_date&skip=" + skip + "&take=" + 10);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "c653ad26-2b89-0f66-bd99-2a8bbe5b14f6");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Token);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }catch(Exception ed)
            {
                ErrorLogs.ErrorLog(0, "getProductLinePO", DateTime.Now, "getProductLinePO", ed.ToString(), "getProductLinePO", 2);
                return ed.ToString();
            }
        }

        public dynamic getProductLinePOCount(string ApiEndPoint, string Token, string CompanyID, string RecType, string vendor)
        {
            try
            {
                var client = new RestClient(ApiEndPoint + "/distone/rest/service/data/count?query=FOR%20EACH%20po_head%20NO-LOCK%20WHERE%20%20po_head.company_it%20%3D%20%22" + CompanyID + "%22%20and%20%20po_head.rec_type%20%3D%20%22" + RecType + "%22%20and%20%20po_head.vendor%20%3D%20%22" + vendor + "%22%20and%20opn%20%3D%22yes%22%2CEACH%20po_line%20NO-LOCK%20WHERE%20po_line.po%20%3D%20po_head.po%20AND%20po_line.rec_seq%20%3D%20po_head.rec_seq%20AND%20po_line.rec_type%20%3D%20po_head.rec_type%20AND%20po_line.company_it%20%3D%20po_head.company_it&columns=po%2Cpo_line.line%2Cpo_line.line_add%2Cpo_line.item%2Cpo_line.descr%2Cpo_line.q_ord_d%2Cpo_line.um_o%2Cpo_line.price%2Cpo_line.o_ext%2Cpo_line.req_date%2Cpo_line.wanted_date");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "c653ad26-2b89-0f66-bd99-2a8bbe5b14f6");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Token);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }catch(Exception ed)
            {
                ErrorLogs.ErrorLog(0, "getProductLinePOCount", DateTime.Now, "getProductLinePOCount", ed.ToString(), "getProductLinePOCount", 2);
                return ed.ToString();
            }
        }

        public dynamic getPoDtl(string ApiEndPoint, string Token, string CompanyID, string vendor, string PONO)
        {
            try
            {
                var client = new RestClient(ApiEndPoint + "/distone/rest/service/data/read?query=FOR%20EACH%20po_head%20NO-LOCK%20WHERE%20%20po_head.company_it%20%3D%20%22" + CompanyID + "%22%20and%20po_head.vendor%20%3D%20%22" + vendor + "%22%20and%20po_head.po%20%3D%20%22" + PONO + "%22%20and%20opn%20%3D%22yes%22%2CEACH%20po_line%20NO-LOCK%20WHERE%20po_line.po%20%3D%20po_head.po%20AND%20po_line.rec_seq%20%3D%20po_head.rec_seq%20AND%20po_line.rec_type%20%3D%20po_head.rec_type%20AND%20po_line.company_it%20%3D%20po_head.company_it%20&columns=vendor%2CPO%2Crec_type%2Cord_date%2Cwanted_date%2Cfollow_up_date%2Cfollow_up_code%2Ccurrency_code%2Cexchange_rate%2Cdelivery_terms%2Center_by%2Cfax%2Cship_ID%2Cship_via_code%2Cname%2Cship_atn%2Cadr%2Cstate%2Cpostal_code%2Cphone%2Cphone_ext%2Cemail%2Ccountry_code%2Cpo_line.line%2Cpo_line.line_add%2Cpo_line.item%2Cpo_line.descr%2Cpo_line.q_ord_d%2Cpo_line.um_o%2Cpo_line.price%2Cpo_line.o_ext%2Cpo_line.req_date%2Cpo_line.wanted_date");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "94c2997f-68e6-3b34-cfad-fe6f58fb0cc4");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Token);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }catch(Exception ed)
            {
                ErrorLogs.ErrorLog(0, "getPoDtl", DateTime.Now, "getPoDtl", ed.ToString(), "getPoDtl", 2);
                return ed.ToString();
            }
        }

        public dynamic SubmitPO(string ApiEndPoint, string Token, string CompanyID, List<POViewModel> list)
        {
            try
            {
                bool retVal = false;
                for (int i = 0; i < list.Count(); i++)
                {
                    string[] date = list[i].wanted_date.Split('-');
                    string dd = date[1] + "-" + date[2] + "-" + date[0];

                    string query = "";

                    bool flag = false;

                    if (!string.IsNullOrEmpty(list[i].wanted_date))
                    {
                        query += "\"wanted_date\":\"" + dd + "\",";
                        flag = true;
                    }
                    if (!string.IsNullOrEmpty(list[i].price))
                    {
                        query += "\"price\":\"" + list[i].price + "\",";
                        flag = true;
                    }

                    if (flag)
                    {
                        var client = new RestClient(ApiEndPoint + "/distone/rest/service/data/update");
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("postman-token", "5ff2d0f5-8e08-2193-f524-3465b9f316fc");
                        request.AddHeader("cache-control", "no-cache");
                        request.AddHeader("content-type", "application/json");
                        request.AddHeader("authorization", Token);
                        request.AddParameter("application/json", "{\r\n\t\"table\":\"po_line\",\r\n\t\"triggers\": true,\r\n\t\"changes\":[{\r\n\t\t" + query + "\"__where\":\"po_line.company_it ='" + CompanyID + "' and po_line.rec_type='" + list[i].RecType + "' and po_line.po ='" + list[i].PO + "' and  po_line.line_add ='" + list[i].line_add + "'\"\r\n\t\t}]\r\n}", ParameterType.RequestBody);
                        //request.AddParameter("application/json", "{\r\n\t\"table\":\"po_line\",\r\n\t\"triggers\": true,\r\n\t\"changes\":[{\r\n\t\t\"wanted_date\":\"" + dd + "\",\"price\":\"" + price + "\",\"__where\":\"po_line.company_it ='" + CompanyID + "' and po_line.rec_type='" + RecType + "' and po_line.po ='" + PO + "' and  po_line.line_add ='" + line_add + "'\"\r\n\t\t}]\r\n}", ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        //return JsonConvert.DeserializeObject(response.Content);
                        retVal = true;
                    }
                    else
                        return null;
                }
                return retVal;
            }catch(Exception ed)
            {
                ErrorLogs.ErrorLog(0, "SubmitPO", DateTime.Now, "SubmitPO", ed.ToString(), "SubmitPO", 2);
                return ed.ToString();
            }
        }

        public dynamic SearchByPart(string ApiEndPoint, string CompanyID, string RecType, string Vendor, string Token, string sword)
        {
            try
            {
                var client = new RestClient(ApiEndPoint + "/distone/rest/service/data/read?query=FOR%20EACH%20po_head%20NO-LOCK%20WHERE%20%20po_head.company_it%20=%20%22" + CompanyID + "%22%20and%20%20po_head.rec_type%20=%20%22" + RecType + "%22%20and%20%20po_head.vendor%20=%20%22" + Vendor + "%22%20and%20opn%20=%22yes%22,EACH%20po_line%20NO-LOCK%20WHERE%20po_line.po%20=%20po_head.po%20AND%20po_line.rec_seq%20=%20po_head.rec_seq%20AND%20po_line.rec_type%20=%20po_head.rec_type%20AND%20po_line.company_it%20=%20po_head.company_it%20and%20%20po_line.item%20matches%20%22%2A" + sword + "%2A%22%0A&columns=po,po_line.line,po_line.line_add,po_line.item,po_line.descr,po_line.q_ord_d,po_line.um_o,po_line.price,po_line.o_ext,po_line.req_date,wanted_date,vendor,ord_date,follow_up_date,follow_up_code,po_line.item,po_line.descr");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "57b2bfe8-8411-4794-9686-4d1b63fefba0");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Token);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }catch(Exception ed)
            {
                ErrorLogs.ErrorLog(0, "SearchByPart", DateTime.Now, "SearchByPart", ed.ToString(), "SearchByPart", 2);
                return ed.ToString();
            }
        }


        public List<VendorLoginDetailsViewModel> GetVendorLoginDetails(string company, string Token, string ApiEndPoint)
        {
            List<VendorLoginDetailsViewModel> VendorLoginDetailsList = new List<VendorLoginDetailsViewModel>();
            try
            {

                var client = new RestClient(ApiEndPoint + "/distone/rest/service/data/read?query=FOR%20EACH%20vendor%20NO-LOCK%20WHERE%20%20vendor.company_ve%20%3D%20%22" + company + "%22%20and%20%20vendor.stat%20%3D%20%22yes%22%20and%20%20vendor.web%20%3D%20%22yes%22%20and%20vendor.web_passwd%20%3C%3E%20%22%22&columns=vendor%2Cweb_passwd%2Ccompany_ve");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "cafb6bd2-5272-e59c-ea68-52401e6afff4");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Token);
                IRestResponse response = client.Execute(request);
                VendorLoginDetailsList = JsonConvert.DeserializeObject<VendorLoginDetailsViewModel[]>(response.Content).ToList();

            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetVendorLoginDetails", DateTime.Now, "GetVendorLoginDetails", ed.ToString(), "GetVendorLoginDetails", 2);
            }
            return VendorLoginDetailsList;
        }

        public dynamic GetTrackingNumber(string Api, string AuthorizationToken, string Company, string ManifiestId)
        {
            try
            {

                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_ship_pack%20NO-LOCK%20WHERE%20%20oe_ship_pack.company_oe%20%3D%20%22" + Company + "%22%20and%20%20oe_ship_pack.Manifest_id%20%3D%20%22" + ManifiestId + "%22");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "a88ccd6f-a3b6-fac0-7b61-e248dfda7e1b");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", AuthorizationToken);
                IRestResponse response = client.Execute(request);

                var result = JsonConvert.DeserializeObject(response.Content);

                return result;
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetTrackingNumber", DateTime.Now, "GetTrackingNumber", ed.ToString(), "GetTrackingNumber", 2);
                return ed.ToString();
            }
        }

        public dynamic Itemavailability(string authorizationtoken, string Api, string item, string warehouse, string format)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/item/availability");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "e6dd3923-34dd-469f-a0e4-c3882f049521");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", authorizationtoken);
                request.AddParameter("undefined", "items=" + item + "&warehouses=" + warehouse + "&format=" + format, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "Itemavailability", DateTime.Now, "Itemavailability", ed.ToString(), "Itemavailability", 2);
                return ed.ToString();
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {

                    this.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}