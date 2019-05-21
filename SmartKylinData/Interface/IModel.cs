using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Robin.Domain.Repositories;
using SmartKylinData.IOTModel;
using SmartKylinData.Repository;

namespace SmartKylinData.Interface
{
    public interface IBaseMonitorRepository : IRepository<BasicMonitorRecord>
    {
        IEnumerable<BasicMonitorRecord> GetList(int pageIndex, int pageSize);
        IEnumerable<BasicMonitorRecord> GetList(Expression<Func<BasicMonitorRecord, bool>> predicate,int pageIndex, int pageSize);
        IEnumerable<BasicMonitorRecord> GetList(Expression<Func<BasicMonitorRecord, bool>> predicate);
        //IQueryable<BasicMonitorRecord> GetList(Expression<Func<BasicMonitorRecord, bool>> predicate, Action<Orderable<BasicMonitorRecord>> order, int pageIndex, int pageSize);
    }

    public interface IConfig : IRepository<ConfigRecord>
    {
    }

    public interface IMsType : IRepository<MsTypeRecord>
    {
    }
}
