using System;

namespace Portal.Dto.Result
{
    /// <summary>
    /// Service result model to implement defensive programming between service and presentation layer
    /// for more info look at dotnettips great article:
    /// https://www.dotnettips.info/post/2893/%D8%B7%D8%B1%D8%A7%D8%AD%DB%8C-%D9%88-%D9%BE%DB%8C%D8%A7%D8%AF%D9%87-%D8%B3%D8%A7%D8%B2%DB%8C-%D8%B2%DB%8C%D8%B1%D8%B3%D8%A7%D8%AE%D8%AA%DB%8C-%D8%A8%D8%B1%D8%A7%DB%8C-%D9%85%D8%AF%DB%8C%D8%B1%DB%8C%D8%AA-%D8%AE%D8%B7%D8%A7%D9%87%D8%A7%DB%8C-%D8%AD%D8%A7%D8%B5%D9%84-%D8%A7%D8%B2-business-rule-validation%D9%87%D8%A7-%D8%AF%D8%B1-servicelayer
    /// </summary>
    public static class ResultExtensions
    {
        public static Result<TK> OnSuccess<T, TK>(this Result<T> result, Func<T, TK> func)
        {
            return !result.Succeeded ? Portal.Dto.Result.Result.Failed<TK>(result.Error) : Portal.Dto.Result.Result.Success(func(result.Value));
        }

        public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, string message)
        {
            if (!result.Succeeded)
                return Portal.Dto.Result.Result.Failed<T>(result.Error);

            return !predicate(result.Value) ? Portal.Dto.Result.Result.Failed<T>(message) : Portal.Dto.Result.Result.Success(result.Value);
        }

        public static Result<TK> Map<T, TK>(this Result<T> result, Func<T, TK> func)
        {
            return !result.Succeeded ? Portal.Dto.Result.Result.Failed<TK>(result.Error) : Portal.Dto.Result.Result.Success(func(result.Value));
        }

        public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.Succeeded) action(result.Value);

            return result;
        }

        public static T OnBoth<T>(this Portal.Dto.Result.Result result, Func<Portal.Dto.Result.Result, T> func)
        {
            return func(result);
        }

        public static Portal.Dto.Result.Result OnSuccess(this Portal.Dto.Result.Result result, Action action)
        {
            if (result.Succeeded) action();

            return result;
        }

        public static Result<T> OnSuccess<T>(this Portal.Dto.Result.Result result, Func<T> func)
        {
            return !result.Succeeded ? Portal.Dto.Result.Result.Failed<T>(result.Error) : Portal.Dto.Result.Result.Success(func());
        }

        public static Result<TK> OnSuccess<T, TK>(this Result<T> result, Func<T, Result<TK>> func)
        {
            return !result.Succeeded ? Portal.Dto.Result.Result.Failed<TK>(result.Error) : func(result.Value);
        }

        public static Result<T> OnSuccess<T>(this Portal.Dto.Result.Result result, Func<Result<T>> func)
        {
            return !result.Succeeded ? Portal.Dto.Result.Result.Failed<T>(result.Error) : func();
        }

        public static Result<TK> OnSuccess<T, TK>(this Result<T> result, Func<Result<TK>> func)
        {
            return !result.Succeeded ? Portal.Dto.Result.Result.Failed<TK>(result.Error) : func();
        }

        public static Portal.Dto.Result.Result OnSuccess<T>(this Result<T> result, Func<T, Portal.Dto.Result.Result> func)
        {
            return !result.Succeeded ? Portal.Dto.Result.Result.Failed(result.Error) : func(result.Value);
        }

        public static Portal.Dto.Result.Result OnSuccess(this Portal.Dto.Result.Result result, Func<Portal.Dto.Result.Result> func)
        {
            return !result.Succeeded ? result : func();
        }

        public static Portal.Dto.Result.Result Ensure(this Portal.Dto.Result.Result result, Func<bool> predicate, string message)
        {
            if (!result.Succeeded)
                return Portal.Dto.Result.Result.Failed(result.Error);

            return !predicate() ? Portal.Dto.Result.Result.Failed(message) : Portal.Dto.Result.Result.Success();
        }

        public static Result<T> Map<T>(this Portal.Dto.Result.Result result, Func<T> func)
        {
            return !result.Succeeded ? Portal.Dto.Result.Result.Failed<T>(result.Error) : Portal.Dto.Result.Result.Success(func());
        }


        public static TK OnBoth<T, TK>(this Result<T> result, Func<Result<T>, TK> func)
        {
            return func(result);
        }

        public static Result<T> OnFailure<T>(this Result<T> result, Action action)
        {
            if (!result.Succeeded) action();

            return result;
        }

        public static Portal.Dto.Result.Result OnFailure(this Portal.Dto.Result.Result result, Action action)
        {
            if (!result.Succeeded) action();

            return result;
        }

        public static Result<T> OnFailure<T>(this Result<T> result, Action<string> action)
        {
            if (!result.Succeeded) action(result.Error);

            return result;
        }

        public static Portal.Dto.Result.Result OnFailure(this Portal.Dto.Result.Result result, Action<string> action)
        {
            if (!result.Succeeded) action(result.Error);

            return result;
        }
    }
}
