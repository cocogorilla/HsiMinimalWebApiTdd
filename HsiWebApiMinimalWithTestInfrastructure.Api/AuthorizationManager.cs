using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace $safeprojectname$
{
    public class AuthorizationManager : ResourceAuthorizationManager
    {
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            switch (context.Resource.First().Value)
            {
                case "Ping":
                    return AuthorizePing(context);
                default:
                    return Nok();
            }
        }

        private Task<bool> AuthorizePing(ResourceAuthorizationContext context)
        {
            switch (context.Action.First().Value)
            {
                //    case "Write":
                //        return Eval(context.Principal.HasClaim("client_role", "HsiFullTrust"));
                case "Read":
                    return Eval(true);
                default:
                    return Nok();
            }
        }
    }
}