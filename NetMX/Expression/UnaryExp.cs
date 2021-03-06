﻿using System;

namespace NetMX
{
    public abstract class UnaryExp<TResult, TInput> : IExpression<TResult>
    {
        private readonly IExpression<TInput> _input;

        protected UnaryExp(IExpression<TInput> input)
        {
            _input = input;
        }

        public TResult Evaluate(IQueryEvaluationContext context)
        {
            return Evaluate(() => _input.Evaluate(context));
        }

        public virtual void Accept(IExpressionTreeVisitor visitor)
        {
            _input.Accept(visitor);
        }

        public abstract TResult Evaluate(Func<TInput> inputValue);
    }
}