using MediatR;

namespace MediatRPortal.Client.Models;

public record GetDesignerInfoQuery : IRequest<DesignerModel>;
