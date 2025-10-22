using Demo.Webapi.Common.Entites;
using Demo.Webapi.Common.Entities;
using Demo.Webapi.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.DL
{
    public interface IAssetDL : IBaseDL<Asset>
    {
        public int UpdateRecord(Asset record);
    }
}
