using Demo.Webapi.Common.Enums;

namespace Demo.Webapi.Common.Entites.DTO
{
    public class ErrorResult
    {
        #region Field
        /// <summary>
        /// Mã lỗi
        /// </summary>
        public ErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Thông báo dev
        /// </summary>
        public string DevMsg { get; set; }

        /// <summary>
        /// Thông báo người dùng
        /// </summary>
        public string UserMsg { get; set; }

        /// <summary>
        /// Traceidentifer
        /// </summary>
        public string TraceId { get; set; }

        /// <summary>
        /// Thông tin bổ sung
        /// </summary>
        public string MoreInfo { get; set; }

        #endregion

        #region Constructor

        public ErrorResult() { }

        public ErrorResult(ErrorCode errorCode, string devMsg, string userMsg, string moreInfo)
        {
            ErrorCode = errorCode;
            this.DevMsg = devMsg;
            this.UserMsg = userMsg;
            this.MoreInfo = moreInfo;
        }

        #endregion
    }
}
