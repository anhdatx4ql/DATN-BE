using Demo.Webapi.Common;
using Demo.Webapi.Common.Entites.DTO;
using Demo.Webapi.Common.Entities;
using Demo.Webapi.Common.Entities.DTO;
using Demo.Webapi.Common.Enums;
using Demo.Webapi.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.BLayer.BaseBL
{
    public class BaseBL<T> : IBaseBL<T>
    {

        #region Field

        private IBaseDL<T> _baseDL;

        #endregion

        #region Constructor

        public BaseBL(IBaseDL<T> baseDL)
        {
            this._baseDL = baseDL;
        }

        #endregion

        /// <summary>
        /// Hàm tạo mới một bản ghi
        /// </summary>
        /// <param name="record">Thông tin bản ghi</param>
        /// <author>Xuân Đào 28/03/2023 </author>
        /// <returns> Service Result </returns>
        public ServiceResult CreateRecord(T record)
        {
            var errors = ValidateRecord(record);
            var customErrors = ValidateCustomize(record);
            if (errors.Count > 0)
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    Message = Resource.invalidField,
                    Data = errors,
                };
            } else if (customErrors.Count > 0)
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    Message = Resource.invalidField,
                    Data = customErrors,
                };
            }
            return new ServiceResult()
            {
                IsSuccess = true,
                Message = Resource.createSuccess,
                Data = _baseDL.CreateRecord(record),
            };
        }

        /// <summary>
        /// Hàm xóa một bản ghi
        /// </summary>
        /// <param id="id">Thông tin id bản ghi</param>
        /// <author>Xuân Đào 28/03/2023 </author>
        /// <returns> Số bản ghi bị ảnh hưởng </returns>
        public int DeleteRecord(Guid id)
        {
            return _baseDL.DeleteRecord(id);
        }

        public int MultipleDeleteRecord(string recordIds)
        {
            return _baseDL.MultipleDeleteRecord(recordIds);
        }

        /// <summary>
        /// Lọc bản ghi theo điều kiện
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="departmentId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public ServiceResult FilterRecord(string? keyWord, int? pageSize, int? pageNumber)
        {
            return _baseDL.FilterRecord(keyWord, pageSize, pageNumber);
        }

        /// <summary>
        /// Tìm kiếm bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResult GetRecordById(Guid id)
        {
            return _baseDL.GetRecordById(id);
        }

        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRecords()
        {
            return _baseDL.GetRecords();
        }

        /// <summary>
        /// Validate dữ liệu từ body
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public List<ErrorResult> ValidateRecord(T record)
        {
            List<ErrorResult> errors = new List<ErrorResult>();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var propName = property.Name;
                var propValue = property.GetValue(record);
                var requiredAttritebutes = (RequiredAttribute?)property.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();
                if (requiredAttritebutes != null && String.IsNullOrEmpty(propValue.ToString()))
                {
                    errors.Add(new ErrorResult(ErrorCode.Required, propName + Resource.devMsg_RequiredField, propName + Resource.userMsg_RequiredField, ""));
                }
                var stringLengthAttritebutes = (StringLengthAttribute?)property.GetCustomAttributes(typeof(StringLengthAttribute), false).FirstOrDefault();
                if (stringLengthAttritebutes != null)
                {
                    if(propValue != null)
                    {
                        if (stringLengthAttritebutes.MaximumLength < propValue.ToString().Length)
                        {
                            errors.Add(new ErrorResult(
                                    ErrorCode.TooLong,
                                    $"{propName} can't {Resource.devMsg_InvalidLength} {stringLengthAttritebutes.MaximumLength} chacractors",
                                    $"{propName} không thể {Resource.userMsg_InvalidLength} {stringLengthAttritebutes.MaximumLength} ký tự!", ""));
                        }
                    }
                }
            }
            return errors;
        }

        public virtual List<ErrorResult> ValidateCustomize(T record)
        {
            if (typeof(T).Name == "Account" || typeof(T).Name == "receipt_payment")
            {
                List<ErrorResult> errList = new List<ErrorResult>();
                Type type = typeof(T);
                string code = "";
                if (typeof(T).Name == "receipt_payment")
                {
                    code = type.GetProperty("re_ref_no").GetValue(record).ToString();
                } else if (typeof(T).Name == "Account")
                {
                    code = type.GetProperty($"{type.Name}Number").GetValue(record).ToString();
                }
                if (!CodeIsValid(code))
                {
                    if (typeof(T).Name == "Account")
                        errList.Add(new ErrorResult(ErrorCode.Duplicate, Resource.devMsg_DuplicateAccount, Resource.userMsg_DuplicateAccount, ""));
                    else if (typeof(T).Name == "receipt_payment")
                        errList.Add(new ErrorResult(ErrorCode.Duplicate, Resource.devMsg_DuplicateAccount, Resource.userMsg_DuplicateReceiptPayment, ""));
                }
                return errList;
            }
            return new List<ErrorResult> { };
        }

        public Boolean CodeIsValid(string code)
        {
            string queryString = $"Select * From {typeof(T).Name} Where {typeof(T).Name}number = {code}";
            if (typeof(T).Name == "receipt_payment")
            {
                queryString = $"Select * From {typeof(T).Name} Where re_ref_no = '{code}'";
            }
            var mysqlConnection = _baseDL.GetOpenConnection();
            var result = _baseDL.Query(mysqlConnection, queryString, commandType: CommandType.Text);
            if (result.Count() == 0) return true;
            else return false;
        }

        /// <summary>
        /// Cập nhật thông tin bản ghi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        public ServiceResult UpdateRecord(Guid id, T record)
        {
            var errors = ValidateRecord(record);
            if (errors.Count > 0)
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    Message = Resource.invalidField,
                    Data = errors,
                };
            }
            return _baseDL.UpdateRecord(id, record);
        }

        public ServiceResult BulkCreate(IEnumerable<T> records)
        {
            return  _baseDL.BulkCreate(records);
        }
    }
}
