using Demo.Webapi.Common.Entites;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using Demo.Webapi.Common;
using Demo.Webapi.Common.Entites.DTO;
using System.Net.Http;
using Demo.Webapi.Common.Enums;
using Microsoft.AspNetCore.Http;
using Demo.Webapi.Common.Entities;
using Demo.Webapi.DL.BaseDL;

namespace Demo.Webapi.DL
{
    public class AssetDL : BaseDL<Asset>, IAssetDL
    {
        /// <summary>
        /// Xử lý cập nhật trạng thái cho tài sản khi thu hồi hoặc cấp phát lại
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public int UpdateRecord(Asset record)
        {
            return 1;
        }
    }
}
