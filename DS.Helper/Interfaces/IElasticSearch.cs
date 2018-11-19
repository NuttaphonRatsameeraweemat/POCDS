using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Helper.Interfaces
{
    public interface IElasticSearch<T> where T : class
    {
        ElasticClient GetClient();
        IEnumerable<T> SearchFilter(Func<SearchDescriptor<T>, ISearchRequest> searchFunc);
        string Insert(T model, string index, string type);
        string Update(T model, string index, string type);
        string Delete(int id, string index, string type);
        string DeleteAll(string index, string type);
        string Bulk(List<T> modelList, string index, string type);
    }
}
