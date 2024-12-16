namespace TestAmplitude
{
   public interface IAmplitudeUserPropertiesProvider
   {
      IDictionary<string, string> GetUserProperties();
   }
}