namespace Devises.Domain
{
    using System.Linq;

    public static class GetHashCodeCombiner
    {
        public static int GetCombinedHashCode(params object[] objects)
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
    }
}
