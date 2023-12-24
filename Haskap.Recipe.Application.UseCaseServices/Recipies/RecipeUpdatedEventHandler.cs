using Haskap.Recipe.Domain;
using Haskap.Recipe.Domain.Common;
using Haskap.Recipe.Domain.RecipeAggregate.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Recipies;
public class RecipeUpdatedEventHandler : INotificationHandler<RecipeUpdatedDomainEvent>
{
    private readonly StepPicturesSettings _stepPicturesSettings;
    private readonly IServiceProvider _serviceProvider;

    public RecipeUpdatedEventHandler(
        IOptions<StepPicturesSettings> cateringPhotoSettingsOptions,
        IServiceProvider serviceProvider)
    {
        _stepPicturesSettings = cateringPhotoSettingsOptions.Value;
        _serviceProvider = serviceProvider;
    }

    public async Task Handle(RecipeUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await DeleteRecipePictureFile(notification, cancellationToken);
        await SaveNewRecipePictureFile(notification, cancellationToken);
    }

    private async Task SaveNewRecipePictureFile(RecipeUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var fullFolderPath = Path.Combine(
            notification.WebRootPath,
            _stepPicturesSettings.FolderName,
            notification.RecipeId.ToString());

        Directory.CreateDirectory(fullFolderPath);

        var fullFileName = Path.Combine(fullFolderPath, $"{notification.NewPictureFile.NewName}{notification.NewPictureFile.Extension}");
        using (var fileStream = System.IO.File.Create(fullFileName))
        {
            await fileStream.WriteAsync(notification.NewPictureFile.Content, cancellationToken);
        }
    }

    private async Task DeleteRecipePictureFile(RecipeUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var fullFileName = Path.Combine(
            notification.WebRootPath,
            _stepPicturesSettings.FolderName,
            notification.RecipeId.ToString(),
            notification.DeletedPictureFile.NewName + notification.DeletedPictureFile.Extension);

        System.IO.File.Delete(fullFileName);
    }
}
