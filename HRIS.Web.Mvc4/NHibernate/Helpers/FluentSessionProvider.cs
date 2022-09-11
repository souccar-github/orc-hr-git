#region

using System;
using System.Configuration;
using System.Data;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernateDBGenerator.NHibernate.Convention;
using NHibernateDBGenerator.NHibernate.Enumerations;
using Configuration = NHibernate.Cfg.Configuration;
using System.Data.SqlClient;
using System.IO;
using HRIS.Mapping.Security.RootEntities;

#endregion

namespace NHibernateDBGenerator.NHibernate.Helpers
{

    public class FluentSessionProvider
    {

        private static SqlConnection _sqlConnection;

        private static SqlCommand _sqlCommand;

        private static DBMSProvider GetCurrentDBMSProvider()
        {
            return DBMSProvider.SQLServer2008;
        }

        private static ISessionFactory _sessionFactory;

        private static ISessionFactory CreateSessionFactory()
        {
            if (_sessionFactory==null)
            _sessionFactory =  GetFluentConfiguration().BuildSessionFactory();
            
            return _sessionFactory;
        }

        private static FluentConfiguration GetFluentConfiguration()
        {
            
            switch (GetCurrentDBMSProvider())
            {
                case DBMSProvider.SQLServer2008:
                    return Fluently.Configure().Database(
                        MsSqlConfiguration.MsSql2008.ConnectionString(ConfigurationManager.
                        ConnectionStrings["DefaultConnection"].ConnectionString)
                        ).Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>().Conventions.Setup(x=>x.Add(AutoImport.Never())).Conventions.Add(new EnumConvention()));
                case DBMSProvider.Oracle:
                    return Fluently.Configure().Database(
                        OracleDataClientConfiguration.Oracle10.ConnectionString(
                             ConfigurationSettings.AppSettings["OracleConnectionString"].ToString())).
                            Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>().Conventions.Add(new CustomManyToManyTableNameConvention()));
                default:
                    throw new NotSupportedException("This DBMS is not supported.");
            }

        }

        internal static Configuration GetConfiguration()
        {
            return GetFluentConfiguration().BuildConfiguration();
        }

        public static ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory == null)
                _sessionFactory = CreateSessionFactory();
            return _sessionFactory ;
        }

        public static ISession GetSession()
        {
            return GetSessionFactory().OpenSession();
        }

        public static bool GenerateSchema(GenerateSchemaMode generateSchemaMode)
        {
            var fileName = string.Format("{0}-{1}.{2}", "updateDB", Guid.NewGuid(),"sql");
            Action<string> updateExport = x =>
                   {
                       using (var file = new FileStream(Path.Combine(Path.GetTempPath(),
                           fileName), 
                           FileMode.Append, FileAccess.Write))
                       using (var sw = new StreamWriter(file))
                       {    
                           sw.Write(x);
                           sw.Close();
                       }
                   };

                switch (generateSchemaMode)
                {
                    case GenerateSchemaMode.Create:
                        new SchemaExport(GetConfiguration()).Create(updateExport, true);
                        break;
                    case GenerateSchemaMode.Drop:
                        new SchemaExport(GetConfiguration()).Drop(true, true);
                        break;
                    case GenerateSchemaMode.Validate:
                        new SchemaValidator(GetConfiguration()).Validate();
                        break;
                    case GenerateSchemaMode.Update:
                        new SchemaUpdate(GetConfiguration()).Execute(updateExport, true);
                        break;
                    default:
                        throw new NotSupportedException("This mode is not supported");
                }
            

            return true;
        }

        public static void DropDataBase()
        {
            new SchemaExport(GetConfiguration()).Drop(true, true);
        }

        public static void PrepareSessionFactory()
        {
            _sessionFactory = CreateSessionFactory();
        }

        public static void DisableAllDatabaseConstraints()
        {
            //Disable all the constraint in database
            ExecuteCommand("EXEC sp_msforeachtable \"ALTER TABLE ? NOCHECK CONSTRAINT all\"");
        }

        public static void EnableAllDatabaseConstraints()
        {
            //Enable all the constraint in database
            ExecuteCommand("EXEC sp_msforeachtable \"ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all\"");
        }

        private static void ExecuteCommand(string command)
        {
            try
            {
                _sqlConnection = 
                    new SqlConnection(ConfigurationSettings.AppSettings["HrisConnectionString"].ToString());

                if (_sqlConnection.State == ConnectionState.Closed)
                    _sqlConnection.Open();

                _sqlCommand = new SqlCommand(command, _sqlConnection);
                _sqlCommand.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                
            }
            finally
            {
                if (_sqlConnection.State == ConnectionState.Open)
                    _sqlConnection.Close();
            }
        }
    }
}