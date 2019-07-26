using System.Text;

namespace DeReviewer
{
    public sealed class MethodUniqueName
    {
        private readonly string fullName;
            
        public static MethodUniqueName Create(string fullNameWithoutReturnType) =>
            new MethodUniqueName(
                MethodUniqueNameExtensions.ReplaceGenericParameters(
                    new StringBuilder(fullNameWithoutReturnType)));

        internal MethodUniqueName(string fullName)
        {
            this.fullName = fullName;
        }
        
        public override string ToString() => fullName;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is MethodUniqueName c) return Equals(c);
            return false;
        }

        public override int GetHashCode()
        {
            return fullName.GetHashCode();
        }

        public static bool operator ==(MethodUniqueName left, MethodUniqueName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MethodUniqueName left, MethodUniqueName right)
        {
            return !Equals(left, right);
        }
        
        private bool Equals(MethodUniqueName other)
        {
            return string.Equals(fullName, other.fullName);
        }
    }
}