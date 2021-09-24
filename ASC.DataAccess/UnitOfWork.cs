using ASC.DataAccess.Interfaces;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASC.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed;
        private bool complete;
        private Dictionary<string, object> _repositories;
        public Queue<Task<Action>> RollbackActions { get; set; }
        public string ConnectionString { get; set; }
        public UnitOfWork(string connectionstring)
        {
            connectionstring = ConnectionString;
            RollbackActions = new Queue<Task<Action>>();
        }
        public void CommitTransaction()
        {
            complete = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);

        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (!complete) RollbackTransaction();
                }
                finally
                {
                    RollbackActions.Clear();
                }
            }
            complete = false;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void RollbackTransaction()
        {
            while (RollbackActions.Count > 0)
            {
                var undoAction = RollbackActions.Dequeue();
                undoAction.Result();
            }
        }

        public IReponsitory<T> Reponsitory<T>() where T : TableEntity
        {
            if (_repositories == null)
                _repositories = new Dictionary<string, object>();
            var type = typeof(T).Name;
            if (_repositories.ContainsKey(type)) return (IReponsitory<T>)_repositories[type];
            var repositoryType = typeof(Reponsitory<>);
            var repositoryInstance =
            Activator.CreateInstance(repositoryType
            .MakeGenericType(typeof(T)), this);
            _repositories.Add(type, repositoryInstance);
            return (IReponsitory<T>)_repositories[type];

            //if (_repositories == null)
            //{
            //    _repositories = new Dictionary<string, object>();
            //    var type = typeof(T).Name;
            //    if (_repositories.ContainsKey(type)) return ((IRepository<T>)_repositories);
            //    var reponsitoryType = typeof(Repository<>);
            //    var reponsitoryInstance = Activator.CreateInstance(reponsitoryType.MakeGenericType(typeof(T)), this);
            //    _repositories.Add(type, reponsitoryInstance);
            //    return (IRepository<T>)_repositories[type];
            //}
        }
    }
}
