using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Infrastructure.Runtime.RuntimeModeValidators;

namespace MethodConf.Cms;

public class RuntimeModeValidatorComposer : IComposer
{
    // We disable HTTPS in production because we're running
    // behind a load balancer that does SSL termination for us
    public void Compose(IUmbracoBuilder builder) => builder.RuntimeModeValidators().Remove<UseHttpsValidator>();
}
