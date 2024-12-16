
namespace TestAmplitude
{
   public class AmplitudeUserPropertiesProvider : IAmplitudeUserPropertiesProvider
   {
      private readonly Dictionary<string, string> _userProperties = new();
      public AmplitudeUserPropertiesProvider()
      {
         //These values ought to/could be always known:
         AddEntitlementUserProperties();
         AddPlatformUserProperties();

         _userProperties.Add( "camtasia installed.state", "yes" );
         _userProperties.Add( "camtasia installed.version", "2023.4.4" );

         _userProperties.Add( "portfolio entitlements.products", "Audiate" );
         
      }

      private void AddEntitlementUserProperties()
      {
         _userProperties.Add( "entitlement.product", "camtasiapro" );
         _userProperties.Add( "entitlement.level", "trial" );
         _userProperties.Add( "entitlement.status", "active" );
      }

      private void AddPlatformUserProperties()
      {
         _userProperties.Add( "platform.platform", "win32" );
         _userProperties.Add( "platform.version", "Windows 10" );
         _userProperties.Add( "platform.type", "Windows_NT" );
         _userProperties.Add( "platform.release", "10.0.22631" );
      }

      public IDictionary<string, string> GetUserProperties()
      {
         SortedDictionary<string, string> result = new( _userProperties );

         //These values might only be know after taking a survey/signing in
         //Real code would probably ask via an injected interface

         result.Add( "entitlement.email.domain", "techsmith.com" );
         result.Add( "entitlement.multi user subscription id", "18293" );

         result.Add( "survey.experience", "pro" );
         result.Add( "survey.audience", "self" );
         result.Add( "survey.persona", "trainer" );
         result.Add( "survey.referrer", "search" );

         return result;
      }
   }
}