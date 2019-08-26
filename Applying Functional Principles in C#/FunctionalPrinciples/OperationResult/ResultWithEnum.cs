﻿using System;

namespace OperationResult
{
    public class ResultWithEnum
    {
        protected ResultWithEnum(bool isSuccess, ErrorType? errorType)
        {
            if (isSuccess && errorType != null)
                throw new InvalidOperationException();
            
            if (!isSuccess && errorType == null)
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            ErrorType = errorType;
        }

        public ErrorType? ErrorType { get; }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public static ResultWithEnum Fail(ErrorType? errorType)
        {
            return new ResultWithEnum(false, errorType);
        }

        public static ResultWithEnum<T> Fail<T>(ErrorType? errorType)
        {
            return new ResultWithEnum<T>(default(T), false, errorType);
        }

        public static ResultWithEnum Ok()
        {
            return new ResultWithEnum(true, null);
        }

        public static ResultWithEnum<T> Ok<T>(T value)
        {
            return new ResultWithEnum<T>(value, true, null);
        }

    }

    public class ResultWithEnum<T> : ResultWithEnum
    {
        private readonly T _value;

        protected internal ResultWithEnum(T value, bool isSuccess, ErrorType? errorType)
            : base(isSuccess, errorType)
        {
            _value = value;
        }

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException();

                return _value;
            }
        }
    }

    public enum ErrorType
    {
        DatabaseIsOffline,
        CustomerAlreadyExists
    }
}
