using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Robin.Modules;
using SmartKylinData;

namespace SmartKylinApp.Module
{
    [DependsOn(typeof(DataMoudle))]
    public class BootstrapMoudle : RobinModule
    {


        public override void Initialize()
        {

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
