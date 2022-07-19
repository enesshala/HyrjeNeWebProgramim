using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Presistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Photos
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor userAccessor;
            private readonly IPhotoAccessor photoAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor, IPhotoAccessor photoAccessor)
            {
                _context = context;
                this.userAccessor = userAccessor;
                this.photoAccessor = photoAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.UserName == userAccessor.GetUsername());

                if (user == null) return null;

                var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);

                if (photo == null) return null;

                if (photo.IsMain) return Result<Unit>.Failure("You cannot delete your main photo");

                var result = await photoAccessor.DeletePhoto(photo.Id);

                if (result == null) return Result<Unit>.Failure("Problem deleting photo Cloudinary");

                user.Photos.Remove(photo);

                var sucess = await _context.SaveChangesAsync() > 0;

                if (sucess) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Problem deleting photo from API");
            }
        }
    }
}
