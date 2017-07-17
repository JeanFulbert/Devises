namespace Devises.Domain
{
    public static class GetHashCodeCombiner
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
    }
}
