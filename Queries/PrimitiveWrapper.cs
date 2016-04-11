using System;

namespace Queries
{
    struct TypeContract
    {
        public TypeContract(Type t)
        {
            Value = t.FriendlyName();
        }

        public TypeContract(object t)
        {
            Value = t.GetType().FriendlyName();
        }

        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if ((obj is TypeContract) == false) return false;

            return Equals((TypeContract)obj);
        }

        public bool Equals(TypeContract other)
        {
            return string.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }

    struct FunctionContract
    {
        public FunctionContract(TypeContract input, TypeContract output)
        {
            Input = input;
            Output = output;
        }

        public TypeContract Input { get; set; }
        public TypeContract Output { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (obj is FunctionContract == false) return false;
            return Equals((FunctionContract)obj);
        }

        bool Equals(FunctionContract other)
        {
            return other.Input.Equals(this.Input) && other.Output.Equals(this.Output);
        }
    }
}
