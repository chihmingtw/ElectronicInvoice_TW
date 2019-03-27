﻿using ElectronicInvoice.Produce.Factroy;
using ElectronicInvoice.Produce.Infrastructure.Helper;
using ElectronicInvoice.Produce.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ElectronicInvoice.Produce;
using ElectronicInvoice.Produce.Attributes;
using ElectronicInvoice.Produce.Base;
using ElectronicInvoice.Produce.Helper;
using ElectronicInvoice.Produce.Infrastructure;
using ElectronicInvoice.Produce.InvoiceResult;
using Newtonsoft.Json;


namespace EInvoiceDemo
{
    public class MyApi : ApiBase<MyQryWinningListModel>
    {
        protected override string SetParameter(MyQryWinningListModel model)
        {
            SortedDictionary<string, string> parameter = new SortedDictionary<string, string>
            {
                ["version"] = "0.2",
                ["action"] = "QryWinningList",
                ["invTerm"] = model.invTerm,
                ["UUID"] = model.UUID,
                ["appID"] = ConfigSetting.GovAppId
            };
            return PraramterHelper.DictionaryToParamter(parameter);
        }
    }


    [ApiType(ApiType = typeof(MyApi))]
    public class MyQryWinningListModel : CommonBaseModel
    {
        public string invTerm { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Register Assembly you want to inject.
            ApiTypeProvider.Instance.RegisterAssembly(Assembly.GetExecutingAssembly());
    
            string result = string.Empty;
            #region 使用工廠模式
            //建立查詢參數  
            //下面範例查詢 發票民國106年7.8月中獎發票
            QryWinningListModel model = new QryWinningListModel()
            {
                invTerm = "10610"
            };

            //建立工廠 將配置檔傳入建構子中
            InvoiceApiFactory factory = new InvoiceApiFactory();

            //在工廠中藉由傳入參數 取得Api產品
            var api = factory.GetProxyInstace(model);

            //api回傳結果
            result = api.ExecuteApi(model);
            Console.WriteLine(result);
            #endregion

            #region 使用 InvoiceApiContext 

            DonateQueryModel donateModel = new DonateQueryModel()
            {
                qKey = "伊甸"
            };
            InvoiceApiContext apiContext = new InvoiceApiContext();
            result = apiContext.ExecuteApi(donateModel); 
            #endregion

            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
