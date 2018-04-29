namespace EmployeesRegister.DataAccess
{
    class MethodResult<T>
    {
        public MethodResult(T value) : this(value, string.Empty) { }

        public MethodResult(T value, string description)
        {
            Value = value;
            Description = description;
        }

        public string Description { get; }

        public virtual bool Success { get { return Value != null; } }

        public T Value { get; }
    }

    class EmptyResult : MethodResult<bool>
    {
        public EmptyResult() : base(true, string.Empty) { }

        public EmptyResult(string description) : base(false, description) { }

        public override bool Success { get { return Value; } }
    }

    class InsertResult : MethodResult<int>
    {
        public InsertResult(int id) : base(id, string.Empty) { }

        public InsertResult(string description) : base(-1, description) { }

        public override bool Success { get { return Value != -1; } }
    }
}
