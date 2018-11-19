using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DS.Helper.Interfaces;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;

namespace DS.Helper
{
    public class ElasticSearch<T> : IElasticSearch<T> where T : class
    {

        #region [Fields]

        /// <summary>
        /// The Configuration Value.
        /// </summary>
        private readonly IConfiguration _configuration;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticSearch" /> class.
        /// </summary>
        /// <param name="configuration">The Configuration Value</param>
        public ElasticSearch(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get elastic search client.
        /// </summary>
        /// <returns></returns>
        public ElasticClient GetClient()
        {
            var node = new Uri(_configuration["ElasticsearchURL"]);
            var settings = new ConnectionSettings(node);
            var client = new ElasticClient(settings);
            return client;
        }

        /// <summary>
        /// Get data from elastic search.
        /// </summary>
        /// <param name="searchFunc">The search descriptor and query container for filter.</param>
        /// <returns></returns>
        public IEnumerable<T> SearchFilter(Func<SearchDescriptor<T>, ISearchRequest> searchFunc)
        {
            var result = new List<T>();

            var client = this.GetClient();
            var searchResponse = client.Search<T>(searchFunc);

            if (searchResponse.IsValid)
            {
                result = searchResponse.Documents.ToList();
            }

            return result;
        }

        /// <summary>
        /// Insert to elastic search.
        /// </summary>
        /// <param name="model">The infomation data.</param>
        /// <param name="index">The index elastic search.</param>
        /// <param name="type">The type elastic search.</param>
        /// <returns></returns>
        public string Insert(T model, string index, string type)
        {
            string result = string.Empty;

            var client = this.GetClient();
            var id = Convert.ToInt32(model.GetType().GetProperty("Id").GetValue(model, null));
            var response = client.Index<T>(model, i => i.Index(index).Type(type).Id(id));
            if (!response.IsValid)
            {
                result = response.ServerError.ToString();
            }

            return result;
        }

        /// <summary>
        /// Update to elastic search.
        /// </summary>
        /// <param name="model">The infomation data.</param>
        /// <param name="index">The index elastic search.</param>
        /// <param name="type">The type elastic search.</param>
        /// <returns></returns>
        public string Update(T model, string index, string type)
        {
            string result = string.Empty;

            var client = this.GetClient();
            var id = Convert.ToInt32(model.GetType().GetProperty("Id").GetValue(model, null));
            var response = client.Update<T>(id, i => i.Index(index).Type(type).Doc(model).RetryOnConflict(10));
            if (!response.IsValid)
            {
                result = response.ServerError.ToString();
            }

            return result;
        }

        /// <summary>
        /// Delete item in elastic search.
        /// </summary>
        /// <param name="id">The identity key.</param>
        /// <param name="index">The index elastic search.</param>
        /// <param name="type">The type elastic search.</param>
        /// <returns></returns>
        public string Delete(int id, string index, string type)
        {
            string result = string.Empty;

            var client = this.GetClient();
            var deleteResponse = client.Delete<T>(id, d => d.Index(index).Type(type));

            if (!deleteResponse.IsValid)
            {
                result = deleteResponse.ServerError.ToString();
            }

            return result;
        }

        /// <summary>
        /// Delete index in elastic search.
        /// </summary>
        /// <param name="index">The identity index.</param>
        /// <param name="type">The type elastic search.</param>
        /// <returns></returns>
        public string DeleteAll(string index, string type)
        {
            string result = string.Empty;

            var client = this.GetClient();
            var deleteResponse = client.DeleteByQuery<T>(d => d.Index(index).Type(type).Query(q => q.MatchAll()));
            var deleteIndexResponse = client.DeleteIndex(index);

            if (!deleteIndexResponse.IsValid)
            {
                result = deleteIndexResponse.ServerError.ToString();
            }

            if (!deleteIndexResponse.IsValid)
            {
                result = string.IsNullOrEmpty(result) ? deleteIndexResponse.ServerError.ToString() : result + ", " + deleteIndexResponse.ServerError.ToString();
            }

            return result;
        }

        /// <summary>
        /// Bulk to elastic search.
        /// </summary>
        /// <param name="modelList">The Generic list data.</param>
        /// <param name="index">The identity index.</param>
        /// <param name="type">The type elastic search.</param>
        /// <returns></returns>
        public string Bulk(List<T> modelList, string index, string type)
        {
            var result = string.Empty;
            var client = this.GetClient();

            int number = modelList.Count;
            int exportNumber = 50000;
            int bulkNumber = (number / exportNumber) + 1;
            
            for (int i = 0; i < bulkNumber; i++)
            {
                var bulkList = new List<IBulkOperation>();
                var request = new BulkRequest()
                {
                    Operations = bulkList
                };

                var exportList = modelList.Take(exportNumber).ToList();
                if (exportList.Count <= 0)
                {
                    break;
                }

                foreach (var item in exportList)
                {
                    int id = Convert.ToInt32(item.GetType().GetProperty("Id").GetValue(item, null));
                    bulkList.Add(new BulkCreateOperation<T>(item) { Id = id });
                }

                var response = client.Bulk(b => b.CreateMany(exportList, (bd, item) => bd.Index(index).Type(type).Document(item).Id(Convert.ToInt32(item.GetType().GetProperty("Id").GetValue(item, null)))));
                if (response.Errors)
                {
                    result = result + response.ServerError.ToString() + " ,";
                }

                //Skip
                modelList = modelList.Skip(exportNumber).ToList();
            }
            return result;
        }

        #endregion

    }
}
