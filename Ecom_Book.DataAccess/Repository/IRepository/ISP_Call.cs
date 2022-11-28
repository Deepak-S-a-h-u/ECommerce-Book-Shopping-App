using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Book.DataAccess.Repository.IRepository
{
    public interface ISP_Call : IDisposable
    {
        T Single<T>(string procedureName, DynamicParameters parms = null);
        T OneRecord<T>(string procedureName, DynamicParameters parms = null);
        void Execute(string procedureName, DynamicParameters parms = null);
        IEnumerable<T> List<T>(string procedureName, DynamicParameters parms = null);
        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null);
    }
}
