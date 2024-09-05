using MediatR;
using MediatRPortal.Client.Features.Designer.Notifications;

namespace MediatRPortal.Client.Features.Designer.Commands;

public record SetCargoDetailsCommand(bool HasCargoDetails) : IRequest;

public class SetCargoDetailsCommandHandler : IRequestHandler<SetCargoDetailsCommand>
{
    private readonly IMediator _mediator;
    public SetCargoDetailsCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Handle(SetCargoDetailsCommand request, CancellationToken cancellationToken)
    {
        return _mediator.Publish(new HasCargoDetailsSetNotification(request.HasCargoDetails), cancellationToken);
    }
}
