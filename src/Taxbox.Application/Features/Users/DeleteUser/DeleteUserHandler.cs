using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Users.DeleteUser;

public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, Result>
{
    private readonly IContext _context;

    public DeleteUserHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => Equals(x.Id, request.Id), cancellationToken);
        if (user is null) return Result.NotFound();
        _context.Users.Remove(user!);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}