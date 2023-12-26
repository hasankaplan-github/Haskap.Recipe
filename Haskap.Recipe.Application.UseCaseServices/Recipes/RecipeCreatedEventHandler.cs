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

namespace Haskap.Recipe.Application.UseCaseServices.Recipes;
public class RecipeCreatedEventHandler : INotificationHandler<RecipeCreatedDomainEvent>
{
    private readonly StepPicturesSettings _stepPicturesSettings;

    public RecipeCreatedEventHandler(IOptions<StepPicturesSettings> recipePictureSettingsOptions)
    {
        _stepPicturesSettings = recipePictureSettingsOptions.Value;
    }

    public async Task Handle(RecipeCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await CreateRecipePictureAsync(notification, cancellationToken);
    }

    private async Task CreateRecipePictureAsync(RecipeCreatedDomainEvent notification, CancellationToken cancellationToken)
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
}
