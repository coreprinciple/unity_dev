namespace Common
{
    public struct Either<TLeft, TRight>
    {
        readonly TLeft left;
        readonly TRight right;
        readonly bool isRight;

        public bool IsRight => isRight;
        public bool IsLeft => !isRight;

        public TLeft Left {
            get {
                if (IsRight)
                    throw new System.InvalidOperationException("No Left value present");
                return left;
            }
        }

        public TRight Right {
            get {
                if (IsLeft)
                    throw new System.InvalidOperationException("No Right value present");
                return right;
            }
        }

        Either(TLeft left, TRight right, bool isRight)
        {
            this.left = left;
            this.right = right;
            this.isRight = isRight;
        }

        public static implicit operator Either<TLeft, TRight>(TLeft left) => FromLeft(left);
        public static implicit operator Either<TLeft, TRight>(TRight right) => FromRight(right);

        public static Either<TLeft, TRight> FromLeft(TLeft left) => new(left, default, false);
        public static Either<TLeft, TRight> FromRight(TRight right) => new(default, right, true);

        public TResult Match<TResult>(System.Func<TLeft, TResult> leftFunc, System.Func<TRight, TResult> rightFunc)
            => IsRight ? rightFunc(right) : leftFunc(left);

        public Either<TLeft, TResult> Select<TResult>(System.Func<TRight, TResult> map)
            => IsRight ? Either<TLeft, TResult>.FromRight(map(right)) : Either<TLeft, TResult>.FromLeft(left);

        public Either<TLeft, TResult> SelectMany<TResult>(System.Func<TRight, Either<TLeft, TResult>> bind)
            => IsRight ? bind(right) : Either<TLeft, TResult>.FromLeft(left);

        public override string ToString() => IsRight ? $"Right({right})" : $"Left({left})";
    }
}
