using System.Text;

namespace DeReviewer.Analysis
{
    public sealed class MethodUniqueSignature
    {
        private readonly string fullName;
            
        public static MethodUniqueSignature Create(string fullNameWithoutReturnType) =>
            new MethodUniqueSignature(
                MethodUniqueSignatureExtensions.ReplaceGenericParameters(
                    new StringBuilder(fullNameWithoutReturnType)));

        internal MethodUniqueSignature(string fullName)
        {
            this.fullName = fullName;
        }
        
        public override string ToString() => fullName;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is MethodUniqueSignature c) return Equals(c);
            return false;
        }

        public override int GetHashCode()
        {
            return fullName.GetHashCode();
        }

        public static bool operator ==(MethodUniqueSignature left, MethodUniqueSignature right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MethodUniqueSignature left, MethodUniqueSignature right)
        {
            return !Equals(left, right);
        }
        
        private bool Equals(MethodUniqueSignature other)
        {
            return string.Equals(fullName, other.fullName);
        }
    }
}