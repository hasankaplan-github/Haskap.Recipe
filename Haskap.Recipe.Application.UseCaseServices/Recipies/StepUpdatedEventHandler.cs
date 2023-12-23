using Haskap.Recipe.Application.Dtos.Common;
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
public class StepUpdatedEventHandler : INotificationHandler<StepUpdatedDomainEvent>
{
    private readonly StepPicturesSettings _stepPicturesSettings;

    public StepUpdatedEventHandler(IOptions<StepPicturesSettings> cateringPhotoSettingsOptions)
    {
        _stepPicturesSettings = cateringPhotoSettingsOptions.Value;
    }

    public async Task Handle(StepUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await CreateStepPicturesAsync(notification, cancellationToken);

        await DeleteStepPicturesAsync(notification, cancellationToken);
    }

    private async Task DeleteStepPicturesAsync(StepUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var fullFolderPath = Path.Combine(
            notification.WebRootPath,
            _stepPicturesSettings.FolderName,
            notification.RecipeId.ToString(),
            notification.StepId.ToString());

        foreach (var pictureFile in notification.DeletedPictureFiles ?? Enumerable.Empty<FileInputDto>())
        {
            var fullFileName = Path.Combine(fullFolderPath, $"{pictureFile.NewName}{pictureFile.Extension}");
            System.IO.File.Delete(fullFileName);
        }

        if (FolderIsEmpty(fullFolderPath))
        {
            Directory.Delete(fullFolderPath);
        }
    }

    private async Task CreateStepPicturesAsync(StepUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        if (!notification.FileInputDtos.Any())
        {
            return;
        }

        var fullFolderPath = Path.Combine(
            notification.WebRootPath,
            _stepPicturesSettings.FolderName,
            notification.RecipeId.ToString(),
            notification.StepId.ToString());

        Directory.CreateDirectory(fullFolderPath);

        var tasks = notification.FileInputDtos
            .AsParallel()
            .Select(async x =>
            {
                var fullFileName = Path.Combine(fullFolderPath, $"{x.NewName}{x.Extension}");
                using (var fileStream = System.IO.File.Create(fullFileName))
                {
                    await fileStream.WriteAsync(x.Content, cancellationToken);
                }
            });

        await Task.WhenAll(tasks);
    }

    private bool FolderIsEmpty(string fullFolderPath)
    {
        return !Directory.EnumerateFiles(fullFolderPath).Any();
    }
}
