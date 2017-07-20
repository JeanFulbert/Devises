using System.Collections;

namespace Devises.Domain
{
    public static class HashCode
    {
        public static int Combine(params object[] objects)
        {
            unchecked // Overflow is fine, just wrap
            {
                var hash = (int)2166136261;

                foreach (var obj in objects)
                {
                    if (obj != null)
                    {
                        hash = (hash * 16777619) ^ obj.GetHashCode();
                    }
                }

                return hash;
            }
        }

        public static int CombineWithAllElementsOf(this int hash, params IEnumerable[] sequences)
        {
            unchecked // Overflow is fine, just wrap
            {
                var combinedHash = hash;

                foreach (var seq in sequences)
                {
                    if (seq != null)
                    {
                        foreach (var obj in seq)
                        {
                            if (obj != null)
                            {
                                combinedHash = (combinedHash * 16777619) ^ obj.GetHashCode();
                            }
                        }
                    }
                }

                return combinedHash;
            }
        }
    }
}
