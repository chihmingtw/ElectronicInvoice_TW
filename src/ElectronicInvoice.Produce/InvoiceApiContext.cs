﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AwesomeProxy;
using ElectronicInvoice.Produce.Attributes;
using ElectronicInvoice.Produce.Base;
using ElectronicInvoice.Produce.Extention;
using ElectronicInvoice.Produce.Factroy;

namespace ElectronicInvoice.Produce
{
    public class InvoiceApiContext
    {
        private static Dictionary<Type, object> _apiMapperCache;

        public InvoiceApiContext()
        {
            _apiMapperCache = ApiTypeProvier.Instance
                         .GetTypeFromAssembly<ApiTypeAttribute>()
                         .ToDictionary(x => x,
                              x => x.GetAttributeValue((ApiTypeAttribute y) => 
                                 Activator.CreateInstance(y.ApiType)));

            
        }

        public string ExcuteApi<TModel>(TModel model) 
            where TModel : class, new()
        {
            return ExcuteApiProccess(model, x => x.ExcuteApi(model));
        }

        public TRtn ExcuteApi<TModel,TRtn>(TModel model) 
            where TModel : class, new()
        {
            return ExcuteApiProccess(model, x=>x.ExcuteApi<TRtn>(model));
        }

        private TRtn ExcuteApiProccess<TModel,TRtn>(TModel model,Func<ApiBase<TModel>, TRtn> fun1)
            where TModel : class, new()
        {
            object apiObject;

            if (_apiMapperCache.TryGetValue(typeof(TModel), out apiObject) &&
                apiObject is ApiBase<TModel>)
            {
                var apiInstance = (ApiBase<TModel>)apiObject;
                return fun1(ProxyFactory.GetProxyInstance(apiInstance));
            }

            throw new Exception("Can't Get Type from ApiMapperTable.");
        }
    }
}