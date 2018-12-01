using System;
using System.Diagnostics;
using System.Linq;

namespace Portal.Dto.Result
{
    /// <summary>
    /// Service result model to implement defensive programming between service and presentation layer
    /// for more info look at dotnettips great article:
    /// https://www.dotnettips.info/post/2893/%D8%B7%D8%B1%D8%A7%D8%AD%DB%8C-%D9%88-%D9%BE%DB%8C%D8%A7%D8%AF%D9%87-%D8%B3%D8%A7%D8%B2%DB%8C-%D8%B2%DB%8C%D8%B1%D8%B3%D8%A7%D8%AE%D8%AA%DB%8C-%D8%A8%D8%B1%D8%A7%DB%8C-%D9%85%D8%AF%DB%8C%D8%B1%DB%8C%D8%AA-%D8%AE%D8%B7%D8%A7%D9%87%D8%A7%DB%8C-%D8%AD%D8%A7%D8%B5%D9%84-%D8%A7%D8%B2-business-rule-validation%D9%87%D8%A7-%D8%AF%D8%B1-servicelayer
    /// </summary>
    public class Result
    {
        private static readonly Result SuccessResult = new Result(true, null);

        protected Result(bool succeeded, string message)
        {
            if (succeeded)
            {
                if (!string.IsNullOrEmpty(message))
                    throw new ArgumentException("There should be no error message for success.", nameof(message));
            }
            else
            {
                if (message == null)
                    throw new ArgumentNullException(nameof(message), "There must be error message for failure.");
            }

            Succeeded = succeeded;
            Error = message;
        }

        public bool Succeeded { get; }
        public string Error { get; }

        [DebuggerStepThrough]
        public static Result Success()
        {
            return SuccessResult;
        }

        [DebuggerStepThrough]
        public static Result Failed(string message)
        {
            return new Result(false, message);
        }

        [DebuggerStepThrough]
        public static Result<T> Failed<T>(string message)
        {
            return new Result<T>(null, false, message);
        }

        [DebuggerStepThrough]
        public static Result<T> Success<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }

        [DebuggerStepThrough]
        public static Result Combine(string seperator, params Result[] results)
        {
            var failedResults = results.Where(x => !x.Succeeded).ToList();

            if (!failedResults.Any())
                return Success();

            var error = string.Join(seperator, failedResults.Select(x => x.Error).ToArray());
            return Failed(error);
        }

        [DebuggerStepThrough]
        public static Result Combine(params Result[] results)
        {
            return Combine(", ", results);
        }

        [DebuggerStepThrough]
        public static Result Combine<T>(params Result<T>[] results)
        {
            return Combine(", ", results);
        }

        [DebuggerStepThrough]
        public static Result Combine<T>(string seperator, params Result<T>[] results)
        {
            var untyped = results.Select(result => (Result)result).ToArray();
            return Combine(seperator, untyped);
        }

        public override string ToString()
        {
            return Succeeded
                ? "Succeeded"
                : $"Failed : {Error}";
        }
    }
    public class Result<T> : Result
    {
        private readonly T _value;

        protected internal Result(T value, bool succeeded, string error)
            : base(succeeded, error)
        {
            _value = value;
        }

        public Result(object value, bool succeeded, string error) : base(succeeded, error)
        {
            _value = (T) value;
        }

        public T Value
        {
            get
            {
                if (!Succeeded)
                    throw new InvalidOperationException("There is no value for failure.");

                return _value;
            }
        }
    }
}
