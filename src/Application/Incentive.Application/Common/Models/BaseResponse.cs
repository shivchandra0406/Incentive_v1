using System.Collections.Generic;

namespace Incentive.Application.Common.Models
{
    public class BaseResponse
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static BaseResponse Success(string? message = null)
        {
            return new BaseResponse
            {
                Succeeded = true,
                Message = message
            };
        }

        public static BaseResponse Failure(string message, List<string>? errors = null)
        {
            return new BaseResponse
            {
                Succeeded = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }

        public static BaseResponse Failure(string message, string error)
        {
            return new BaseResponse
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { error }
            };
        }
    }

    public class BaseResponse<T> : BaseResponse
    {
        public T? Data { get; set; }

        public static BaseResponse<T> Success(T data, string? message = null)
        {
            return new BaseResponse<T>
            {
                Succeeded = true,
                Message = message,
                Data = data
            };
        }

        public new static BaseResponse<T> Failure(string message, List<string>? errors = null)
        {
            return new BaseResponse<T>
            {
                Succeeded = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }

        public new static BaseResponse<T> Failure(string message, string error)
        {
            return new BaseResponse<T>
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { error }
            };
        }
    }
}
