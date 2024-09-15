using MediatR;
using MediatRPortal.Client.Features.Application.Components.Base;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Commands;

public record GenerateRoutesCommand(Guid SessionId, int NumberToGenerate) : RequestBase<List<RouteModel>>(SessionId);

public class GenerateRoutesCommandHandler : IRequestHandler<GenerateRoutesCommand, List<RouteModel>>
{
    private readonly IMediator _mediator;

    public GenerateRoutesCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<List<RouteModel>> Handle(GenerateRoutesCommand request, CancellationToken cancellationToken)
    {
        var routeIds = new List<RouteModel>();

        for (int i = 0; i < request.NumberToGenerate; i++)
        {
            var route = new RouteModel();
            route.Origin = $"Origin {route.Id}";
            route.Destination = $"Destination {i}";
            route.Currency = "AUD";
            route.ChargeTypes = ChargeTypes.Origin | ChargeTypes.Destination | ChargeTypes.Freight;
            route.JobClasses = JobClasses.FCL | JobClasses.LCL;

            await _mediator.Publish(new RouteAddedNotification(request.SessionId, route), cancellationToken);

            routeIds.Add(route);
        }

        return routeIds;

    }
}