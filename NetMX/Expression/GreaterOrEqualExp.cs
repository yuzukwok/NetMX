﻿using System;

namespace NetMX
{
    public class GreaterOrEqualExp : BinaryExp<bool, Number, Number>
    {
        public GreaterOrEqualExp(IExpression<Number> left, IExpression<Number> right)
            : base(left, right)
        {
        }

        public override void Accept(IExpressionTreeVisitor visitor)
        {
            base.Accept(visitor);
            visitor.Visit(this);
        }

        public override bool Evaluate(Func<Number> leftValue, Func<Number> rightValue)
        {
            return leftValue() >= rightValue();
        }
    }
}