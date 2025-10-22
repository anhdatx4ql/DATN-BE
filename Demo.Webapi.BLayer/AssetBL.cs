using Demo.Webapi.BLayer.BaseBL;
using Demo.Webapi.Common;
using Demo.Webapi.Common.Entites;
using Demo.Webapi.Common.Entites.DTO;
using Demo.Webapi.Common.Entities;
using Demo.Webapi.Common.Entities.DTO;
using Demo.Webapi.Common.Enums;
using Demo.Webapi.DL;
using Demo.Webapi.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.BLayer
{
    public class AssetBL : BaseBL<Asset>, IAssetBL
    {
        IAssetDL _assetDL;

        public AssetBL(IBaseDL<Asset> baseDL, IAssetDL assetDL) : base(baseDL)
        {
            _assetDL = assetDL;
        }

        protected override void AfterUpdateRecord(Asset record)
        {
            // Nếu base.AfterUpdateRecord chứa logic bạn vẫn muốn chạy, gọi nó:
            base.AfterUpdateRecord(record);

            // Thực hiện logic chuyên biệt cho Asset (ví dụ: cập nhật trạng thái liên quan)
            _assetDL.UpdateRecord(record);
        }

    }
}
