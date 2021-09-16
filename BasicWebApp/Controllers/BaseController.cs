using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace BasicWebApp.Controllers
{
    public class BaseController : Controller
    {
        protected SQLServerAccess _SQLServerdbconn = null;
        public SQLServerAccess SQLServerDBConn
        {
            get
            {
                return _SQLServerdbconn;
            }
        }

        public BaseController()
        {
            ///DB connection
            _SQLServerdbconn = new SQLServerAccess();
            _SQLServerdbconn.DBServer = Startup.DefaultDBServer;
            _SQLServerdbconn.DBPort = Startup.DefaultDBPort;
            _SQLServerdbconn.DBName = Startup.DefaultDBName;
            _SQLServerdbconn.DBID = Startup.DefaultDBUser;
            _SQLServerdbconn.DBPwd = Startup.DefaultDBPassword;
            _SQLServerdbconn.Connect();

        }
    }
}