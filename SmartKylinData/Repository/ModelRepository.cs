using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion.Lambda;
using NHibernate.Linq;
using Robin.Collections.Extensions;
using Robin.NHibernate;
using Robin.NHibernate.Repositories;
using SmartKylinData.Interface;
using SmartKylinData.IOTModel;

namespace SmartKylinData.Repository
{
    public class BaseMonitorRepository : NhRepositoryBase<BasicMonitorRecord>, IBaseMonitorRepository
    {
        public BaseMonitorRepository(ISessionProvider sessionProvider) : base(sessionProvider)
        {
        }

        public IEnumerable<BasicMonitorRecord> GetList(int pageIndex, int pageSize)
        {
            return Session.QueryOver<BasicMonitorRecord>().Skip((pageIndex - 1) * pageSize).Take(pageSize).List();
        }

        public IEnumerable<BasicMonitorRecord> GetList(Expression<Func<BasicMonitorRecord, bool>> predicate, int pageIndex, int pageSize)
        {
            var a = Session.Query<BasicMonitorRecord>().Cacheable().Where(predicate);
            var b=a.Skip(pageIndex).Take(pageSize);
            return b;
        }

        public IEnumerable<BasicMonitorRecord> GetList(Expression<Func<BasicMonitorRecord, bool>> predicate)
        {
            var data = Session.Query<BasicMonitorRecord>().Cacheable();
            var aa = data.Where(predicate);
            return null;
        }

        public IQueryable<BasicMonitorRecord> GetList(Expression<Func<BasicMonitorRecord, bool>> predicate, Action<Orderable<BasicMonitorRecord>> order, int pageIndex, int pageSize)
        {
            var orderable = new Orderable<BasicMonitorRecord>(Session.QueryOver<BasicMonitorRecord>().Where(predicate) as IQueryable<BasicMonitorRecord>);
            order(orderable);
            return orderable.Queryable.Skip(pageIndex).Take(pageSize);
        }
    }

    public class ConfigRepository : NhRepositoryBase<ConfigRecord>, IConfig
    {
        public ConfigRepository(ISessionProvider sessionProvider) : base(sessionProvider)
        {
        }
    }

    public class MsTypeRepository : NhRepositoryBase<MsTypeRecord>, IMsType
    {
        public MsTypeRepository(ISessionProvider sessionProvider) : base(sessionProvider)
        {
        }
    }
}
