using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.Common
{
    public class Result<T>
    {
        public T Payload { get; set; }
        public ResultStatus Status { get; set; }
        public ErrorCode ErrorCode { get; set; }

        public Result(T payload)
        {
            Payload = payload;
            Status = ResultStatus.Success;
        }

        public Result(ErrorCode errorCode, T payload = default(T))
        {
            Payload = payload;
            Status = ResultStatus.Failure;
            ErrorCode = errorCode;
        }

        public static Result<T> Ok(T payload)
        {
            return new Result<T>(payload);
        }

        public static Result<T> Failed(ErrorCode errorCode, T payload = default(T))
        {
            return new Result<T>(errorCode, payload);
        }

    }

    public enum ResultStatus
    {
        Success, Failure
    }

    public enum ErrorCode
    {
        StepPhotoRequired = 1
    }
}
