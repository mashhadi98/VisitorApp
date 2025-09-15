﻿namespace VisitorApp.Domain.Shared
{
    public class Error : IEquatable<Error>
    {
        public static readonly Error None = new(string.Empty);
        public static readonly Error NullValue = new("The specified result value is null.");

        public Error(string message)
        {
            Message = message;
        }

        public string Message { get; }

        public static implicit operator string(Error error) => error.Message;

        public static bool operator ==(Error? a, Error? b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Error? a, Error? b) => !(a == b);

        public virtual bool Equals(Error? other)
        {
            if (other is null)
            {
                return false;
            }

            return Message == other.Message;
        }

        public override bool Equals(object? obj) => obj is Error error && Equals(error);

        public override int GetHashCode() => HashCode.Combine(Message);

        public override string ToString() => Message;
    }
}
