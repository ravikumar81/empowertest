using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empower.Repository.Factory {
    public interface IConnectionFactory {
        List<T> GetData<T>(string sql);
        void AddMapper<T>(Dictionary<string,string> columnNames);
        List<T> ExecuteProcedure<T>(string procedureName, object parameter = null);
    }
}
